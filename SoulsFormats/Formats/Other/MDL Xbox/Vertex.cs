using System;
using System.Drawing;
using System.Numerics;

namespace SoulsFormats.Other
{
    public partial class MDL : SoulsFile<MDL>
    {
        /// <summary>
        /// The format of the vertex.
        /// </summary>
        public enum VertexFormat
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            A,

            /// <summary>
            /// Unknown.
            /// </summary>
            B,

            /// <summary>
            /// Unknown.
            /// </summary>
            C
        }

        /// <summary>
        /// A single point in a mesh.
        /// </summary>
        public class Vertex
        {
            /// <summary>
            /// Where the vertex is.
            /// </summary>
            public virtual Vector3 Position { get; set; }

            /// <summary>
            /// Vector pointing away from the surface.
            /// </summary>
            public virtual Vector3 Normal { get; set; }

            /// <summary>
            /// Vector pointing perpendicular to the normal.
            /// </summary>
            public virtual Vector3 Tangent { get; set; }

            /// <summary>
            /// Vector pointing perpendicular to the normal and tangent.
            /// </summary>
            public virtual Vector3 Bitangent { get; set; }

            /// <summary>
            /// Data used for alpha, blending, etc.
            /// </summary>
            public Color Color;

            /// <summary>
            /// Texture coordinates of the vertex.
            /// </summary>
            public Vector2[] UVs;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short UnkShortA;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short UnkShortB;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float UnkFloatA;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float UnkFloatB;

            /// <summary>
            /// Creates a new vertex with new UVs.
            /// </summary>
            public Vertex()
            {
                UVs = new Vector2[4];
            }

            internal Vertex(BinaryReaderEx br, VertexFormat format)
            {
                Position = br.ReadVector3();
                Normal = Read11_11_10Vector3(br);
                Tangent = Read11_11_10Vector3(br);
                Bitangent = Read11_11_10Vector3(br);
                Color = br.ReadRGBA();

                UVs = new Vector2[4];
                for (int i = 0; i < 4; i++)
                    UVs[i] = br.ReadVector2();

                if (format >= VertexFormat.B)
                {
                    // Both may be 0, 4, 8, 12, etc
                    UnkShortA = br.ReadInt16();
                    UnkShortB = br.ReadInt16();
                }

                if (format >= VertexFormat.C)
                {
                    UnkFloatA = br.ReadSingle();
                    UnkFloatB = br.ReadSingle();
                }
            }

            internal void Write(BinaryWriterEx bw, VertexFormat format)
            {
                bw.WriteVector3(Position);
                Write11_11_10Vector3(bw, Normal);
                Write11_11_10Vector3(bw, Tangent);
                Write11_11_10Vector3(bw, Bitangent);
                bw.WriteRGBA(Color);

                for (int i = 0; i < 4; i++)
                    bw.WriteVector2(UVs[i]);

                if (format >= VertexFormat.B)
                {
                    // Both may be 0, 4, 8, 12, etc
                    bw.WriteInt16(UnkShortA);
                    bw.WriteInt16(UnkShortB);
                }

                if (format >= VertexFormat.C)
                {
                    bw.WriteSingle(UnkFloatA);
                    bw.WriteSingle(UnkFloatB);
                }
            }
        }

        /// <summary>
        /// Unknown.
        /// </summary>
        public class VertexD : Vertex
        {
            /// <summary>
            /// Where the vertex is.
            /// </summary>
            public Vector3[] Positions;

            /// <summary>
            /// Where the vertex is.
            /// </summary>
            public override Vector3 Position
            {
                get => Positions[0];
                set => Positions[0] = value;
            }

            /// <summary>
            /// Vector pointing away from the surface.
            /// </summary>
            public Vector3[] Normals;

            /// <summary>
            /// Vector pointing away from the surface.
            /// </summary>
            public override Vector3 Normal
            {
                get => Normals[0];
                set => Normals[0] = value;
            }

