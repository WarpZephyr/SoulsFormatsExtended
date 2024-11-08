using System;
using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    public partial class FLVER0
    {
        /// <summary>
        /// An individual chunk of a model.
        /// </summary>
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
            public bool UseTriangleStrips { get; set; }

            /// <summary>
            /// Apparently does nothing. Usually points to a dummy bone named after the model, possibly just for labelling.
            /// </summary>
            public short DefaultBoneIndex { get; set; }

            /// <summary>
            /// Indexes of bones in the bone collection which may be used by vertices in this mesh.
            /// </summary>
            /// <remarks>
            /// Always has 28 indices; Unused indices are set to -1.
            /// </remarks>
            public short[] BoneIndices { get; private set; }

            /// <summary>
            /// Get the number of used bone indices.
            /// </summary>
            public int BoneCount
            {
                get
                {
                    int count = 0;
                    for (int i = 0; i < 28; i++)
                    {
                        short index = BoneIndices[i];
                        if (index != -1)
                        {
                            count++;
                        }
                    }
                    return count;
                }
            }

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk46 { get; set; }

            /// <summary>
            /// Indexes of the vertices of this mesh.
            /// </summary>
            public List<int> Indices { get; set; }

            /// <summary>
            /// Vertices in this mesh.
            /// </summary>
            public List<FLVER.Vertex> Vertices { get; set; }
            IReadOnlyList<FLVER.Vertex> IFlverMesh.Vertices => Vertices;

            /// <summary>
            /// The index of the BufferLayout used by this mesh.
            /// </summary>
            public int LayoutIndex { get; set; }

            /// <summary>
            /// Create a new and empty Mesh with default values.
            /// </summary>
            public Mesh()
            {
                Dynamic = 0;
                MaterialIndex = 0;
                CullBackfaces = true;
                UseTriangleStrips = false;
                DefaultBoneIndex = 0;
                BoneIndices = new short[28];
                Indices = new List<int>();
                Vertices = new List<FLVER.Vertex>();
                for (int i = 0; i < 28; i++)
                    BoneIndices[i] = -1;
            }

            /// <summary>
            /// Clone an existing Mesh.
            /// </summary>
            public Mesh(Mesh mesh)
            {
                Dynamic = mesh.Dynamic;
                MaterialIndex = mesh.MaterialIndex;
                CullBackfaces = mesh.CullBackfaces;
                UseTriangleStrips = mesh.UseTriangleStrips;
                DefaultBoneIndex = mesh.DefaultBoneIndex;
                BoneIndices = new short[28];
                Indices = new List<int>();
                Vertices = new List<FLVER.Vertex>();
                for (int i = 0; i < 28; i++)
                    BoneIndices[i] = mesh.BoneIndices[i];

                Unk46 = mesh.Unk46;
                for (int i = 0; i < mesh.Indices.Count; i++)
                    Indices[i] = mesh.Indices[i];
                for (int i = 0; i < mesh.Vertices.Count; i++)
                    Vertices[i] = new FLVER.Vertex(mesh.Vertices[i]);

                LayoutIndex = mesh.LayoutIndex;
            }

            /// <summary>
            /// Read a Mesh from a stream.
            /// </summary>
            /// <param name="br">The stream.</param>
            /// <param name="flv">The model so the materials list can be retrieved.</param>
            /// <param name="dataOffset">The starting offset of data in the model.</param>
            /// <exception cref="NotSupportedException">There were more than one vertex buffer.</exception>
            internal Mesh(BinaryReaderEx br, FLVER0 flv, int dataOffset)
            {
                Dynamic = br.ReadByte();
                MaterialIndex = br.ReadByte();
                CullBackfaces = br.ReadBoolean();
                UseTriangleStrips = br.ReadBoolean();

                int vertexIndexCount = br.ReadInt32();
                int vertexCount = br.ReadInt32();
                DefaultBoneIndex = br.ReadInt16();
                BoneIndices = br.ReadInt16s(28);
                Unk46 = br.ReadInt16();
                br.ReadInt32(); // Vertex indices length
                int vertexIndicesOffset = br.ReadInt32();
                int bufferDataLength = br.ReadInt32();
                int bufferDataOffset = br.ReadInt32();
                int vertexBuffersOffset1 = ReadVarEndianInt32(br, flv.Header.Version);
                int vertexBuffersOffset2 = ReadVarEndianInt32(br, flv.Header.Version);
                br.AssertInt32(0);

                if (flv.Header.VertexIndexSize == 16)
                {
                    Indices = new List<int>(vertexCount);
                    foreach (ushort index in br.GetUInt16s(dataOffset + vertexIndicesOffset, vertexIndexCount))
                        Indices.Add(index);
                }
                else if (flv.Header.VertexIndexSize == 32)
                {
                    Indices = new List<int>(br.GetInt32s(dataOffset + vertexIndicesOffset, vertexIndexCount));
                }

                VertexBuffer buffer;
                // Stupid hack for old (version F?) flvers; for example DeS o9993.
                if (vertexBuffersOffset1 == 0)
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
                    br.StepIn(vertexBuffersOffset1);
                    {
                        List<VertexBuffer> vertexBuffers1 = VertexBuffer.ReadVertexBuffers(br, flv.Header.Version);
                        if (vertexBuffers1.Count == 0)
                            throw new NotSupportedException("First vertex buffer list is expected to contain at least 1 buffer.");
                        for (int i = 1; i < vertexBuffers1.Count; i++)
                            if (vertexBuffers1[i].BufferLength != 0)
                                throw new NotSupportedException("Vertex buffers after the first one in the first buffer list are expected to be empty.");
                        buffer = vertexBuffers1[0];
                    }
                    br.StepOut();
                }

                if (vertexBuffersOffset2 != 0)
                {
                    br.StepIn(vertexBuffersOffset2);
                    {
                        List<VertexBuffer> vertexBuffers2 = VertexBuffer.ReadVertexBuffers(br, flv.Header.Version);
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
                    if (flv.Header.Version == 0x12|| !br.BigEndian)
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

            /// <summary>
            /// Write this Mesh to a stream.
            /// </summary>
            /// <param name="bw">The stream.</param>
            /// <param name="flv">The model so the materials list can be retrieved.</param>
            /// <param name="index">The index of this Mesh for reserving offset values to be filled later.</param>
            internal void Write(BinaryWriterEx bw, FLVER0 flv, int index)
            {
                Material material = flv.Materials[MaterialIndex];
                bw.WriteByte(Dynamic);
                bw.WriteByte(MaterialIndex);
                bw.WriteBoolean(CullBackfaces);
                bw.WriteBoolean(UseTriangleStrips);

                bw.WriteInt32(Indices.Count);
                bw.WriteInt32(Vertices.Count);
                bw.WriteInt16(DefaultBoneIndex);
                bw.WriteInt16s(BoneIndices);
                bw.WriteInt16(Unk46);
                bw.WriteInt32(Indices.Count * 2);
                bw.ReserveInt32($"VertexIndicesOffset{index}");
                bw.WriteInt32(material.Layouts[LayoutIndex].Size * Vertices.Count);
                bw.ReserveInt32($"VertexBufferOffset{index}");
                bw.ReserveInt32($"VertexBufferListOffset{index}");
                bw.WriteInt32(0); //We don't intend to fill vertexBuffersOffset2 so we'll just write it 0 now.
                bw.WriteInt32(0);
            }

            /// <summary>
            /// Write the VertexIndices of this Mesh to a stream.
            /// </summary>
            /// <param name="bw">The stream.</param>
            /// <param name="vertexIndexSize">The size in bits of each vertex index.</param>
            /// <param name="dataOffset">The starting offset of data in the model.</param>
            /// <param name="index">The index of this Mesh for filling reserved offset values.</param>
            internal void WriteVertexIndices(BinaryWriterEx bw, byte vertexIndexSize, int dataOffset, int index)
            {
                bw.FillInt32($"VertexIndicesOffset{index}", (int)bw.Position - dataOffset);
                if (vertexIndexSize == 16)
                {
                    for (int i = 0; i < Indices.Count; i++)
                    {
                        bw.WriteUInt16((ushort)Indices[i]);
                    }
                }
                else if (vertexIndexSize == 32)
                {
                    for (int i = 0; i < Indices.Count; i++)
                    {
                        bw.WriteInt32(Indices[i]);
                    }
                }
            }

            /// <summary>
            /// Write the header of the VertexBuffer this mesh uses to a stream.
            /// </summary>
            /// <param name="bw">The stream.</param>
            /// <param name="flv">The model so the materials list can be retrieved.</param>
            /// <param name="index">The index of this Mesh for reserving offset values to be filled later.</param>
            internal void WriteVertexBufferHeader(BinaryWriterEx bw, FLVER0 flv, int index)
            {
                bw.FillInt32($"VertexBufferListOffset{index}", (int)bw.Position);

                bw.WriteInt32(1); //bufferCount
                bw.ReserveInt32($"VertexBufferInfoOffset{index}");
                bw.WriteInt32(0);
                bw.WriteInt32(0);

                bw.FillInt32($"VertexBufferInfoOffset{index}", (int)bw.Position);

                //Since only the first VertexBuffer data is kept no matter what, we'll only write the first
                bw.WriteInt32(LayoutIndex);
                bw.WriteInt32(flv.Materials[MaterialIndex].Layouts[LayoutIndex].Size * Vertices.Count);
                bw.ReserveInt32($"VertexBufferOffset{index}_{0}");
                bw.WriteInt32(0);
            }

            /// <summary>
            /// Write the vertex buffer data of this Mesh to a stream.
            /// </summary>
            /// <param name="bw">The stream.</param>
            /// <param name="flv">The model so the materials list can be retrieved.</param>
            /// <param name="dataOffset">The starting offset of data in the model.</param>
            /// <param name="index">The index of this Mesh for filling reserved offset values.</param>
            internal void WriteVertexBufferData(BinaryWriterEx bw, FLVER0 flv, int dataOffset, int index)
            {
                bw.FillInt32($"VertexBufferOffset{index}", (int)bw.Position - dataOffset);
                bw.FillInt32($"VertexBufferOffset{index}_{0}", (int)bw.Position - dataOffset);

                foreach (FLVER.Vertex vertex in Vertices)
                    vertex.PrepareWrite();

                float uvFactor = 1024;
                if (!bw.BigEndian)
                    uvFactor = 2048;

                foreach (FLVER.Vertex vertex in Vertices)
                    vertex.Write(bw, flv.Materials[MaterialIndex].Layouts[LayoutIndex], uvFactor);

                foreach (FLVER.Vertex vertex in Vertices)
                    vertex.FinishWrite();
            }

            /// <summary>
            /// Get a list of faces as index arrays.
            /// </summary>
            /// <param name="version">The FLVER version.</param>
            /// <param name="doCheckFlip">Whether or not to do the check flip fix.</param>
            /// <param name="includeDegenerateFaces">Whether or not to include degenerate faces.</param>
            public List<int[]> GetFaceIndices(int version, bool doCheckFlip, bool includeDegenerateFaces)
            {
                List<int> indices = Triangulate(version, doCheckFlip, includeDegenerateFaces);
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

            /// <summary>
            /// Get an approximate triangle count for the mesh indices.
            /// </summary>
            /// <param name="version">The FLVER version.</param>
            /// <param name="includeDegenerateFaces">Whether or not to include degenerate faces.</param>
            /// <returns>An approximate triangle count.</returns>
            public int GetFaceCount(int version, bool includeDegenerateFaces)
            {
                if (version >= 0x15 && UseTriangleStrips == false)
                {
                    // No triangle strip
                    var alignedValue = Indices.Count + (3 - (Indices.Count % 3));
                    return alignedValue / 3;
                }

                // Triangle strip
                int counter = 0;
                for (int i = 0; i < Indices.Count - 2; i++)
                {
                    int vi1 = Indices[i];
                    int vi2 = Indices[i + 1];
                    int vi3 = Indices[i + 2];

                    bool notRestart = vi1 != 0xFFFF && vi2 != 0xFFFF && vi3 != 0xFFFF;
                    bool included = includeDegenerateFaces || (vi1 != vi2 && vi1 != vi3 && vi2 != vi3);
                    if (notRestart && included)
                    {
                        counter++;
                    }
                }

                return counter;
            }

            /// <summary>
            /// Triangulate the mesh face indices.
            /// </summary>
            /// <param name="version">The FLVER version.</param>
            /// <param name="doCheckFlip">Whether or not to do the check flip fix.</param>
            /// <param name="includeDegenerateFaces">Whether or not to include degenerate faces.</param>
            /// <returns>A list of triangulated mesh face indices.</returns>
            public List<int> Triangulate(int version, bool doCheckFlip, bool includeDegenerateFaces)
            {
                if (version >= 0x15 && UseTriangleStrips == false)
                {
                    return new List<int>(Indices);
                }
                else
                {
                    var triangles = new List<int>();
                    bool checkFlip = false;
                    bool flip = false;
                    for (int i = 0; i < Indices.Count - 2; i++)
                    {
                        int vi1 = Indices[i];
                        int vi2 = Indices[i + 1];
                        int vi3 = Indices[i + 2];

                        if (vi1 == 0xFFFF || vi2 == 0xFFFF || vi3 == 0xFFFF)
                        {
                            checkFlip = true;
                            flip = false;
                        }
                        else
                        {
                            if (includeDegenerateFaces || (vi1 != vi2 && vi1 != vi3 && vi2 != vi3))
                            {
                                // Every time the triangle strip restarts, compare the average vertex normal to the face normal
                                // and flip the starting direction if they're pointing away from each other.
                                // I don't know why this is necessary; in most models they always restart with the same orientation
                                // as you'd expect. But on some, I can't discern any logic to it, thus this approach.
                                // It's probably hideously slow because I don't know anything about math.
                                // Feel free to hit me with a PR. :slight_smile:

                                // Some ACFA map model faces will mess up using this, so an argument has been added to disable it
                                if (doCheckFlip && checkFlip)
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

                    return triangles;
                }
            }

            /// <summary>
            /// Auto detect the vertex index size.
            /// </summary>
            /// <returns>The vertex index size in bits.</returns>
            public int GetVertexIndexSize()
            {
                foreach (int index in Indices)
                    if (index > ushort.MaxValue + 1)
                        return 32;
                return 16;
            }
        }
    }
}
