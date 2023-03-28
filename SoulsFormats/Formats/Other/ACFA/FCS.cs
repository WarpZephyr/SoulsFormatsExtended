namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// An FCS part in an ACPARTS file.
        /// </summary>
        public class FCS
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk00 { get; set; }

            /// <summary>
            /// The minimum distance laser blades start to home in on targets from.
            /// </summary>
            public ushort BladeLockDistance { get; set; }

            /// <summary>
            /// The Parallel Processing of this FCS.
            /// </summary>
            public ushort ParallelProcessing { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk06 { get; set; }

            /// <summary>
            /// The maximum distance at which this FCS can still lock onto a target in view.
            /// </summary>
            public ushort LockDistance { get; set; }

            /// <summary>
            /// The maximum number of targets this FCS can lock onto at once.
            /// </summary>
            public ushort LockTargetMax { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk10;

            /// <summary>
            /// How fast Weapons with Missiles can lock on using this FCS.
            /// </summary>
            public ushort MissileLockSpeed { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk14 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk16 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk18 { get; set; }

            /// <summary>
            /// A Component which contains Radar stats.
            /// </summary>
            public RadarComponent RadarComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2E { get; set; }

            /// <summary>
            /// Reads an FCS part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal FCS(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);

                Unk00 = br.ReadUInt16();
                BladeLockDistance = br.ReadUInt16();
                ParallelProcessing = br.ReadUInt16();
                Unk06 = br.ReadUInt16();
                LockDistance = br.ReadUInt16();
                LockTargetMax = br.ReadUInt16();
                Unk0C = br.ReadUInt16();
                Unk0E = br.ReadUInt16();
                Unk10 = br.ReadUInt16();
                MissileLockSpeed = br.ReadUInt16();
                Unk14 = br.ReadUInt16();
                Unk16 = br.ReadUInt16();
                Unk18 = br.ReadUInt16();

                RadarComponent = new RadarComponent(br);

                Unk28 = br.ReadUInt16();
                Unk2A = br.ReadUInt16();
                Unk2C = br.ReadUInt16();
                Unk2E = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an FCS part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);

                bw.WriteUInt16(Unk00);
                bw.WriteUInt16(BladeLockDistance);
                bw.WriteUInt16(ParallelProcessing);
                bw.WriteUInt16(Unk06);
                bw.WriteUInt16(LockDistance);
                bw.WriteUInt16(LockTargetMax);
                bw.WriteUInt16(Unk0C);
                bw.WriteUInt16(Unk0E);
                bw.WriteUInt16(Unk10);
                bw.WriteUInt16(MissileLockSpeed);
                bw.WriteUInt16(Unk14);
                bw.WriteUInt16(Unk16);
                bw.WriteUInt16(Unk18);

                RadarComponent.Write(bw);

                bw.WriteUInt16(Unk28);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(Unk2C);
                bw.WriteUInt16(Unk2E);
            }
        }
    }
}
