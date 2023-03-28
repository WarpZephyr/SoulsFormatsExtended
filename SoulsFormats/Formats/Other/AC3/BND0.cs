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
        public ushort Unk5;

        /// <summary>
        /// Unknown.
        /// </summary>
        public ushort Unk6;

        /// <summary>
        /// What offset to align to.
        /// </summary>
        public ushort DataPad;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk8;

        /// <summary>
        /// Unknown.
        /// </summary>
        public ushort Unk10;

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
            Unk3 = br.AssertInt32(0xD3, 0xCA);
            Unk5 = br.ReadUInt16();
            Unk6 = br.ReadUInt16();
            int fileCount = br.ReadInt32();
            Unk8 = br.ReadInt32();
            DataPad = br.ReadUInt16();
            Unk10 = br.ReadUInt16();
            br.AssertInt32(0);

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(new File(br));
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            throw new NotImplementedException();
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
            /// Will be set to ID if name does not exist.
            /// </summary>
            public string Name;

            /// <summary>
            /// The raw data of this file.
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

            /// <summary>
            /// Serializes file data to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                throw new NotImplementedException();
            }
        }
    }
}
