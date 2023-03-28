namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// An Arm Unit part in an ACPARTS file.
        /// </summary>
        public class ArmUnit
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains stats for projectiles.
            /// </summary>
            public ProjectileComponent ProjectileComponent{ get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk50 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk52 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk53 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk54 { get; set; }

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
                ProjectileComponent = new ProjectileComponent(br);

                Unk50 = br.ReadUInt16();
                Unk52 = br.ReadUInt16();
                Unk53 = br.ReadByte();
                Unk54 = br.ReadByte();
                Unk56 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an Arm Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                ProjectileComponent.Write(bw);

                bw.WriteUInt16(Unk50);
                bw.WriteUInt16(Unk52);
                bw.WriteByte(Unk53);
                bw.WriteByte(Unk54);
                bw.WriteUInt16(Unk56);
            }
        }

        /// <summary>
        /// A Back Unit part in an ACPARTS file.
        /// </summary>
        public class BackUnit
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains stats for projectiles.
            /// </summary>
            public ProjectileComponent ProjectileComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk50 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk52 { get; set; }

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
            /// A Component which contains Defense stats.
            /// </summary>
            public DefenseComponent DefenseComponent { get; set; }

            /// <summary>
            /// Reads a Back Unit part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BackUnit(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                ProjectileComponent = new ProjectileComponent(br);

                Unk50 = br.ReadUInt16();
                Unk52 = br.ReadUInt16();
                Unk54 = br.ReadUInt16();
                Unk56 = br.ReadUInt16();
                Unk58 = br.ReadUInt16();

                RadarComponent = new RadarComponent(br);
                WeaponBoosterComponent = new WeaponBoosterComponent(br);
                AssaultCannonComponent = new AssaultCannonComponent(br);
                DefenseComponent = new DefenseComponent(br);
            }

            /// <summary>
            /// Writes a Back Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                ProjectileComponent.Write(bw);

                bw.WriteUInt16(Unk50);
                bw.WriteUInt16(Unk52);
                bw.WriteUInt16(Unk54);
                bw.WriteUInt16(Unk56);
                bw.WriteUInt16(Unk58);

                RadarComponent.Write(bw);
                WeaponBoosterComponent.Write(bw);
                AssaultCannonComponent.Write(bw);
                DefenseComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Shoulder Unit part in an ACPARTS file.
        /// </summary>
        public class ShoulderUnit
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Defense stats.
            /// </summary>
            public DefenseComponent DefenseComponent { get; set; }

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
            /// A Component which contains stats for projectiles.
            /// </summary>
            public ProjectileComponent ProjectileComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk78 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk7A { get; set; }

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
                DefenseComponent = new DefenseComponent(br);

                ShoulderType = br.ReadFixStr(0x10);

                NumberofUses = br.ReadUInt16();
                EffectDuration = br.ReadUInt16();
                FireRate = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                AAAttackPower = br.ReadSingle();
                AARangeBoost = br.ReadSingle();

                ProjectileComponent = new ProjectileComponent(br);

                Unk78 = br.ReadUInt16();
                Unk7A = br.ReadUInt16();

                WeaponBoosterComponent = new WeaponBoosterComponent(br);
            }

            /// <summary>
            /// Writes a Shoulder Unit part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);

                bw.WriteFixStr(ShoulderType, 0x10);

                bw.WriteUInt16(NumberofUses);
                bw.WriteUInt16(EffectDuration);
                bw.WriteUInt16(FireRate);
                bw.WriteUInt16(Unk1E);
                bw.WriteSingle(AAAttackPower);
                bw.WriteSingle(AARangeBoost);

                ProjectileComponent.Write(bw);

                bw.WriteUInt16(Unk78);
                bw.WriteUInt16(Unk7A);

                WeaponBoosterComponent.Write(bw);
            }
        }
    }
}
