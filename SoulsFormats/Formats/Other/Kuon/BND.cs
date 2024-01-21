using System;
using System.Collections.Generic;

namespace SoulsFormats.Kuon
{
    /// <summary>
    /// The format of Binder files in Kuon except for the main archive.
    /// <para>The difference is that these do not include a size field in file entries.</para>
    /// </summary>
    public class BND : SoulsFile<BND>
    {
        /// <summary>
        /// Files in this BND.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// The version of the file.
        /// </summary>
        public int FileVersion;

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
            FileVersion = br.AssertInt32(0xC8, 0xCA);
            int fileSize = br.ReadInt32();
            int fileCount = br.ReadInt32();

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                int nextOffset = fileSize;
                if (i < fileCount - 1)
                {
                    nextOffset = br.GetInt32(br.Position + 0xC + 4);
                }

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
            bw.WriteInt32(FileVersion);
            bw.ReserveInt32("FileSize");
            bw.WriteInt32(Files.Count);

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Write(bw, i);
            }
            bw.Pad(0x800);

            for (int i = 0; i < Files.Count; i++)
            {
                bw.FillInt32($"NameOffset_{i}", (int)bw.Position);
                bw.WriteShiftJIS(Files[i].Name);
            }

            for (int i = 0; i < Files.Count; i++)
            {
                bw.FillInt32($"DataOffset_{i}", (int)bw.Position);
                bw.WriteBytes(Files[i].Bytes);
                bw.Pad(0x800);
            }
        }

        /// <summary>
        /// A <see cref="File"/> in a <see cref="BND"/>.
        /// </summary>
        public class File
        {
            /// <summary>
            /// The ID of this <see cref="File"/>.
            /// </summary>
            public int ID;

            /// <summary>
            /// Name of this <see cref="File"/>.
            /// </summary>
            public string Name;

            /// <summary>
            /// The raw data of this <see cref="File"/>.
            /// </summary>
            public byte[] Bytes;

            /// <summary>
            /// Creates a <see cref="File"/>.
            /// </summary>
            public File()
            {
                ID = -1;
                Name = string.Empty;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID.
            /// </summary>
            public File(int id)
            {
                ID = id;
                Name = string.Empty;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with a name.
            /// </summary>
            public File(string name)
            {
                ID = -1;
                Name = name;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with bytes.
            /// </summary>
            public File(byte[] bytes)
            {
                ID = -1;
                Name = string.Empty;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID and name.
            /// </summary>
            public File(int id, string name)
            {
                ID = id;
                Name = name;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID and bytes.
            /// </summary>
            public File(int id, byte[] bytes)
            {
                ID = id;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with a name and bytes.
            /// </summary>
            public File(string name, byte[] bytes)
            {
                ID = -1;
                Name = name;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an id, name, and bytes.
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
