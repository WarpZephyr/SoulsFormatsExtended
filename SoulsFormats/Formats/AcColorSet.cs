using System.Drawing;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// Armored Core AcColorSet files and structures, bin files of just the structure are formatted color%04d.bin.
    /// </summary>
    public class AcColorSet : SoulsFile<AcColorSet>
    {
        /// <summary>
        /// All the color sets in this AcColorSet.
        /// </summary>
        public AllColorSets ColorSets;

        /// <summary>
        /// The color pattern selections in this AcColorSet.
        /// </summary>
        public ColorPatternSelections PatternSelections;

        /// <summary>
        /// The head eye color of this AcColorSet.
        /// </summary>
        public Color EyeColor;

        /// <summary>
        /// Verify a byte array containing color pattern selections' bytes all do not go out of the correct value range.
        /// </summary>
        /// <param name="pattern">A byte array containing a color selections.</param>
        /// <returns>Whether or not a byte array containing color pattern selections' bytes all do not go out of the correct value range.</returns>
        public static bool VerifyColorPattern(byte[] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] < 0 || pattern[i] > 11)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a file may be an AcColorSet; WARNING: There is not a very good way to verify a file is an AcColorSet.
        /// </summary>
        public static bool Match(BinaryReaderEx br)
        {
            if (br.Length < 856)
                return false;

            byte[] pattern = br.GetBytes(816, 36);
            return br.Length == 856 && VerifyColorPattern(pattern);
        }

        /// <summary>
        /// Checks if a file may be an AcColorSet; WARNING: There is not a very good way to verify a file is an AcColorSet.
        /// </summary>
        public static bool Match(byte[] bytes)
        {
            if (bytes.Length == 0)
                return false;

            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return Match(SFUtil.GetDecompressedBR(br, out _));
        }

        /// <summary>
        /// Checks if a file may be an AcColorSet; WARNING: There is not a very good way to verify a file is an AcColorSet.
        /// </summary>
        public static bool Match(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                if (stream.Length == 0)
                    return false;

                BinaryReaderEx br = new BinaryReaderEx(false, stream);
                return Match(SFUtil.GetDecompressedBR(br, out _));
            }
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        public void Read(BinaryReaderEx br, bool bigendian)
        {
            br.BigEndian = bigendian;
            ColorSets = new AllColorSets(br);
            PatternSelections = new ColorPatternSelections(br);
            EyeColor = br.ReadRGBA();
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        public void Write(BinaryWriterEx bw, bool bigendian)
        {
            bw.BigEndian = bigendian;
            ColorSets.Write(bw);
            PatternSelections.Write(bw);
            bw.WriteRGBA(EyeColor);
        }

        /// <summary>
        /// Create a new, empty AcColorSet with all values set to their defaults.
        /// </summary>
        public AcColorSet()
        {
            ColorSets = new AllColorSets();
            PatternSelections = new ColorPatternSelections();
            EyeColor = Color.FromArgb(0, 0, 0, 0);
        }

        /// <summary>
        /// All color sets in this AcColorSet, excluding eye color.
        /// </summary>
        public class AllColorSets
        {
            /// <summary>
            /// All frame color sets.
            /// </summary>
            public FrameColorSets FrameColorSets;

            /// <summary>
            /// All unit color sets.
            /// </summary>
            public UnitColorSets UnitColorSets;

            /// <summary>
            /// All stabilizer color sets.
            /// </summary>
            public StabilizerColorSets StabilizerColorSets;

            /// <summary>
            /// Create a new, empty group of all color sets with all values set to their defaults.
            /// </summary>
            public AllColorSets()
            {
                FrameColorSets = new FrameColorSets();
                UnitColorSets = new UnitColorSets();
                StabilizerColorSets = new StabilizerColorSets();
            }

            /// <summary>
            /// Read all color sets from a stream.
            /// </summary>
            internal AllColorSets(BinaryReaderEx br)
            {
                FrameColorSets = new FrameColorSets(br);
                UnitColorSets = new UnitColorSets(br);
                StabilizerColorSets = new StabilizerColorSets(br);
            }

            /// <summary>
            /// Write all color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                FrameColorSets.Write(bw);
                UnitColorSets.Write(bw);
                StabilizerColorSets.Write(bw);
            }
        }

        /// <summary>
        /// All frame color sets in this AcColorSet.
        /// </summary>
        public class FrameColorSets
        {
            /// <summary>
            /// The head color set.
            /// </summary>
            public ColorSet HeadColor;

            /// <summary>
            /// The core color set.
            /// </summary>
            public ColorSet CoreColor;

            /// <summary>
            /// The right arm color set.
            /// </summary>
            public ColorSet ArmRightColor;

            /// <summary>
            /// The left arm color set.
            /// </summary>
            public ColorSet ArmLeftColor;

            /// <summary>
            /// The color set for legs.
            /// </summary>
            public ColorSet LegsColor;

            /// <summary>
            /// Create a new, empty group of all frame color sets with all values set to their defaults.
            /// </summary>
            public FrameColorSets()
            {
                HeadColor = new ColorSet(0, 0, 0, 0);
                CoreColor = new ColorSet(0, 0, 0, 0);
                ArmRightColor = new ColorSet(0, 0, 0, 0);
                ArmLeftColor = new ColorSet(0, 0, 0, 0);
                LegsColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all frame color sets from a stream.
            /// </summary>
            internal FrameColorSets(BinaryReaderEx br)
            {
                HeadColor = new ColorSet(br);
                CoreColor = new ColorSet(br);
                ArmRightColor = new ColorSet(br);
                ArmLeftColor = new ColorSet(br);
                LegsColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all frame color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                HeadColor.Write(bw);
                CoreColor.Write(bw);
                ArmRightColor.Write(bw);
                ArmLeftColor.Write(bw);
                LegsColor.Write(bw);
            }
        }

        /// <summary>
        /// All unit color sets in this AcColorSet.
        /// </summary>
        public class UnitColorSets
        {
            /// <summary>
            /// The right arm unit color set.
            /// </summary>
            public ColorSet ArmUnitRightColor;

            /// <summary>
            /// The left arm unit color set.
            /// </summary>
            public ColorSet ArmUnitLeftColor;

            /// <summary>
            /// The right back unit color set.
            /// </summary>
            public ColorSet BackUnitRightColor;

            /// <summary>
            /// The left back unit color set.
            /// </summary>
            public ColorSet BackUnitLeftColor;

            /// <summary>
            /// The shoulder unit color set.
            /// </summary>
            public ColorSet ShoulderUnitColor;

            /// <summary>
            /// The right hanger unit color set.
            /// </summary>
            public ColorSet HangerUnitRightColor;

            /// <summary>
            /// The left hanger unit color set.
            /// </summary>
            public ColorSet HangerUnitLeftColor;

            /// <summary>
            /// Create a new, empty group of all unit color sets with all values set to their defaults.
            /// </summary>
            public UnitColorSets()
            {
                ArmUnitRightColor = new ColorSet(0, 0, 0, 0);
                ArmUnitLeftColor = new ColorSet(0, 0, 0, 0);
                BackUnitRightColor = new ColorSet(0, 0, 0, 0);
                BackUnitLeftColor = new ColorSet(0, 0, 0, 0);
                ShoulderUnitColor = new ColorSet(0, 0, 0, 0);
                HangerUnitRightColor = new ColorSet(0, 0, 0, 0);
                HangerUnitLeftColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all unit color sets from a stream.
            /// </summary>
            internal UnitColorSets(BinaryReaderEx br)
            {
                ArmUnitRightColor = new ColorSet(br);
                ArmUnitLeftColor = new ColorSet(br);
                BackUnitRightColor = new ColorSet(br);
                BackUnitLeftColor = new ColorSet(br);
                ShoulderUnitColor = new ColorSet(br);
                HangerUnitRightColor = new ColorSet(br);
                HangerUnitLeftColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all unit color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                ArmUnitRightColor.Write(bw);
                ArmUnitLeftColor.Write(bw);
                BackUnitRightColor.Write(bw);
                BackUnitLeftColor.Write(bw);
                ShoulderUnitColor.Write(bw);
                HangerUnitRightColor.Write(bw);
                HangerUnitLeftColor.Write(bw);
            }
        }

        /// <summary>
        /// All stabilizer color sets in this AcColorSet.
        /// </summary>
        public class StabilizerColorSets
        {
            /// <summary>
            /// All head stabilizer color sets.
            /// </summary>
            public HeadStabilizerColorSets AllHeadStabilizerColorSets;

            /// <summary>
            /// All core stabilizer color sets.
            /// </summary>
            public CoreStabilizerColorSets AllCoreStabilizerColorSets;

            /// <summary>
            /// All arm stabilizer color sets.
            /// </summary>
            public ArmStabilizerColorSets AllArmStabilizerColorSets;

            /// <summary>
            /// All leg stabilizer color sets.
            /// </summary>
            public LegStabilizerColorSets AllLegStabilizerColorSets;

            /// <summary>
            /// Create a new, empty group of all stabilizer color sets with all values set to their defaults.
            /// </summary>
            public StabilizerColorSets()
            {
                AllHeadStabilizerColorSets = new HeadStabilizerColorSets();
                AllCoreStabilizerColorSets = new CoreStabilizerColorSets();
                AllArmStabilizerColorSets = new ArmStabilizerColorSets();
                AllLegStabilizerColorSets = new LegStabilizerColorSets();
            }

            /// <summary>
            /// Read all stabilizer color sets from a stream.
            /// </summary>
            internal StabilizerColorSets(BinaryReaderEx br)
            {
                AllHeadStabilizerColorSets = new HeadStabilizerColorSets(br);
                AllCoreStabilizerColorSets = new CoreStabilizerColorSets(br);
                AllArmStabilizerColorSets = new ArmStabilizerColorSets(br);
                AllLegStabilizerColorSets = new LegStabilizerColorSets(br);
            }

            /// <summary>
            /// Write all stabilizer color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                AllHeadStabilizerColorSets.Write(bw);
                AllCoreStabilizerColorSets.Write(bw);
                AllArmStabilizerColorSets.Write(bw);
                AllLegStabilizerColorSets.Write(bw);
            }
        }

        /// <summary>
        /// All head stabilizer color sets in this AcColorSet.
        /// </summary>
        public class HeadStabilizerColorSets
        {
            /// <summary>
            /// The top head stabilizer color set.
            /// </summary>
            public ColorSet HeadTopStabilizerColor;

            /// <summary>
            /// The right side head stabilizer color set.
            /// </summary>
            public ColorSet HeadRightStabilizerColor;

            /// <summary>
            /// The left side head stabilizer color set.
            /// </summary>
            public ColorSet HeadLeftStabilizerColor;

            /// <summary>
            /// Create a new, empty group of all head stabilizer color sets with all values set to their defaults.
            /// </summary>
            public HeadStabilizerColorSets()
            {
                HeadTopStabilizerColor = new ColorSet(0, 0, 0, 0);
                HeadRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                HeadLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all head stabilizer color sets from a stream.
            /// </summary>
            internal HeadStabilizerColorSets(BinaryReaderEx br)
            {
                HeadTopStabilizerColor = new ColorSet(br);
                HeadRightStabilizerColor = new ColorSet(br);
                HeadLeftStabilizerColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all head stabilizer color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                HeadTopStabilizerColor.Write(bw);
                HeadRightStabilizerColor.Write(bw);
                HeadLeftStabilizerColor.Write(bw);
            }
        }

        /// <summary>
        /// All core stabilizer color sets in this AcColorSet.
        /// </summary>
        public class CoreStabilizerColorSets
        {
            /// <summary>
            /// The upper right side core stabilizer color set.
            /// </summary>
            public ColorSet CoreUpperRightStabilizerColor;

            /// <summary>
            /// The upper left side core stabilizer color set.
            /// </summary>
            public ColorSet CoreUpperLeftStabilizerColor;

            /// <summary>
            /// The lower right side core stabilizer color set.
            /// </summary>
            public ColorSet CoreLowerRightStabilizerColor;

            /// <summary>
            /// The lower left side core stabilizer color set.
            /// </summary>
            public ColorSet CoreLowerLeftStabilizerColor;

            /// <summary>
            /// Create a new, empty group of all core stabilizer color sets with all values set to their defaults.
            /// </summary>
            public CoreStabilizerColorSets()
            {
                CoreUpperRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                CoreUpperLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
                CoreLowerRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                CoreLowerLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all core stabilizer color sets from a stream.
            /// </summary>
            internal CoreStabilizerColorSets(BinaryReaderEx br)
            {
                CoreUpperRightStabilizerColor = new ColorSet(br);
                CoreUpperLeftStabilizerColor = new ColorSet(br);
                CoreLowerRightStabilizerColor = new ColorSet(br);
                CoreLowerLeftStabilizerColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all core stabilizer color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                CoreUpperRightStabilizerColor.Write(bw);
                CoreUpperLeftStabilizerColor.Write(bw);
                CoreLowerRightStabilizerColor.Write(bw);
                CoreLowerLeftStabilizerColor.Write(bw);
            }
        }

        /// <summary>
        /// All arm stabilizer color sets in this AcColorSet.
        /// </summary>
        public class ArmStabilizerColorSets
        {
            /// <summary>
            /// The right arm stabilizer color set.
            /// </summary>
            public ColorSet ArmRightStabilizerColor;

            /// <summary>
            /// The left arm stabilizer color set.
            /// </summary>
            public ColorSet ArmLeftStabilizerColor;

            /// <summary>
            /// Create a new, empty group of all arm stabilizer color sets with all values set to their defaults.
            /// </summary>
            public ArmStabilizerColorSets()
            {
                ArmRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                ArmLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all arm stabilizer color sets from a stream.
            /// </summary>
            internal ArmStabilizerColorSets(BinaryReaderEx br)
            {
                ArmRightStabilizerColor = new ColorSet(br);
                ArmLeftStabilizerColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all arm stabilizer color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                ArmRightStabilizerColor.Write(bw);
                ArmLeftStabilizerColor.Write(bw);
            }
        }

        /// <summary>
        /// All leg stabilizer color sets in this AcColorSet.
        /// </summary>
        public class LegStabilizerColorSets
        {
            /// <summary>
            /// The back leg stabilizer color set.
            /// </summary>
            public ColorSet LegsBackStabilizerColor;

            /// <summary>
            /// The back leg stabilizer color set.
            /// </summary>
            public ColorSet LegsUpperRightStabilizerColor;

            /// <summary>
            /// The upper left leg stabilizer color set.
            /// </summary>
            public ColorSet LegsUpperLeftStabilizerColor;

            /// <summary>
            /// The back upper right leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsUpperRightBackStabilizerColor;

            /// <summary>
            /// The back upper left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsUpperLeftBackStabilizerColor;

            /// <summary>
            /// The middle right side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsMiddleRightStabilizerColor;

            /// <summary>
            /// The middle left side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsMiddleLeftStabilizerColor;

            /// <summary>
            /// The back middle right leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsMiddleRightBackStabilizerColor;

            /// <summary>
            /// The back middle left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsMiddleLeftBackStabilizerColor;

            /// <summary>
            /// The lower right side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsLowerRightStabilizerColor;

            /// <summary>
            /// The lower left side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsLowerLeftStabilizerColor;

            /// <summary>
            /// The back right left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsLowerRightBackStabilizerColor;

            /// <summary>
            /// The back lower left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsLowerLeftBackStabilizerColor;

            /// <summary>
            /// Create a new, empty group of all leg stabilizer color sets with all values set to their defaults.
            /// </summary>
            public LegStabilizerColorSets()
            {
                LegsBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsUpperRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsUpperLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsUpperRightBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsUpperLeftBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsMiddleRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsMiddleLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsMiddleRightBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsMiddleLeftBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsLowerRightStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsLowerLeftStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsLowerRightBackStabilizerColor = new ColorSet(0, 0, 0, 0);
                LegsLowerLeftBackStabilizerColor = new ColorSet(0, 0, 0, 0);
            }

            /// <summary>
            /// Read all leg stabilizer color sets from a stream.
            /// </summary>
            internal LegStabilizerColorSets(BinaryReaderEx br)
            {
                LegsBackStabilizerColor = new ColorSet(br);
                LegsUpperRightStabilizerColor = new ColorSet(br);
                LegsUpperLeftStabilizerColor = new ColorSet(br);
                LegsUpperRightBackStabilizerColor = new ColorSet(br);
                LegsUpperLeftBackStabilizerColor = new ColorSet(br);
                LegsMiddleRightStabilizerColor = new ColorSet(br);
                LegsMiddleLeftStabilizerColor = new ColorSet(br);
                LegsMiddleRightBackStabilizerColor = new ColorSet(br);
                LegsMiddleLeftBackStabilizerColor = new ColorSet(br);
                LegsLowerRightStabilizerColor = new ColorSet(br);
                LegsLowerLeftStabilizerColor = new ColorSet(br);
                LegsLowerRightBackStabilizerColor = new ColorSet(br);
                LegsLowerLeftBackStabilizerColor = new ColorSet(br);
            }

            /// <summary>
            /// Write all leg stabilizer color sets to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                LegsBackStabilizerColor.Write(bw);
                LegsUpperRightStabilizerColor.Write(bw);
                LegsUpperLeftStabilizerColor.Write(bw);
                LegsUpperRightBackStabilizerColor.Write(bw);
                LegsUpperLeftBackStabilizerColor.Write(bw);
                LegsMiddleRightStabilizerColor.Write(bw);
                LegsMiddleLeftStabilizerColor.Write(bw);
                LegsMiddleRightBackStabilizerColor.Write(bw);
                LegsMiddleLeftBackStabilizerColor.Write(bw);
                LegsLowerRightStabilizerColor.Write(bw);
                LegsLowerLeftStabilizerColor.Write(bw);
                LegsLowerRightBackStabilizerColor.Write(bw);
                LegsLowerLeftBackStabilizerColor.Write(bw);
            }
        }

        /// <summary>
        /// The 36 selected color patterns in an AcColorSet.
        /// </summary>
        public class ColorPatternSelections
        {
            /// <summary>
            /// Twelve color pattern options that color categories can choose from.
            /// </summary>
            public enum ColorPattern : byte
            {
                /// <summary>
                /// The solid color pattern, on by default.
                /// </summary>
                Solid = 0,

                /// <summary>
                /// The second color pattern option.
                /// </summary>
                Pattern2 = 1,

                /// <summary>
                /// The third color pattern option.
                /// </summary>
                Pattern3 = 2,

                /// <summary>
                /// The fourth color pattern option.
                /// </summary>
                Pattern4 = 3,

                /// <summary>
                /// The fifth color pattern option.
                /// </summary>
                Pattern5 = 4,

                /// <summary>
                /// The sixth color pattern option.
                /// </summary>
                Pattern6 = 5,

                /// <summary>
                /// The seventh color pattern option.
                /// </summary>
                Pattern7 = 6,

                /// <summary>
                /// The eighth color pattern option.
                /// </summary>
                Pattern8 = 7,

                /// <summary>
                /// The ninth color pattern option.
                /// </summary>
                Pattern9 = 8,

                /// <summary>
                /// The tenth color pattern option.
                /// </summary>
                Pattern10 = 9,

                /// <summary>
                /// The eleventh color pattern option.
                /// </summary>
                Pattern11 = 10,

                /// <summary>
                /// The twelfth color pattern option.
                /// </summary>
                Pattern12 = 11
            };

            /// <summary>
            /// The color pattern to be applied to all color sets.
            /// </summary>
            public ColorPattern All;

            /// <summary>
            /// The color pattern to be applied to all frame color sets.
            /// </summary>
            public ColorPattern AllFrames;

            /// <summary>
            /// The color pattern to be applied to all unit color sets.
            /// </summary>
            public ColorPattern AllUnits;

            /// <summary>
            /// The color pattern to be applied to all stabilizer color sets.
            /// </summary>
            public ColorPattern AllStabilizers;

            /// <summary>
            /// The color pattern set for the head color set.
            /// </summary>
            public ColorPattern Head;

            /// <summary>
            /// The color pattern set for the core color set.
            /// </summary>
            public ColorPattern Core;

            /// <summary>
            /// The color pattern set for the right arm color set.
            /// </summary>
            public ColorPattern ArmRight;

            /// <summary>
            /// The color pattern set for the left arm color set.
            /// </summary>
            public ColorPattern ArmLeft;

            /// <summary>
            /// The color pattern set for the legs color set.
            /// </summary>
            public ColorPattern Legs;

            /// <summary>
            /// The color pattern set for the right arm unit color set.
            /// </summary>
            public ColorPattern ArmUnitRight;

            /// <summary>
            /// The color pattern set for the left arm unit color set.
            /// </summary>
            public ColorPattern ArmUnitLeft;

            /// <summary>
            /// The color pattern set for the right back unit color set.
            /// </summary>
            public ColorPattern BackUnitRight;

            /// <summary>
            /// The color pattern set for the left back unit color set.
            /// </summary>
            public ColorPattern BackUnitLeft;

            /// <summary>
            /// The color pattern set for the shoulder unit color set.
            /// </summary>
            public ColorPattern ShoulderUnit;

            /// <summary>
            /// The color pattern set for the right hangar unit color set.
            /// </summary>
            public ColorPattern HangerUnitRight;

            /// <summary>
            /// The color pattern set for the left hangar unit color set.
            /// </summary>
            public ColorPattern HangerUnitLeft;

            /// <summary>
            /// The color pattern to be applied to all head stabilizer color sets.
            /// </summary>
            public ColorPattern AllHeadStabilizers;

            /// <summary>
            /// The color pattern to be applied to all core stabilizer color sets.
            /// </summary>
            public ColorPattern AllCoreStabilizers;

            /// <summary>
            /// The color pattern to be applied to all arm stabilizer color sets.
            /// </summary>
            public ColorPattern AllArmStabilizers;

            /// <summary>
            /// The color pattern to be applied to all leg stabilizer color sets.
            /// </summary>
            public ColorPattern AllLegStabilizers;

            /// <summary>
            /// The color pattern set for the top head stabilizer color set.
            /// </summary>
            public ColorPattern HeadTopStabilizer;

            /// <summary>
            /// The color pattern set for the right head stabilizer color set.
            /// </summary>
            public ColorPattern HeadRightStabilizer;

            /// <summary>
            /// The color pattern set for the left head stabilizer color set.
            /// </summary>
            public ColorPattern HeadLeftStabilizer;

            /// <summary>
            /// The color pattern set for the upper right core stabilizer color set.
            /// </summary>
            public ColorPattern CoreUpperRightStabilizer;

            /// <summary>
            /// The color pattern set for the upper left core stabilizer color set.
            /// </summary>
            public ColorPattern CoreUpperLeftStabilizer;

            /// <summary>
            /// The color pattern set for the lower right core stabilizer color set.
            /// </summary>
            public ColorPattern CoreLowerRightStabilizer;

            /// <summary>
            /// The color pattern set for the lower left core stabilizer color set.
            /// </summary>
            public ColorPattern CoreLowerLeftStabilizer;

            /// <summary>
            /// The color pattern set for the right arm stabilizer color set.
            /// </summary>
            public ColorPattern ArmRightStabilizer;

            /// <summary>
            /// The color pattern set for the left arm stabilizer color set.
            /// </summary>
            public ColorPattern ArmLeftStabilizer;

            /// <summary>
            /// The color pattern set for the stabilizer on the back of legs' color set.
            /// </summary>
            public ColorPattern LegsBackStabilizer;

            /// <summary>
            /// The color pattern set for the upper right stabilizer color set.
            /// </summary>
            public ColorPattern LegsUpperRightStabilizer;

            /// <summary>
            /// The color pattern set for the upper left stabilizer color set.
            /// </summary>
            public ColorPattern LegsUpperLeftStabilizer;

            /// <summary>
            /// The color pattern set for the middle right stabilizer color set.
            /// </summary>
            public ColorPattern LegsMiddleRightStabilizer;

            /// <summary>
            /// The color pattern set for the middle left stabilizer color set.
            /// </summary>
            public ColorPattern LegsMiddleLeftStabilizer;

            /// <summary>
            /// The color pattern set for the lower right stabilizer color set.
            /// </summary>
            public ColorPattern LegsLowerRightStabilizer;

            /// <summary>
            /// The color pattern set for the lower left stabilizer color set.
            /// </summary>
            public ColorPattern LegsLowerLeftStabilizer;

            /// <summary>
            /// Create a new color pattern selection structure with all values set to their defaults.
            /// </summary>
            public ColorPatternSelections()
            {
                All = ColorPattern.Solid;
                AllFrames = ColorPattern.Solid;
                AllUnits = ColorPattern.Solid;
                AllStabilizers = ColorPattern.Solid;
                Head = ColorPattern.Solid;
                Core = ColorPattern.Solid;
                ArmRight = ColorPattern.Solid;
                ArmLeft = ColorPattern.Solid;
                Legs = ColorPattern.Solid;
                ArmUnitRight = ColorPattern.Solid;
                ArmUnitLeft = ColorPattern.Solid;
                BackUnitRight = ColorPattern.Solid;
                BackUnitLeft = ColorPattern.Solid;
                ShoulderUnit = ColorPattern.Solid;
                HangerUnitRight = ColorPattern.Solid;
                HangerUnitLeft = ColorPattern.Solid;
                AllHeadStabilizers = ColorPattern.Solid;
                AllCoreStabilizers = ColorPattern.Solid;
                AllArmStabilizers = ColorPattern.Solid;
                AllLegStabilizers = ColorPattern.Solid;
                HeadTopStabilizer = ColorPattern.Solid;
                HeadRightStabilizer = ColorPattern.Solid;
                HeadLeftStabilizer = ColorPattern.Solid;
                CoreUpperRightStabilizer = ColorPattern.Solid;
                CoreUpperLeftStabilizer = ColorPattern.Solid;
                CoreLowerRightStabilizer = ColorPattern.Solid;
                CoreLowerLeftStabilizer = ColorPattern.Solid;
                ArmRightStabilizer = ColorPattern.Solid;
                ArmLeftStabilizer = ColorPattern.Solid;
                LegsBackStabilizer = ColorPattern.Solid;
                LegsUpperRightStabilizer = ColorPattern.Solid;
                LegsUpperLeftStabilizer = ColorPattern.Solid;
                LegsMiddleRightStabilizer = ColorPattern.Solid;
                LegsMiddleLeftStabilizer = ColorPattern.Solid;
                LegsLowerRightStabilizer = ColorPattern.Solid;
                LegsLowerLeftStabilizer = ColorPattern.Solid;
            }

            /// <summary>
            /// Read all 36 color pattern selections from a stream.
            /// </summary>
            internal ColorPatternSelections(BinaryReaderEx br)
            {
                All = br.ReadEnum8<ColorPattern>();
                AllFrames = br.ReadEnum8<ColorPattern>();
                AllUnits = br.ReadEnum8<ColorPattern>();
                AllStabilizers = br.ReadEnum8<ColorPattern>();
                Head = br.ReadEnum8<ColorPattern>();
                Core = br.ReadEnum8<ColorPattern>();
                ArmRight = br.ReadEnum8<ColorPattern>();
                ArmLeft = br.ReadEnum8<ColorPattern>();
                Legs = br.ReadEnum8<ColorPattern>();
                ArmUnitRight = br.ReadEnum8<ColorPattern>();
                ArmUnitLeft = br.ReadEnum8<ColorPattern>();
                BackUnitRight = br.ReadEnum8<ColorPattern>();
                BackUnitLeft = br.ReadEnum8<ColorPattern>();
                ShoulderUnit = br.ReadEnum8<ColorPattern>();
                HangerUnitRight = br.ReadEnum8<ColorPattern>();
                HangerUnitLeft = br.ReadEnum8<ColorPattern>();
                AllHeadStabilizers = br.ReadEnum8<ColorPattern>();
                AllCoreStabilizers = br.ReadEnum8<ColorPattern>();
                AllArmStabilizers = br.ReadEnum8<ColorPattern>();
                AllLegStabilizers = br.ReadEnum8<ColorPattern>();
                HeadTopStabilizer = br.ReadEnum8<ColorPattern>();
                HeadRightStabilizer = br.ReadEnum8<ColorPattern>();
                HeadLeftStabilizer = br.ReadEnum8<ColorPattern>();
                CoreUpperRightStabilizer = br.ReadEnum8<ColorPattern>();
                CoreUpperLeftStabilizer = br.ReadEnum8<ColorPattern>();
                CoreLowerRightStabilizer = br.ReadEnum8<ColorPattern>();
                CoreLowerLeftStabilizer = br.ReadEnum8<ColorPattern>();
                ArmRightStabilizer = br.ReadEnum8<ColorPattern>();
                ArmLeftStabilizer = br.ReadEnum8<ColorPattern>();
                LegsBackStabilizer = br.ReadEnum8<ColorPattern>();
                LegsUpperRightStabilizer = br.ReadEnum8<ColorPattern>();
                LegsUpperLeftStabilizer = br.ReadEnum8<ColorPattern>();
                LegsMiddleRightStabilizer = br.ReadEnum8<ColorPattern>();
                LegsMiddleLeftStabilizer = br.ReadEnum8<ColorPattern>();
                LegsLowerRightStabilizer = br.ReadEnum8<ColorPattern>();
                LegsLowerLeftStabilizer = br.ReadEnum8<ColorPattern>();
            }

            /// <summary>
            /// Write to all 36 color pattern selections to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteByte((byte)All);
                bw.WriteByte((byte)AllFrames);
                bw.WriteByte((byte)AllUnits);
                bw.WriteByte((byte)AllStabilizers);
                bw.WriteByte((byte)Head);
                bw.WriteByte((byte)Core);
                bw.WriteByte((byte)ArmRight);
                bw.WriteByte((byte)ArmLeft);
                bw.WriteByte((byte)Legs);
                bw.WriteByte((byte)ArmUnitRight);
                bw.WriteByte((byte)ArmUnitLeft);
                bw.WriteByte((byte)BackUnitRight);
                bw.WriteByte((byte)BackUnitLeft);
                bw.WriteByte((byte)ShoulderUnit);
                bw.WriteByte((byte)HangerUnitRight);
                bw.WriteByte((byte)HangerUnitLeft);
                bw.WriteByte((byte)AllHeadStabilizers);
                bw.WriteByte((byte)AllCoreStabilizers);
                bw.WriteByte((byte)AllArmStabilizers);
                bw.WriteByte((byte)AllLegStabilizers);
                bw.WriteByte((byte)HeadTopStabilizer);
                bw.WriteByte((byte)HeadRightStabilizer);
                bw.WriteByte((byte)HeadLeftStabilizer);
                bw.WriteByte((byte)CoreUpperRightStabilizer);
                bw.WriteByte((byte)CoreUpperLeftStabilizer);
                bw.WriteByte((byte)CoreLowerRightStabilizer);
                bw.WriteByte((byte)CoreLowerLeftStabilizer);
                bw.WriteByte((byte)ArmRightStabilizer);
                bw.WriteByte((byte)ArmLeftStabilizer);
                bw.WriteByte((byte)LegsBackStabilizer);
                bw.WriteByte((byte)LegsUpperRightStabilizer);
                bw.WriteByte((byte)LegsUpperLeftStabilizer);
                bw.WriteByte((byte)LegsMiddleRightStabilizer);
                bw.WriteByte((byte)LegsMiddleLeftStabilizer);
                bw.WriteByte((byte)LegsLowerRightStabilizer);
                bw.WriteByte((byte)LegsLowerLeftStabilizer);
            }
        }

        /// <summary>
        /// A color set containing six RGBA order colors.
        /// </summary>
        public struct ColorSet
        {
            /// <summary>
            /// The main coloring in this color set.
            /// </summary>
            public Color Main;

            /// <summary>
            /// The sub coloring in this color set.
            /// </summary>
            public Color Sub;

            /// <summary>
            /// The support coloring in this color set.
            /// </summary>
            public Color Support;

            /// <summary>
            /// The optional coloring in this color set.
            /// </summary>
            public Color Optional;

            /// <summary>
            /// The joint coloring in this color set.
            /// </summary>
            public Color Joint;

            /// <summary>
            /// The device coloring in this color set.
            /// </summary>
            public Color Device;

            /// <summary>
            /// Read a new color set from a stream.
            /// </summary>
            internal ColorSet(BinaryReaderEx br)
            {
                Main = br.ReadRGBA();
                Sub = br.ReadRGBA();
                Support = br.ReadRGBA();
                Optional = br.ReadRGBA();
                Joint = br.ReadRGBA();
                Device = br.ReadRGBA();
            }

            /// <summary>
            /// Create a new ColorSet using six RGBA order colors.
            /// </summary>
            /// <param name="main">The main coloring in this color set.</param>
            /// <param name="sub">The sub coloring in this color set.</param>
            /// <param name="support">The support coloring in this color set.</param>
            /// <param name="optional">The optional coloring in this color set.</param>
            /// <param name="joint">The joint coloring in this color set.</param>
            /// <param name="device">The device coloring in this color set.</param>
            public ColorSet(Color main, Color sub, Color support, Color optional, Color joint, Color device)
            {
                Main = main;
                Sub = sub;
                Support = support;
                Optional = optional;
                Joint = joint;
                Device = device;
            }

            /// <summary>
            /// Make a new color set with all values set the specified color.
            /// </summary>
            /// <param name="color">A color in RGBA order.</param>
            public ColorSet(Color color)
            {
                Main = color;
                Sub = color;
                Support = color;
                Optional = color;
                Joint = color;
                Device = color;
            }

            /// <summary>
            /// Make a new color set with all values set the specified color values.
            /// </summary>
            /// <param name="red">The red color to set all colors to.</param>
            /// <param name="green">The green color to set all colors to.</param>
            /// <param name="blue">The blue color to set all colors to.</param>
            /// <param name="alpha">The alpha to set all color alphas to.</param>
            public ColorSet(byte red, byte green, byte blue, byte alpha)
            {
                Color color = Color.FromArgb(alpha, red, green, blue);
                Main = color;
                Sub = color;
                Support = color;
                Optional = color;
                Joint = color;
                Device = color;
            }

            /// <summary>
            /// Set this color set's colors all to the selected color.
            /// </summary>
            /// <param name="color">A color in RGBA order.</param>
            public void SetColorSet(Color color)
            {
                Main = color;
                Sub = color;
                Support = color;
                Optional = color;
                Joint = color;
                Device = color;
            }

            /// <summary>
            /// Set a single color's values to 0.
            /// </summary>
            /// <param name="color">A color.</param>
            public void SetColorEmpty(ref Color color)
            {
                color = Color.FromArgb(0, 0, 0, 0);
            }

            /// <summary>
            /// Set all the colors in this color set's values to 0.
            /// </summary>
            public void SetColorSetEmpty()
            {
                SetColorEmpty(ref Main);
                SetColorEmpty(ref Sub);
                SetColorEmpty(ref Support);
                SetColorEmpty(ref Optional);
                SetColorEmpty(ref Joint);
                SetColorEmpty(ref Device);
            }

            /// <summary>
            /// Checks if a color has all values set to 0.
            /// </summary>
            /// <param name="color">A color.</param>
            /// <returns>Whether or not the passed color has all values set to 0.</returns>
            public bool IsEmptyColor(Color color)
            {
                return color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0;
            }

            /// <summary>
            /// Do all colors in this color set have their values set to 0.
            /// </summary>
            /// <returns>Whether or not this color set has all values set to 0.</returns>
            public bool IsEmptyColorSet()
            {
                return IsEmptyColor(Main)
                    && IsEmptyColor(Sub)
                    && IsEmptyColor(Support)
                    && IsEmptyColor(Optional)
                    && IsEmptyColor(Joint)
                    && IsEmptyColor(Device);
            }

            /// <summary>
            /// Read a new color set from a stream.
            /// </summary>
            public ColorSet Read(BinaryReaderEx br)
            {
                this = new ColorSet
                {
                    Main = br.ReadRGBA(),
                    Sub = br.ReadRGBA(),
                    Support = br.ReadRGBA(),
                    Optional = br.ReadRGBA(),
                    Joint = br.ReadRGBA(),
                    Device = br.ReadRGBA()
                };
                return this;
            }

            /// <summary>
            /// Read and return a new color set from a stream.
            /// </summary>
            /// <returns>A new color set.</returns>
            public static ColorSet ReadColorSet(BinaryReaderEx br)
            {
                ColorSet colorSet = new ColorSet
                {
                    Main = br.ReadRGBA(),
                    Sub = br.ReadRGBA(),
                    Support = br.ReadRGBA(),
                    Optional = br.ReadRGBA(),
                    Joint = br.ReadRGBA(),
                    Device = br.ReadRGBA()
                };
                return colorSet;
            }

            /// <summary>
            /// Write this color set to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteRGBA(Main);
                bw.WriteRGBA(Sub);
                bw.WriteRGBA(Support);
                bw.WriteRGBA(Optional);
                bw.WriteRGBA(Joint);
                bw.WriteRGBA(Device);
            }

            /// <summary>
            /// Write a color set to a stream.
            /// </summary>
            /// <param name="bw">A BinaryWriterEx stream.</param>
            /// <param name="colorset">A color set.</param>
            public static void WriteColorSet(BinaryWriterEx bw, ColorSet colorset)
            {
                bw.WriteRGBA(colorset.Main);
                bw.WriteRGBA(colorset.Sub);
                bw.WriteRGBA(colorset.Support);
                bw.WriteRGBA(colorset.Optional);
                bw.WriteRGBA(colorset.Joint);
                bw.WriteRGBA(colorset.Device);
            }
        }
    }
}
