using System.Collections.Generic;

namespace SoulsFormats.AC1
{
    /// <summary>
    /// An archive used in the first Armored Core.
    /// </summary>
    public class T : SoulsFile<T>
    {
        /// <summary>
        /// Unknown, seen as 1.
        /// </summary>
        public int Unk04;

        /// <summary>
        /// The files inside of the archive.
        /// </summary>
        public List<byte[]> Files;

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            int fileCount = br.ReadInt32();
            Unk04 = br.ReadInt32();

            var alignSizes = new List<ushort>();
            for (int i = 0; i < fileCount; i++)
                alignSizes.Add(br.ReadUInt16());

            br.Pad(0x800);

            // Seems to include padding, I'm not sure how to determine what is and isn't padding.
            // There is a strange int or prehaps 2 ushorts at the end of each padding.
            for (int i = 0; i < fileCount; i++)
                Files.Add(br.ReadBytes((alignSizes[i] * 0x800) - (int)br.Position));
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            bw.WriteInt32(Files.Count);
            bw.WriteInt32(Unk04);

            foreach (byte[] file in Files)
                bw.WriteUInt16((ushort)((file.Length + (int)bw.Position) / 0x800));

            bw.Pad(0x800);

            foreach (byte[] file in Files)
                bw.WriteBytes(file);
        }
    }
}
