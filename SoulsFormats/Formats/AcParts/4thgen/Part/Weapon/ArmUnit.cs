using static SoulsFormats.AcParts4.Component;

namespace SoulsFormats
{
    public partial class AcParts4
    {
        public partial class Part
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
                    /// The Arm Unit requires a tank hangar to be hangared.
                    /// </summary>
                    TankOnly = 1,

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
                public WeaponComponent WeaponComponent { get; set; }

                /// <summary>
                /// The hangar requirements of this Arm Unit.
                /// </summary>
                public HangarType HangarRequirement { get; set; }

                /// <summary>
                /// Changes what stat descriptions are pulled from assemmenu.bin, assumed to be an index of some kind.
                /// Seen as a labeled stat like how types are in the txt files.
                /// </summary>
                public byte DisplayType { get; set; }

                /// <summary>
                /// Unknown; Is always 0.
                /// </summary>
                public ushort Unk56 { get; set; }

                /// <summary>
                /// Reads an Arm Unit part from a stream.
                /// </summary>
                /// <param name="br">A binary reader.</param>
                /// <param name="version">The version indicating which 4thgen game's AcParts is being read.</param>
                internal ArmUnit(BinaryReaderEx br, AcParts4Version version)
                {
                    PartComponent = new PartComponent(br, version);
                    WeaponComponent = new WeaponComponent(br);

                    HangarRequirement = br.ReadEnum8<HangarType>();
                    DisplayType = br.ReadByte();
                    Unk56 = br.ReadUInt16();
                }

                /// <summary>
                /// Writes an Arm Unit part to a stream.
                /// </summary>
                /// <param name="bw">A binary writer.</param>
                /// <param name="version">The version indicating which 4thgen game's AcParts is being written.</param>
                public void Write(BinaryWriterEx bw, AcParts4Version version)
                {
                    PartComponent.Write(bw, version);
                    WeaponComponent.Write(bw);

                    bw.WriteByte((byte)HangarRequirement);
                    bw.WriteByte(DisplayType);
                    bw.WriteUInt16(Unk56);
                }
            }
        }
    }
}
