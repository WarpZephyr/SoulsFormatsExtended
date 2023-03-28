namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// A Main Booster part in an ACPARTS file.
        /// </summary>
        public class MainBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Horizontal Booster stats.
            /// </summary>
            public HorizontalBoosterComponent HorizontalBoosterComponent { get; set; }

            /// <summary>
            /// A Component which contains Vertical Booster Stats.
            /// </summary>
            public VerticalBoosterComponent VerticalBoosterComponent { get; set; }

            /// <summary>
            /// A Component which contains Quick Booster Stats.
            /// </summary>
            public QuickBoosterComponent QuickBoosterComponent { get; set; }

            /// <summary>
            /// Reads a Main Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal MainBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoosterComponent = new HorizontalBoosterComponent(br);
                VerticalBoosterComponent = new VerticalBoosterComponent(br);
                QuickBoosterComponent = new QuickBoosterComponent(br);
            }

            /// <summary>
            /// Writes a Main Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoosterComponent.Write(bw);
                VerticalBoosterComponent.Write(bw);
                QuickBoosterComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Back Booster part in an ACPARTS file.
        /// </summary>
        public class BackBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Horizontal Booster stats.
            /// </summary>
            public HorizontalBoosterComponent HorizontalBoosterComponent { get; set; }

            /// <summary>
            /// A Component which contains Quick Booster Stats.
            /// </summary>
            public QuickBoosterComponent QuickBoosterComponent { get; set; }

            /// <summary>
            /// Reads a Back Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BackBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoosterComponent = new HorizontalBoosterComponent(br);
                QuickBoosterComponent = new QuickBoosterComponent(br);
            }

            /// <summary>
            /// Writes a Back Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoosterComponent.Write(bw);
                QuickBoosterComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Side Booster part in an ACPARTS file.
        /// </summary>
        public class SideBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Horizontal Booster stats.
            /// </summary>
            public HorizontalBoosterComponent HorizontalBoosterComponent { get; set; }

            /// <summary>
            /// A Component which contains Quick Booster Stats.
            /// </summary>
            public QuickBoosterComponent QuickBoosterComponent { get; set; }

            /// <summary>
            /// Reads a Side Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal SideBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoosterComponent = new HorizontalBoosterComponent(br);
                QuickBoosterComponent = new QuickBoosterComponent(br);
            }

            /// <summary>
            /// Writes a Side Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoosterComponent.Write(bw);
                QuickBoosterComponent.Write(bw);
            }
        }

        /// <summary>
        /// An Overed Booster part in an ACPARTS file.
        /// </summary>
        public class OveredBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// How much Horizontal Thrust this booster adds.
            /// </summary>
            public uint OveredBoostThrust;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk04;

            /// <summary>
            /// Unknown; Is always 1.
            /// </summary>
            public ushort Unk08;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A;

            /// <summary>
            /// How much Energy Horizontal movement with this Booster will cost.
            /// </summary>
            public uint OveredBoostENCost;

            /// <summary>
            /// How much Kojima Particles Horizontal movement with this Booster will cost Primal Armor.
            /// </summary>
            public ushort OveredBoostKPCost;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk12;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk14;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk18;

            /// <summary>
            /// How much push this Booster adds after Overboost activation.
            /// </summary>
            public uint OBActivationThrust;

            /// <summary>
            /// How much Energy Overboost Activation with this Booster will cost.
            /// </summary>
            public uint OBActivationENCost;

            /// <summary>
            /// How much Kojima Particles Overboost Activation with this Booster will cost Primal Armor.
            /// </summary>
            public uint OBActivationKPCost;

            /// <summary>
            /// The Overboost Activation Limit for this Booster.
            /// </summary>
            public uint OBActivationLimit;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk2C;

            /// <summary>
            /// The damage Assault Armor from this Overed Booster will do.
            /// </summary>
            public ushort AssaultArmorAttack;

            /// <summary>
            /// The range Assault Armor from this Overed Booster will affect.
            /// </summary>
            public ushort AssaultArmorRange;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk34;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk38;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk3C;

            /// <summary>
            /// Reads an Overed Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal OveredBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);

                OveredBoostThrust = br.ReadUInt32();
                Unk04 = br.ReadUInt16();

                Unk08 = br.ReadUInt16();
                Unk0A = br.ReadUInt16();

                OveredBoostENCost = br.ReadUInt32();
                OveredBoostKPCost = br.ReadUInt16();
                Unk12 = br.ReadUInt16();
                Unk14 = br.ReadUInt32();
                Unk18 = br.ReadUInt32();

                OBActivationThrust = br.ReadUInt32();
                OBActivationENCost = br.ReadUInt32();
                OBActivationKPCost = br.ReadUInt32();
                OBActivationLimit = br.ReadUInt32();
                Unk2C = br.ReadUInt32();

                AssaultArmorAttack = br.ReadUInt16();
                AssaultArmorRange = br.ReadUInt16();
                Unk34 = br.ReadUInt32();
                Unk38 = br.ReadUInt32();
                Unk3C = br.ReadUInt32();
            }

            /// <summary>
            /// Writes an Overed Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);

                bw.WriteUInt32(OveredBoostThrust);
                bw.WriteUInt32(Unk04);

                bw.WriteUInt16(Unk08);
                bw.WriteUInt16(Unk0A);

                bw.WriteUInt32(OveredBoostENCost);
                bw.WriteUInt16(OveredBoostKPCost);
                bw.WriteUInt16(Unk12);
                bw.WriteUInt32(Unk14);
                bw.WriteUInt32(Unk18);

                bw.WriteUInt32(OBActivationThrust);
                bw.WriteUInt32(OBActivationENCost);
                bw.WriteUInt32(OBActivationKPCost);
                bw.WriteUInt32(OBActivationLimit);
                bw.WriteUInt32(Unk2C);

                bw.WriteUInt32(AssaultArmorAttack);
                bw.WriteUInt32(AssaultArmorRange);
                bw.WriteUInt32(Unk34);
                bw.WriteUInt32(Unk38);
                bw.WriteUInt32(Unk3C);
            }
        }
    }
}
