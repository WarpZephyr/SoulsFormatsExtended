namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// A Generator part in an ACPARTS file.
        /// </summary>
        public class Generator
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// The max amount of energy this Generator can hold.
            /// </summary>
            public uint EnergyCapacity { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk04 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk06 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk08 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0E { get; set; }

            /// <summary>
            /// How fast this Generator outputs energy.
            /// </summary>
            public ushort EnergyOutput { get; set; }

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
            /// How fast this Generator outputs Kojima Particles to recharge Primal Armor.
            /// </summary>
            public ushort KPOutput { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E { get; set; }

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

            }

            /// <summary>
            /// Writes a Generator part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer to write stats to a stream.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);

            }
        }
    }
}
