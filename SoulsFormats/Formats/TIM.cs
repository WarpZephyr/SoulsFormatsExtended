using System;
using System.Drawing;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// The texture format used in PS1 games. Implementation not complete.
    /// </summary>
    public class TIM : SoulsFile<TIM>
    {
        /// <summary>
        /// The different types of TIM.
        /// </summary>
        public enum TIMType : uint
        {
            /// <summary>
            /// 4-bits per pixel with no color lookup table.
            /// </summary>
            BPP4NoCLUT = 0,

            /// <summary>
            /// 8-bits per pixel with no color lookup table.
            /// </summary>
            BPP8NoCLUT = 1,

            /// <summary>
            /// 16-bits per pixel, does not need a color lookup table.
            /// </summary>
            BPP16 = 2,

            /// <summary>
            /// 24-bits per pixel, does not need a color lookup table.
            /// </summary>
            BPP24 = 3,

            /// <summary>
            /// 4-bits per pixel.
            /// </summary>
            BPP4 = 8,

            /// <summary>
            /// 8-bits per pixel.
            /// </summary>
            BPP8 = 9
        }

        /// <summary>
        /// The type, determining how many bits per pixel and if there is a color lookup table.
        /// </summary>
        public TIMType Type { get; set; }

        /// <summary>
        /// Color lookup table X location in memory if one is present.
        /// </summary>
        public ushort CLUT_X { get; set; }

        /// <summary>
        /// Color lookup table Y location in memory if one is present.
        /// </summary>
        public ushort CLUT_Y { get; set; }

        /// <summary>
        /// Image X location in memory.
        /// </summary>
        public ushort ImageX { get; set; }

        /// <summary>
        /// Image Y location in memory.
        /// </summary>
        public ushort ImageY { get; set; }

        /// <summary>
        /// The width of the image.
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// The height of the image.
        /// </summary>
        public ushort Height { get; set; }

        /// <summary>
        /// Gets the bits per pixel depending on the version.
        /// </summary>
        public int BitsPerPixel
        {
            get
            {
                switch (Type)
                {
                    case TIMType.BPP4NoCLUT:
                    case TIMType.BPP4:
                        return 4;
                    case TIMType.BPP8NoCLUT:
                    case TIMType.BPP8:
                        return 8;
                    case TIMType.BPP16:
                        return 16;
                    case TIMType.BPP24:
                        return 24;
                    default:
                        throw new NotSupportedException($"{nameof(Type)} {Type} is not supported for {nameof(BitsPerPixel)}");
                }
            }
        }

        /// <summary>
        /// All of the different palettes in this TIM.
        /// <para>Will be null if not supported.</para>
        /// </summary>
        public Color[,] Palettes { get; set; }

        /// <summary>
        /// Indices to the chosen color palette.
        /// <para>Will be null if not supported.</para>
        /// </summary>
        public byte[] PixelPaletteIndices { get; set; }

        /// <summary>
        /// All of the pixels in this TIM.
        /// <para>Will be set to the first palette if supported.</para>
        /// </summary>
        public Color[,] Pixels { get; set; }

        /// <summary>
        /// Returns true if data appears to be a TIM texture.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length - br.Position < 20)
            {
                return false;
            }

            uint magic = br.ReadUInt32();
            TIM.TIMType type = (TIM.TIMType)br.ReadUInt32();
            bool bTypeAssert = type == TIM.TIMType.BPP8NoCLUT || type == TIM.TIMType.BPP4NoCLUT || type == TIM.TIMType.BPP16 || type == TIM.TIMType.BPP24 || type == TIM.TIMType.BPP4 || type == TIM.TIMType.BPP8;

            return magic == 0x10 && bTypeAssert;
        }

        /// <summary>
        /// Reads a TIM from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            br.AssertUInt32(0x10);
            Type = br.ReadEnum32<TIMType>();

            switch (Type)
            {
                case TIMType.BPP4:
                case TIMType.BPP8:
                    br.ReadInt32(); // CLUT length
                    CLUT_X = br.ReadUInt16();
                    CLUT_Y = br.ReadUInt16();
                    Palettes = ReadPaletteColors(br);
                    break;
                default:
                    PixelPaletteIndices = null;
                    Palettes = null;
                    break;
            }

            int dataLength = br.ReadInt32();
            ImageX = br.ReadUInt16();
            ImageY = br.ReadUInt16();
            Width = GetTrueWidth(br.ReadUInt16());
            Height = br.ReadUInt16();
            Pixels = new Color[Width, Height];

            byte[] values;
            switch (Type)
            {
                case TIMType.BPP4NoCLUT:
                    values = To4Bit(br.ReadBytes(dataLength - 12));
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            byte value = values[x + (y * Width)];
                            Pixels[x, y] = Color.FromArgb(value, value, value);
                        }
                    }
                    break;
                case TIMType.BPP4:
                    values = To4Bit(br.ReadBytes(dataLength - 12));
                    PixelPaletteIndices = values;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            byte value = values[x + (y * Width)];
                            Pixels[x, y] = Palettes[0, value];
                        }
                    }
                    break;
                case TIMType.BPP8NoCLUT:
                    values = br.ReadBytes(dataLength - 12);
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            byte value = values[x + (y * Width)];
                            Pixels[x, y] = Color.FromArgb(value, value, value);
                        }
                    }
                    break;
                case TIMType.BPP8:
                    values = br.ReadBytes(dataLength - 12);
                    PixelPaletteIndices = values;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            Pixels[x, y] = Palettes[0, values[x + (y * Width)]];
                        }
                    }
                    break;
                case TIMType.BPP16:
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            Pixels[x, y] = FromA1B5G5R5(br.ReadUInt16());
                        }
                    }
                    break;
                case TIMType.BPP24:
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            // Is this the correct order?
                            byte blue = br.ReadByte();
                            byte green = br.ReadByte();
                            byte red = br.ReadByte();
                            Pixels[x, y] = Color.FromArgb(red, green, blue);
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException($"{nameof(Type)} {Type} is not supported or implemented.");
            }
        }

        /// <summary>
        /// Writes a TIM to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            throw new NotSupportedException("Writing is not supported for now.");
        }

        private ushort GetTrueWidth(ushort width)
        {
            switch (Type)
            {
                case TIMType.BPP4NoCLUT:
                case TIMType.BPP4:
                    return (ushort)(width * 4);
                case TIMType.BPP8NoCLUT:
                case TIMType.BPP8:
                    return (ushort)(width * 2);
                case TIMType.BPP16:
                    return width;
                case TIMType.BPP24:
                    return (ushort)(width / 2);
                default:
                    throw new NotSupportedException($"{nameof(Type)} {Type} is not supported or implemented.");
            }
        }

        private ushort GetRawWidth()
        {
            switch (Type)
            {
                case TIMType.BPP4NoCLUT:
                case TIMType.BPP4:
                    return (ushort)(Width / 4);
                case TIMType.BPP8NoCLUT:
                case TIMType.BPP8:
                    return (ushort)(Width / 2);
                case TIMType.BPP16:
                    return Width;
                case TIMType.BPP24:
                    return (ushort)(Width * 2);
                default:
                    throw new NotSupportedException($"{nameof(Type)} {Type} is not supported or implemented.");
            }
        }

        private Color[,] ReadPaletteColors(BinaryReaderEx br)
        {
            ushort colorCount = br.ReadUInt16();
            ushort paletteCount = br.ReadUInt16();
            Color[,] palettes = new Color[paletteCount, colorCount];
            for (int i = 0; i < paletteCount; i++)
            {
                for (int j = 0; j < colorCount; j++)
                {
                    palettes[i, j] = FromA1B5G5R5(br.ReadUInt16());
                }
            }
            return palettes;
        }

        /// <summary>
        /// Switches pixels to use colors from another palette by index.
        /// <para>Only supported in versions with a colorlookup table.</para>
        /// </summary>
        /// <param name="index">The index of the palette to use.</param>
        /// <exception cref="InvalidOperationException">The palettes or palette indices were null, meaning the version when read or created did not support them.</exception>
        public void SwitchPalette(int index)
        {
            if (Palettes == null)
            {
                throw new InvalidOperationException($"{nameof(Palettes)} was null.");
            }

            if (PixelPaletteIndices == null)
            {
                throw new InvalidOperationException($"{nameof(PixelPaletteIndices)} was null.");
            }

            if (Pixels.Length != Width * Height)
            {
                throw new InvalidOperationException($"Total pixel count was {Pixels.Length} but {nameof(Width)} and {nameof(Height)} gave a count of {Width}x{Height}={Width * Height}");
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Pixels[x, y] = Palettes[index, PixelPaletteIndices[x + (y * Width)]];
                }
            }
        }

        #region Bit Operations

        internal static Color FromA1B5G5R5(ushort value)
        {
            int STP = ((value & 0b1_00000_00000_00000) >> 15);
            int blue = ((value & 0b0_11111_00000_00000) >> 10) * 8;
            int green = ((value & 0b0_00000_11111_00000) >> 5) * 8;
            int red = (value & 0b0_00000_00000_11111) * 8;

            return Color.FromArgb(red, green, blue);
        }
        internal static ushort ToA1B5G5R5(Color color) => (ushort)((color.A / 255) << 15 | (color.B / 8) << 10 | (color.G / 8) << 5 | (color.R / 8));

        private static byte[] To4Bit(byte[] bytes)
        {
            byte[] output = new byte[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                To4Bit(bytes[i], out byte high, out byte low);
                output[i] = high;
                output[i + 1] = low;
            }
            return output;
        }

        private static void To4Bit(byte input, out byte high, out byte low)
        {
            high = (byte)(input & 0x0F);
            low = (byte)((input & 0xF0) >> 4);
        }

        private static byte[] To8Bit(byte[] bytes)
        {
            byte[] input;
            if (bytes.Length % 2 != 0)
            {
                input = new byte[bytes.Length + 1];
                Array.Copy(bytes, input, bytes.Length);
                input[bytes.Length] = 0;
            }
            else
            {
                input = bytes;
            }

            byte[] output = new byte[input.Length / 2];

            int inputIndex = 0;
            for (int i = 0; i < output.Length; i++)
            {
                byte high = input[inputIndex];
                byte low = input[inputIndex + 1];
                if (high > 15)
                {
                    throw new InvalidDataException($"Byte value of {input[inputIndex]} was too large to pack into 4 bits.");
                }

                if (low > 15)
                {
                    throw new InvalidDataException($"Byte value of {input[inputIndex + 1]} was too large to pack into 4 bits.");
                }

                output[i] = To8Bit(high, low);
                inputIndex += 2;
            }

            return output;
        }

        private static byte To8Bit(byte high, byte low) => (byte)((high << 4) | (low & 0b00001111));

        #endregion

    }
}
