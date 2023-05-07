namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// A Head part in an ACPARTS file.
        /// </summary>
        public class Head : IPart, IFrame
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
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// A Component which contains frame part stats.
            /// </summary>
            public FrameComponent FrameComponent { get; set; }

            /// <summary>
            /// Indicates the stability of the AC.
            /// The larger the value, the more stable the AC.
            /// </summary>
            public ushort Stability { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1E { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort StabilityTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool StabilityCanBeTuned { get; set; }

            /// <summary>
            /// Functional effectiveness of the head-mounted camera eye. 
            /// Improves base FCS lock-on capability.
            /// </summary>
            public ushort CameraFunctionality { get; set; }

            /// <summary>
            /// Ability to recover from flash related interference.
            /// The larger this value, the faster the recovery.
            /// </summary>
            public ushort SystemRecovery { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk30 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// Reads a Head part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Head(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                PAComponent = new PAComponent(br);
                FrameComponent = new FrameComponent(br);

                Stability = br.ReadUInt16();
                Unk1E = br.ReadUInt16();
                StabilityTuneTarget = br.ReadUInt16();
                StabilityCanBeTuned = br.AssertInt16(0, 1) == 1;
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
                PAComponent.Write(bw);
                FrameComponent.Write(bw);

                bw.WriteUInt16(Stability);
                bw.WriteUInt16(Unk1E);
                bw.WriteUInt16(StabilityTuneTarget);
                bw.WriteUInt16((ushort)(StabilityCanBeTuned ? 1 : 0));
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
        public class Core : IPart, IFrame
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
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// A Component which contains frame part stats.
            /// </summary>
            public FrameComponent FrameComponent { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk1C { get; set; }

            /// <summary>
            /// Assumed to be Unk1C's Tune Target from positioning.
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort Unk1CTuneTarget { get; set; }

            /// <summary>
            /// Assumed to be Unk1C's CanBeTuned from positioning.
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool Unk1CCanBeTuned { get; set; }

            /// <summary>
            /// Indicates the stability of the AC.
            /// The larger the value, the more stable the AC.
            /// </summary>
            public ushort Stability { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort StabilityTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool StabilityCanBeTuned { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk28 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk2A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk2C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk2E { get; set; }

            /// <summary>
            /// Reads a Core part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Core(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                PAComponent = new PAComponent(br);
                FrameComponent = new FrameComponent(br);

                Unk1C = br.ReadUInt16();
                Unk1CTuneTarget = br.ReadUInt16();
                Unk1CCanBeTuned = br.AssertInt16(0, 1) == 1;
                Stability = br.ReadUInt16();
                StabilityTuneTarget = br.ReadUInt16();
                StabilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                Unk28 = br.ReadInt16();
                Unk2A = br.ReadInt16();
                Unk2C = br.ReadInt16();
                Unk2E = br.ReadInt16();
            }

            /// <summary>
            /// Writes a Core part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                DefenseComponent.Write(bw);
                PAComponent.Write(bw);
                FrameComponent.Write(bw);

                bw.WriteUInt16(Unk1C);
                bw.WriteUInt16(Unk1CTuneTarget);
                bw.WriteUInt16((ushort)(Unk1CCanBeTuned ? 1 : 0));
                bw.WriteUInt16(Stability);
                bw.WriteUInt16(StabilityTuneTarget);
                bw.WriteUInt16((ushort)(StabilityCanBeTuned ? 1 : 0));
                bw.WriteInt16(Unk28);
                bw.WriteInt16(Unk2A);
                bw.WriteInt16(Unk2C);
                bw.WriteInt16(Unk2E);
            }
        }

        /// <summary>
        /// An Arm part in an ACPARTS file.
        /// </summary>
        public class Arm : IPart, IFrame, IWeapon
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
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// A Component which contains frame part stats.
            /// </summary>
            public FrameComponent FrameComponent { get; set; }

            /// <summary>
            /// Whether or not this Arm is a Weapon Arm and to use Weapon Arm Stats;
            /// Equipping Arm Units or having Hangar Units will be disabled upon enabling this.
            /// </summary>
            public bool IsWeaponArm { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public byte Unk1D { get; set; }

            /// <summary>
            /// Reduces loss of maneuverability due to weapon weight 
            /// and suppresses weapon recoil when firing.
            /// </summary>
            public ushort FiringStability { get; set; }

            /// <summary>
            /// Boosts attack power of energy weapons.
            /// A larger value indicates stronger energy weapon attack power.
            /// </summary>
            public ushort EnergyWeaponSkill { get; set; }

            /// <summary>
            /// Enables rapid weapon movement and targeting.
            /// Larger values indicate the weapon acquires enemies quickly.
            /// </summary>
            public ushort WeaponManeuverability { get; set; }

            /// <summary>
            /// Indicates weapon accuracy.
            /// The larger the value, the more accurate the attack.
            /// </summary>
            public ushort AimPrecision { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk26 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk28 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk2A { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort FiringStabilityTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool FiringStabilityCanBeTuned { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort EnergyWeaponSkillTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool EnergyWeaponSkillCanBeTuned { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort WeaponManeuverabilityTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool WeaponManeuverabilityCanBeTuned { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort AimPrecisionTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool AimPrecisionCanBeTuned { get; set; }

            /// <summary>
            /// Unknown; Is always 0 (Other arms), 3 (Energy arms?), 5 (Missile arms?), or 11 (Kojima arms?).
            /// </summary>
            public byte Unk3C { get; set; }

            /// <summary>
            /// A description identifing whether or not an arm is a Weapon Arm; Likely for developers in developer tools at FromSoftware.
            /// </summary>
            public string ArmDescription { get; set; }

            /// <summary>
            /// The Weapon Arm Stats for this Arm.
            /// </summary>
            public WeaponComponent WeaponComponent { get; set; }

            /// <summary>
            /// Unknown; Is always 0x64 across all arms
            /// </summary>
            public ushort UnkA4 { get; set; }

            /// <summary>
            /// Unknown; Is always 0 across all arms
            /// </summary>
            public ushort UnkA6 { get; set; }

            /// <summary>
            /// Reads an Arm part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Arm(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                PAComponent = new PAComponent(br);
                FrameComponent = new FrameComponent(br);

                IsWeaponArm = br.ReadBoolean();
                Unk1D = br.ReadByte();
                FiringStability = br.ReadUInt16();
                EnergyWeaponSkill = br.ReadUInt16();
                WeaponManeuverability = br.ReadUInt16();
                AimPrecision = br.ReadUInt16();
                Unk26 = br.ReadUInt16();
                Unk28 = br.ReadUInt16();
                Unk2A = br.ReadUInt16();

                FiringStabilityTuneTarget = br.ReadUInt16();
                FiringStabilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                EnergyWeaponSkillTuneTarget = br.ReadUInt16();
                EnergyWeaponSkillCanBeTuned = br.AssertInt16(0, 1) == 1;
                WeaponManeuverabilityTuneTarget = br.ReadUInt16();
                WeaponManeuverabilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                AimPrecisionTuneTarget = br.ReadUInt16();
                AimPrecisionCanBeTuned = br.AssertInt16(0, 1) == 1;

                Unk3C = br.ReadByte();
                br.AssertByte(0);
                br.AssertByte(0);
                br.AssertByte(0);
                ArmDescription = br.ReadFixStr(0x10);

                WeaponComponent = new WeaponComponent(br);

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
                PAComponent.Write(bw);
                FrameComponent.Write(bw);

                bw.WriteBoolean(IsWeaponArm);
                bw.WriteByte(Unk1D);
                bw.WriteUInt16(FiringStability);
                bw.WriteUInt16(EnergyWeaponSkill);
                bw.WriteUInt16(WeaponManeuverability);
                bw.WriteUInt16(AimPrecision);
                bw.WriteUInt16(Unk26);
                bw.WriteUInt16(Unk28);
                bw.WriteUInt16(Unk2A);
                bw.WriteUInt16(FiringStabilityTuneTarget);
                bw.WriteUInt16((ushort)(FiringStabilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16(EnergyWeaponSkillTuneTarget);
                bw.WriteUInt16((ushort)(EnergyWeaponSkillCanBeTuned ? 1 : 0));
                bw.WriteUInt16(WeaponManeuverabilityTuneTarget);
                bw.WriteUInt16((ushort)(WeaponManeuverabilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16(AimPrecisionTuneTarget);
                bw.WriteUInt16((ushort)(AimPrecisionCanBeTuned ? 1 : 0));

                bw.WriteByte(Unk3C);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteFixStr(ArmDescription, 0x10);

                WeaponComponent.Write(bw);

                bw.WriteUInt16(UnkA4);
                bw.WriteUInt16(UnkA6);
            }
        }

        /// <summary>
        /// A Leg part in an ACPARTS file.
        /// </summary>
        public class Leg : IPart, IFrame
        {
            /// <summary>
            /// The leg type affecting legs in different ways such as hanger size and part category blacklisting.
            /// </summary>
            public enum LegType : byte
            {
                /// <summary>
                /// Bipedal leg type.
                /// </summary>
                Bipedal = 0,

                /// <summary>
                /// Reverse Joint leg type.
                /// </summary>
                ReverseJoint = 1,

                /// <summary>
                /// Quad leg type meaning four legs.
                /// </summary>
                Quad = 2,

                /// <summary>
                /// Tank leg type meaning tank legs.
                /// </summary>
                Tank = 3
            }

            /// <summary>
            /// The animation type affecting walking animation, laser blade swinging animation,
            /// </summary>
            public enum AnimationType : byte
            {
                /// <summary>
                /// The LegType used by Leg Parts with two legs of middle weight.
                /// </summary>
                TwoLegsMiddle = 0,

                /// <summary>
                /// The LegType used by Leg Parts with two reverse jointed legs.
                /// </summary>
                ReverseJoint = 1,

                /// <summary>
                /// The LegType used by Leg Parts with four legs.
                /// </summary>
                FourLegs = 2,

                /// <summary>
                /// The LegType used by Tank Leg Parts.
                /// Makes AC slide across the ground slowly when walking.
                /// Main Booster, Back Booster, and Leg Stabilizers are blacklisted and not allowed to be equipped.
                /// </summary>
                Tank = 3,

                /// <summary>
                /// The LegType used by Leg Parts with two legs of light weight.
                /// </summary>
                TwoLegsLight = 4,

                /// <summary>
                /// The LegType used by Leg Parts with two legs of heavy weight.
                /// </summary>
                TwoLegsHeavy = 5,

                /// <summary>
                /// A LegType only used by Lahire Legs, but can be applied to any.
                /// Has a good Laser Blade swing animation for combat.
                /// </summary>
                TwoLegsLightAlternate = 6
            }

            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Defense Stats.
            /// </summary>
            public DefenseComponent DefenseComponent { get; set; }

            /// <summary>
            /// A Component which contains Primal Armor stats.
            /// </summary>
            public PAComponent PAComponent { get; set; }

            /// <summary>
            /// A Component which contains frame part stats.
            /// </summary>
            public FrameComponent FrameComponent { get; set; }

            /// <summary>
            /// The chosen leg type affecting legs in unknown ways, does affect hanger size and part category blacklisting.
            /// </summary>
            public LegType Type { get; set; }

            /// <summary>
            /// The animation type of these legs, affecting several animations.
            /// Is likely an animation index choosing animations from common AC animations.
            /// </summary>
            public AnimationType LegAnimationType { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public ushort Unk1E { get; set; }

            /// <summary>
            /// The max amount of weight this Leg can hold;
            /// Going past this value turns the weight gauge red and will not allow exit from the Assemble menu.
            /// </summary>
            public ushort MaxLoad { get; set; }

            /// <summary>
            /// The amount of weight the leg units can bear. 
            /// The larger the value, the greater the carrying capacity.
            /// </summary>
            public ushort Load { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// The slider will have the increments inbetween calculated automatically.
            /// The value can be lower or higher than the current stat value if needed, lower means the slider will lower the stat.
            /// </summary>
            public ushort LoadTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool LoadCanBeTuned { get; set; }

            /// <summary>
            /// Raises back unit angle.
            /// Used on quad legs and tank legs to make sure back units do not clip.
            /// Causes camera to break at extremely high values.
            /// </summary>
            public int BackUnitAngle { get; set; }

            /// <summary>
            /// How fast these legs walk.
            /// </summary>
            public int MovementAbility { get; set; }

            /// <summary>
            /// Indicates turning ability of the AC.
            /// Larger values allow the AC to turn more quickly.
            /// Negative values reverse turning controls.
            /// </summary>
            public int TurningAbility { get; set; }

            /// <summary>
            /// How fast these legs are able to halt movement after landing and stopping.
            /// </summary>
            public int BrakingAbility { get; set; }

            /// <summary>
            /// How high these legs' initial jump speed is.
            /// </summary>
            public int JumpingAbility { get; set; }

            /// <summary>
            /// Indicates the stability of the AC.
            /// The larger the value, the more stable the AC.
            /// </summary>
            public int Stability { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk40 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk44 { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int MovementAbilityTuneTarget { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int TurningAbilityTuneTarget { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int BrakingAbilityTuneTarget { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int JumpingAbilityTuneTarget { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int StabilityTuneTarget { get; set; }

            /// <summary>
            /// Assumed to be Unk40 Tune Target from position.
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int Unk40TuneTarget { get; set; }

            /// <summary>
            /// Assumed to be Unk44 Tune Target from position.
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public int Unk44TuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool MovementAbilityCanBeTuned { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool TurningAbilityCanBeTuned { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool BrakingAbilityCanBeTuned { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool JumpingAbilityCanBeTuned { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool StabilityCanBeTuned { get; set; }

            /// <summary>
            /// Assumed to be a CanBeTuned from positioning.
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool UnknownCanBeTuned6E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public uint Unk70 { get; set; }

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
            /// Unknown.
            /// </summary>
            public ushort UnkA4 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkA6 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkA8 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkAA { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkAC { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkAE { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB0 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB2 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB4 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB6 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkB8 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBA { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBC { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkBE { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkC0 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort UnkC2 { get; set; }

            /// <summary>
            /// Reads a Leg part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal Leg(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                DefenseComponent = new DefenseComponent(br);
                PAComponent = new PAComponent(br);
                FrameComponent = new FrameComponent(br);
                Type = br.ReadEnum8<LegType>();
                LegAnimationType = br.ReadEnum8<AnimationType>();
                Unk1E = br.ReadUInt16();
                MaxLoad = br.ReadUInt16();
                Load = br.ReadUInt16();
                LoadTuneTarget = br.ReadUInt16();
                LoadCanBeTuned = br.AssertInt16(0, 1) == 1;
                BackUnitAngle = br.ReadInt32();
                MovementAbility = br.ReadInt32();
                TurningAbility = br.ReadInt32();
                BrakingAbility = br.ReadInt32();
                JumpingAbility = br.ReadInt32();
                Stability = br.ReadInt32();
                Unk40 = br.ReadInt32();
                Unk44 = br.ReadInt32();
                MovementAbilityTuneTarget = br.ReadInt32();
                TurningAbilityTuneTarget = br.ReadInt32();
                BrakingAbilityTuneTarget = br.ReadInt32();
                JumpingAbilityTuneTarget = br.ReadInt32();
                StabilityTuneTarget = br.ReadInt32();
                Unk40TuneTarget = br.ReadInt32();
                Unk44TuneTarget = br.ReadInt32();
                MovementAbilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                TurningAbilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                BrakingAbilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                JumpingAbilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                StabilityCanBeTuned = br.AssertInt16(0, 1) == 1;
                UnknownCanBeTuned6E = br.AssertInt16(0, 1) == 1;
                Unk70 = br.ReadUInt32();
                HorizontalBoost = new BoosterComponent(br);
                VerticalBoost = new BoosterComponent(br);
                QuickBoost = new BoosterComponent(br);
                UnkA4 = br.ReadUInt16();
                UnkA6 = br.ReadUInt16();
                UnkA8 = br.ReadUInt16();
                UnkAA = br.ReadUInt16();
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
                PAComponent.Write(bw);
                FrameComponent.Write(bw);
                bw.WriteByte((byte)Type);
                bw.WriteByte((byte)LegAnimationType);
                bw.WriteUInt16(Unk1E);
                bw.WriteUInt16(MaxLoad);
                bw.WriteUInt16(Load);
                bw.WriteUInt16(LoadTuneTarget);
                bw.WriteUInt16((ushort)(LoadCanBeTuned ? 1 : 0));
                bw.WriteInt32(BackUnitAngle);
                bw.WriteInt32(MovementAbility);
                bw.WriteInt32(TurningAbility);
                bw.WriteInt32(BrakingAbility);
                bw.WriteInt32(JumpingAbility);
                bw.WriteInt32(Stability);
                bw.WriteInt32(Unk40);
                bw.WriteInt32(Unk44);
                bw.WriteInt32(MovementAbilityTuneTarget);
                bw.WriteInt32(TurningAbilityTuneTarget);
                bw.WriteInt32(BrakingAbilityTuneTarget);
                bw.WriteInt32(JumpingAbilityTuneTarget);
                bw.WriteInt32(StabilityTuneTarget);
                bw.WriteInt32(Unk40TuneTarget);
                bw.WriteInt32(Unk44TuneTarget);
                bw.WriteUInt16((ushort)(MovementAbilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16((ushort)(TurningAbilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16((ushort)(BrakingAbilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16((ushort)(JumpingAbilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16((ushort)(StabilityCanBeTuned ? 1 : 0));
                bw.WriteUInt16((ushort)(UnknownCanBeTuned6E ? 1 : 0));
                bw.WriteUInt32(Unk70);
                HorizontalBoost.Write(bw);
                VerticalBoost.Write(bw);
                QuickBoost.Write(bw);
                bw.WriteUInt16(UnkA4);
                bw.WriteUInt16(UnkA6);
                bw.WriteUInt16(UnkA8);
                bw.WriteUInt16(UnkAA);
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
