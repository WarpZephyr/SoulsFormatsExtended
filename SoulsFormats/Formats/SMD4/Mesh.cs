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
            /// When 1, mesh is in bind pose; when 0, it isn't. Most likely has further implications.
            /// </summary>
            public byte Dynamic { get; set; }

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
            public byte TriangleStrip { get; set; }

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
            /// Create a new Mesh with default values.
            /// </summary>
            public Mesh()
            {
                Dynamic = 0;
                MaterialIndex = 0;
                CullBackfaces = true;
                TriangleStrip = 0;
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
                Dynamic = mesh.Dynamic;
                MaterialIndex = mesh.MaterialIndex;
                CullBackfaces = mesh.CullBackfaces;
                TriangleStrip = mesh.TriangleStrip;
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

            internal Mesh(BinaryReaderEx br, int dataOffset)
            {
                Dynamic = br.ReadByte();
                MaterialIndex = br.ReadByte();
                CullBackfaces = br.ReadBoolean();
                TriangleStrip = br.ReadByte();
                short vertexIndexCount = br.ReadInt16();
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
                int vertexCount = vertexBufferLength / 16;
                for (int i = 0; i < vertexCount; i++)
                {
                    Vertices.Add(new Vertex(br));
                }
                br.StepOut();
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteByte(Dynamic);
                bw.WriteByte(MaterialIndex);
                bw.WriteBoolean(CullBackfaces);
                bw.WriteByte(TriangleStrip);
                bw.WriteInt16((short)VertexIndices.Count);
                bw.WriteInt16(DefaultBoneIndex);
                bw.WriteInt16s(BoneIndices);
                bw.WriteInt32(VertexIndices.Count * 2);
                bw.ReserveInt32($"vertexIndicesOffset_{index}");
                bw.WriteInt32(Vertices.Count * 16);
                bw.ReserveInt32($"vertexBufferOffset_{index}");
            }

            /// <summary>
            /// Get the calculated strip count from the VertexIndices of this Mesh.
            /// </summary>
            public int GetStripCount()
            {
                return new TriangleStripCollection(VertexIndices, 65535).StripCount;
            }

            /// <summary>
            /// Get the calculated face count from the VertexIndices of this Mesh.
            /// </summary>
            public int GetFaceCount()
            {
                return new TriangleStripCollection(VertexIndices, 65535).FaceCount;
            }

            /// <summary>
            /// Get a list of faces containing vertex indices.
            /// </summary>
            public List<int[]> GetFaces()
            {
                return new TriangleStripCollection(VertexIndices, 65535).GetFaces();
            }
        }
    }
}
