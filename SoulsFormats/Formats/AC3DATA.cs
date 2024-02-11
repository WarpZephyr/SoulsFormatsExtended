namespace SoulsFormats
{
    /// <summary>
    /// A data archive in Armored Core 3 named AC3DATA.BIN. AC25DATA.BIN in Armored Core 2 Another Age is similar.
    /// </summary>
    public class AC3DATA : SoulsFile<AC3DATA>
    {
        /// <summary>
        /// The size of each block.
        /// </summary>
        public const int BLOCK_SIZE = 0x800;

        /// <summary>
        /// Where each file pads to.
        /// </summary>
        public const int FILE_ALIGNMENT = 0x8000;

        /// <summary>
        /// The address entry start blocks base themselves from.
        /// </summary>
        public const int BASE_ADDRESS = ENTRY_COUNT * 4;

        /// <summary>
        /// The number of entries.
        /// </summary>
        public const int ENTRY_COUNT = 8192;

        /// <summary>
        /// The files in this <see cref="AC3DATA"/> archive.
        /// </summary>
        public byte[][] Files = new byte[ENTRY_COUNT][];

        /// <summary>
        /// Reads an <see cref="AC3DATA"/> archive from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            for (int i = 0; i < ENTRY_COUNT; i++)
            {
                int start_block = br.ReadInt32();
                int block_count = br.ReadInt32();
                if (block_count > 0)
                {
                    int startByte = start_block * BLOCK_SIZE;
                    int position = startByte + BASE_ADDRESS;
                    int length = block_count * BLOCK_SIZE;
                    Files[i] = br.GetBytes(position, length);
                }
            }
        }

        /// <summary>
        /// Writes this <see cref="AC3DATA"/> archive to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            for (int i = 0; i < ENTRY_COUNT; i++)
            {
                if (Files[i] != null)
                {
                    bw.ReserveInt32($"StartBlock_{i}");

                    int length = Files[i].Length;
                    int remainder = length % FILE_ALIGNMENT;
                    int remaining = length - remainder;
                    int totalLength = length + remaining;
                    int blockCount = totalLength / BLOCK_SIZE;
                    bw.WriteInt32(blockCount);
                }
                else
                {
                    bw.WriteInt32(0);
                    bw.WriteInt32(0);
                }
            }

            for (int i = 0; i < ENTRY_COUNT; i++)
            {
                if (Files[i] != null)
                {
                    int startByte = (int)(bw.Position - BASE_ADDRESS);
                    int startBlock = startByte / BLOCK_SIZE;
                    bw.FillInt32($"StartBlock_{i}", startBlock);
                    bw.WriteBytes(Files[i]);
                    bw.Pad(FILE_ALIGNMENT);
                }
            }
        }
    }
}
