using System;
using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    public partial class FLVER0
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class Mesh : IFlverMesh
        {
            /// <summary>
            /// When 1, mesh is in bind pose; when 0, it isn't. Most likely has further implications.
            /// </summary>
            public byte Dynamic { get; set; }

            /// <summary>
            /// Index of the material used by all triangles in this mesh.
            /// </summary>
            public byte MaterialIndex { get; set; }
            int IFlverMesh.MaterialIndex => MaterialIndex;

            /// <summary>
            /// Whether triangles can be seen through from behind.
            /// </summary>
            public bool CullBackfaces { get; set; }

            /// <summary>
            /// Whether vertices are defined as a triangle strip or individual triangles.
            /// </summary>
            public byte TriangleStrip { get; set; }

            /// <summary>
            /// Apparently does nothing. Usually points to a dummy bone named after the model, possibly just for labelling.
            /// </summary>
            public short DefaultBoneIndex { get; set; }

            /// <summary>
            /// Indexes of bones in the bone collection which may be used by vertices in this mesh.
            /// </summary>
            public short[] BoneIndices { get; private set; }

            public short Unk46 { get; set; }

            public List<int> VertexIndices { get; set; }

            public List<FLVER.Vertex> Vertices { get; set; }

            IReadOnlyList<FLVER.Vertex> IFlverMesh.Vertices => Vertices;

            public List<VertexBuffer> VertexBuffers1 { get; set; }

            public int LayoutIndex { get; set; }

            /// <summary>
            /// Creates a new Mesh with default values.
            /// </summary>
            public Mesh()
            {
                DefaultBoneIndex = 0;
                BoneIndices = new short[28];
                BoneIndices[0] = 0;

                for (int i = 0; i < 27; i++)
                    BoneIndices[i] = -1;
                VertexIndices = new List<int>();
                Vertices = new List<FLVER.Vertex>();
                VertexBuffers1 = new List<VertexBuffer>() { new VertexBuffer() };
            }
            internal Mesh(BinaryReaderEx br, FLVER0 flv, int dataOffset)
            {
                Dynamic = br.ReadByte();
                MaterialIndex = br.ReadByte();
                CullBackfaces = br.ReadBoolean();
                TriangleStrip = br.ReadByte();

                int vertexIndexCount = br.ReadInt32();
                int vertexCount = br.ReadInt32();
                DefaultBoneIndex = br.ReadInt16();
                BoneIndices = br.ReadInt16s(28);
                Unk46 = br.ReadInt16();
                br.ReadInt32(); // Vertex indices length
                int vertexIndicesOffset = br.ReadInt32();
                int bufferDataLength = br.ReadInt32();
                int bufferDataOffset = br.ReadInt32();
                int vertexBuffersHeaderOffset1 = br.ReadInt32();
                int vertexBuffersHeaderOffset2 = br.ReadInt32();
                br.AssertInt32(0);

                if (flv.Header.VertexIndexSize == 16)
                {
                    VertexIndices = new List<int>(vertexCount);
                    foreach (ushort index in br.GetUInt16s(dataOffset + vertexIndicesOffset, vertexIndexCount))
                        VertexIndices.Add(index);
                }
                else if (flv.Header.VertexIndexSize == 32)
                {
                    VertexIndices = new List<int>(br.GetInt32s(dataOffset + vertexIndicesOffset, vertexIndexCount));
                }
                VertexBuffer buffer;
                // Stupid hack for old (version F?) flvers; for example DeS o9993.
                if (vertexBuffersHeaderOffset1 == 0)
                {
                    buffer = new VertexBuffer()
                    {
                        BufferLength = bufferDataLength,
                        BufferOffset = bufferDataOffset,
                        LayoutIndex = 0,
                    };
                }
                else
                {
                    br.StepIn(vertexBuffersHeaderOffset1);
                    {
                        VertexBuffers1 = VertexBuffer.ReadVertexBuffers(br);
                        if (VertexBuffers1.Count == 0)
                            throw new NotSupportedException("First vertex buffer list is expected to contain at least 1 buffer.");
                        for (int i = 1; i < VertexBuffers1.Count; i++)
                            if (VertexBuffers1[i].BufferLength != 0)
                                throw new NotSupportedException("Vertex buffers after the first one in the first buffer list are expected to be empty.");
                        buffer = VertexBuffers1[0];
                    }
                    br.StepOut();
                }

                if (vertexBuffersHeaderOffset2 != 0)
                {
                    br.StepIn(vertexBuffersHeaderOffset2);
                    {
                        List<VertexBuffer> vertexBuffers2 = VertexBuffer.ReadVertexBuffers(br);
                        if (vertexBuffers2.Count != 0)
                            throw new NotSupportedException("Second vertex buffer list is expected to contain exactly 0 buffers.");
                    }
                    br.StepOut();
                }

                br.StepIn(dataOffset + buffer.BufferOffset);
                {
                    LayoutIndex = buffer.LayoutIndex;
                    BufferLayout layout = flv.Materials[MaterialIndex].Layouts[LayoutIndex];

                    float uvFactor = 1024;
                    // NB hack
                    if (!br.BigEndian)
                        uvFactor = 2048;

                    Vertices = new List<FLVER.Vertex>(vertexCount);
                    for (int i = 0; i < vertexCount; i++)
                    {
                        var vert = new FLVER.Vertex();
                        vert.Read(br, layout, uvFactor);
                        Vertices.Add(vert);
                    }
                }
                br.StepOut();
            }

            public List<FLVER.Vertex[]> GetFaces(int version)
            {
                List<int> indices = Triangulate(version);
                var faces = new List<FLVER.Vertex[]>();
                for (int i = 0; i < indices.Count; i += 3)
                {
                    faces.Add(new FLVER.Vertex[]
                    {
                        Vertices[indices[i + 0]],
                        Vertices[indices[i + 1]],
                        Vertices[indices[i + 2]],
                    });
                }
                return faces;
            }

            public List<int[]> GetFaceVertexIndices(int version)
            {
                List<int> indices = Triangulate(version);
                var faces = new List<int[]>();
                for (int i = 0; i < indices.Count; i += 3)
                {
                    faces.Add(new int[]
                    {
                        indices[i + 0],
                        indices[i + 1],
                        indices[i + 2]
                    });
                }
                return faces;
            }

            public List<int> Triangulate(int version)
            {
                var triangles = new List<int>();
                if (version >= 0x15 && TriangleStrip == 0)
                {
                    triangles = new List<int>(VertexIndices);
                }
                else
                {
                    bool checkFlip = false;
                    bool flip = false;
                    for (int i = 0; i < VertexIndices.Count - 2; i++)
                    {
                        int vi1 = VertexIndices[i];
                        int vi2 = VertexIndices[i + 1];
                        int vi3 = VertexIndices[i + 2];

                        if (vi1 == 0xFFFF || vi2 == 0xFFFF || vi3 == 0xFFFF)
                        {
                            checkFlip = true;
                        }
                        else
                        {
                            if (vi1 != vi2 && vi1 != vi3 && vi2 != vi3)
                            {
                                // Every time the triangle strip restarts, compare the average vertex normal to the face normal
                                // and flip the starting direction if they're pointing away from each other.
                                // I don't know why this is necessary; in most models they always restart with the same orientation
                                // as you'd expect. But on some, I can't discern any logic to it, thus this approach.
                                // It's probably hideously slow because I don't know anything about math.
                                // Feel free to hit me with a PR. :slight_smile:
                                if (checkFlip)
                                {
                                    FLVER.Vertex v1 = Vertices[vi1];
                                    FLVER.Vertex v2 = Vertices[vi2];
                                    FLVER.Vertex v3 = Vertices[vi3];
                                    Vector3 n1 = v1.Normal;
                                    Vector3 n2 = v2.Normal;
                                    Vector3 n3 = v3.Normal;
                                    Vector3 vertexNormal = Vector3.Normalize((n1 + n2 + n3) / 3);
                                    Vector3 faceNormal = Vector3.Normalize(Vector3.Cross(v2.Position - v1.Position, v3.Position - v1.Position));
                                    float angle = Vector3.Dot(faceNormal, vertexNormal) / (faceNormal.Length() * vertexNormal.Length());
                                    flip = angle >= 0;
                                    checkFlip = false;
                                }

                                if (!flip)
                                {
                                    triangles.Add(vi1);
                                    triangles.Add(vi2);
                                    triangles.Add(vi3);
                                }
                                else
                                {
                                    triangles.Add(vi3);
                                    triangles.Add(vi2);
                                    triangles.Add(vi1);
                                }
                            }
                            flip = !flip;
                        }
                    }
                }
                return triangles;
            }

            internal void AddFaceCounts(int version, ref int trueFaceCount, ref int totalFaceCount)
            {

                totalFaceCount += Triangulate(version).Count / 3;
                trueFaceCount += Triangulate(version).Count / 3;
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteByte(Dynamic);
                bw.WriteByte(MaterialIndex);
                bw.WriteBoolean(CullBackfaces);
                bw.WriteByte(TriangleStrip);

                bw.WriteInt32(VertexIndices.Count);
                bw.WriteInt32(Vertices.Count);
                bw.WriteInt16(DefaultBoneIndex);
                bw.WriteInt16s(BoneIndices);
                bw.WriteInt16(Unk46);
                bw.ReserveInt32($"vertexIndicesLength{index}");//TEMPSHIT // Vertex indices length
                bw.ReserveInt32($"vertexIndicesOffset{index}");
                bw.ReserveInt32($"bufferDataLength{index}");//TEMPSHIT bufferDataLength
                bw.ReserveInt32($"bufferDataOffset{index}");
                bw.ReserveInt32($"vertexBuffersHeaderOffset1_{index}");
                bw.WriteInt32(0);//vertexBuffersHeaderOffset2
                bw.WriteInt32(0);
            }

            internal void WriteVertexBuffers1(BinaryWriterEx bw, int meshIndex)
            {
                bw.WriteInt32(VertexBuffers1.Count);
                bw.ReserveInt32($"BuffersOffset");
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.FillInt32($"BuffersOffset", (int)bw.Position);
                for (int i = 0; i < VertexBuffers1.Count; i++)
                {
                    VertexBuffers1[i].WriteVertexBuffers(bw, VertexBuffers1.Count, meshIndex);
                }
            }

            internal void WriteIndexes(BinaryWriterEx bw, byte VertexIndexSize)
            {
                if (VertexIndexSize == 16)
                {
                    List<short> newVertexIndices = new List<short>();
                    foreach (var vIndex in VertexIndices)
                        newVertexIndices.Add((short)vIndex);
                    bw.WriteInt16s(newVertexIndices);
                }
                else if (VertexIndexSize == 32)
                    bw.WriteInt32s(VertexIndices);
            }

            internal void WriteVertices(BinaryWriterEx bw, List<Material> Materials, int index)
            {
                BufferLayout layout = Materials[MaterialIndex].Layouts[LayoutIndex];
                bw.FillInt32($"bufferDataLength{index}", layout.Size * Vertices.Count);
                foreach (var vertex in Vertices)
                {
                    float uvFactor = 1024;
                    // NB hack
                    if (!bw.BigEndian)
                        uvFactor = 2048;
                    vertex.PrepareWrite();
                    vertex.Write(bw, layout, uvFactor);
                    vertex.FinishWrite();
                }
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
