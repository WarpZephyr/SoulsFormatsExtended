using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats.Other
{
    public partial class MDL : SoulsFile<MDL>
    {
        /// <summary>
        /// Determines how vertices in a mesh are connected to form triangles.
        /// </summary>
        public class Faceset
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            /// <summary>
            /// Unknown; A material index.
            /// </summary>
            public byte MaterialIndex { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk01 { get; set; }

            /// <summary>
            /// The number of vertices in this faceset.
            /// </summary>
            public short VertexCount { get; set; }

            /// <summary>
            /// The number of indices in this faceset.
            /// </summary>
            public int IndexCount { get; set; }

            /// <summary>
            /// The start vertex.
            /// </summary>
            public int StartVertex { get; set; }

            /// <summary>
            /// The start index.
            /// </summary>
            public int StartIndex { get; set; }

            internal Faceset(BinaryReaderEx br)
            {
                MaterialIndex = br.ReadByte();
                Unk01 = br.ReadByte();
                VertexCount = br.ReadInt16();
                IndexCount = br.ReadInt32();
                StartVertex = br.ReadInt32();
                StartIndex = br.ReadInt32();
            }
        }

        public class FacesetC
        {
            public List<Faceset> Facesets;
            public byte IndexCount;
            public byte Unk03;
            public short[] Indices;

            internal FacesetC(BinaryReaderEx br)
            {
                short facesetCount = br.ReadInt16();
                IndexCount = br.ReadByte();
                Unk03 = br.ReadByte();
                int facesetsOffset = br.ReadInt32();
                Indices = br.ReadInt16s(8);

                br.StepIn(facesetsOffset);
                {
                    Facesets = new List<Faceset>(facesetCount);
                    for (int i = 0; i < facesetCount; i++)
                        Facesets.Add(new Faceset(br));
                }
                br.StepOut();
            }
        }

        public List<Vertex[]> GetFaces(Faceset faceset, List<Vertex> vertices)
        {
            List<ushort> indices = Triangulate(faceset, vertices);
            var faces = new List<Vertex[]>();
            for (int i = 0; i < indices.Count; i += 3)
            {
                faces.Add(new Vertex[]
                {
                    vertices[indices[i + 0]],
                    vertices[indices[i + 1]],
                    vertices[indices[i + 2]],
                });
            }
            return faces;
        }

        public List<ushort> Triangulate(Faceset faceset, List<Vertex> vertices)
        {
            bool flip = false;
            var triangles = new List<ushort>();
            for (int i = faceset.StartIndex; i < faceset.StartIndex + faceset.IndexCount - 2; i++)
            {
                ushort vi1 = Indices[i];
                ushort vi2 = Indices[i + 1];
                ushort vi3 = Indices[i + 2];

                if (vi1 == 0xFFFF || vi2 == 0xFFFF || vi3 == 0xFFFF)
                {
                    flip = false;
                }
                else
                {
                    if (vi1 != vi2 && vi1 != vi3 && vi2 != vi3)
                    {
                        Vertex v1 = vertices[vi1];
                        Vertex v2 = vertices[vi2];
                        Vertex v3 = vertices[vi3];
                        Vector3 vertexNormal = Vector3.Normalize((v1.Normal + v2.Normal + v3.Normal) / 3);
                        Vector3 faceNormal = Vector3.Normalize(Vector3.Cross(v2.Position - v1.Position, v3.Position - v1.Position));
                        float angle = Vector3.Dot(faceNormal, vertexNormal) / (faceNormal.Length() * vertexNormal.Length());
                        flip = angle <= 0;

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
