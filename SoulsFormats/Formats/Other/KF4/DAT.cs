using System;
using System.Collections.Generic;

namespace SoulsFormats.KF4
{
    /// <summary>
    /// Specifically KF4.DAT, the main archive.
    /// </summary>
    public class DAT : SoulsFile<DAT>
    {
        /// <summary>
        /// Files in the archive.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            br.AssertByte(0x00);
            br.AssertByte(0x80);
            br.AssertByte(0x04);
            br.AssertByte(0x1E);

            int fileCount = br.ReadInt32();

            br.AssertPattern(0x38, 0);

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
                Files.Add(new File(br));
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;

            bw.WriteByte(0x00);
            bw.WriteByte(0x80);
            bw.WriteByte(0x04);
            bw.WriteByte(0x1E);

            bw.WriteInt32(Files.Count);

            bw.WritePattern(0x38, 0);

            for (int i = 0; i < Files.Count; i++)
                Files[i].Write(bw, i);
            for (int i = 0; i < Files.Count; i++)
            {
                bw.FillInt32($"Offset_{i}", (int)bw.Position);
                bw.WriteBytes(Files[i].Bytes);
            }
        }

        /// <summary>
        /// A file in a DAT archive.
        /// </summary>
        public class File
        {
            /// <summary>
            /// The path of the file.
            /// </summary>
            public string Name;

            /// <summary>
            /// The file's data.
            /// </summary>
            public byte[] Bytes;

            internal File(BinaryReaderEx br)
            {
                Name = br.ReadFixStr(0x34);
                int size = br.ReadInt32();
                int paddedSize = br.ReadInt32();
                int offset = br.ReadInt32();

                Bytes = br.GetBytes(offset, size);
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteFixStr(Name, 0x34);
                bw.WriteInt32(Bytes.Length);
                bw.WriteInt32(0);
                bw.ReserveInt32($"Offset_{index}");
            }
        }
    }
}
