using System;
using System.Data.SqlTypes;
using static SoulsFormats.AcParts4.Component;

namespace SoulsFormats
{
    public partial class AcParts4
    {
        public partial class Part
        {
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
                /// Unknown; Is always 1.
                /// </summary>
                public byte Unk54 { get; set; }

                /// <summary>
                /// Unknown; Is always 0.
                /// </summary>
                public byte Unk55 { get; set; }

                /// <summary>
                /// Unknown; Is always 0.
                /// </summary>
                public ushort Unk56 { get; set; }

                /// <summary>
                /// Unknown; Is always 0.
                /// </summary>
                public ushort Unk58 { get; set; }

                /// <summary>
                /// A Component which contains Radar stats.
                /// </summary>
                public RadarComponent RadarComponent { get; set; }

                /// <summary>
                /// A Component which contains Booster stats for Weapons; ACFA only.
                /// </summary>
                public WeaponBoosterComponent WeaponBoosterComponent { get; set; } = new WeaponBoosterComponent();

                /// <summary>
                /// Attack power delivered when Assault Cannon hits the target.
                /// The larger this value, the greater the damage the Assault Cannon can deliver.
                /// Considered energy weaponry.
                /// ACFA only.
                /// </summary>
                public ushort AssaultCannonAttackPower { get; set; } = 0;

                /// <summary>
                /// Unknown; Is always 0; ACFA only.
                /// </summary>
                public ushort Unk8A { get; set; } = 0;

                /// <summary>
                /// Impact delivered by the AC when it strikes the target.
                /// The larger the value, the greater the impact; ACFA only.
                /// </summary>
                public ushort AssaultCannonImpact { get; set; } = 0;

                /// <summary>
                /// Level of damage to Primal Armor when attacking with an Assault Cannon.
                /// The larger the value, the greater the damage; ACFA only.
                /// </summary>
                public ushort AssaultCannonAttentuation { get; set; } = 0;

                /// <summary>
                /// Level of Primal Armor penetration when hit with an Assault Cannon.
                /// The larger the value, the less effective the Primal Armor will be; ACFA only.
                /// </summary>
                public ushort AssaultCannonPenetration { get; set; } = 0;

                /// <summary>
                /// Unknown; Is always 0; ACFA only.
                /// </summary>
                public ushort Unk92 { get; set; } = 0;

                /// <summary>
                /// Changes how the Back Unit is used.
                /// </summary>
                public byte BackUnitType { get; set; }

                /// <summary>
                /// Changes what stat descriptions are pulled from AssemMenu.bin, assumed to be an index of some kind.
                /// Seen as a labeled stat like how types are in the txt files.
                /// </summary>
                public byte DisplayType { get; set; }

                /// <summary>
                /// Unknown; Is always 0; AC4 only.
                /// </summary>
                public byte Unk6A { get; set; } = 0;

                /// <summary>
                /// Unknown; Is always 0; AC4 only.
                /// </summary>
                public byte Unk6B { get; set; } = 0;

                /// <summary>
                /// Whether or not this Back Unit takes both Back Unit slots.
                /// ACFA only, Unk6A for AC4 did not make a weapon take both slots.
                /// </summary>
                public bool TakesBothSlots { get; set; } = false;

                /// <summary>
                /// Unknown; Is always 0, 1, 2, or 3; Is very likely a type of some kind; ACFA only.
                /// </summary>
                public byte Unk97 { get; set; } = 0;

                /// <summary>
                /// A Component which contains Primal Armor stats.
                /// </summary>
                public PAComponent PAComponent { get; set; }

                /// <summary>
                /// Reads a Back Unit part from a stream.
                /// </summary>
                /// <param name="br">A binary reader.</param>
                /// <param name="version">The version indicating which 4thgen game's AcParts is being read.</param>
                internal BackUnit(BinaryReaderEx br, AcParts4Version version)
                {
                    PartComponent = new PartComponent(br, version);
                    WeaponComponent = new WeaponComponent(br);
                    Unk54 = br.ReadByte();
                    Unk55 = br.ReadByte();
                    Unk56 = br.ReadUInt16();
                    Unk58 = br.ReadUInt16();
                    RadarComponent = new RadarComponent(br);

                    if (version == AcParts4Version.ACFA)
                    {
                        WeaponBoosterComponent = new WeaponBoosterComponent(br);
                        AssaultCannonAttackPower = br.ReadUInt16();
                        Unk8A = br.ReadUInt16();
                        AssaultCannonImpact = br.ReadUInt16();
                        AssaultCannonAttentuation = br.ReadUInt16();
                        AssaultCannonPenetration = br.ReadUInt16();
                        Unk92 = br.ReadUInt16();
                    }

                    BackUnitType = br.ReadByte();
                    DisplayType = br.ReadByte();

                    if (version == AcParts4Version.AC4)
                    {
                        Unk6A = br.ReadByte();
                        Unk6B = br.ReadByte();
                    }
                    else if (version == AcParts4Version.ACFA)
                    {
                        TakesBothSlots = br.ReadBoolean();
                        Unk97 = br.ReadByte();
                    }

                    PAComponent = new PAComponent(br);
                }

                /// <summary>
                /// Writes a Back Unit part to a stream.
                /// </summary>
                /// <param name="bw">A binary writer.</param>
                /// <param name="version">The version indicating which 4thgen game's AcParts is being written.</param>
                public void Write(BinaryWriterEx bw, AcParts4Version version)
                {
                    PartComponent.Write(bw, version);
                    WeaponComponent.Write(bw);
                    bw.WriteByte(Unk54);
                    bw.WriteByte(Unk55);
                    bw.WriteUInt16(Unk56);
                    bw.WriteUInt16(Unk58);
                    RadarComponent.Write(bw);

                    if (version == AcParts4Version.ACFA)
                    {
                        WeaponBoosterComponent.Write(bw);
                        bw.WriteUInt16(AssaultCannonAttackPower);
                        bw.WriteUInt16(Unk8A);
                        bw.WriteUInt16(AssaultCannonImpact);
                        bw.WriteUInt16(AssaultCannonAttentuation);
                        bw.WriteUInt16(AssaultCannonPenetration);
                        bw.WriteUInt16(Unk92);
                    }

                    bw.WriteByte(BackUnitType);
                    bw.WriteByte(DisplayType);

                    if (version == AcParts4Version.AC4)
                    {
                        bw.WriteByte(Unk6A);
                        bw.WriteByte(Unk6B);
                    }
                    else if (version == AcParts4Version.ACFA)
                    {
                        bw.WriteBoolean(TakesBothSlots);
                        bw.WriteByte(Unk97);
                    }

                    PAComponent.Write(bw);
                }
            }
        }
    }
}