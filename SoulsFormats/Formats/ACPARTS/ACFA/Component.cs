namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        public class PartComponent
        {
            /// <summary>
            /// The category of part this part is.
            /// </summary>
            public enum PartCategory : byte
            {
                /// <summary>
                /// A head part.
                /// </summary>
                Head = 0,

                /// <summary>
                /// A core, or torso part.
                /// </summary>
                Core = 1,

                /// <summary>
                /// An arms part.
                /// </summary>
                Arms = 2,

                /// <summary>
                /// A legs part.
                /// </summary>
                Legs = 3,

                /// <summary>
                /// A Fire Control System part for locking on.
                /// </summary>
                FCS = 4,

                /// <summary>
                /// A Generator pat for generating energy.
                /// </summary>
                Generator = 5,

                /// <summary>
                /// A Main Booster part for forward movement.
                /// </summary>
                MainBooster = 6,

                /// <summary>
                /// A Back Booster part for backwards movement.
                /// </summary>
                BackBooster = 7,

                /// <summary>
                /// A Side Booster part for side-to-side movement.
                /// </summary>
                SideBooster = 8,

                /// <summary>
                /// A Overed Booster part for over boosting in forward movement.
                /// </summary>
                OveredBooster = 9,

                /// <summary>
                /// An Arm Unit part, for hand-held weapons.
                /// </summary>
                ArmUnit = 10,

                /// <summary>
                /// A Back Unit part for over-the-shoulder weapons and other misc parts.
                /// </summary>
                BackUnit = 11,

                /// <summary>
                /// A Shoulder Unit part for weapons or other misc parts on the sides of Shoulders.
                /// </summary>
                ShoulderUnit = 12,

                /// <summary>
                /// A Stabilizer on the top of Heads.
                /// </summary>
                HeadTopStabilizer = 13,

                /// <summary>
                /// Stabilizers on the sides of Heads.
                /// </summary>
                HeadSideStabilizer = 14,

                /// <summary>
                /// Stabilizers on the upper sides of Cores.
                /// </summary>
                CoreUpperSideStabilizer = 15,

                /// <summary>
                /// Stabilizers on the lower sides of Cores.
                /// </summary>
                CoreLowerSideStabilizer = 16,

                /// <summary>
                /// Stabilizers on the sides of Arms.
                /// </summary>
                ArmStabilizer = 17,

                /// <summary>
                /// Stabilizers on the back of a set of Legs.
                /// </summary>
                LegBackStabilizer = 18,

                /// <summary>
                /// Stabilizers on the upper sides of Legs.
                /// </summary>
                LegUpperStabilizer = 19,

                /// <summary>
                /// Stabilizers on the middle sides of Legs.
                /// </summary>
                LegMiddleStabilizer = 20,

                /// <summary>
                /// Stabilizers on the lower sides of Legs.
                /// </summary>
                LegLowerStabilizer = 21
            }

            /// <summary>
            /// The ID for this part, used to identify it in many places.
            /// </summary>
            public ushort PartID { get; set; }

            /// <summary>
            /// The ID of the model used by this part, formatted with four digits.
            /// </summary>
            public ushort ModelID { get; set; }

            /// <summary>
            /// The price of this part in the shop.
            /// </summary>
            public int Price { get; set; }

            /// <summary>
            /// The part's weight.
            /// Heavier parts will cause your AC to move more slowly.
            /// </summary>
            public ushort Weight { get; set; }

            /// <summary>
            /// The energy cost of this part
            /// </summary>
            public ushort ENCost { get; set; }

            /// <summary>
            /// The category/type of part this part is.
            /// </summary>
            public PartCategory Category { get; set; }

            /// <summary>
            /// Unknown; May be a part of the Category identifier; Is always 0.
            /// </summary>
            public byte Unk0F { get; set; }

            /// <summary>
            /// Unknown; Is an ID of some kind;
            /// Only set on Arm Units, Back Units, and Shoulder Units.
            /// </summary>
            public ushort UnkID { get; set; }

            /// <summary>
            /// The Name of this part.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The name of the company who makes of this part.
            /// </summary>
            public string MakerName { get; set; }

            /// <summary>
            /// A description describing weight group, part type, or other misc things.
            /// </summary>
            public string SubCategory { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk70 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk72 { get; set; }

            /// <summary>
            /// An internal description describing this part, likely used by FromSoftware development tools.
            /// </summary>
            public string Explain { get; set; }

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

                Category = br.ReadEnum8<PartCategory>();
                Unk0F = br.ReadByte();
                UnkID = br.ReadUInt16();
                Name = br.ReadFixStr(0x20);
                MakerName = br.ReadFixStr(0x20);
                SubCategory = br.ReadFixStr(0x20);

                Unk70 = br.ReadUInt16();
                Unk72 = br.ReadUInt16();
                Explain = br.ReadFixStr(0xFC);
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

                bw.WriteByte((byte)Category);
                bw.WriteByte(Unk0F);
                bw.WriteUInt16(UnkID);
                bw.WriteFixStr(Name, 0x20);
                bw.WriteFixStr(MakerName, 0x20);
                bw.WriteFixStr(SubCategory, 0x20);

                bw.WriteUInt16(Unk70);
                bw.WriteUInt16(Unk72);
                bw.WriteFixStr(Explain, 0xFC);
            }
        }

        /// <summary>
        /// A Component which contains Defense stats.
        /// </summary>
        public class DefenseComponent
        {
            /// <summary>
            /// Ability to withstand damage from projectiles and shells.
            /// </summary>
            public ushort BallisticDefense { get; set; }

            /// <summary>
            /// Ability to withstand damage from energy weapons.
            /// </summary>
            public ushort EnergyDefense { get; set; }

            /// <summary>
            /// Reads a Defense component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal DefenseComponent(BinaryReaderEx br)
            {
                BallisticDefense = br.ReadUInt16();
                EnergyDefense = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Defense component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(BallisticDefense);
                bw.WriteUInt16(EnergyDefense);
            }
        }

        /// <summary>
        /// A Component which contains Primal Armor stats.
        /// </summary>
        public class PAComponent
        {
            /// <summary>
            /// The Primal Armor Rectification added for equipping this part.
            /// </summary>
            public ushort PARectification { get; set; }

            /// <summary>
            /// The Primal Armor Durability added for equipping this part.
            /// </summary>
            public ushort PADurability { get; set; }

            /// <summary>
            /// Reads a PA component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal PAComponent(BinaryReaderEx br)
            {
                PARectification = br.ReadUInt16();
                PADurability = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a PA component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(PARectification);
                bw.WriteUInt16(PADurability);
            }
        }

        /// <summary>
        /// A Component which contains frame part stats.
        /// </summary>
        public class FrameComponent
        {
            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort RectificationTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool RectificationCanBeTuned { get; set; }

            /// <summary>
            /// Durability. The larger this value, the more durable the part.
            /// </summary>
            public ushort AP { get; set; }

            /// <summary>
            /// Unknown; Is always 0, may be a CanBeTuned boolean.
            /// </summary>
            public ushort Unk06 { get; set; }

            /// <summary>
            /// How much drag the frame part has, slowing it down when moving forward.
            /// </summary>
            public float DragCoefficient { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0E { get; set; }

            /// <summary>
            /// Unknown; Commonly the same across all of its part type.
            /// </summary>
            public ushort Unk10 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk12 { get; set; }

            /// <summary>
            /// Reads a Frame component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal FrameComponent(BinaryReaderEx br)
            {
                RectificationTuneTarget = br.ReadUInt16();
                RectificationCanBeTuned = br.AssertInt16(0, 1) == 1;
                AP = br.ReadUInt16();
                Unk06 = br.ReadUInt16();
                DragCoefficient = br.ReadSingle();
                Unk0C = br.ReadUInt16();
                Unk0E = br.ReadUInt16();
                Unk10 = br.ReadUInt16();
                Unk12 = br.ReadUInt16();
            }

            /// <summary>
            /// Writes a Frame component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(RectificationTuneTarget);
                bw.WriteUInt16((ushort)(RectificationCanBeTuned ? 1 : 0));
                bw.WriteUInt16(AP);
                bw.WriteUInt16(Unk06);
                bw.WriteSingle(DragCoefficient);
                bw.WriteUInt16(Unk0C);
                bw.WriteUInt16(Unk0E);
                bw.WriteUInt16(Unk10);
                bw.WriteUInt16(Unk12);
            }
        }

        /// <summary>
        /// A Component which contains stats for weapons.
        /// </summary>
        public class WeaponComponent
        {
            /// <summary>
            /// The firing mode of the weapon determining how it behaves when it is fired.
            /// </summary>
            public enum FiringMode : byte
            {
                /// <summary>
                /// The weapon can be fired by holding the trigger without needing to let go.
                /// </summary>
                Automatic = 0,

                /// <summary>
                /// The weapon requires the trigger to be pressed each time it is fired.
                /// </summary>
                Single = 1,

                /// <summary>
                /// The weapon fires like Automatic does,
                /// but goes into brace mode after pressing the firing button while standing still on ground.
                /// The weapon also fires faster in brace mode and allows for wider attack angles.
                /// </summary>
                Cannon = 2,

                /// <summary>
                /// The weapon is melee and lunges towards its enemies.
                /// </summary>
                LaserBlade = 4,

                /// <summary>
                /// The weapon is melee and does not lunge.
                /// </summary>
                ParryBlade = 5,
            }

            /// <summary>
            /// Whether or not the weapon has lock on, or is manual with no lock on.
            /// </summary>
            public enum LockOnMode : byte
            {
                /// <summary>
                /// The weapon does not use lock on.
                /// </summary>
                Manual = 0,

                /// <summary>
                /// The weapon uses lock on.
                /// </summary>
                Automatic = 1
            }

            /// <summary>
            /// The firing mode of the weapon determining how it behaves when it is fired.
            /// </summary>
            public FiringMode WeaponFiringMode { get; set; }

            /// <summary>
            /// Whether or not the weapon has lock on, or is manual with no lock on.
            /// </summary>
            public LockOnMode WeaponLockOnMode { get; set; }

            /// <summary>
            /// Unknown; Used on missile weapons usually.
            /// </summary>
            public ushort Unk02 { get; set; }

            /// <summary>
            /// The effective firing range of the weapon.
            /// Larger values allow you to target more distant enemies.
            /// </summary>
            public ushort FiringRange { get; set; }

            /// <summary>
            /// Ability to wield close quarter combat weapons. 
            /// Larger values indicate increased weapon maneuverability,
            /// improving close combat effectiveness.
            /// </summary>
            public ushort MeleeAbility { get; set; }

            /// <summary>
            /// The bullet ID from bullet params to generate when firing this weapon.
            /// </summary>
            public uint BulletID { get; set; }

            /// <summary>
            /// What SFX to use, determines the appearance of the bullets fired from this weapon.
            /// </summary>
            public uint SFXID { get; set; }

            /// <summary>
            /// What landing effect to use.
            /// </summary>
            public uint HitEffectID { get; set; }

            /// <summary>
            /// Velocity of fired ammunition.
            /// </summary>
            public float BallisticsVelocity { get; set; }

            /// <summary>
            /// Energy cost upon activation.
            /// </summary>
            public float ActivationCostEN { get; set; }

            /// <summary>
            /// Unknown; Is always 0 or 1.
            /// </summary>
            public byte Unk1C { get; set; }

            /// <summary>
            /// Number of projectiles fired in a single launch or shot.
            /// </summary>
            public byte ProjectileCount { get; set; }

            /// <summary>
            /// Number of projectiles fired in a row in a single launch.
            /// </summary>
            public byte ContinuousFireCount { get; set; }

            /// <summary>
            /// Unknown; Is always 0.
            /// </summary>
            public byte Unk1F { get; set; }

            /// <summary>
            /// Unknown; Is always 0, may be part of TimeBetweenContinuousShots, this is untested.
            /// </summary>
            public ushort Unk20 { get; set; }

            /// <summary>
            /// How long to wait between continuous shots.
            /// </summary>
            public ushort TimeBetweenContinuousShots { get; set; }

            /// <summary>
            /// After firing, time required before weapon can be fired again.
            /// </summary>
            public ushort FireRate { get; set; }

            /// <summary>
            /// Amount of recoil when firing the weapon.
            /// </summary>
            public ushort Recoil { get; set; }

            /// <summary>
            /// The cost of firing a single projectile (COAM Units).
            /// </summary>
            public ushort CostPerRound { get; set; }

            /// <summary>
            /// Indicates the amount that fired shots will deviate from the targeted point.
            /// The larger the value, the more accurate the weapon.
            /// Currently unknown how this value is factored.
            /// </summary>
            public ushort ShotPrecision { get; set; }

            /// <summary>
            /// Total number of magazines available.
            /// </summary>
            public ushort NumberofMagazines { get; set; }

            /// <summary>
            /// The number of rounds in a single magazine.
            /// </summary>
            public ushort MagazineCapacity { get; set; }

            /// <summary>
            /// How fast this weapon reloads.
            /// </summary>
            public ushort MagazineReloadTime { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk32 { get; set; }

            /// <summary>
            /// Amount of time required to recharge Kojima particles. 
            /// Attack power increases related to charge time.
            /// </summary>
            public ushort ChargeTime { get; set; }

            /// <summary>
            /// While Kojima Particles are being charged, the amount of KP consumed over time.
            /// </summary>
            public ushort KPChargeCost { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk38 { get; set; }

            /// <summary>
            /// After use, time required before the weapon is available.
            /// </summary>
            public ushort AttackLatency { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk3E { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk40 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk41 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk42 { get; set; }

            /// <summary>
            /// Indicates attack power of this type of weaponry.
            /// The larger this value, the greater damage the AC can deliver.
            /// </summary>
            public float AttackPower { get; set; }

            /// <summary>
            /// Amount of force delivered upon striking the target.
            /// The larger the value, the greater the impact.
            /// </summary>
            public float ImpactForce { get; set; }

            /// <summary>
            /// Ability to reduce PA when striking the target.
            /// The larger the value, the more damage PA can receive.
            /// </summary>
            public float PAAttentuation { get; set; }

            /// <summary>
            /// Ability to successfully penetrate PA.
            /// The larger the value, the less protection PA affords.
            /// </summary>
            public float PAPenetration { get; set; }

            /// <summary>
            /// Reads a Projectile component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal WeaponComponent(BinaryReaderEx br)
            {
                WeaponFiringMode = br.ReadEnum8<FiringMode>();
                WeaponLockOnMode = br.ReadEnum8<LockOnMode>();
                Unk02 = br.ReadUInt16();
                FiringRange = br.ReadUInt16();
                MeleeAbility = br.ReadUInt16();
                BulletID = br.ReadUInt32();
                SFXID = br.ReadUInt32();
                HitEffectID = br.ReadUInt32();
                BallisticsVelocity = br.ReadSingle();
                ActivationCostEN = br.ReadSingle();
                Unk1C = br.ReadByte();
                ProjectileCount = br.ReadByte();
                ContinuousFireCount = br.ReadByte();
                Unk1F = br.ReadByte();
                Unk20 = br.ReadUInt16();
                TimeBetweenContinuousShots = br.ReadUInt16();
                FireRate = br.ReadUInt16();
                Recoil = br.ReadUInt16();
                CostPerRound = br.ReadUInt16();
                ShotPrecision = br.ReadUInt16();
                NumberofMagazines = br.ReadUInt16();
                MagazineCapacity = br.ReadUInt16();
                MagazineReloadTime = br.ReadUInt16();
                Unk32 = br.ReadUInt16();
                ChargeTime = br.ReadUInt16();
                KPChargeCost = br.ReadUInt16();
                Unk38 = br.ReadSingle();
                AttackLatency = br.ReadUInt16();
                Unk3E = br.ReadUInt16();
                Unk40 = br.ReadByte();
                Unk41 = br.ReadByte();
                Unk42 = br.ReadUInt16();
                AttackPower = br.ReadSingle();
                ImpactForce = br.ReadSingle();
                PAAttentuation = br.ReadSingle();
                PAPenetration = br.ReadSingle();
            }

            /// <summary>
            /// Writes a Projectile component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteByte((byte)WeaponFiringMode);
                bw.WriteByte((byte)WeaponLockOnMode);
                bw.WriteUInt16(Unk02);
                bw.WriteUInt16(FiringRange);
                bw.WriteUInt16(MeleeAbility);
                bw.WriteUInt32(BulletID);
                bw.WriteUInt32(SFXID);
                bw.WriteUInt32(HitEffectID);
                bw.WriteSingle(BallisticsVelocity);
                bw.WriteSingle(ActivationCostEN);
                bw.WriteByte(Unk1C);
                bw.WriteByte(ProjectileCount);
                bw.WriteByte(ContinuousFireCount);
                bw.WriteByte(Unk1F);
                bw.WriteUInt16(Unk20);
                bw.WriteUInt16(TimeBetweenContinuousShots);
                bw.WriteUInt16(FireRate);
                bw.WriteUInt16(Recoil);
                bw.WriteUInt16(CostPerRound);
                bw.WriteUInt16(ShotPrecision);
                bw.WriteUInt16(NumberofMagazines);
                bw.WriteUInt16(MagazineCapacity);
                bw.WriteUInt16(MagazineReloadTime);
                bw.WriteUInt16(Unk32);
                bw.WriteUInt16(ChargeTime);
                bw.WriteUInt16(KPChargeCost);
                bw.WriteSingle(Unk38);
                bw.WriteUInt16(AttackLatency);
                bw.WriteUInt16(Unk3E);
                bw.WriteByte(Unk40);
                bw.WriteByte(Unk41);
                bw.WriteUInt16(Unk42);
                bw.WriteSingle(AttackPower);
                bw.WriteSingle(ImpactForce);
                bw.WriteSingle(PAAttentuation);
                bw.WriteSingle(PAPenetration);
            }
        }

        /// <summary>
        /// A Component which contains Radar stats.
        /// </summary>
        public class RadarComponent
        {
            /// <summary>
            /// The effective range of enemy detection radar.
            /// Larger values allow you to detect more distant targets.
            /// </summary>
            public ushort RadarRange { get; set; }

            /// <summary>
            /// Radar's resistance to ECM interference.
            /// The larger this value, the more this radar can withstand ECM.
            /// </summary>
            public ushort ECMResistance { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort ECMResistanceTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool ECMResistanceCanBeTuned { get; set; }

            /// <summary>
            /// Increases refresh rate of the radar HUD.
            /// Larger values indicate enemy positions are refreshed more frequently.
            /// </summary>
            public ushort RadarRefreshRate { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public ushort RadarRefreshRateTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool RadarRefreshRateCanBeTuned { get; set; }

            /// <summary>
            /// Reads a Radar component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal RadarComponent(BinaryReaderEx br)
            {
                RadarRange = br.ReadUInt16();
                ECMResistance = br.ReadUInt16();
                ECMResistanceTuneTarget = br.ReadUInt16();
                ECMResistanceCanBeTuned = br.AssertInt16(0, 1) == 1;
                RadarRefreshRate = br.ReadUInt16();
                RadarRefreshRateTuneTarget = br.ReadUInt16();
                RadarRefreshRateCanBeTuned = br.AssertInt16(0, 1) == 1;
            }

            /// <summary>
            /// Writes a Radar component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(RadarRange);
                bw.WriteUInt16(ECMResistance);
                bw.WriteUInt16(ECMResistanceTuneTarget);
                bw.WriteUInt16((ushort)(ECMResistanceCanBeTuned ? 1 : 0));
                bw.WriteUInt16(RadarRefreshRate);
                bw.WriteUInt16(RadarRefreshRateTuneTarget);
                bw.WriteUInt16((ushort)(RadarRefreshRateCanBeTuned ? 1 : 0));
            }
        }

        /// <summary>
        /// A Component which contains Assault Cannon stats for Weapons.
        /// </summary>
        public class AssaultCannonComponent
        {
            /// <summary>
            /// Attack power delivered when Assault Cannon hits the target.
            /// The larger this value, the greater the damage the Assault Cannon can deliver.
            /// Considered energy weaponry.
            /// </summary>
            public ushort AssaultCannonAttackPower { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk02 { get; set; }

            /// <summary>
            /// Impact delivered by the AC when it strikes the target.
            /// The larger the value, the greater the impact.
            /// </summary>
            public ushort AssaultCannonImpact { get; set; }

            /// <summary>
            /// Level of damage to Primal Armor when attacking with an Assault Cannon.
            /// The larger the value, the greater the damage.
            /// </summary>
            public ushort AssaultCannonAttentuation { get; set; }

            /// <summary>
            /// Level of Primal Armor penetration when hit with an Assault Cannon.
            /// The larger the value, the less effective the Primal Armor will be.
            /// </summary>
            public ushort AssaultCannonPenetration { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ushort Unk0A { get; set; }

            /// <summary>
            /// Reads an Assault Cannon component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal AssaultCannonComponent(BinaryReaderEx br)
            {
                AssaultCannonAttackPower = br.ReadUInt16();
                Unk02 = br.ReadUInt16();
                AssaultCannonImpact = br.ReadUInt16();
                AssaultCannonAttentuation = br.ReadUInt16();
                AssaultCannonPenetration = br.ReadUInt16();
                Unk0A = br.ReadUInt16();
            }

            /// <summary>
            /// Writes an Assault Cannon component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt16(AssaultCannonAttackPower);
                bw.WriteUInt16(Unk02);
                bw.WriteUInt16(AssaultCannonImpact);
                bw.WriteUInt16(AssaultCannonAttentuation);
                bw.WriteUInt16(AssaultCannonPenetration);
                bw.WriteUInt16(Unk0A);
            }
        }

        /// <summary>
        /// A Component which contains Booster stats for Weapons.
        /// </summary>
        public class WeaponBoosterComponent
        {
            /// <summary>
            /// Capacity for horizontal thrust using standard boost.
            /// A larger value allows for faster lateral movement.
            /// </summary>
            public uint HorizontalThrust { get; set; }

            /// <summary>
            /// Capacity for vertical thrust using standard boost.
            /// A larger value allows for faster vertical movement.
            /// </summary>
            public uint VerticalThrust { get; set; }

            /// <summary>
            /// Amount of thrust produced during quick boost.
            /// A larger value indicates faster quick boost movement.
            /// </summary>
            public uint QuickBoost { get; set; }

            /// <summary>
            /// Unknown; Likely is another type of Boost.
            /// </summary>
            public uint Unk0C { get; set; }

            /// <summary>
            /// Energy consumed when engaging horizontal thrust.
            /// Larger values indicate a greater EN cost requirement for use.
            /// </summary>
            public uint HorizontalENCost { get; set; }

            /// <summary>
            /// Energy consumed when engaging vertical thrust.
            /// Larger values indicate a greater EN cost requirement for use.
            /// </summary>
            public uint VerticalENCost { get; set; }

            /// <summary>
            /// Energy consumed when engaging quick boost.
            /// Larger values indicate a greater EN cost requirement for use.
            /// </summary>
            public uint QuickBoostENCost { get; set; }

            /// <summary>
            /// Unknown; Likely is another type of Boost's EN cost.
            /// </summary>
            public uint Unk1C { get; set; }

            /// <summary>
            /// Reads a Weapon Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal WeaponBoosterComponent(BinaryReaderEx br)
            {
                HorizontalThrust = br.ReadUInt32();
                VerticalThrust = br.ReadUInt32();
                QuickBoost = br.ReadUInt32();
                Unk0C = br.ReadUInt32();
                HorizontalENCost = br.ReadUInt32();
                VerticalENCost = br.ReadUInt32();
                QuickBoostENCost = br.ReadUInt32();
                Unk1C = br.ReadUInt32();
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
                bw.WriteUInt32(Unk0C);
                bw.WriteUInt32(HorizontalENCost);
                bw.WriteUInt32(VerticalENCost);
                bw.WriteUInt32(QuickBoostENCost);
                bw.WriteUInt32(Unk1C);
            }
        }

        /// <summary>
        /// A Component which contains Booster stats.
        /// </summary>
        public class BoosterComponent
        {
            /// <summary>
            /// Capacity for thrust using standard boost.
            /// A larger value allows for faster movement in the specified direction.
            /// </summary>
            public uint Thrust { get; set; }

            /// <summary>
            /// The value the tune slider will set the mentioned stat to at the end of the slider, 50 FRS points in.
            /// </summary>
            public uint ThrustTuneTarget { get; set; }

            /// <summary>
            /// Whether or not putting FRS points into tuning the stat will change it or not.
            /// </summary>
            public bool ThrustCanBeTuned { get; set; }

            /// <summary>
            /// Only usable on Quick Booster stats.
            /// Length of time quick boost can be engaged.
            /// The larger this value, the further the AC can move during
            /// quick boost, but overall energy consumption will increase.
            /// </summary>
            public ushort QuickBoostDuration { get; set; }

            /// <summary>
            /// Energy consumed when engaging thrust.
            /// Larger values indicate a greater EN cost requirement for use.
            /// </summary>
            public uint ThrustENCost { get; set; }

            /// <summary>
            /// Reads a Quick Booster component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal BoosterComponent(BinaryReaderEx br)
            {
                Thrust = br.ReadUInt32();
                ThrustTuneTarget = br.ReadUInt32();
                ThrustCanBeTuned = br.AssertInt16(0, 1) == 1;
                QuickBoostDuration = br.ReadUInt16();
                ThrustENCost = br.ReadUInt32();
            }

            /// <summary>
            /// Writes a Quick Booster component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteUInt32(Thrust);
                bw.WriteUInt32(ThrustTuneTarget);
                bw.WriteUInt16((ushort)(ThrustCanBeTuned ? 1 : 0));
                bw.WriteUInt16(QuickBoostDuration);
                bw.WriteUInt32(ThrustENCost);
            }
        }

        /// <summary>
        /// A Component which contains Stabilizer stats.
        /// </summary>
        public class StabilizerComponent
        {
            /// <summary>
            /// The stabilizer category of this stabilizer, should be the same as part category in stabilizer part component.
            /// </summary>
            public byte Category { get; set; }

            /// <summary>
            /// Corrects the AC's center of gravity.
            /// The larger the value, the more the center of gravity will be corrected.
            /// </summary>
            public float ControlCalibration { get; set; }

            /// <summary>
            /// Reads a Stabilizer component from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal StabilizerComponent(BinaryReaderEx br)
            {
                Category = br.ReadByte();
                br.AssertByte(0);
                br.AssertByte(0);
                br.AssertByte(0);
                ControlCalibration = br.ReadSingle();
            }

            /// <summary>
            /// Writes a Stabilizer component to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                bw.WriteByte(Category);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteSingle(ControlCalibration);
            }
        }
    }
}
