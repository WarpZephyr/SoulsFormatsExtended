using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class SMD4
    {
        /// <summary>
        /// An individual chunk of a model.
        /// </summary>
        public class Mesh
        {
            /// <summary>
            /// The format of vertices in the vertex buffer.
            /// </summary>
            public byte VertexFormat { get; set; }

            /// <summary>
            /// Index of the material used by all triangles in this mesh.
            /// </summary>
            public byte MaterialIndex { get; set; }

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
            public short[] BoneIndices { get; set; }

            /// <summary>
            /// The vertex indices in this mesh.
            /// </summary>
            public List<ushort> VertexIndices { get; set; }

            /// <summary>
            /// The vertices in this mesh.
            /// </summary>
            public List<Vertex> Vertices { get; set; }

            /// <summary>
            /// Create a new and empty Mesh with default values.
            /// </summary>
            public Mesh()
            {
                VertexFormat = 0;
                MaterialIndex = 0;
                CullBackfaces = true;
                UseTriangleStrips = false;
                DefaultBoneIndex = 0;
                BoneIndices = new short[28];
                VertexIndices = new List<ushort>();
                Vertices = new List<Vertex>();
                for (int i = 0; i < 28; i++)
                    BoneIndices[i] = -1;
            }

            /// <summary>
            /// Clone an existing Mesh.
            /// </summary>
            public Mesh(Mesh mesh)
            {
                VertexFormat = mesh.VertexFormat;
                MaterialIndex = mesh.MaterialIndex;
                CullBackfaces = mesh.CullBackfaces;
                UseTriangleStrips = mesh.UseTriangleStrips;
                DefaultBoneIndex = mesh.DefaultBoneIndex;
                BoneIndices = new short[28];
                VertexIndices = new List<ushort>();
                Vertices = new List<Vertex>();
                for (int i = 0; i < 28; i++)
                    BoneIndices[i] = mesh.BoneIndices[i];
                for (int i = 0; i < mesh.VertexIndices.Count; i++)
                    VertexIndices[i] = mesh.VertexIndices[i];
                for (int i = 0; i < mesh.Vertices.Count; i++)
                    Vertices[i] = new Vertex(mesh.Vertices[i]);
            }

            /// <summary>
            /// Read a new Mesh from a stream.
            /// </summary>
            internal Mesh(BinaryReaderEx br, int dataOffset, int version)
            {
                VertexFormat = br.ReadByte();
                MaterialIndex = br.ReadByte();
                CullBackfaces = br.ReadBoolean();
                UseTriangleStrips = br.ReadBoolean();
                ushort vertexIndexCount = br.ReadUInt16();
                DefaultBoneIndex = br.ReadInt16();
                BoneIndices = br.ReadInt16s(28);
                int vertexIndicesLength = br.AssertInt32(vertexIndexCount * 2);
                int vertexIndicesOffset = br.ReadInt32();
                int vertexBufferLength = br.ReadInt32();
                int vertexBufferOffset = br.ReadInt32();

                VertexIndices = new List<ushort>();
                Vertices = new List<Vertex>();

                VertexIndices.AddRange(br.GetUInt16s(dataOffset + vertexIndicesOffset, vertexIndexCount));

                br.StepIn(dataOffset + vertexBufferOffset);
                int vertexCount = vertexBufferLength / GetVertexSize(version);
                for (int i = 0; i < vertexCount; i++)
                {
                    Vertices.Add(new Vertex(br, version, VertexFormat));
                }
                br.StepOut();
            }

            /// <summary>
            /// Write this Mesh to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw, int index, int version)
            {
                bw.WriteByte(VertexFormat);
                bw.WriteByte(MaterialIndex);
                bw.WriteBoolean(CullBackfaces);
                bw.WriteBoolean(UseTriangleStrips);
                bw.WriteInt16((short)VertexIndices.Count);
                bw.WriteInt16(DefaultBoneIndex);
                bw.WriteInt16s(BoneIndices);
                bw.WriteInt32(VertexIndices.Count * 2);
                bw.ReserveInt32($"vertexIndicesOffset_{index}");
                bw.WriteInt32(Vertices.Count * GetVertexSize(version));
                bw.ReserveInt32($"vertexBufferOffset_{index}");
            }

            /// <summary>
            /// Get the size of each Vertex.
            /// </summary>
            internal int GetVertexSize(int version)
            {
                if (version == 0x40001)
                {
                    if (VertexFormat == 0)
                    {
                        return 16;
                    }
                    else if (VertexFormat == 2)
                    {
                        return 36;
                    }
                    else
                    {
                        throw new NotSupportedException($"VertexFormat {VertexFormat} is not currently supported for Version {version}.");
                    }
                }
                else
                {
                    throw new NotSupportedException($"Version {version} is not currently supported.");
                }
            }

            /// <summary>
            /// Get the calculated face count from the VertexIndices of this Mesh.
            /// </summary>
            public int GetFaceCount()
            {
                return new TriangleStripCollection(VertexIndices, 65535).FaceCount;
            }

            /// <summary>
            /// Get the calculated strip count from the VertexIndices of this Mesh.
            /// </summary>
            public int GetStripCount()
            {
                return new TriangleStripCollection(VertexIndices, 65535).StripCount;
            }

            /// <summary>
            /// Get a list of faces as index arrays.
            /// </summary>
            public List<int[]> GetFaceIndices()
            {
                return new TriangleStripCollection(VertexIndices, 65535).GetFaceIndices();
            }

            /// <summary>
            /// Get a list of faces as index arrays.
            /// </summary>
            public List<ushort[]> GetFaceIndicesUShort()
            {
                return new TriangleStripCollection(VertexIndices, 65535).GetFaceIndicesUShort();
            }

            /// <summary>
            /// Get a list of all indices.
            /// </summary>
            public List<int> GetIndices()
            {
                return new TriangleStripCollection(VertexIndices, 65535).GetIndices();
            }

            /// <summary>
            /// Get a list of indices.
            /// </summary>
            public List<ushort> GetIndicesUShort()
            {
                return new TriangleStripCollection(VertexIndices, 65535).GetIndicesUShort();
            }

            /// <summary>
            /// Gets a list of the faces used by this mesh as a list of vertex arrays.
            /// </summary>
            public List<Vertex[]> GetFaces()
            {
                List<ushort> indices = GetIndicesUShort();
                var faces = new List<Vertex[]>();
                for (int i = 0; i < indices.Count; i += 3)
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
        }
    }
}
