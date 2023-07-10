using System.Collections.Generic;

namespace SoulsFormats.MWC
{
    /// <summary>
    /// Container for model-related files used in Metal Wolf Chaos. Extension: _m.dat
    /// </summary>
    public class MDAT : SoulsFile<MDAT>
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk1C;

        /// <summary>
        /// Seen as MDL data, unknown if anything else.
        /// </summary>
        public byte[] Data1;

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] Data2;

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] Data3;

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] Data5;

        /// <summary>
        /// Unknown.
        /// </summary>
        public byte[] Data6;

        /// <summary>
        /// Deserialize file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            int fileSize = br.ReadInt32();
            int offset1 = br.ReadInt32();
            int offset2 = br.ReadInt32();
            int offset3 = br.ReadInt32();
            br.AssertInt32(0);
            int offset5 = br.ReadInt32();
            int offset6 = br.ReadInt32();
            Unk1C = br.ReadInt32();

            var offsets = new List<int> { fileSize, offset1, offset2, offset3, offset5, offset6 };
            offsets.Sort();

            if (offset1 != 0)
                Data1 = br.GetBytes(offset1, offsets[offsets.IndexOf(offset1) + 1] - offset1);
            if (offset2 != 0)
                Data2 = br.GetBytes(offset2, offsets[offsets.IndexOf(offset2) + 1] - offset2);
            if (offset3 != 0)
                Data3 = br.GetBytes(offset3, offsets[offsets.IndexOf(offset3) + 1] - offset3);
            if (offset5 != 0)
                Data5 = br.GetBytes(offset5, offsets[offsets.IndexOf(offset5) + 1] - offset5);
            if (offset6 != 0)
                Data6 = br.GetBytes(offset6, offsets[offsets.IndexOf(offset6) + 1] - offset6);
        }

        /// <summary>
        /// Serialize file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;
            bw.ReserveInt32("FileSize");
            bw.ReserveInt32("Offset1");
            bw.ReserveInt32("Offset2");
            bw.ReserveInt32("Offset3");
            bw.WriteInt32(0);
            bw.ReserveInt32("Offset5");
            bw.ReserveInt32("Offset6");
            bw.WriteInt32(Unk1C);

            FillData(bw, "Offset1", Data1);
            FillData(bw, "Offset2", Data2);
            FillData(bw, "Offset3", Data3);
            FillData(bw, "Offset5", Data5);
            FillData(bw, "Offset6", Data6);

            bw.FillInt32("FileSize", (int)bw.Position);
        }

        /// <summary>
        /// Helper method for filling offsets and data.
        /// </summary>
        /// <param name="bw">A BinaryWriterEx.</param>
        /// <param name="offsetName">The name of the offset reservation to fill.</param>
        /// <param name="data">The data to fill at the offset.</param>
        private void FillData(BinaryWriterEx bw, string offsetName, byte[] data)
        {
            if (data.Length == 0)
            {
                bw.FillInt32(offsetName, 0);
            }
            else
            {
                bw.FillInt32(offsetName, (int)bw.Position);
                bw.WriteBytes(data);
            }
        }
    }
}
