namespace SoulsFormats
{
    /// <summary>
    /// The 36 selected color patterns in an AcColorSet.
    /// </summary>
    public class ColorPatternSet
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
        public ColorPattern All { get; set; }

        /// <summary>
        /// The color pattern to be applied to all frame color sets.
        /// </summary>
        public ColorPattern AllFrames { get; set; }

        /// <summary>
        /// The color pattern to be applied to all unit color sets.
        /// </summary>
        public ColorPattern AllUnits { get; set; }

        /// <summary>
        /// The color pattern to be applied to all stabilizer color sets.
        /// </summary>
        public ColorPattern AllStabilizers { get; set; }

        /// <summary>
        /// The color pattern set for the head color set.
        /// </summary>
        public ColorPattern Head { get; set; }

        /// <summary>
        /// The color pattern set for the core color set.
        /// </summary>
        public ColorPattern Core { get; set; }

        /// <summary>
        /// The color pattern set for the right arm color set.
        /// </summary>
        public ColorPattern ArmRight { get; set; }

        /// <summary>
        /// The color pattern set for the left arm color set.
        /// </summary>
        public ColorPattern ArmLeft { get; set; }

        /// <summary>
        /// The color pattern set for the legs color set.
        /// </summary>
        public ColorPattern Legs { get; set; }

        /// <summary>
        /// The color pattern set for the right arm unit color set.
        /// </summary>
        public ColorPattern ArmUnitRight { get; set; }

        /// <summary>
        /// The color pattern set for the left arm unit color set.
        /// </summary>
        public ColorPattern ArmUnitLeft { get; set; }

        /// <summary>
        /// The color pattern set for the right back unit color set.
        /// </summary>
        public ColorPattern BackUnitRight { get; set; }

        /// <summary>
        /// The color pattern set for the left back unit color set.
        /// </summary>
        public ColorPattern BackUnitLeft { get; set; }

        /// <summary>
        /// The color pattern set for the shoulder unit color set.
        /// </summary>
        public ColorPattern ShoulderUnit { get; set; }

        /// <summary>
        /// The color pattern set for the right hangar unit color set.
        /// </summary>
        public ColorPattern HangerUnitRight { get; set; }

        /// <summary>
        /// The color pattern set for the left hangar unit color set.
        /// </summary>
        public ColorPattern HangerUnitLeft { get; set; }

        /// <summary>
        /// The color pattern to be applied to all head stabilizer color sets.
        /// </summary>
        public ColorPattern AllHeadStabilizers { get; set; }

        /// <summary>
        /// The color pattern to be applied to all core stabilizer color sets.
        /// </summary>
        public ColorPattern AllCoreStabilizers { get; set; }

        /// <summary>
        /// The color pattern to be applied to all arm stabilizer color sets.
        /// </summary>
        public ColorPattern AllArmStabilizers { get; set; }

        /// <summary>
        /// The color pattern to be applied to all leg stabilizer color sets.
        /// </summary>
        public ColorPattern AllLegStabilizers { get; set; }

        /// <summary>
        /// The color pattern set for the top head stabilizer color set.
        /// </summary>
        public ColorPattern HeadTopStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the right head stabilizer color set.
        /// </summary>
        public ColorPattern HeadRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the left head stabilizer color set.
        /// </summary>
        public ColorPattern HeadLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the upper right core stabilizer color set.
        /// </summary>
        public ColorPattern CoreUpperRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the upper left core stabilizer color set.
        /// </summary>
        public ColorPattern CoreUpperLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the lower right core stabilizer color set.
        /// </summary>
        public ColorPattern CoreLowerRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the lower left core stabilizer color set.
        /// </summary>
        public ColorPattern CoreLowerLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the right arm stabilizer color set.
        /// </summary>
        public ColorPattern ArmRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the left arm stabilizer color set.
        /// </summary>
        public ColorPattern ArmLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the stabilizer on the back of legs' color set.
        /// </summary>
        public ColorPattern LegsBackStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the upper right stabilizer color set.
        /// </summary>
        public ColorPattern LegsUpperRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the upper left stabilizer color set.
        /// </summary>
        public ColorPattern LegsUpperLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the middle right stabilizer color set.
        /// </summary>
        public ColorPattern LegsMiddleRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the middle left stabilizer color set.
        /// </summary>
        public ColorPattern LegsMiddleLeftStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the lower right stabilizer color set.
        /// </summary>
        public ColorPattern LegsLowerRightStabilizer { get; set; }

