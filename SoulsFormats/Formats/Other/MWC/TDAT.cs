using System.Collections.Generic;

namespace SoulsFormats.MWC
{
    /// <summary>
    /// Texture container used in Metal Wolf Chaos. Extension: _t.dat
    /// </summary>
    public class TDAT : SoulsFile<TDAT>
    {
        /// <summary>
        /// Unknown, seen as 0 or 2.
        /// </summary>
        public int Unk1C;

        /// <summary>
        /// The textures within this TDAT.
        /// </summary>
        public List<Texture> Textures;

        /// <summary>
        /// Deserialize file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            br.ReadInt32(); // File size
            br.AssertPattern(0xC, 0x00);
            int textureCount = br.ReadInt32();
            br.AssertPattern(0x8, 0x00);
            Unk1C = br.ReadInt32();

            Textures = new List<Texture>(textureCount);
            for (int i = 0; i < textureCount; i++)
                Textures.Add(new Texture(br));
        }

        /// <summary>
        /// Serialize file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            bw.WriteInt32((int)bw.Length);
            bw.WritePattern(0xC, 0x00);
            bw.WriteInt32(Textures.Count);
            bw.WritePattern(0x8, 0x00);
            bw.WriteInt32(Unk1C);

            for (int i = 0; i < Textures.Count; i++)
                Textures[i].Write(bw, i);

            for (int i = 0; i < Textures.Count; i++)
            {
                bw.FillInt32($"Texture_Name_Offset_{i}", (int)bw.Position);
                bw.WriteShiftJIS(Textures[i].Name, true);
            }

            for (int i = 0; i < Textures.Count; i++)
            {
                bw.FillInt32($"Texture_Data_Offset_{i}", (int)bw.Position);
                bw.WriteBytes(Textures[i].Data);
            }
        }

        /// <summary>
        /// A texture in a TDAT archive.
        /// </summary>
        public class Texture
        {
            /// <summary>
            /// The name of the texture.
            /// </summary>
            public string Name;

            /// <summary>
            /// The data of the texture.
            /// </summary>
            public byte[] Data;

            /// <summary>
            /// Deserialize a texture from a stream.
            /// </summary>
            /// <param name="br">A BinaryReaderEx.</param>
            internal Texture(BinaryReaderEx br)
            {
                int dataLength = br.ReadInt32();
                int dataOffset = br.ReadInt32();
                int nameOffset = br.ReadInt32();

                Name = br.GetShiftJIS(nameOffset);
                Data = br.GetBytes(dataOffset, dataLength);
            }

            /// <summary>
            /// Serialize a texture to a stream.
            /// </summary>
            /// <param name="bw">A BinaryWriterEx.</param>
            /// <param name="index">The index of the texture for offset reservation purposes.</param>
            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteInt32(Data.Length);
                bw.ReserveInt32($"Texture_Data_Offset_{index}");
                bw.ReserveInt32($"Texture_Name_Offset_{index}");
            }
        }
    }
}
