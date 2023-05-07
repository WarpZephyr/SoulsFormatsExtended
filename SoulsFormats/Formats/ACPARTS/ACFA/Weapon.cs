namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// An Arm Unit part in an ACPARTS file.
        /// </summary>
        public class ArmUnit : IPart, IWeapon
        {
            /// <summary>
            /// The hangar requirements of an Arm Unit.
            /// </summary>
            public enum HangarType : byte
            {
                /// <summary>
                /// The Arm Unit cannot be placed in a hangar.
                /// </summary>
                NotHangarable = 0,

                /// <summary>
                /// The Arm Unit requires a large hangar to be hangared.
                /// </summary>
                LargeHangarRequired = 1,

                /// <summary>
                /// The Arm Unit can be placed in any hangar size.
                /// </summary>
                AnyHangar = 2,
            }

            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains stats for weapons.
            /// </summary>
            public WeaponComponent WeaponComponent{ get; set; }

            /// <summary>
            /// The hangar requirements of this Arm Unit.
            /// </summary>
            public HangarType HangarRequirement { get; set; }

            /// <summary>
            /// Changes what stat descriptions are pulled from AssemMenu.bin, assumed to be an index of some kind.
            /// </summary>
            public byte DescriptionGroupIndex { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk56 { get; set; }

            /// <summary>
            /// Reads an Arm Unit part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal ArmUnit(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                WeaponComponent = new WeaponComponent(br);

                HangarRequirement = br.ReadEnum8<HangarType>();
                DescriptionGroupIndex = br.ReadByte();
                Unk56 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an Arm Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                WeaponComponent.Write(bw);

                bw.WriteByte((byte)HangarRequirement);
                bw.WriteByte(DescriptionGroupIndex);
                bw.WriteUInt16(Unk56);
            }
        }

        /// <summary>
        /// A Back Unit part in an ACPARTS file.
        /// </summary>
        public class BackUnit : IPart, IWeapon
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains stats for weapons.
            /// </summary>
            public WeaponComponent WeaponComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk54 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk56 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk58 { get; set; }

            /// <summary>
            /// A Component which contains Radar stats.
            /// </summary>
            public RadarComponent RadarComponent { get; set; }

            /// <summary>
            /// A Component which contains Booster stats for Weapons.
            /// </summary>
            public WeaponBoosterComponent WeaponBoosterComponent { get; set; }

            /// <summary>
            /// A Component which contains Assault Cannon stats for Weapons.
            /// </summary>
            public AssaultCannonComponent AssaultCannonComponent { get; set; }

            /// <summary>
            /// Changes how the Back Unit is used.
            /// </summary>
            public byte BackUnitType { get; set; }

            /// <summary>
            /// Changes what stat descriptions are pulled from AssemMenu.bin, assumed to be an index of some kind.
            /// </summary>
            public byte DescriptionGroupIndex { get; set; }

            /// <summary>
            /// Whether or not this Back Unit takes both Back Unit slots.
            /// </summary>
            public bool TakesBothSlots { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk97 { get; set; }

            /// <summary>
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// Reads a Back Unit part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BackUnit(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                WeaponComponent = new WeaponComponent(br);

                Unk54 = br.ReadUInt16();
                Unk56 = br.ReadUInt16();
                Unk58 = br.ReadUInt16();

                RadarComponent = new RadarComponent(br);
                WeaponBoosterComponent = new WeaponBoosterComponent(br);
                AssaultCannonComponent = new AssaultCannonComponent(br);

                BackUnitType = br.ReadByte();
                DescriptionGroupIndex = br.ReadByte();
                TakesBothSlots = br.ReadBoolean();
                Unk97 = br.ReadByte();

                PAComponent = new PAComponent(br);
            }

            /// <summary>
            /// Writes a Back Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                WeaponComponent.Write(bw);

                bw.WriteUInt16(Unk54);
                bw.WriteUInt16(Unk56);
                bw.WriteUInt16(Unk58);

                RadarComponent.Write(bw);
                WeaponBoosterComponent.Write(bw);
                AssaultCannonComponent.Write(bw);

                bw.WriteByte(BackUnitType);
                bw.WriteByte(DescriptionGroupIndex);
                bw.WriteBoolean(TakesBothSlots);
                bw.WriteByte(Unk97);

                PAComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Shoulder Unit part in an ACPARTS file.
        /// </summary>
        public class ShoulderUnit : IPart, IWeapon
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
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// Identifies whether or not a shoulder unit is a stealth shoulder unit; Likely for developers in developer tools at FromSoftware.
            /// </summary>
            public string ShoulderType { get; set; }

            /// <summary>
            /// The amount of times this shoulder unit can be used; Similar to magazine capacity.
            /// </summary>
            public ushort NumberofUses { get; set; }

            /// <summary>
            /// How long non-projectile generating Shoulder Unit effects last.
            /// </summary>
            public ushort EffectDuration { get; set; }

            /// <summary>
            /// How fast non-projectile generating Shoulder Units fire.
            /// </summary>
            public ushort FireRate { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E { get; set; }

            /// <summary>
            /// How much attack power is added to Assault Armor while this Shoulder Unit is equipped.
            /// </summary>
            public float AAAttackPower { get; set; }

            /// <summary>
            /// How much range is added to Assault Armor while this Shoulder Unit is equipped.
            /// </summary>
            public float AARangeBoost { get; set; }

            /// <summary>
            /// A Component which contains stats for weapons.
            /// </summary>
            public WeaponComponent WeaponComponent { get; set; }

            /// <summary>
            /// A Component which contains Booster stats for Weapons.
            /// </summary>
            public WeaponBoosterComponent WeaponBoosterComponent { get; set; }

            /// <summary>
            /// Reads a Shoulder Unit part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal ShoulderUnit(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);

                Unk00 = br.ReadByte();
                Unk01 = br.ReadByte();
                Unk02 = br.ReadByte();
                Unk03 = br.ReadByte();

                PAComponent = new PAComponent(br);
                ShoulderType = br.ReadFixStr(0x10);

                NumberofUses = br.ReadUInt16();
                EffectDuration = br.ReadUInt16();
                FireRate = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                AAAttackPower = br.ReadSingle();
                AARangeBoost = br.ReadSingle();

                WeaponComponent = new WeaponComponent(br);
                WeaponBoosterComponent = new WeaponBoosterComponent(br);
            }

            /// <summary>
            /// Writes a Shoulder Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);

                bw.WriteByte(Unk00);
                bw.WriteByte(Unk01);
                bw.WriteByte(Unk02);
                bw.WriteByte(Unk03);

                PAComponent.Write(bw);
                bw.WriteFixStr(ShoulderType, 0x10);

                bw.WriteUInt16(NumberofUses);
                bw.WriteUInt16(EffectDuration);
                bw.WriteUInt16(FireRate);
                bw.WriteUInt16(Unk1E);
                bw.WriteSingle(AAAttackPower);
                bw.WriteSingle(AARangeBoost);

                WeaponComponent.Write(bw);
                WeaponBoosterComponent.Write(bw);
            }
        }
    }
}
