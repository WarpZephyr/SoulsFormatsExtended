using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// Armored Core AcColorSet files and structures, the name of bin files of just the structure are formatted color%04d.bin.
    /// </summary>
    public class AcColorSet : SoulsFile<AcColorSet>
    {
        /// <summary>
        /// All the color sets in this AcColorSet.
        /// </summary>
        public AllColorSets ColorSets { get; set; }

        /// <summary>
        /// The color pattern selections in this AcColorSet.
        /// </summary>
        public ColorPatternSet ColorPatterns { get; set; }

        /// <summary>
        /// The head eye color of this AcColorSet.
        /// </summary>
        public Color EyeColor { get; set; }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            ColorSets = new AllColorSets(br);
            ColorPatterns = new ColorPatternSet(br);
            EyeColor = br.ReadRGBA();
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            ColorSets.Write(bw);
            ColorPatterns.Write(bw);
            bw.WriteRGBA(EyeColor);
        }

        /// <summary>
        /// Create a new, empty AcColorSet with all values set to their defaults.
        /// </summary>
        public AcColorSet()
        {
            ColorSets = new AllColorSets();
            ColorPatterns = new ColorPatternSet();
            EyeColor = Color.FromArgb(0, 0, 0, 0);
        }

        /// <summary>
        /// Get a list of all colors, including the eye color.
        /// </summary>
        /// <returns>A list of colors.</returns>
        public List<Color> GetColors()
        {
            var colors = new List<Color>();
            colors.AddRange(ColorSets.GetColors());
            colors.Add(EyeColor);
            return colors;
        }

        /// <summary>
        /// Get an array of all colors, including the eye color.
        /// </summary>
        /// <returns>An array of colors.</returns>
        public Color[] GetColorArray()
        {
            return GetColors().ToArray();
        }

        /// <summary>
        /// All color sets in this AcColorSet, excluding eye color.
        /// </summary>
        public class AllColorSets
        {
            /// <summary>
            /// All frame color sets.
            /// </summary>
            public FrameColorSets FrameColorSets { get; set; }

            /// <summary>
            /// All unit color sets.
            /// </summary>
            public UnitColorSets UnitColorSets { get; set; }

            /// <summary>
            /// All stabilizer color sets.
            /// </summary>
            public StabilizerColorSets StabilizerColorSets { get; set; }

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
            public AllColorSets(BinaryReaderEx br)
            {
                FrameColorSets = new FrameColorSets(br);
                UnitColorSets = new UnitColorSets(br);
                StabilizerColorSets = new StabilizerColorSets(br);
            }

            /// <summary>
            /// Read a new AllColorSets from a color array.
            /// </summary>
            /// <param name="colors">A color array.</param>
            /// <exception cref="InvalidOperationException">There was too little colors to proceed.</exception>
            public AllColorSets(Color[] colors)
            {
                if (colors.Length < 816 / 4)
                {
                    throw new InvalidOperationException($"Must have at least {816 / 4} colors to read an AllColorSets.");
                }

                byte[] colorBytes = new byte[816];
                for (int i = 0; i < colorBytes.Length; i++)
                {
                    colorBytes[i] = colors[i].R;
                    colorBytes[i + 1] = colors[i].G;
                    colorBytes[i + 2] = colors[i].B;
                    colorBytes[i + 3] = colors[i].A;
                    i += 4;
                }

                var br = new BinaryReaderEx(true, colorBytes);
                FrameColorSets = new FrameColorSets(br);
                UnitColorSets = new UnitColorSets(br);
                StabilizerColorSets = new StabilizerColorSets(br);
            }

            /// <summary>
            /// Write all color sets to a stream.
            /// </summary>
            public void Write(BinaryWriterEx bw)
            {
                FrameColorSets.Write(bw);
                UnitColorSets.Write(bw);
                StabilizerColorSets.Write(bw);
            }

            /// <summary>
            /// Get a list of all colors.
            /// </summary>
            /// <returns>A list of all colors.</returns>
            public List<Color> GetColors()
            {
                return ColorSet.GetColors(GetColorSets());
            }

            /// <summary>
            /// Get an array of all colors.
            /// </summary>
            /// <returns>An array of all colors.</returns>
            public Color[] GetColorArray()
            {
                return GetColors().ToArray();
            }

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                var colorsets = new List<ColorSet>();
                colorsets.AddRange(FrameColorSets.GetColorSets());
                colorsets.AddRange(UnitColorSets.GetColorSets());
                colorsets.AddRange(StabilizerColorSets.GetColorSets());
                return colorsets;
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
            public ColorSet HeadColor { get; set; }

            /// <summary>
            /// The core color set.
            /// </summary>
            public ColorSet CoreColor { get; set; }

            /// <summary>
            /// The right arm color set.
            /// </summary>
            public ColorSet ArmRightColor { get; set; }

            /// <summary>
            /// The left arm color set.
            /// </summary>
            public ColorSet ArmLeftColor { get; set; }

            /// <summary>
            /// The color set for legs.
            /// </summary>
            public ColorSet LegsColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    HeadColor,
                    CoreColor,
                    ArmRightColor,
                    ArmLeftColor,
                    LegsColor
                };
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
            public ColorSet ArmUnitRightColor { get; set; }

            /// <summary>
            /// The left arm unit color set.
            /// </summary>
            public ColorSet ArmUnitLeftColor { get; set; }

            /// <summary>
            /// The right back unit color set.
            /// </summary>
            public ColorSet BackUnitRightColor { get; set; }

            /// <summary>
            /// The left back unit color set.
            /// </summary>
            public ColorSet BackUnitLeftColor { get; set; }

            /// <summary>
            /// The shoulder unit color set.
            /// </summary>
            public ColorSet ShoulderUnitColor { get; set; }

            /// <summary>
            /// The right hanger unit color set.
            /// </summary>
            public ColorSet HangerUnitRightColor { get; set; }

            /// <summary>
            /// The left hanger unit color set.
            /// </summary>
            public ColorSet HangerUnitLeftColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    ArmUnitRightColor,
                    ArmUnitLeftColor,
                    BackUnitRightColor,
                    BackUnitLeftColor,
                    ShoulderUnitColor,
                    HangerUnitRightColor,
                    HangerUnitLeftColor
                };
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
            public HeadStabilizerColorSets AllHeadStabilizerColorSets { get; set; }

            /// <summary>
            /// All core stabilizer color sets.
            /// </summary>
            public CoreStabilizerColorSets AllCoreStabilizerColorSets { get; set; }

            /// <summary>
            /// All arm stabilizer color sets.
            /// </summary>
            public ArmStabilizerColorSets AllArmStabilizerColorSets { get; set; }

            /// <summary>
            /// All leg stabilizer color sets.
            /// </summary>
            public LegStabilizerColorSets AllLegStabilizerColorSets { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                var colorsets = new List<ColorSet>();
                colorsets.AddRange(AllHeadStabilizerColorSets.GetColorSets());
                colorsets.AddRange(AllCoreStabilizerColorSets.GetColorSets());
                colorsets.AddRange(AllArmStabilizerColorSets.GetColorSets());
                return colorsets;
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
            public ColorSet HeadTopStabilizerColor { get; set; }

            /// <summary>
            /// The right side head stabilizer color set.
            /// </summary>
            public ColorSet HeadRightStabilizerColor { get; set; }

            /// <summary>
            /// The left side head stabilizer color set.
            /// </summary>
            public ColorSet HeadLeftStabilizerColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    HeadTopStabilizerColor,
                    HeadRightStabilizerColor,
                    HeadLeftStabilizerColor
                };
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
            public ColorSet CoreUpperRightStabilizerColor { get; set; }

            /// <summary>
            /// The upper left side core stabilizer color set.
            /// </summary>
            public ColorSet CoreUpperLeftStabilizerColor { get; set; }

            /// <summary>
            /// The lower right side core stabilizer color set.
            /// </summary>
            public ColorSet CoreLowerRightStabilizerColor { get; set; }

            /// <summary>
            /// The lower left side core stabilizer color set.
            /// </summary>
            public ColorSet CoreLowerLeftStabilizerColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    CoreUpperRightStabilizerColor,
                    CoreUpperLeftStabilizerColor,
                    CoreLowerRightStabilizerColor,
                    CoreLowerLeftStabilizerColor
                };
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
            public ColorSet ArmRightStabilizerColor { get; set; }

            /// <summary>
            /// The left arm stabilizer color set.
            /// </summary>
            public ColorSet ArmLeftStabilizerColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    ArmRightStabilizerColor,
                    ArmLeftStabilizerColor
                };
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
            public ColorSet LegsBackStabilizerColor { get; set; }

            /// <summary>
            /// The back leg stabilizer color set.
            /// </summary>
            public ColorSet LegsUpperRightStabilizerColor { get; set; }

            /// <summary>
            /// The upper left leg stabilizer color set.
            /// </summary>
            public ColorSet LegsUpperLeftStabilizerColor { get; set; }

            /// <summary>
            /// The back upper right leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsUpperRightBackStabilizerColor { get; set; }

            /// <summary>
            /// The back upper left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsUpperLeftBackStabilizerColor { get; set; }

            /// <summary>
            /// The middle right side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsMiddleRightStabilizerColor { get; set; }

            /// <summary>
            /// The middle left side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsMiddleLeftStabilizerColor { get; set; }

            /// <summary>
            /// The back middle right leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsMiddleRightBackStabilizerColor { get; set; }

            /// <summary>
            /// The back middle left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsMiddleLeftBackStabilizerColor { get; set; }

            /// <summary>
            /// The lower right side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsLowerRightStabilizerColor { get; set; }

            /// <summary>
            /// The lower left side leg stabilizer color set.
            /// </summary>
            public ColorSet LegsLowerLeftStabilizerColor { get; set; }

            /// <summary>
            /// The back right left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsLowerRightBackStabilizerColor { get; set; }

            /// <summary>
            /// The back lower left leg stabilizer color set, unused.
            /// </summary>
            public ColorSet LegsLowerLeftBackStabilizerColor { get; set; }

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

            /// <summary>
            /// Get all the colorsets.
            /// </summary>
            /// <returns>A list of colorsets.</returns>
            public List<ColorSet> GetColorSets()
            {
                return new List<ColorSet>()
                {
                    LegsBackStabilizerColor,
                    LegsUpperRightStabilizerColor,
                    LegsUpperLeftStabilizerColor,
                    LegsUpperRightBackStabilizerColor,
                    LegsUpperLeftBackStabilizerColor,
                    LegsMiddleRightStabilizerColor,
                    LegsMiddleLeftStabilizerColor,
                    LegsMiddleRightBackStabilizerColor,
                    LegsMiddleLeftBackStabilizerColor,
                    LegsLowerRightStabilizerColor,
                    LegsLowerLeftStabilizerColor,
                    LegsLowerRightBackStabilizerColor,
                    LegsLowerLeftBackStabilizerColor
                };
            }
        }
    }
}