        /// <summary>
        /// The color pattern set for the lower left stabilizer color set.
        /// </summary>
        public ColorPattern LegsLowerLeftStabilizer { get; set; }

        /// <summary>
        /// Create a new color pattern selection structure with all values set to their defaults.
        /// </summary>
        public ColorPatternSet()
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
        internal ColorPatternSet(BinaryReaderEx br)
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

        /// <summary>
        /// Verify a byte array containing color pattern selections' bytes all do not go out of the correct value range.
        /// </summary>
        /// <param name="pattern">A byte array containing a color selections.</param>
        /// <returns>Whether or not a byte array containing color pattern selections' bytes all do not go out of the correct value range.</returns>
        public static bool VerifyPatterns(byte[] pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] < 0 || pattern[i] > 11)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Apply the pattern to be set to all patterns from the All pattern.
        /// </summary>
        public void ApplyAllPattern()
        {
            AllFrames = All;
            ApplyAllFramesPattern();

            AllUnits = All;
            ApplyAllUnitsPattern();

            AllStabilizers = All;
            ApplyAllStabilizersPattern();
        }

        /// <summary>
        /// Apply the pattern to be set to all frame patterns from the AllFrames pattern.
        /// </summary>
        public void ApplyAllFramesPattern()
        {
            Head = AllFrames;
            Core = AllFrames;
            ArmRight = AllFrames;
            ArmLeft = AllFrames;
            Legs = AllFrames;
        }

        /// <summary>
        /// Apply the pattern to be set to all unit patterns from the AllUnits pattern.
        /// </summary>
        public void ApplyAllUnitsPattern()
        {
            ArmUnitRight = AllUnits;
            ArmUnitLeft = AllUnits;
            BackUnitRight = AllUnits;
            BackUnitLeft = AllUnits;
            ShoulderUnit = AllUnits;
        }

        /// <summary>
        /// Apply the pattern to be set to all stabilizer patterns from the AllStabilizers pattern.
        /// </summary>
        public void ApplyAllStabilizersPattern()
        {
            AllHeadStabilizers = AllStabilizers;
            ApplyAllHeadStabilizersPattern();

            AllCoreStabilizers = AllStabilizers;
            ApplyAllCoreStabilizersPattern();

            AllArmStabilizers = AllStabilizers;
            ApplyAllArmStabilizersPattern();

            AllLegStabilizers = AllStabilizers;
            ApplyAllLegStabilizersPattern();
        }

        /// <summary>
        /// Apply the pattern to be set to all head stabilizer patterns from the AllHeadStabilizers pattern.
        /// </summary>
        public void ApplyAllHeadStabilizersPattern()
        {
            HeadTopStabilizer = AllHeadStabilizers;
            HeadRightStabilizer = AllHeadStabilizers;
            HeadLeftStabilizer = AllHeadStabilizers;
        }

        /// <summary>
        /// Apply the pattern to be set to all core stabilizer patterns from the AllCoreStabilizers pattern.
        /// </summary>
        public void ApplyAllCoreStabilizersPattern()
        {
            CoreUpperRightStabilizer = AllCoreStabilizers;
            CoreUpperLeftStabilizer = AllCoreStabilizers;
            CoreLowerRightStabilizer = AllCoreStabilizers;
            CoreLowerLeftStabilizer = AllCoreStabilizers;
        }

        /// <summary>
        /// Apply the pattern to be set to all arm stabilizer patterns from the AllArmStabilizers pattern.
        /// </summary>
        public void ApplyAllArmStabilizersPattern()
        {
            ArmRightStabilizer = AllArmStabilizers;
            ArmLeftStabilizer = AllArmStabilizers;
        }

        /// <summary>
        /// Apply the pattern to be set to all leg stabilizer patterns from the AllLegStabilizers pattern.
        /// </summary>
        public void ApplyAllLegStabilizersPattern()
        {
            LegsBackStabilizer = AllLegStabilizers;
            LegsUpperRightStabilizer = AllLegStabilizers;
            LegsUpperLeftStabilizer = AllLegStabilizers;
            LegsMiddleRightStabilizer = AllLegStabilizers;
            LegsMiddleLeftStabilizer = AllLegStabilizers;
            LegsLowerRightStabilizer = AllLegStabilizers;
            LegsLowerLeftStabilizer = AllLegStabilizers;
        }
    }
}
