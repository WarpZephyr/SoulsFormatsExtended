namespace SoulsFormats.AC3
{
    /// <summary>
    /// A data archive in Armored Core 3 named AC3DATA.BIN.
    /// </summary>
    public class AC3DATA : SoulsFile<AC3DATA>
    {
        /// <summary>
        /// The alignment of files or size of blocks.
        /// </summary>
        public const int ALIGNMENT = 0x800;

        /// <summary>
        /// The address entry start blocks base themselves from.
        /// </summary>
        public const int BASE_ADDRESS = 0x10000;

        /// <summary>
        /// The files in the AC3DATA.BIN archive with a limit of 8192 files.
        /// </summary>
        public byte[][] Files = new byte[8192][];

        /// <summary>
        /// Deserializes archive data from a stream.
        /// </summary>
        /// <param name="br">A BinaryReaderEx.</param>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            for (int i = 0; i < 8192; i++)
            {
                int start_block = br.ReadInt32();
                int block_count = br.ReadInt32();
                if (start_block > 0 || block_count > 0)
                {
                    br.StepIn((start_block * ALIGNMENT) + BASE_ADDRESS);
                    Files[i] = br.ReadBytes(block_count * ALIGNMENT);
                    br.StepOut();
                }
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        /// <param name="bw">A BinaryWriterEx.</param>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            long data_pos = BASE_ADDRESS;
            for (int i = 0; i < 8192; i++)
            {
                var file = Files[i];
                if (file == null)
                {
                    bw.WriteInt32(0);
                    bw.WriteInt32(0);
                }
                else
                {
                    int block_length = file.Length;

                    int remainder = block_length % ALIGNMENT;
                    if (remainder != 0)
                    {
                        block_length += block_length - remainder;
                    }

                    int start_block = ((int)data_pos - BASE_ADDRESS) / ALIGNMENT;
                    int block_count = block_length / ALIGNMENT;

                    int pad_blocks = 0;
                    if (block_count % 16 != 0)
                    {
                        pad_blocks = 16 - block_count % 16;
                    }

                    bw.WriteInt32(start_block);
                    bw.WriteInt32(block_count);

                    bw.StepIn(data_pos);
                    bw.WriteBytes(file);
                    if (block_length != file.Length)
                    {
                        bw.WriteBytes(new byte[block_length - file.Length]);
                    }
                    if (pad_blocks != 0)
                    {
                        bw.WriteBytes(new byte[pad_blocks * ALIGNMENT]);
                    }
                    data_pos = bw.Position;
                    bw.StepOut();
                }
            }
        }
    }
}
