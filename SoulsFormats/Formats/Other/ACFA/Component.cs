namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        public class PartComponent
        {
            /// <summary>
            /// The ID for this part
            /// </summary>
            public ushort PartID { get; set; }

            /// <summary>
            /// The ID of the model used by this part
            /// </summary>
            public ushort ModelID { get; set; }

            /// <summary>
            /// The price of this part in the shop
            /// </summary>
            public int Price { get; set; }

            /// <summary>
            /// The Weight of this part
            /// </summary>
            public ushort Weight { get; set; }

            /// <summary>
            /// The energy cost of this part
            /// </summary>
            public ushort ENCost { get; set; }

            /// <summary>
            /// Unknown; In front of Name, Manufacturer, and PartGroup string struct.
            /// </summary>
            public ushort Unk0E { get; set; }

            /// <summary>
            /// Unknown; In front of Name, Manufacturer, and PartGroup string struct.
            /// </summary>
            public ushort Unk10 { get; set; }

            /// <summary>
            /// The Name of this part.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The Manufacturer of this part
            /// </summary>
            public string Manufacturer { get; set; }

            /// <summary>
            /// A description describing weight group, part type, or other similar things.
            /// </summary>
            public string PartGroup { get; set; }

            /// <summary>
            /// Unknown; In front of Description string struct.
            /// </summary>
            public ushort Unk70 { get; set; }

            /// <summary>
            /// Unknown; In front of Description string struct.
            /// </summary>
            public ushort Unk72 { get; set; }

            /// <summary>
            /// An internal description describing this part, likely used by FromSoftware development tools.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Reads a Part component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal PartComponent(BinaryReaderEx br)
            {
                PartID = br.ReadUInt16();
                ModelID = br.ReadUInt16();
                Price = br.ReadInt32();
                Weight = br.ReadUInt16();
                ENCost = br.ReadUInt16();

                Unk0E = br.ReadUInt16();
                Unk10 = br.ReadUInt16();
                Name = br.ReadFixStr(0x20);
                Manufacturer = br.ReadFixStr(0x20);
                PartGroup = br.ReadFixStr(0x20);

                Unk70 = br.ReadUInt16();
                Unk72 = br.ReadUInt16();
                Description = br.ReadFixStr(0xFC);
            }

            /// <summary>
            /// Writes a Part component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(PartID);
                bw.WriteUInt16(ModelID);
                bw.WriteInt32(Price);
                bw.WriteUInt16(Weight);
                bw.WriteUInt16(ENCost);

                bw.WriteUInt16(Unk0E);
                bw.WriteUInt16(Unk10);
                bw.WriteFixStr(Name, 0x20);
                bw.WriteFixStr(Manufacturer, 0x20);
                bw.WriteFixStr(PartGroup, 0x20);

                bw.WriteUInt16(Unk70);
                bw.WriteUInt16(Unk72);
                bw.WriteFixStr(Description, 0xFC);
            }
        }

        /// <summary>
        /// A Component which contains Defense stats.
        /// </summary>
        public class DefenseComponent
        {
            /// <summary>
            /// The Ballistic Defense added for equipping this part.
            /// </summary>
            public ushort BallisticDefense { get; set; }

            /// <summary>
            /// The Energy Defense added for equipping this part.
            /// </summary>
            public ushort EnergyDefense { get; set; }

            /// <summary>
            /// The Primal Armor Rectification added for equipping this part.
            /// </summary>
            public ushort PARectification { get; set; }

            /// <summary>
            /// The Primal Armor Durability added for equipping this part.
            /// </summary>
            public ushort PADurability { get; set; }

            /// <summary>
            /// Reads a Defense component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal DefenseComponent(BinaryReaderEx br)
            {
                BallisticDefense = br.ReadUInt16();
                EnergyDefense = br.ReadUInt16();
                PARectification = br.ReadUInt16();
                PADurability = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Defense component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(BallisticDefense);
                bw.WriteUInt16(EnergyDefense);
                bw.WriteUInt16(PARectification);
                bw.WriteUInt16(PADurability);
            }
        }

        /// <summary>
        /// A Component which contains body part stats.
        /// </summary>
        public class BodyComponent
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk08 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// The Armor Points added for equipping this part.
            /// </summary>
            public ushort AP { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0E { get; set; }

            /// <summary>
            /// Unknown; Believed to be a float.
            /// </summary>
            public float Unk10 { get; set; }

            /// <summary>
            /// Unknown; May identify debug or non-debug parts in some way.
            /// </summary>
            public ushort Unk14 { get; set; }

            /// <summary>
            /// Unknown; May identify debug or non-debug parts in some way.
            /// </summary>
            public ushort Unk16 { get; set; }

            /// <summary>
            /// Unknown; Commonly the same across all of its part type.
            /// </summary>
            public ushort Unk18 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1A { get; set; }

            /// <summary>
            /// Reads a Body component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BodyComponent(BinaryReaderEx br)
            {
                Unk08 = br.ReadUInt16();
                Unk0A = br.ReadUInt16();
                AP = br.ReadUInt16();
                Unk0E = br.ReadUInt16();
                Unk10 = br.ReadSingle();
                Unk14 = br.ReadUInt16();
                Unk16 = br.ReadUInt16();
                Unk18 = br.ReadUInt16();
                Unk1A = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Body component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(Unk08);
                bw.WriteUInt16(Unk0A);
                bw.WriteUInt16(AP);
                bw.WriteUInt16(Unk0E);
                bw.WriteSingle(Unk10);
                bw.WriteUInt16(Unk14);
                bw.WriteUInt16(Unk16);
                bw.WriteUInt16(Unk18);
                bw.WriteSingle(Unk1A);
            }
        }

        /// <summary>
        /// A Component which contains stats for projectiles.
        /// </summary>
        public class ProjectileComponent
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk00 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk02 { get; set; }

            /// <summary>
            /// The max range bullets from a weapon using this projectile travel.
            /// </summary>
            public ushort FiringRange { get; set; }

            /// <summary>
            /// The Melee Ability of a weapon using this projectile.
            /// </summary>
            public ushort MeleeAbility { get; set; }

            /// <summary>
            /// The bullet ID from bullet params to generate when firing a weapon using this projectile.
            /// </summary>
            public uint BulletID { get; set; }

            /// <summary>
            /// Unknown; Very likely an ID of some sort.
            /// </summary>
            public uint Unk0C { get; set; }

            /// <summary>
            /// Unknown; Very likely an ID of some sort.
            /// </summary>
            public uint Unk10 { get; set; }

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
            /// Unknown.
            /// </summary>
            public ushort Unk1A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk1C { get; set; }

            /// <summary>
            /// The number of bullets to generate when firing a weapon using this projectile.
            /// </summary>
            public byte ProjectileCount { get; set; }

            /// <summary>
            /// How many times to repeat firing a weapon using this projectile.
            /// </summary>
            public byte ContinuousFireCount { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk1F { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk20 { get; set; }

            /// <summary>
            /// How fast a weapon using this projectile fires, lower is faster.
            /// </summary>
            public ushort FireRate { get; set; }

            /// <summary>
            /// The recoil generated by firing a weapon using this projectile for each shot.
            /// </summary>
            public ushort Recoil { get; set; }

            /// <summary>
            /// The Cost Per Round for a weapon using this projectile.
            /// </summary>
            public ushort CostPerRound { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A { get; set; }

            /// <summary>
            /// The number of magazines a weapon using this projectile has or the number of reloads for a weapon using this projectile.
            /// </summary>
            public ushort NumberofMagazines { get; set; }

            /// <summary>
            /// The amount of bullets per magazine for a weapon using this projectile or the number of bullets per reload for a weapon using this projectile.
            /// </summary>
            public ushort MagazineCapacity { get; set; }

            /// <summary>
            /// How fast a weapon using this projectile reloads.
            /// </summary>
            public ushort MagazineReloadTime { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// How long to charge kojima for a weapon using this projectile.
            /// </summary>
            public ushort ChargeTime { get; set; }

            /// <summary>
            /// The amount of Kojima Particles drained from Primal Armor while charging a weapon using this projectile.
            /// </summary>
            public ushort KPChargeCost { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk38 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3A { get; set; }

            /// <summary>
            /// How long until laser blade can be used again for a weapon using this projectile.
            /// </summary>
            public ushort AttackLatency { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk40 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk42 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk44 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk46 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk48 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk4A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk4C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk4E { get; set; }

            /// <summary>
            /// Reads a Projectile component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal ProjectileComponent(BinaryReaderEx br)
            {
                Unk00 = br.ReadUInt16();
                Unk02 = br.ReadUInt16();
                FiringRange = br.ReadUInt16();
                MeleeAbility = br.ReadUInt16();
                BulletID = br.ReadUInt32();
                Unk0C = br.ReadUInt32();
                Unk10 = br.ReadUInt32();
                Unk14 = br.ReadUInt16();
                Unk16 = br.ReadUInt16();
                Unk18 = br.ReadUInt16();
                Unk1A = br.ReadUInt16();
                Unk1C = br.ReadByte();
                ProjectileCount = br.ReadByte();
                ContinuousFireCount = br.ReadByte();
                Unk1F = br.ReadByte();
                Unk20 = br.ReadUInt32();
                FireRate = br.ReadUInt16();
                Recoil = br.ReadUInt16();
                CostPerRound = br.ReadUInt16();
                Unk2A = br.ReadUInt16();
                NumberofMagazines = br.ReadUInt16();
                MagazineCapacity = br.ReadUInt16();
                MagazineReloadTime = br.ReadUInt16();
                Unk32 = br.ReadUInt16();
                ChargeTime = br.ReadUInt16();
                KPChargeCost = br.ReadUInt16();
                Unk38 = br.ReadUInt16();
                Unk3A = br.ReadUInt16();
                AttackLatency = br.ReadUInt16();
                Unk3E = br.ReadUInt16();
                Unk40 = br.ReadUInt16();
                Unk42 = br.ReadUInt16();
                Unk44 = br.ReadUInt16();
                Unk46 = br.ReadUInt16();
                Unk48 = br.ReadUInt16();
                Unk4A = br.ReadUInt16();
                Unk4C = br.ReadUInt16();
                Unk4E = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Projectile component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(Unk00);
                bw.WriteUInt16(Unk02);
                bw.WriteUInt16(FiringRange);
                bw.WriteUInt16(MeleeAbility);
                bw.WriteUInt32(BulletID);
                bw.WriteUInt32(Unk0C);
                bw.WriteUInt32(Unk10);
                bw.WriteUInt16(Unk14);
                bw.WriteUInt16(Unk16);
                bw.WriteUInt16(Unk18);
                bw.WriteUInt16(Unk1A);
                bw.WriteByte(Unk1C);
                bw.WriteByte(ProjectileCount);
                bw.WriteByte(ContinuousFireCount);
                bw.WriteByte(Unk1F);
                bw.WriteUInt32(Unk20);
                bw.WriteUInt16(FireRate);
                bw.WriteUInt16(Recoil);
                bw.WriteUInt16(CostPerRound);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(NumberofMagazines);
                bw.WriteUInt16(MagazineCapacity);
                bw.WriteUInt16(MagazineReloadTime);
                bw.WriteUInt16(Unk32);
                bw.WriteUInt16(ChargeTime);
                bw.WriteUInt16(KPChargeCost);
                bw.WriteUInt16(Unk38);
                bw.WriteUInt16(Unk3A);
                bw.WriteUInt16(AttackLatency);
                bw.WriteUInt16(Unk3E);
                bw.WriteUInt16(Unk40);
                bw.WriteUInt16(Unk42);
                bw.WriteUInt16(Unk44);
                bw.WriteUInt16(Unk46);
                bw.WriteUInt16(Unk48);
                bw.WriteUInt16(Unk4A);
                bw.WriteUInt16(Unk4C);
                bw.WriteUInt16(Unk4E);
            }
        }

        /// <summary>
        /// A Component which contains Radar stats.
        /// </summary>
        public class RadarComponent
        {
            /// <summary>
            /// The range of this Radar.
            /// </summary>
            public ushort RadarRange { get; set; }

            /// <summary>
            /// How much this Radar resists ECM jamming.
            /// </summary>
            public ushort ECMResistance { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk5E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk60 { get; set; }

            /// <summary>
            /// How often this Radar refreshes.
            /// </summary>
            public ushort RadarRefreshRate { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk64 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk66 { get; set; }

            /// <summary>
            /// Reads a Radar component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal RadarComponent(BinaryReaderEx br)
            {
                RadarRange = br.ReadUInt16();
                ECMResistance = br.ReadUInt16();
                Unk5E = br.ReadUInt16();
                Unk60 = br.ReadUInt16();
                RadarRefreshRate = br.ReadUInt16();
                Unk64 = br.ReadUInt16();
                Unk66 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Radar component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(RadarRange);
                bw.WriteUInt16(ECMResistance);
                bw.WriteUInt16(Unk5E);
                bw.WriteUInt16(Unk60);
                bw.WriteUInt16(RadarRefreshRate);
                bw.WriteUInt16(Unk64);
                bw.WriteUInt16(Unk66);
            }
        }

        /// <summary>
        /// A Component which contains Assault Cannon stats for Weapons.
        /// </summary>
        public class AssaultCannonComponent
        {
            /// <summary>
            /// The amount of damage Assault Cannon attacks will do.
            /// </summary>
            public ushort AssaultCannonAttack { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk8A { get; set; }

            /// <summary>
            /// The Assault Cannon Impact of this Back Assault Cannon.
            /// </summary>
            public ushort AssaultCannonImpact { get; set; }

            /// <summary>
            /// The AC Attentuation of this Assault Cannon.
            /// </summary>
            public ushort ACAttentuation { get; set; }

            /// <summary>
            /// How much Primal Armor is ignored when dealing damage from this Assault Cannon.
            /// </summary>
            public ushort ACPenetration { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk92 { get; set; }

            /// <summary>
            /// Reads an Assault Cannon component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal AssaultCannonComponent(BinaryReaderEx br)
            {
                AssaultCannonAttack = br.ReadUInt16();
                Unk8A = br.ReadUInt16();
                AssaultCannonImpact = br.ReadUInt16();
                ACAttentuation = br.ReadUInt16();
                ACPenetration = br.ReadUInt16();
                Unk92 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an Assault Cannon component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(AssaultCannonAttack);
                bw.WriteUInt16(Unk8A);
                bw.WriteUInt16(AssaultCannonImpact);
                bw.WriteUInt16(ACAttentuation);
                bw.WriteUInt16(ACPenetration);
                bw.WriteUInt16(Unk92);
            }
        }

        /// <summary>
        /// A Component which contains Booster stats for Weapons.
        /// </summary>
        public class WeaponBoosterComponent
        {
            /// <summary>
            /// How much Horizontal Thrust this Weapon Booster adds.
            /// </summary>
            public uint HorizontalThrust { get; set; }

            /// <summary>
            /// How much Vertical Thrust this Weapon Booster adds.
            /// </summary>
            public uint VerticalThrust { get; set; }

            /// <summary>
            /// How much Thrust this Weapon Booster adds while Quick Boosting.
            /// </summary>
            public uint QuickBoost { get; set; }

            /// <summary>
            /// Unknown; Likely is another type of Boost.
            /// </summary>
            public uint Unk88 { get; set; }

            /// <summary>
            /// How much Energy Horizontal boost movement with this Weapon Booster will cost.
            /// </summary>
            public uint HorizontalENCost { get; set; }

            /// <summary>
            /// How much Energy Vertical boost movement with this Weapon Booster will cost.
            /// </summary>
            public uint VerticalENCost { get; set; }

            /// <summary>
            /// How much Energy Quick Boosting with this Weapon Booster will cost.
            /// </summary>
            public uint QuickBoostENCost { get; set; }

            /// <summary>
            /// Unknown; Likely is another type of Boost's EN cost.
            /// </summary>
            public uint Unk98 { get; set; }

            /// <summary>
            /// Reads a Weapon Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal WeaponBoosterComponent(BinaryReaderEx br)
            {
                HorizontalThrust = br.ReadUInt32();
                VerticalThrust = br.ReadUInt32();
                QuickBoost = br.ReadUInt32();
                Unk88 = br.ReadUInt32();
                HorizontalENCost = br.ReadUInt32();
                VerticalENCost = br.ReadUInt32();
                QuickBoostENCost = br.ReadUInt32();
                Unk98 = br.ReadUInt32();
            }

            /// <summary>
            /// Writes a Weapon Booster component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt32(HorizontalThrust);
                bw.WriteUInt32(VerticalThrust);
                bw.WriteUInt32(QuickBoost);
                bw.WriteUInt32(Unk88);
                bw.WriteUInt32(HorizontalENCost);
                bw.WriteUInt32(VerticalENCost);
                bw.WriteUInt32(QuickBoostENCost);
                bw.WriteUInt32(Unk98);
            }
        }

        /// <summary>
        /// A Component which contains Horizontal Booster stats.
        /// </summary>
        public class HorizontalBoosterComponent
        {
            /// <summary>
            /// How much Horizontal Thrust this Booster adds.
            /// </summary>
            public uint HorizontalThrust { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk04 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk08 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// How much Energy Horizontal Boost movement with this Booster will cost.
            /// </summary>
            public uint HorizontalENCost { get; set; }

            /// <summary>
            /// Reads a Horizontal Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal HorizontalBoosterComponent(BinaryReaderEx br)
            {
                HorizontalThrust = br.ReadUInt32();
                Unk04 = br.ReadUInt16();
                Unk08 = br.ReadUInt16();
                Unk0A = br.ReadUInt16();
                HorizontalENCost = br.ReadUInt32();
            }

            /// <summary>
            /// Writes a Horizontal Booster Component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt32(HorizontalThrust);
                bw.WriteUInt32(Unk04);
                bw.WriteUInt16(Unk08);
                bw.WriteUInt16(Unk0A);
                bw.WriteUInt32(HorizontalENCost);
            }
        }

        /// <summary>
        /// A Component which contains Vertical Booster stats.
        /// </summary>
        public class VerticalBoosterComponent
        {
            /// <summary>
            /// How much Vertical Thrust this booster adds.
            /// </summary>
            public uint VerticalThrust { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk04 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk08 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// How much Energy Vertical Boost movement with this Booster will cost.
            /// </summary>
            public uint VerticalENCost { get; set; }

            /// <summary>
            /// Reads a Vertical Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal VerticalBoosterComponent(BinaryReaderEx br)
            {
                VerticalThrust = br.ReadUInt32();
                Unk04 = br.ReadUInt16();
                Unk08 = br.ReadUInt16();
                Unk0A = br.ReadUInt16();
                VerticalENCost = br.ReadUInt32();
            }

            /// <summary>
            /// Writes a Vertical Booster component to a binary writer stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt32(VerticalThrust);
                bw.WriteUInt32(Unk04);
                bw.WriteUInt16(Unk08);
                bw.WriteUInt16(Unk0A);
                bw.WriteUInt32(VerticalENCost);
            }
        }

        /// <summary>
        /// A Component which contains Quick Booster stats.
        /// </summary>
        public class QuickBoosterComponent
        {
            /// <summary>
            /// How much Thrust this Booster adds while Quick Boosting.
            /// </summary>
            public uint QuickBoost { get; set; }

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
            /// How long a Quick Boost will last with this boost.
            /// </summary>
            public ushort QuickBoostDuration { get; set; }

            /// <summary>
            /// How much Energy Quick Boosting with this Booster will cost.
            /// </summary>
            public uint QuickBoostENCost { get; set; }

            /// <summary>
            /// Quick Boost cooldown per direction.
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
            /// Reads a Quick Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal QuickBoosterComponent(BinaryReaderEx br)
            {
                QuickBoost = br.ReadUInt32();
                Unk04 = br.ReadUInt16();
                Unk06 = br.ReadUInt16();
                Unk08 = br.ReadUInt16();
                QuickBoostDuration = br.ReadUInt16();
                QuickBoostENCost = br.ReadUInt32();
                QuickReloadTime = br.ReadByte();
                Unk31 = br.ReadByte();
                Unk32 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Quick Booster component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt32(QuickBoost);
                bw.WriteUInt16(Unk04);
                bw.WriteUInt16(Unk06);
                bw.WriteUInt16(Unk08);
                bw.WriteUInt16(QuickBoostDuration);
                bw.WriteUInt32(QuickBoostENCost);
                bw.WriteByte(QuickReloadTime);
                bw.WriteByte(Unk31);
                bw.WriteUInt16(Unk32);
            }
        }

        /// <summary>
        /// A Component which contains Stabilizer stats.
        /// </summary>
        public class StabilizerComponent
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk00 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk01 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk02 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk03 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk04 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk05 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk06 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk07 { get; set; }

            /// <summary>
            /// Reads a Stabilizer component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal StabilizerComponent(BinaryReaderEx br)
            {
                Unk00 = br.ReadByte();
                Unk01 = br.ReadByte();
                Unk02 = br.ReadByte();
                Unk03 = br.ReadByte();
                Unk04 = br.ReadByte();
                Unk05 = br.ReadByte();
                Unk06 = br.ReadByte();
                Unk07 = br.ReadByte();
            }

            /// <summary>
            /// Writes a Stabilizer component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteByte(Unk00);
                bw.WriteByte(Unk01);
                bw.WriteByte(Unk02);
                bw.WriteByte(Unk03);
                bw.WriteByte(Unk04);
                bw.WriteByte(Unk05);
                bw.WriteByte(Unk06);
                bw.WriteByte(Unk07);
            }
        }
    }
}
