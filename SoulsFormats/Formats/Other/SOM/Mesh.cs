using System.Collections.Generic;

namespace SoulsFormats.SOM
{
    public partial class MDO
    {
        /// <summary>
        /// An individual chunk of a model.
        /// </summary>
        public class Mesh
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk00;

            /// <summary>
            /// The index of the texture this mesh uses.
            /// </summary>
            public short TextureIndex;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk06;

            /// <summary>
            /// The vertex indices in this mesh.
            /// </summary>
            public ushort[] VertexIndices;

            /// <summary>
            /// The vertices in this mesh.
            /// </summary>
            public List<Vertex> Vertices;

            internal Mesh(BinaryReaderEx br)
            {
                Unk00 = br.ReadInt32();
                TextureIndex = br.ReadInt16();
                Unk06 = br.ReadInt16();
                ushort indexCount = br.ReadUInt16();
                ushort vertexCount = br.ReadUInt16();
                uint indicesOffset = br.ReadUInt32();
                uint verticesOffset = br.ReadUInt32();

                VertexIndices = br.GetUInt16s(indicesOffset, indexCount);

                br.StepIn(verticesOffset);
                {
                    Vertices = new List<Vertex>(vertexCount);
                    for (int i = 0; i < vertexCount; i++)
                        Vertices.Add(new Vertex(br));
                }
                br.StepOut();
            }

            /// <summary>
            /// Get a list of faces from this mesh.
            /// </summary>
            /// <returns></returns>
            public List<Vertex[]> GetFaces()
            {
                var faces = new List<Vertex[]>();
                for (int i = 0; i < VertexIndices.Length; i += 3)
                {
                    faces.Add(new Vertex[]
                    {
                        Vertices[VertexIndices[i + 0]],
                        Vertices[VertexIndices[i + 1]],
                        Vertices[VertexIndices[i + 2]],
                    });
                }
                return faces;
            }
        }
    }
}
