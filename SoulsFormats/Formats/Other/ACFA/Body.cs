namespace SoulsFormats.Formats.Other.ACFA
{
    /// <summary>
    /// A part configuration format used in 4th generation Armored Core.
    /// </summary>
    public partial class ACPARTS
    {
        /// <summary>
        /// A Head part in an ACPARTS file.
        /// </summary>
        public class Head
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Defense Stats.
            /// </summary>
            public DefenseComponent DefenseComponent { get; set; }

            /// <summary>
            /// A Component which contains body part stats.
            /// </summary>
            public BodyComponent BodyComponent { get; set; }

            /// <summary>
            /// The Stability added for equipping this Head.
            /// </summary>
            public ushort Stability;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk20;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk22;

            /// <summary>
            /// The Camera Functionality for this Head.
            /// </summary>
            public ushort CameraFunctionality;

            /// <summary>
            /// The System Recovery for this Head.
            /// </summary>
            public ushort SystemRecovery;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2E;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk30;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32;

            /// <summary>
            /// Reads a Head part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Head(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                BodyComponent = new BodyComponent(br);

                Stability = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                Unk20 = br.ReadUInt16();
                Unk22 = br.ReadUInt16();
                CameraFunctionality = br.ReadUInt16();
                SystemRecovery = br.ReadUInt16();
                Unk28 = br.ReadUInt16();
                Unk2A = br.ReadUInt16();
                Unk2C = br.ReadUInt16();
                Unk2E = br.ReadUInt16();
                Unk30 = br.ReadUInt16();
                Unk32 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Head part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);
                BodyComponent.Write(bw);

                bw.WriteUInt16(Stability);
                bw.WriteUInt16(Unk1E);
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(Unk22);
                bw.WriteUInt16(CameraFunctionality);
                bw.WriteUInt16(SystemRecovery);
                bw.WriteUInt16(Unk28);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(Unk2C);
                bw.WriteUInt16(Unk2E);
                bw.WriteUInt16(Unk30);
                bw.WriteUInt16(Unk32);
            }
        }

        /// <summary>
        /// A Core part in an ACPARTS file.
        /// </summary>
        public class Core
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
            /// A Component which contains body part stats.
            /// </summary>
            public BodyComponent BodyComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk20;

            /// <summary>
            /// The Stability added for equipping this Core.
            /// </summary>
            public ushort Stability;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk24;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk26;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2E;

            /// <summary>
            /// Reads a Core part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Core(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                BodyComponent = new BodyComponent(br);

                Unk1C = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                Unk20 = br.ReadUInt16();
                Stability = br.ReadUInt16();
                Unk24 = br.ReadUInt16();
                Unk26 = br.ReadUInt16();
                Unk28 = br.ReadUInt16();
                Unk2A = br.ReadUInt16();
                Unk2C = br.ReadUInt16();
                Unk2E = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Core part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);
                BodyComponent.Write(bw);

                bw.WriteUInt16(Unk1C);
                bw.WriteUInt16(Unk1E);
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(Stability);
                bw.WriteUInt16(Unk24);
                bw.WriteUInt16(Unk26);
                bw.WriteUInt16(Unk28);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(Unk2C);
                bw.WriteUInt16(Unk2E);
            }
        }

        /// <summary>
        /// An Arm part in an ACPARTS file.
        /// </summary>
        public class Arm
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
            /// A Component which contains body part stats.
            /// </summary>
            public BodyComponent BodyComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1C;

            /// <summary>
            /// The Firing Stability of this Arm.
            /// </summary>
            public ushort FiringStability;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk20;

            /// <summary>
            /// The Maneuverability of this Arm.
            /// </summary>
            public ushort Maneuverability;

            /// <summary>
            /// The Aim Precision of this Arm.
            /// </summary>
            public ushort AimPrecision;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk26;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2E;

            /// <summary>
            /// The Energy Weapon Skill of this Arm.
            /// </summary>
            public ushort EnergyWeaponSkill;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk34;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk36;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk38;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3A;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3D;

            /// <summary>
            /// Identifies whether or not an arm is a Weapon Arm; Likely for developers in developer tools at FromSoftware.
            /// </summary>
            public string ArmType;

            /// <summary>
            /// The Weapon Arm Stats for this Arm.
            /// </summary>
            public ProjectileComponent WeaponArmStats;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint UnkA0;

            /// <summary>
            /// Unknown; Is always 0x64 across all arms
            /// </summary>
            public ushort UnkA4;

            /// <summary>
            /// Unknown; Is always 0 across all arms
            /// </summary>
            public ushort UnkA6;

            /// <summary>
            /// Reads an Arm part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Arm(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                BodyComponent = new BodyComponent(br);

                Unk1C = br.ReadUInt16();
                FiringStability = br.ReadUInt16();
                Unk20 = br.ReadUInt16();
                Maneuverability = br.ReadUInt16();
                AimPrecision = br.ReadUInt16();
                Unk26 = br.ReadUInt16();
                Unk28 = br.ReadUInt16();
                Unk2A = br.ReadUInt16();
                Unk2C = br.ReadUInt16();
                Unk2E = br.ReadUInt16();
                EnergyWeaponSkill = br.ReadUInt16();
                Unk32 = br.ReadUInt16();
                Unk34 = br.ReadUInt16();
                Unk36 = br.ReadUInt16();
                Unk38 = br.ReadUInt16();
                Unk3A = br.ReadUInt16();

                Unk3C = br.ReadUInt16();
                Unk3D = br.ReadUInt16();
                ArmType = br.ReadFixStr(0x10);

                WeaponArmStats = new ProjectileComponent(br);

                UnkA0 = br.ReadUInt32();
                UnkA4 = br.ReadUInt16();
                UnkA6 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an Arm part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);
                BodyComponent.Write(bw);

                bw.WriteUInt16(Unk1C);
                bw.WriteUInt16(FiringStability);
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(Maneuverability);
                bw.WriteUInt16(AimPrecision);
                bw.WriteUInt16(Unk26);
                bw.WriteUInt16(Unk28);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(Unk2C);
                bw.WriteUInt16(Unk2E);
                bw.WriteUInt16(EnergyWeaponSkill);
                bw.WriteUInt16(Unk32);
                bw.WriteUInt16(Unk34);
                bw.WriteUInt16(Unk36);
                bw.WriteUInt16(Unk38);
                bw.WriteUInt16(Unk3A);

                bw.WriteUInt16(Unk3C);
                bw.WriteUInt16(Unk3D);
                bw.WriteFixStr(ArmType, 0x10);

                WeaponArmStats.Write(bw);

                bw.WriteUInt32(UnkA0);
                bw.WriteUInt16(UnkA4);
                bw.WriteUInt16(UnkA6);
            }
        }

        /// <summary>
        /// A Leg part in an ACPARTS file.
        /// </summary>
        public class Leg
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Defense Stats.
            /// </summary>
            public DefenseComponent DefenseComponent { get; set; }

            /// <summary>
            /// A Component which contains body part stats.
            /// </summary>
            public BodyComponent BodyComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk20;

            /// <summary>
            /// The amount of weight this Leg can hold.
            /// </summary>
            public ushort Load;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk24;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk26;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk28;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk2C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk30;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk34;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk38;

            /// <summary>
            /// The Stability added for equipping this Leg.
            /// </summary>
            public uint Stability;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk40;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk44;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk48;

            /// <summary>
            /// How fast these Legs can turn; Negative values reverse turning controls.
            /// </summary>
            public int TurningAbility;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk50;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk54;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk58;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk5C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk60;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk62;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk64;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk66;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk68;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk6A;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk6C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk70;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk74;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk78;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk7C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk80;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk84;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk88;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk8C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk90;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk94;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk98;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk9C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint UnkA0;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkA4;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkA6;

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint UnkA8;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkAC;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkAE;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB0;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB2;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB4;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB6;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB8;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBA;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBC;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBE;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkC0;

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkC2;

            /// <summary>
            /// Reads a Leg part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Leg(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                BodyComponent = new BodyComponent(br);

                Unk1C = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                Unk20 = br.ReadUInt16();
                Load = br.ReadUInt16();
                Unk24 = br.ReadUInt16();
                Unk26 = br.ReadUInt16();
                Unk28 = br.ReadUInt32();
                Unk2C = br.ReadUInt32();
                Unk30 = br.ReadUInt32();
                Unk34 = br.ReadUInt32();
                Unk38 = br.ReadUInt32();
                Stability = br.ReadUInt32();
                Unk40 = br.ReadUInt32();
                Unk44 = br.ReadUInt32();
                Unk48 = br.ReadUInt32();
                TurningAbility = br.ReadInt32();
                Unk50 = br.ReadUInt32();
                Unk54 = br.ReadUInt32();
                Unk58 = br.ReadUInt32();
                Unk5C = br.ReadUInt32();
                Unk60 = br.ReadUInt32();
                Unk62 = br.ReadUInt16();
                Unk64 = br.ReadUInt16();
                Unk66 = br.ReadUInt16();
                Unk68 = br.ReadUInt16();
                Unk6A = br.ReadUInt16();
                Unk6C = br.ReadUInt16();
                Unk70 = br.ReadUInt32();
                Unk74 = br.ReadUInt32();
                Unk78 = br.ReadUInt32();
                Unk7C = br.ReadUInt32();
                Unk80 = br.ReadUInt32();
                Unk84 = br.ReadUInt32();
                Unk88 = br.ReadUInt32();
                Unk8C = br.ReadUInt32();
                Unk90 = br.ReadUInt32();
                Unk94 = br.ReadUInt32();
                Unk98 = br.ReadUInt32();
                Unk9C = br.ReadUInt32();
                UnkA0 = br.ReadUInt32();
                UnkA4 = br.ReadUInt16();
                UnkA6 = br.ReadUInt16();
                UnkA8 = br.ReadUInt32();
                UnkAC = br.ReadUInt16();
                UnkAE = br.ReadUInt16();
                UnkB0 = br.ReadUInt16();
                UnkB2 = br.ReadUInt16();
                UnkB4 = br.ReadUInt16();
                UnkB6 = br.ReadUInt16();
                UnkB8 = br.ReadUInt16();
                UnkBA = br.ReadUInt16();
                UnkBC = br.ReadUInt16();
                UnkBE = br.ReadUInt16();
                UnkC0 = br.ReadUInt16();
                UnkC2 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Leg part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);
                BodyComponent.Write(bw);

                bw.WriteUInt16(Unk1C);
                bw.WriteUInt16(Unk1E);
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(Load);
                bw.WriteUInt16(Unk24);
                bw.WriteUInt16(Unk26);
                bw.WriteUInt32(Unk28);
                bw.WriteUInt32(Unk2C);
                bw.WriteUInt32(Unk30);
                bw.WriteUInt32(Unk34);
                bw.WriteUInt32(Unk38);
                bw.WriteUInt32(Stability);
                bw.WriteUInt32(Unk40);
                bw.WriteUInt32(Unk44);
                bw.WriteUInt32(Unk48);
                bw.WriteInt32(TurningAbility);
                bw.WriteUInt32(Unk50);
                bw.WriteUInt32(Unk54);
                bw.WriteUInt32(Unk58);
                bw.WriteUInt32(Unk5C);
                bw.WriteUInt32(Unk60);
                bw.WriteUInt16(Unk62);
                bw.WriteUInt16(Unk64);
                bw.WriteUInt16(Unk66);
                bw.WriteUInt16(Unk68);
                bw.WriteUInt16(Unk6A);
                bw.WriteUInt16(Unk6C);
                bw.WriteUInt32(Unk70);
                bw.WriteUInt32(Unk74);
                bw.WriteUInt32(Unk78);
                bw.WriteUInt32(Unk7C);
                bw.WriteUInt32(Unk80);
                bw.WriteUInt32(Unk84);
                bw.WriteUInt32(Unk88);
                bw.WriteUInt32(Unk8C);
                bw.WriteUInt32(Unk90);
                bw.WriteUInt32(Unk94);
                bw.WriteUInt32(Unk98);
                bw.WriteUInt32(Unk9C);
                bw.WriteUInt32(UnkA0);
                bw.WriteUInt16(UnkA4);
                bw.WriteUInt16(UnkA6);
                bw.WriteUInt32(UnkA8);
                bw.WriteUInt16(UnkAC);
                bw.WriteUInt16(UnkAE);
                bw.WriteUInt16(UnkB0);
                bw.WriteUInt16(UnkB2);
                bw.WriteUInt16(UnkB4);
                bw.WriteUInt16(UnkB6);
                bw.WriteUInt16(UnkB8);
                bw.WriteUInt16(UnkBA);
                bw.WriteUInt16(UnkBC);
                bw.WriteUInt16(UnkBE);
                bw.WriteUInt16(UnkC0);
                bw.WriteUInt16(UnkC2);
            }
        }
    }
}
