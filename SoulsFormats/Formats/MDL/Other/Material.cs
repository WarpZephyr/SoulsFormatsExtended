namespace SoulsFormats.Other
{
    public partial class MDL : SoulsFile<MDL>
    {
        /// <summary>
        /// A material containing parameters for various material related things.
        /// </summary>
        public class Material
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk04;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk08;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk0C;

            /// <summary>
            /// The texture index for the texture of this material.
            /// </summary>
            public int TextureIndex;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk14;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk18;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk1C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk20;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk24;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk28;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk2C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk30;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk34;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk38;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk3C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk40;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk44;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk48;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk4C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk60;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk64;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk68;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk6C;

            internal Material(BinaryReaderEx br)
            {
                br.AssertInt32(0);
                Unk04 = br.ReadInt32();
                Unk08 = br.ReadInt32();
                Unk0C = br.ReadInt32();
                TextureIndex = br.ReadInt32();
                Unk14 = br.ReadInt32();
                Unk18 = br.ReadInt32();
                Unk1C = br.ReadInt32();
                Unk20 = br.ReadSingle();
                Unk24 = br.ReadSingle();
                Unk28 = br.ReadSingle();
                Unk2C = br.ReadSingle();
                Unk30 = br.ReadSingle();
                Unk34 = br.ReadSingle();
                Unk38 = br.ReadSingle();
                Unk3C = br.ReadSingle();
                Unk40 = br.ReadSingle();
                Unk44 = br.ReadSingle();
                Unk48 = br.ReadSingle();
                Unk4C = br.ReadSingle();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
                Unk60 = br.ReadSingle();
                Unk64 = br.ReadSingle();
                Unk68 = br.ReadSingle();
                Unk6C = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteInt32(0);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(Unk08);
                bw.WriteInt32(Unk0C);
                bw.WriteInt32(TextureIndex);
                bw.WriteInt32(Unk14);
                bw.WriteInt32(Unk18);
                bw.WriteInt32(Unk1C);
                bw.WriteSingle(Unk20);
                bw.WriteSingle(Unk24);
                bw.WriteSingle(Unk28);
                bw.WriteSingle(Unk2C);
                bw.WriteSingle(Unk30);
                bw.WriteSingle(Unk34);
                bw.WriteSingle(Unk38);
                bw.WriteSingle(Unk3C);
                bw.WriteSingle(Unk40);
                bw.WriteSingle(Unk44);
                bw.WriteSingle(Unk48);
                bw.WriteSingle(Unk4C);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteSingle(Unk60);
                bw.WriteSingle(Unk64);
                bw.WriteSingle(Unk68);
                bw.WriteSingle(Unk6C);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
            }
        }
    }
}
