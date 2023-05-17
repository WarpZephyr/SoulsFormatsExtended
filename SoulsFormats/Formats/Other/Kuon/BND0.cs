using System;
using System.Collections.Generic;

namespace SoulsFormats.Kuon
{
    /// <summary>
    /// Most BNDs inside ALL/ELL. Extension: .bnd
    /// </summary>
    public class BND0 : SoulsFile<BND0>
    {
        /// <summary>
        /// Files in this BND.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// Unknown; 0xC8 or 0xCA.
        /// </summary>
        public int Unk04;

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
            Unk04 = br.AssertInt32(0xC8, 0xCA);
            int fileSize = br.ReadInt32();
            int fileCount = br.ReadInt32();

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                int nextOffset = fileSize;
                if (i < fileCount - 1)
                    nextOffset = br.GetInt32(br.Position + 0xC + 4);
                Files.Add(new File(br, nextOffset));
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;

            bw.WriteASCII("BND\0");
            bw.WriteInt32(Unk04);
            bw.ReserveInt32("FileSize");
            bw.WriteInt32(Files.Count);

            for (int i = 0; i < Files.Count; i++)
                Files[i].Write(bw, i);

            // This makes an assumpation based on things I've seen before
            // I need a sample to actually finish this.
            for (int i = 0; i < Files.Count; i++)
            {
                bw.FillInt32($"NameOffset_{i}", (int)bw.Position);
                bw.WriteShiftJIS(Files[i].Name);
            }

            for (int i = 0; i < Files.Count; i++)
            {
                bw.FillInt32($"DataOffset_{i}", (int)bw.Position);
                bw.WriteBytes(Files[i].Bytes);
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

            /// <summary>
            /// Creates a new blank File.
            /// </summary>
            public File() { }

            /// <summary>
            /// Creates a new file with an ID.
            /// </summary>
            public File(int id)
            {
                ID = id;
            }

            /// <summary>
            /// Creates a new file with a name.
            /// </summary>
            /// <param name="name"></param>
            public File(string name)
            {
                Name = name;
            }

            /// <summary>
            /// Creates a new file with bytes.
            /// </summary>
            public File(byte[] bytes)
            {
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a new File with an ID and bytes.
            /// </summary>
            public File(int id, byte[] bytes)
            {
                ID = id;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a new File with an id, name, and bytes.
            /// </summary>
            public File(int id, string name, byte[] bytes)
            {
                ID = id;
                Name = name;
                Bytes = bytes;
            }

            internal File(BinaryReaderEx br, int nextOffset)
            {
                ID = br.ReadInt32();
                int dataOffset = br.ReadInt32();
                int nameOffset = br.ReadInt32();

                Name = br.GetShiftJIS(nameOffset);
                Bytes = br.GetBytes(dataOffset, nextOffset - dataOffset);
            }

            /// <summary>
            /// Serializes file data to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteInt32(ID);
                bw.ReserveInt32($"DataOffset_{index}");
                bw.ReserveInt32($"NameOffset_{index}");
            }
        }
    }
}
