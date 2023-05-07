using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsFormats
{
    /// <summary>
    /// A 3D model format used in early PS3/X360 games. Extension: .mdl
    /// </summary>
    public partial class MDL4
    {
        /// <summary>
        /// An individual chunk of a model.
        /// </summary>
        public class Mesh
        {
            /// <summary>
            /// Determines vertex size and if UnkBlocks is present.
            /// </summary>
            public byte VertexFormat;

            /// <summary>
            /// Index of the material used by all triangles in this mesh.
            /// </summary>
            public byte MaterialIndex;

            /// <summary>
            /// Unknown.
            /// </summary>
            public bool Unk02;

            /// <summary>
            /// Unknown.
            /// </summary>
            public bool Unk03;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk08;

            /// <summary>
            /// Indexes of bones in the bone collection which may be used by vertices in this mesh.
            /// </summary>
            public short[] BoneIndices;

            /// <summary>
            /// Indexes of the vertices of this mesh.
            /// </summary>
            public ushort[] VertexIndices;

            /// <summary>
            /// Vertices in this mesh.
            /// </summary>
            public List<Vertex> Vertices;

            /// <summary>
            /// Unknown; Only present if VertexFormat is 2.
            /// </summary>
            public byte[][] UnkBlocks;

            /// <summary>
            /// Reads a mesh from a BinaryReaderEx.
            /// </summary>
            internal Mesh(BinaryReaderEx br, int dataOffset, int version)
            {
                VertexFormat = br.AssertByte(0, 1, 2);
                MaterialIndex = br.ReadByte();
                Unk02 = br.ReadBoolean();
                Unk03 = br.ReadBoolean();
                ushort vertexIndexCount = br.ReadUInt16();
                Unk08 = br.ReadInt16();
                BoneIndices = br.ReadInt16s(28);
                int vertexIndicesLength = br.ReadInt32();
                int vertexIndicesOffset = br.ReadInt32();
                int bufferLength = br.ReadInt32();
                int bufferOffset = br.ReadInt32();

                if (VertexFormat == 2)
                {
                    UnkBlocks = new byte[16][];
                    for (int i = 0; i < 16; i++)
                    {
                        int length = br.ReadInt32();
                        int offset = br.ReadInt32();
                        UnkBlocks[i] = br.GetBytes(dataOffset + offset, length);
                    }
                }

                VertexIndices = br.GetUInt16s(dataOffset + vertexIndicesOffset, vertexIndexCount);

                br.StepIn(dataOffset + bufferOffset);
                {
                    int vertexSize = 0;
                    if (version == 0x40001)
                    {
                        if (VertexFormat == 0)
                            vertexSize = 0x40;
                        else if (VertexFormat == 1)
                            vertexSize = 0x54;
                        else if (VertexFormat == 2)
                            vertexSize = 0x3C;
                    }
                    else if (version == 0x40002)
                    {
                        if (VertexFormat == 0)
                            vertexSize = 0x28;
                    }
                    int vertexCount = bufferLength / vertexSize;
                    Vertices = new List<Vertex>(vertexCount);
                    for (int i = 0; i < vertexCount; i++)
                        Vertices.Add(new Vertex(br, version, VertexFormat));
                }
                br.StepOut();
            }

            /// <summary>
            /// Writes a mesh to a BinaryWriterEx.
            /// </summary>
            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteByte(VertexFormat);
                bw.WriteByte(MaterialIndex);
                bw.WriteBoolean(Unk02);
                bw.WriteBoolean(Unk03);
                bw.WriteUInt16((ushort)VertexIndices.Length); // Vertex Index Count
                bw.WriteInt16(Unk08);
                bw.WriteInt16s(BoneIndices);
                bw.ReserveInt32($"VertexIndicesLength_{index}");
                bw.ReserveInt32($"VertexIndicesOffset_{index}");
                bw.ReserveInt32($"BufferLength_{index}");
                bw.ReserveInt32($"BufferOffset_{index}");
            }

            /// <summary>
            /// Gets a list of the faces used by this mesh as a list of vertex arrays.
            /// </summary>
            public List<Vertex[]> GetFaces()
            {
                ushort[] indices = ToTriangleList();
                var faces = new List<Vertex[]>();
                for (int i = 0; i < indices.Length; i += 3)
                {
                    faces.Add(new Vertex[]
                    {
                        Vertices[indices[i + 0]],
                        Vertices[indices[i + 1]],
                        Vertices[indices[i + 2]],
                    });
                }
                return faces;
            }

            /// <summary>
            /// Gets a list of the indices of faces used by this mesh as a list of int arrays.
            /// </summary>
            public List<int[]> GetFaceVertexIndices()
            {
                ushort[] indices = ToTriangleList();
                var faces = new List<int[]>();
                for (int i = 0; i < indices.Length; i += 3)
                {
                    faces.Add(new int[]
                    {
                        indices[i + 0],
                        indices[i + 1],
                        indices[i + 2],
                    });
                }
                return faces;
            }

            /// <summary>
            /// Gets the indices of triangles in this mesh as an unsigned short array.
            /// </summary>
            public ushort[] ToTriangleList()
            {
                var converted = new List<ushort>();
                bool flip = false;
                for (int i = 0; i < VertexIndices.Length - 2; i++)
                {
                    ushort vi1 = VertexIndices[i];
                    ushort vi2 = VertexIndices[i + 1];
                    ushort vi3 = VertexIndices[i + 2];

                    if (vi1 == 0xFFFF || vi2 == 0xFFFF || vi3 == 0xFFFF)
                    {
                        flip = false;
                    }
                    else
                    {
                        if (vi1 != vi2 && vi1 != vi3 && vi2 != vi3)
                        {
                            if (!flip)
                            {
                                converted.Add(vi1);
                                converted.Add(vi2);
                                converted.Add(vi3);
                            }
                            else
                            {
                                converted.Add(vi3);
                                converted.Add(vi2);
                                converted.Add(vi1);
                            }
                        }
                        flip = !flip;
                    }
                }
                return converted.ToArray();
            }
        }
    }
}
