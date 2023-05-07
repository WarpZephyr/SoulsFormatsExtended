namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// An FCS part in an ACPARTS file.
        /// </summary>
        public class FCS : IPart
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk00 { get; set; }

            /// <summary>
            /// The maximum number of targets this FCS can lock onto at once.
            /// </summary>
            public byte LockTargetMax { get; set; }

            /// <summary>
            /// The minimum distance laser blades start to home in on targets from.
            /// </summary>
            public ushort BladeLockDistance { get; set; }

            /// <summary>
            /// Ability to use two weapons in tandem.
            /// Reduces lock-on degradation when firing two weapons simultaneously.
            /// </summary>
            public ushort ParallelProcessing { get; set; }

            /// <summary>
            /// Unknown; Found in fcs.txt for EnemyParts.bin.
            /// </summary>
            public ushort Visibility { get; set; }

            /// <summary>
            /// The maximum distance at which this FCS can still lock onto a target in view.
            /// </summary>
            public ushort LockDistance { get; set; }

            /// <summary>
            /// Unknown; Part of the Lock Range/Lock Distance stat found in Fcs.txt for EnemyParts.bin.
            /// </summary>
            public ushort UnkLockRange2 { get; set; }

            /// <summary>
            /// Unknown; Part of the Lock Range/Lock Distance stat found in Fcs.txt for EnemyParts.bin.
            /// </summary>
            public ushort UnkLockRange3 { get; set; }

            /// <summary>
            /// Unknown; Part of the Lock Range/Lock Distance stat found in Fcs.txt for EnemyParts.bin.
            /// </summary>
            public ushort UnkLockRange4 { get; set; }

            /// <summary>
            /// 6000 divided by this value rounded is the lock speed of this FCS.
            /// </summary>
            public ushort SecondLockTime { get; set; }

            /// <summary>
            /// How fast Weapons with Missiles can lock on using this FCS.
            /// </summary>
            public ushort MissileLockSpeed { get; set; }

            /// <summary>
            /// Unknown; Is always 0 or 1.
            /// </summary>
            public byte Unk14 { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public byte Unk15 { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
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
            /// Tuning relating to lock speed, see SecondLockTime.
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public ushort SecondLockTimeTuneTarget { get; set; }

            /// <summary>
            /// Tuning relating to lock speed, see SecondLockTime.
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool SecondLockTimeCanBeTuned { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public ushort MissileLockSpeedTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool MissileLockSpeedCanBeTuned { get; set; }

            /// <summary>
            /// Reads an FCS part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal FCS(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                Unk00 = br.ReadByte();
                LockTargetMax = br.ReadByte();
                BladeLockDistance = br.ReadUInt16();
                ParallelProcessing = br.ReadUInt16();
                Visibility = br.ReadUInt16();
                LockDistance = br.ReadUInt16();
                UnkLockRange2 = br.ReadUInt16();
                UnkLockRange3 = br.ReadUInt16();
                UnkLockRange4 = br.ReadUInt16();
                SecondLockTime = br.ReadUInt16();
                MissileLockSpeed = br.ReadUInt16();
                Unk14 = br.ReadByte();
                Unk15 = br.ReadByte();
                Unk16 = br.ReadUInt16();
                Unk18 = br.ReadUInt16();
                RadarComponent = new RadarComponent(br);
                SecondLockTimeTuneTarget = br.ReadUInt16();
                SecondLockTimeCanBeTuned = br.AssertInt16(0, 1) == 1;
                MissileLockSpeedTuneTarget = br.ReadUInt16();
                MissileLockSpeedCanBeTuned = br.AssertInt16(0, 1) == 1;
            }

            /// <summary>
            /// Writes an FCS part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                bw.WriteByte(Unk00);
                bw.WriteByte(LockTargetMax);
                bw.WriteUInt16(BladeLockDistance);
                bw.WriteUInt16(ParallelProcessing);
                bw.WriteUInt16(Visibility);
                bw.WriteUInt16(LockDistance);
                bw.WriteUInt16(UnkLockRange2);
                bw.WriteUInt16(UnkLockRange3);
                bw.WriteUInt16(UnkLockRange4);
                bw.WriteUInt16(SecondLockTime);
                bw.WriteUInt16(MissileLockSpeed);
                bw.WriteByte(Unk14);
                bw.WriteByte(Unk15);
                bw.WriteUInt16(Unk16);
                bw.WriteUInt16(Unk18);
                RadarComponent.Write(bw);
                bw.WriteUInt16(SecondLockTimeTuneTarget);
                bw.WriteUInt16((ushort)(SecondLockTimeCanBeTuned ? 1 : 0));
                bw.WriteUInt16(MissileLockSpeedTuneTarget);
                bw.WriteUInt16((ushort)(MissileLockSpeedCanBeTuned ? 1 : 0));
            }
        }
    }
}
