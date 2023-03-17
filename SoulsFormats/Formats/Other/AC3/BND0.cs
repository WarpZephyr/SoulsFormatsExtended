using System;
using System.Collections.Generic;

namespace SoulsFormats.AC3
{
    /// <summary>
    /// BND file from Armored Core Nine Breaker and Armored Core Last Raven
    /// </summary>
    public class BND0 : SoulsFile<BND0>
    {
        /// <summary>
        /// Files in this BND.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// Unknown; 0xD3 or 0xCA.
        /// </summary>
        public int Unk3;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk5;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk6;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk9;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk12;

        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(0, 4);
            return magic == "BND\0";
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            br.AssertASCII("BND\0");
            br.AssertInt32(0xFFFF);
            Unk3 = br.AssertUInt16(0xD3, 0xCA);
            br.AssertUInt16(0);
            Unk5 = br.ReadUInt16();
            Unk6 = br.ReadUInt16();
            ushort fileCount = br.ReadUInt16();
            br.AssertUInt16(0);
            Unk9 = br.ReadUInt16();
            br.AssertUInt16(0);
            ushort dataPad = br.ReadUInt16();
            Unk12 = br.ReadUInt16();
            br.AssertUInt16(0);
            br.AssertUInt16(0);

            //br.Pad(dataPad);

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(new File(br));
            }
        }

        /// <summary>
        /// A file in a BND0.
        /// </summary>
        public class File
        {
            /// <summary>
            /// ID of this file.
            /// </summary>
            public int ID;

            /// <summary>
            /// Name of this file.
            /// </summary>
            public string Name;

            /// <summary>
            /// File data.
            /// </summary>
            public byte[] Bytes;

            internal File(BinaryReaderEx br)
            {
                ID = br.ReadInt32();
                int dataOffset = br.ReadInt32();
                int nextOffset = br.ReadInt32();
                int nameOffset = br.ReadInt32();

                if (nameOffset == 0) Name = $"{ID}";
                else Name = br.GetShiftJIS(nameOffset);

                Bytes = br.GetBytes(dataOffset, nextOffset);
            }
        }
    }
}
