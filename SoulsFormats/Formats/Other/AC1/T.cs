using System.Collections.Generic;

namespace SoulsFormats.AC1
{
    /// <summary>
    /// An archive used in the first Armored Core.
    /// </summary>
    public class T : SoulsFile<T>
    {
        /// <summary>
        /// The size of each block.
        /// </summary>
        private const int BLOCK_SIZE = 0x800;

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
            int fileCount = br.ReadInt16();
            br.Position += 2; // Block count for the headers?

            var alignSizes = new List<ushort>();
            for (int i = 0; i < fileCount; i++)
            {
                alignSizes.Add(br.ReadUInt16());
            }

            br.Pad(BLOCK_SIZE);

            // Seems to include padding, I'm not sure how to determine what is and isn't padding.
            // There is a strange 4 bytes at the end of each padding.
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(br.ReadBytes((alignSizes[i] * BLOCK_SIZE) - (int)br.Position));
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            bw.WriteInt16((short)Files.Count);
            bw.WriteInt16(1); // Block count for the headers?

            short blockCount = 0;
            foreach (byte[] file in Files)
            {
                blockCount += (short)(file.Length / BLOCK_SIZE);
                bw.WriteInt16(blockCount);
            }

            bw.Pad(BLOCK_SIZE);
            foreach (byte[] file in Files)
            {
                bw.WriteBytes(file);
                bw.Pad(BLOCK_SIZE);
            }
        }
    }
}
