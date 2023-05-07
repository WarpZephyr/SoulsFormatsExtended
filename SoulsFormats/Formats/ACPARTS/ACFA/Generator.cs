namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// A Generator part in an ACPARTS file.
        /// </summary>
        public class Generator : IPart
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// The max amount of energy this Generator can hold.
            /// </summary>
            public int EnergyCapacity { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public int EnergyCapacityTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool EnergyCapacityCanBeTuned { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// The overuse limit for energy output.
            /// If total Energy Cost exceeds this number the Energy Cost gauge will turn orange,
            /// and energy output performance will be affected.
            /// </summary>
            public int EnergyOutputSoftLimit { get; set; }

            /// <summary>
            /// How fast this Generator outputs energy.
            /// </summary>
            public int EnergyOutput { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public int EnergyOutputTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool EnergyOutputCanBeTuned { get; set; }

            /// <summary>
            /// How fast this Generator outputs Kojima Particles to recharge Primal Armor.
            /// </summary>
            public ushort KPOutput { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public ushort KPOutputTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool KPOutputCanBeTuned { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk20 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk22 { get; set; }

            /// <summary>
            /// Reads a Generator part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Generator(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                EnergyCapacity = br.ReadInt32();
                EnergyCapacityTuneTarget = br.ReadInt32();
                EnergyCapacityCanBeTuned = br.AssertInt16(0, 1) == 1;
                Unk0A = br.ReadUInt16();
                EnergyOutputSoftLimit = br.ReadInt32();
                EnergyOutput = br.ReadInt32();
                EnergyOutputTuneTarget = br.ReadInt32();
                EnergyOutputCanBeTuned = br.AssertInt16(0, 1) == 1;
                KPOutput = br.ReadUInt16();
                KPOutputTuneTarget = br.ReadUInt16();
                KPOutputCanBeTuned = br.AssertInt16(0, 1) == 1;
                Unk20 = br.ReadUInt16();
                Unk22 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Generator part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                bw.WriteInt32(EnergyCapacity);
                bw.WriteInt32(EnergyCapacityTuneTarget);
                bw.WriteUInt16((ushort)(EnergyCapacityCanBeTuned ? 1 : 0));
                bw.WriteUInt16(Unk0A);
                bw.WriteInt32(EnergyOutputSoftLimit);
                bw.WriteInt32(EnergyOutput);
                bw.WriteInt32(EnergyOutputTuneTarget);
                bw.WriteUInt16((ushort)(EnergyOutputCanBeTuned ? 1 : 0));
                bw.WriteUInt16(KPOutput);
                bw.WriteUInt16(KPOutputTuneTarget);
                bw.WriteUInt16((ushort)(KPOutputCanBeTuned ? 1 : 0));
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(Unk22);
            }
        }
    }
}
