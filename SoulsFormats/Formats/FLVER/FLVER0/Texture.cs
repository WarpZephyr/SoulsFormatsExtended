namespace SoulsFormats
{
    public partial class FLVER0
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class Texture : IFlverTexture
        {
            public string Type { get; set; }

            public string Path { get; set; }

            /// <summary>
            /// Creates a new Texture with null or default values.
            /// </summary>
            public Texture()
            {
                Path = "";
                Type = "";
            }

            internal Texture(BinaryReaderEx br, FLVER0 flv)
            {
                int pathOffset = br.ReadInt32();
                int typeOffset = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);

                Path = flv.Header.Unicode ? br.GetUTF16(pathOffset) : br.GetShiftJIS(pathOffset);
                if (typeOffset > 0)
                    Type = flv.Header.Unicode ? br.GetUTF16(typeOffset) : br.GetShiftJIS(typeOffset);
                else
                    Type = null;
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.ReserveInt32($"TexturePathOffset{index}");
                bw.ReserveInt32($"TextureTypeOffset{index}");
                bw.WriteInt32(0);
                bw.WriteInt32(0);
            }

            internal void WriteStrings(BinaryWriterEx bw, bool Unicode, int index)
            {
                bw.FillInt32($"TexturePathOffset{index}", (int)bw.Position);
                if (Unicode)
                    bw.WriteUTF16(Path, true);
                else
                    bw.WriteShiftJIS(Path, true);

                bw.FillInt32($"TextureTypeOffset{index}", (int)bw.Position);
                if (Unicode)
                    bw.WriteUTF16(Type, true);
                else
                    bw.WriteShiftJIS(Type, true);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
