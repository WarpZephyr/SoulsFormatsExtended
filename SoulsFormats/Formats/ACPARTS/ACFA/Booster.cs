namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// A Main Booster part in an ACPARTS file.
        /// </summary>
        public class MainBooster : IPart, IBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Horizontal Booster stats.
            /// </summary>
            public BoosterComponent HorizontalBoost { get; set; }

            /// <summary>
            /// Vertical Booster stats.
            /// </summary>
            public BoosterComponent VerticalBoost { get; set; }

            /// <summary>
            /// Quick Booster stats.
            /// </summary>
            public BoosterComponent QuickBoost { get; set; }

            /// <summary>
            /// After using the quick boost, indicates the amount of time before it becomes available again.
            /// </summary>
            public byte QuickReloadTime { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk31 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// Reads a Main Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal MainBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoost = new BoosterComponent(br);
                VerticalBoost = new BoosterComponent(br);
                QuickBoost = new BoosterComponent(br);
            }

            /// <summary>
            /// Writes a Main Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoost.Write(bw);
                VerticalBoost.Write(bw);
                QuickBoost.Write(bw);
            }
        }

        /// <summary>
        /// A Back Booster part in an ACPARTS file.
        /// </summary>
        public class BackBooster : IPart, IBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Horizontal Booster stats.
            /// </summary>
            public BoosterComponent HorizontalBoost { get; set; }

            /// <summary>
            /// Quick Booster Stats.
            /// </summary>
            public BoosterComponent QuickBooster { get; set; }

            /// <summary>
            /// After using quick boost, indicates the amount of time before it becomes available again.
            /// </summary>
            public byte QuickReloadTime { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk31 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// Reads a Back Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BackBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoost = new BoosterComponent(br);
                QuickBooster = new BoosterComponent(br);
                QuickReloadTime = br.ReadByte();
                Unk31 = br.ReadByte();
                Unk32 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Back Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoost.Write(bw);
                QuickBooster.Write(bw);
                bw.WriteByte(QuickReloadTime);
                bw.WriteByte(Unk31);
                bw.WriteUInt16(Unk32);
            }
        }

        /// <summary>
        /// A Side Booster part in an ACPARTS file.
        /// </summary>
        public class SideBooster : IPart, IBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Horizontal Booster stats.
            /// </summary>
            public BoosterComponent HorizontalBoost { get; set; }

            /// <summary>
            /// Quick Booster Stats.
            /// </summary>
            public BoosterComponent QuickBooster { get; set; }

            /// <summary>
            /// After using quick boost, indicates the amount of time before it becomes available again.
            /// </summary>
            public byte QuickReloadTime { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk31 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// Reads a Side Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal SideBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                HorizontalBoost = new BoosterComponent(br);
                QuickBooster = new BoosterComponent(br);
                QuickReloadTime = br.ReadByte();
                Unk31 = br.ReadByte();
                Unk32 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Side Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                HorizontalBoost.Write(bw);
                QuickBooster.Write(bw);
                bw.WriteByte(QuickReloadTime);
                bw.WriteByte(Unk31);
                bw.WriteUInt16(Unk32);
            }
        }

        /// <summary>
        /// An Overed Booster part in an ACPARTS file.
        /// </summary>
        public class OveredBooster : IPart, IBooster
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// Overed Booster stats.
            /// </summary>
            public BoosterComponent HorizontalBoost { get; set; }

            /// <summary>
            /// Kojima Particle (Primal Armor Gauge) consumed when Overed Boost is engaged.
            /// Larger values indicate a greater Kojima Particle cost requirement for use.
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
            /// Thrust power during OB activation.
            /// The larger the value, the more powerful the Overed Boost activation thrust.
            /// </summary>
            public uint OBActivationThrust;

            /// <summary>
            /// Energy consumed upon activating Overed Boost.
            /// Larger values indicate a greater EN cost requirement for use.
            /// </summary>
            public uint OBActivationENCost;

            /// <summary>
            /// Kojima Particle (Primal Armor Gauge) consumed upon activating Overed Boost.
            /// Larger values indicate a greater Kojima Particle cost requirement for use.
            /// </summary>
            public uint OBActivationKPCost;

            /// <summary>
            /// Amount of time Overed Boost activation thrust is enabled.
            /// The larger the value, the longer the thrust, but the more EN and KP consumed.
            /// </summary>
            public uint OBActivationLimit;

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public uint Unk2C;

            /// <summary>
            /// Attack power of Assault Armor.
            /// Considered energy weaponry.
            /// </summary>
            public ushort AssaultArmorAttackPower;

            /// <summary>
            /// Effective range of Assault Armor.
            /// </summary>
            public ushort AssaultArmorRange;

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public uint Unk34;

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public uint Unk38;

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public uint Unk3C;

            /// <summary>
            /// Reads an Overed Booster part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal OveredBooster(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);

                HorizontalBoost = new BoosterComponent(br);
                OveredBoostKPCost = br.ReadUInt16();
                Unk12 = br.AssertUInt16(0);
                Unk14 = br.ReadUInt32();
                Unk18 = br.ReadUInt32();

                OBActivationThrust = br.ReadUInt32();
                OBActivationENCost = br.ReadUInt32();
                OBActivationKPCost = br.ReadUInt32();
                OBActivationLimit = br.ReadUInt32();
                Unk2C = br.AssertUInt32(0);

                AssaultArmorAttackPower = br.ReadUInt16();
                AssaultArmorRange = br.ReadUInt16();
                Unk34 = br.AssertUInt32(0);
                Unk38 = br.AssertUInt32(0);
                Unk3C = br.AssertUInt32(0);
            }

            /// <summary>
            /// Writes an Overed Booster part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);

                HorizontalBoost.Write(bw);
                bw.WriteUInt16(OveredBoostKPCost);
                bw.WriteUInt16(Unk12);
                bw.WriteUInt32(Unk14);
                bw.WriteUInt32(Unk18);

                bw.WriteUInt32(OBActivationThrust);
                bw.WriteUInt32(OBActivationENCost);
                bw.WriteUInt32(OBActivationKPCost);
                bw.WriteUInt32(OBActivationLimit);
                bw.WriteUInt32(Unk2C);

                bw.WriteUInt16(AssaultArmorAttackPower);
                bw.WriteUInt16(AssaultArmorRange);
                bw.WriteUInt32(Unk34);
                bw.WriteUInt32(Unk38);
                bw.WriteUInt32(Unk3C);
            }
        }
    }
}