            /// <summary>
            /// Vector pointing perpendicular to the normal.
            /// </summary>
            public Vector3[] Tangents;

            /// <summary>
            /// Vector pointing perpendicular to the normal.
            /// </summary>
            public override Vector3 Tangent
            {
                get => Tangents[0];
                set => Tangents[0] = value;
            }

            /// <summary>
            /// Vector pointing perpendicular to the normal and tangent.
            /// </summary>
            public Vector3[] Bitangents;

            /// <summary>
            /// Vector pointing perpendicular to the normal and tangent.
            /// </summary>
            public override Vector3 Bitangent
            {
                get => Bitangents[0];
                set => Bitangents[0] = value;
            }

            internal VertexD(BinaryReaderEx br)
            {
                Positions = new Vector3[16];
                for (int i = 0; i < 16; i++)
                    Positions[i] = br.ReadVector3();

                Normals = new Vector3[16];
                for (int i = 0; i < 16; i++)
                    Normals[i] = Read11_11_10Vector3(br);

                Tangents = new Vector3[16];
                for (int i = 0; i < 16; i++)
                    Tangents[i] = Read11_11_10Vector3(br);

                Bitangents = new Vector3[16];
                for (int i = 0; i < 16; i++)
                    Bitangents[i] = Read11_11_10Vector3(br);

                Color = br.ReadRGBA();

                UVs = new Vector2[4];
                for (int i = 0; i < 4; i++)
                    UVs[i] = br.ReadVector2();

                UnkShortA = br.ReadInt16();
                UnkShortB = br.ReadInt16();
                UnkFloatA = br.ReadSingle();
                UnkFloatB = br.ReadSingle();
            }

            internal void Write(BinaryWriterEx bw)
            {
                for (int i = 0; i < 16; i++)
                    bw.WriteVector3(Positions[i]);

                for (int i = 0; i < 16; i++)
                    Write11_11_10Vector3(bw, Normals[i]);

                for (int i = 0; i < 16; i++)
                    Write11_11_10Vector3(bw, Tangents[i]);

                for (int i = 0; i < 16; i++)
                    Write11_11_10Vector3(bw, Bitangents[i]);

                bw.WriteRGBA(Color);

                for (int i = 0; i < 4; i++)
                    bw.WriteVector2(UVs[i]);

                bw.WriteInt16(UnkShortA);
                bw.WriteInt16(UnkShortB);
                bw.WriteSingle(UnkFloatA);
                bw.WriteSingle(UnkFloatB);
            }
        }

        /// <summary>
        /// Unknown.
        /// </summary>
        public class Struct7
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk00;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk04;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk08;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk0C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk10;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk14;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk18;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk1C;

            internal Struct7(BinaryReaderEx br)
            {
                Unk00 = br.ReadSingle();
                Unk04 = br.ReadSingle();
                Unk08 = br.ReadSingle();
                Unk0C = br.ReadSingle();
                Unk10 = br.ReadSingle();
                Unk14 = br.ReadSingle();
                Unk18 = br.ReadInt32();
                Unk1C = br.ReadInt32();
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteSingle(Unk00);
                bw.WriteSingle(Unk04);
                bw.WriteSingle(Unk08);
                bw.WriteSingle(Unk0C);
                bw.WriteSingle(Unk10);
                bw.WriteSingle(Unk14);
                bw.WriteInt32(Unk18);
                bw.WriteInt32(Unk1C);
            }
        }

        private static Vector3 Read11_11_10Vector3(BinaryReaderEx br)
        {
            int vector = br.ReadInt32();
            int x = vector << 21 >> 21;
            int y = vector << 10 >> 21;
            int z = vector << 0 >> 22;
            return new Vector3(x / (float)0b11_1111_1111, y / (float)0b11_1111_1111, z / (float)0b1_1111_1111);
        }

        private static void Write11_11_10Vector3(BinaryWriterEx bw, Vector3 vector)
        {
            throw new NotImplementedException();
        }
    }
}
