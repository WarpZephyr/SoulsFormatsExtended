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
        /// Checks if a file may be an AcColorSet; WARNING: There is not a very good way to verify a file is an AcColorSet.
        /// </summary>
        public static bool Match(BinaryReaderEx br)
        {
            return br.Length == 856 && ColorPatternSet.VerifyPatterns(br.GetBytes(816, 36));
        }

        /// <summary>
        /// Checks if a file may be an AcColorSet; WARNING: There is not a very good way to verify a file is an AcColorSet.
        /// </summary>
        public static bool Match(byte[] bytes)
        {
            if (bytes.Length == 0)
                return false;

            BinaryReaderEx br = new BinaryReaderEx(true, bytes);
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

                BinaryReaderEx br = new BinaryReaderEx(true, stream);
                return Match(SFUtil.GetDecompressedBR(br, out _));
            }
        }

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
        /// Get a random color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetRandomColor()
        {
            byte[] color = new byte[4];
            Random random = new Random();
            random.NextBytes(color);
            return Color.FromArgb(color[0], color[1], color[2], color[3]);
        }

        /// <summary>
        /// Randomize the entire AcColorSet.
        /// </summary>
        public void Randomize()
        {
            ColorSets.Randomize();
            ColorPatterns.Randomize();
            EyeColor = GetRandomColor();
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
        /// Randomize a list of colors.
        /// </summary>
        /// <param name="colors">A list of colors.</param>
        /// <returns>A randomized list of colors.</returns>
        public List<Color> RandomizeColors(List<Color> colors)
        {
            for (int i = 0; i < colors.Count; i++)
                colors[i] = GetRandomColor();
            return colors;
        }

        /// <summary>
        /// Randomize an array of colors.
        /// </summary>
        /// <param name="colors">An array of colors.</param>
        /// <returns>A randomized array of colors.</returns>
        public Color[] RandomizeColorArray(Color[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
                colors[i] = GetRandomColor();
            return colors;
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                FrameColorSets.SetAll(colorset);
                UnitColorSets.SetAll(colorset);
                StabilizerColorSets.SetAll(colorset);
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                FrameColorSets.SetRed(red);
                UnitColorSets.SetRed(red);
                StabilizerColorSets.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                FrameColorSets.SetGreen(green);
                UnitColorSets.SetGreen(green);
                StabilizerColorSets.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                FrameColorSets.SetBlue(blue);
                UnitColorSets.SetBlue(blue);
                StabilizerColorSets.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                FrameColorSets.SetAlpha(alpha);
                UnitColorSets.SetAlpha(alpha);
                StabilizerColorSets.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                FrameColorSets.SetMain(color);
                UnitColorSets.SetMain(color);
                StabilizerColorSets.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                FrameColorSets.SetSub(color);
                UnitColorSets.SetSub(color);
                StabilizerColorSets.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                FrameColorSets.SetSupport(color);
                UnitColorSets.SetSupport(color);
                StabilizerColorSets.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                FrameColorSets.SetOptional(color);
                UnitColorSets.SetOptional(color);
                StabilizerColorSets.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                FrameColorSets.SetJoint(color);
                UnitColorSets.SetJoint(color);
                StabilizerColorSets.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                FrameColorSets.SetDevice(color);
                UnitColorSets.SetDevice(color);
                StabilizerColorSets.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                FrameColorSets.Randomize();
                UnitColorSets.Randomize();
                StabilizerColorSets.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                HeadColor = colorset;
                CoreColor = colorset;
                ArmRightColor = colorset;
                ArmLeftColor = colorset;
                LegsColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                HeadColor.SetRed(red);
                CoreColor.SetRed(red);
                ArmRightColor.SetRed(red);
                ArmLeftColor.SetRed(red);
                LegsColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                HeadColor.SetGreen(green);
                CoreColor.SetGreen(green);
                ArmRightColor.SetGreen(green);
                ArmLeftColor.SetGreen(green);
                LegsColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                HeadColor.SetBlue(blue);
                CoreColor.SetBlue(blue);
                ArmRightColor.SetBlue(blue);
                ArmLeftColor.SetBlue(blue);
                LegsColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                HeadColor.SetAlpha(alpha);
                CoreColor.SetAlpha(alpha);
                ArmRightColor.SetAlpha(alpha);
                ArmLeftColor.SetAlpha(alpha);
                LegsColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                HeadColor.SetMain(color);
                CoreColor.SetMain(color);
                ArmRightColor.SetMain(color);
                ArmLeftColor.SetMain(color);
                LegsColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                HeadColor.SetSub(color);
                CoreColor.SetSub(color);
                ArmRightColor.SetSub(color);
                ArmLeftColor.SetSub(color);
                LegsColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                HeadColor.SetSupport(color);
                CoreColor.SetSupport(color);
                ArmRightColor.SetSupport(color);
                ArmLeftColor.SetSupport(color);
                LegsColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                HeadColor.SetOptional(color);
                CoreColor.SetOptional(color);
                ArmRightColor.SetOptional(color);
                ArmLeftColor.SetOptional(color);
                LegsColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                HeadColor.SetJoint(color);
                CoreColor.SetJoint(color);
                ArmRightColor.SetJoint(color);
                ArmLeftColor.SetJoint(color);
                LegsColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                HeadColor.SetDevice(color);
                CoreColor.SetDevice(color);
                ArmRightColor.SetDevice(color);
                ArmLeftColor.SetDevice(color);
                LegsColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                HeadColor.Randomize();
                CoreColor.Randomize();
                ArmRightColor.Randomize();
                ArmLeftColor.Randomize();
                LegsColor.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                ArmUnitRightColor = colorset;
                ArmUnitLeftColor = colorset;
                BackUnitRightColor = colorset;
                BackUnitLeftColor = colorset;
                ShoulderUnitColor = colorset;
                HangerUnitRightColor = colorset;
                HangerUnitLeftColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                ArmUnitRightColor.SetRed(red);
                ArmUnitLeftColor.SetRed(red);
                BackUnitRightColor.SetRed(red);
                BackUnitLeftColor.SetRed(red);
                ShoulderUnitColor.SetRed(red);
                HangerUnitRightColor.SetRed(red);
                HangerUnitLeftColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                ArmUnitRightColor.SetGreen(green);
                ArmUnitLeftColor.SetGreen(green);
                BackUnitRightColor.SetGreen(green);
                BackUnitLeftColor.SetGreen(green);
                ShoulderUnitColor.SetGreen(green);
                HangerUnitRightColor.SetGreen(green);
                HangerUnitLeftColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                ArmUnitRightColor.SetBlue(blue);
                ArmUnitLeftColor.SetBlue(blue);
                BackUnitRightColor.SetBlue(blue);
                BackUnitLeftColor.SetBlue(blue);
                ShoulderUnitColor.SetBlue(blue);
                HangerUnitRightColor.SetBlue(blue);
                HangerUnitLeftColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                ArmUnitRightColor.SetAlpha(alpha);
                ArmUnitLeftColor.SetAlpha(alpha);
                BackUnitRightColor.SetAlpha(alpha);
                BackUnitLeftColor.SetAlpha(alpha);
                ShoulderUnitColor.SetAlpha(alpha);
                HangerUnitRightColor.SetAlpha(alpha);
                HangerUnitLeftColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                ArmUnitRightColor.SetMain(color);
                ArmUnitLeftColor.SetMain(color);
                BackUnitRightColor.SetMain(color);
                BackUnitLeftColor.SetMain(color);
                ShoulderUnitColor.SetMain(color);
                HangerUnitRightColor.SetMain(color);
                HangerUnitLeftColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                ArmUnitRightColor.SetSub(color);
                ArmUnitLeftColor.SetSub(color);
                BackUnitRightColor.SetSub(color);
                BackUnitLeftColor.SetSub(color);
                ShoulderUnitColor.SetSub(color);
                HangerUnitRightColor.SetSub(color);
                HangerUnitLeftColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                ArmUnitRightColor.SetSupport(color);
                ArmUnitLeftColor.SetSupport(color);
                BackUnitRightColor.SetSupport(color);
                BackUnitLeftColor.SetSupport(color);
                ShoulderUnitColor.SetSupport(color);
                HangerUnitRightColor.SetSupport(color);
                HangerUnitLeftColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                ArmUnitRightColor.SetOptional(color);
                ArmUnitLeftColor.SetOptional(color);
                BackUnitRightColor.SetOptional(color);
                BackUnitLeftColor.SetOptional(color);
                ShoulderUnitColor.SetOptional(color);
                HangerUnitRightColor.SetOptional(color);
                HangerUnitLeftColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                ArmUnitRightColor.SetJoint(color);
                ArmUnitLeftColor.SetJoint(color);
                BackUnitRightColor.SetJoint(color);
                BackUnitLeftColor.SetJoint(color);
                ShoulderUnitColor.SetJoint(color);
                HangerUnitRightColor.SetJoint(color);
                HangerUnitLeftColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                ArmUnitRightColor.SetDevice(color);
                ArmUnitLeftColor.SetDevice(color);
                BackUnitRightColor.SetDevice(color);
                BackUnitLeftColor.SetDevice(color);
                ShoulderUnitColor.SetDevice(color);
                HangerUnitRightColor.SetDevice(color);
                HangerUnitLeftColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                ArmUnitRightColor.Randomize();
                ArmUnitLeftColor.Randomize();
                BackUnitRightColor.Randomize();
                BackUnitLeftColor.Randomize();
                ShoulderUnitColor.Randomize();
                HangerUnitRightColor.Randomize();
                HangerUnitLeftColor.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                AllHeadStabilizerColorSets.SetAll(colorset);
                AllCoreStabilizerColorSets.SetAll(colorset);
                AllArmStabilizerColorSets.SetAll(colorset);
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                AllHeadStabilizerColorSets.SetRed(red);
                AllCoreStabilizerColorSets.SetRed(red);
                AllArmStabilizerColorSets.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                AllHeadStabilizerColorSets.SetGreen(green);
                AllCoreStabilizerColorSets.SetGreen(green);
                AllArmStabilizerColorSets.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                AllHeadStabilizerColorSets.SetBlue(blue);
                AllCoreStabilizerColorSets.SetBlue(blue);
                AllArmStabilizerColorSets.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                AllHeadStabilizerColorSets.SetAlpha(alpha);
                AllCoreStabilizerColorSets.SetAlpha(alpha);
                AllArmStabilizerColorSets.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                AllHeadStabilizerColorSets.SetMain(color);
                AllCoreStabilizerColorSets.SetMain(color);
                AllArmStabilizerColorSets.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                AllHeadStabilizerColorSets.SetSub(color);
                AllCoreStabilizerColorSets.SetSub(color);
                AllArmStabilizerColorSets.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                AllHeadStabilizerColorSets.SetSupport(color);
                AllCoreStabilizerColorSets.SetSupport(color);
                AllArmStabilizerColorSets.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                AllHeadStabilizerColorSets.SetOptional(color);
                AllCoreStabilizerColorSets.SetOptional(color);
                AllArmStabilizerColorSets.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                AllHeadStabilizerColorSets.SetJoint(color);
                AllCoreStabilizerColorSets.SetJoint(color);
                AllArmStabilizerColorSets.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                AllHeadStabilizerColorSets.SetDevice(color);
                AllCoreStabilizerColorSets.SetDevice(color);
                AllArmStabilizerColorSets.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                AllHeadStabilizerColorSets.Randomize();
                AllCoreStabilizerColorSets.Randomize();
                AllArmStabilizerColorSets.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                HeadTopStabilizerColor = colorset;
                HeadRightStabilizerColor = colorset;
                HeadLeftStabilizerColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                HeadTopStabilizerColor.SetRed(red);
                HeadRightStabilizerColor.SetRed(red);
                HeadLeftStabilizerColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                HeadTopStabilizerColor.SetGreen(green);
                HeadRightStabilizerColor.SetGreen(green);
                HeadLeftStabilizerColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                HeadTopStabilizerColor.SetBlue(blue);
                HeadRightStabilizerColor.SetBlue(blue);
                HeadLeftStabilizerColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                HeadTopStabilizerColor.SetAlpha(alpha);
                HeadRightStabilizerColor.SetAlpha(alpha);
                HeadLeftStabilizerColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                HeadTopStabilizerColor.SetMain(color);
                HeadRightStabilizerColor.SetMain(color);
                HeadLeftStabilizerColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                HeadTopStabilizerColor.SetSub(color);
                HeadRightStabilizerColor.SetSub(color);
                HeadLeftStabilizerColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                HeadTopStabilizerColor.SetSupport(color);
                HeadRightStabilizerColor.SetSupport(color);
                HeadLeftStabilizerColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                HeadTopStabilizerColor.SetOptional(color);
                HeadRightStabilizerColor.SetOptional(color);
                HeadLeftStabilizerColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                HeadTopStabilizerColor.SetJoint(color);
                HeadRightStabilizerColor.SetJoint(color);
                HeadLeftStabilizerColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                HeadTopStabilizerColor.SetDevice(color);
                HeadRightStabilizerColor.SetDevice(color);
                HeadLeftStabilizerColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                HeadTopStabilizerColor.Randomize();
                HeadRightStabilizerColor.Randomize();
                HeadLeftStabilizerColor.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                CoreUpperRightStabilizerColor = colorset;
                CoreUpperLeftStabilizerColor = colorset;
                CoreLowerRightStabilizerColor = colorset;
                CoreLowerLeftStabilizerColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                CoreUpperRightStabilizerColor.SetRed(red);
                CoreUpperLeftStabilizerColor.SetRed(red);
                CoreLowerRightStabilizerColor.SetRed(red);
                CoreLowerLeftStabilizerColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                CoreUpperRightStabilizerColor.SetGreen(green);
                CoreUpperLeftStabilizerColor.SetGreen(green);
                CoreLowerRightStabilizerColor.SetGreen(green);
                CoreLowerLeftStabilizerColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                CoreUpperRightStabilizerColor.SetBlue(blue);
                CoreUpperLeftStabilizerColor.SetBlue(blue);
                CoreLowerRightStabilizerColor.SetBlue(blue);
                CoreLowerLeftStabilizerColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                CoreUpperRightStabilizerColor.SetAlpha(alpha);
                CoreUpperLeftStabilizerColor.SetAlpha(alpha);
                CoreLowerRightStabilizerColor.SetAlpha(alpha);
                CoreLowerLeftStabilizerColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                CoreUpperRightStabilizerColor.SetMain(color);
                CoreUpperLeftStabilizerColor.SetMain(color);
                CoreLowerRightStabilizerColor.SetMain(color);
                CoreLowerLeftStabilizerColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                CoreUpperRightStabilizerColor.SetSub(color);
                CoreUpperLeftStabilizerColor.SetSub(color);
                CoreLowerRightStabilizerColor.SetSub(color);
                CoreLowerLeftStabilizerColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                CoreUpperRightStabilizerColor.SetSupport(color);
                CoreUpperLeftStabilizerColor.SetSupport(color);
                CoreLowerRightStabilizerColor.SetSupport(color);
                CoreLowerLeftStabilizerColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                CoreUpperRightStabilizerColor.SetOptional(color);
                CoreUpperLeftStabilizerColor.SetOptional(color);
                CoreLowerRightStabilizerColor.SetOptional(color);
                CoreLowerLeftStabilizerColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                CoreUpperRightStabilizerColor.SetJoint(color);
                CoreUpperLeftStabilizerColor.SetJoint(color);
                CoreLowerRightStabilizerColor.SetJoint(color);
                CoreLowerLeftStabilizerColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                CoreUpperRightStabilizerColor.SetDevice(color);
                CoreUpperLeftStabilizerColor.SetDevice(color);
                CoreLowerRightStabilizerColor.SetDevice(color);
                CoreLowerLeftStabilizerColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                CoreUpperRightStabilizerColor.Randomize();
                CoreUpperLeftStabilizerColor.Randomize();
                CoreLowerRightStabilizerColor.Randomize();
                CoreLowerLeftStabilizerColor.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                ArmRightStabilizerColor = colorset;
                ArmLeftStabilizerColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                ArmRightStabilizerColor.SetRed(red);
                ArmLeftStabilizerColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                ArmRightStabilizerColor.SetGreen(green);
                ArmLeftStabilizerColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                ArmRightStabilizerColor.SetBlue(blue);
                ArmLeftStabilizerColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                ArmRightStabilizerColor.SetAlpha(alpha);
                ArmLeftStabilizerColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                ArmRightStabilizerColor.SetMain(color);
                ArmLeftStabilizerColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                ArmRightStabilizerColor.SetSub(color);
                ArmLeftStabilizerColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                ArmRightStabilizerColor.SetSupport(color);
                ArmLeftStabilizerColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                ArmRightStabilizerColor.SetOptional(color);
                ArmLeftStabilizerColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                ArmRightStabilizerColor.SetJoint(color);
                ArmLeftStabilizerColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                ArmRightStabilizerColor.SetDevice(color);
                ArmLeftStabilizerColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                ArmRightStabilizerColor.Randomize();
                ArmLeftStabilizerColor.Randomize();
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
            /// Set all colorsets to the given colorset.
            /// </summary>
            /// <param name="colorset">A colorset to set all colorsets to.</param>
            public void SetAll(ColorSet colorset)
            {
                LegsBackStabilizerColor = colorset;
                LegsUpperRightStabilizerColor = colorset;
                LegsUpperLeftStabilizerColor = colorset;
                LegsUpperRightBackStabilizerColor = colorset;
                LegsUpperLeftBackStabilizerColor = colorset;
                LegsMiddleRightStabilizerColor = colorset;
                LegsMiddleLeftStabilizerColor = colorset;
                LegsMiddleRightBackStabilizerColor = colorset;
                LegsMiddleLeftBackStabilizerColor = colorset;
                LegsLowerRightStabilizerColor = colorset;
                LegsLowerLeftStabilizerColor = colorset;
                LegsLowerRightBackStabilizerColor = colorset;
                LegsLowerLeftBackStabilizerColor = colorset;
            }

            /// <summary>
            /// Set all colorsets to the given color.
            /// </summary>
            /// <param name="color">A color to set all colorsets to.</param>
            public void SetAll(Color color)
            {
                SetAll(new ColorSet(color));
            }

            /// <summary>
            /// Set all colorsets to the given red, green, blue, and alpha values.
            /// </summary>
            /// <param name="red">The red value to set all colorsets to.</param>
            /// <param name="green">The green value to set all colorsets to.</param>
            /// <param name="blue">The blue value to set all colorsets to.</param>
            /// <param name="alpha">The alpha value to set all colorsets to.</param>
            public void SetAll(byte red, byte green, byte blue, byte alpha)
            {
                SetAll(new ColorSet(red, green, blue, alpha));
            }

            /// <summary>
            /// Set all colorsets' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colorsets.</param>
            public void SetRed(byte red)
            {
                LegsBackStabilizerColor.SetRed(red);
                LegsUpperRightStabilizerColor.SetRed(red);
                LegsUpperLeftStabilizerColor.SetRed(red);
                LegsUpperRightBackStabilizerColor.SetRed(red);
                LegsUpperLeftBackStabilizerColor.SetRed(red);
                LegsMiddleRightStabilizerColor.SetRed(red);
                LegsMiddleLeftStabilizerColor.SetRed(red);
                LegsMiddleRightBackStabilizerColor.SetRed(red);
                LegsMiddleLeftBackStabilizerColor.SetRed(red);
                LegsLowerRightStabilizerColor.SetRed(red);
                LegsLowerLeftStabilizerColor.SetRed(red);
                LegsLowerRightBackStabilizerColor.SetRed(red);
                LegsLowerLeftBackStabilizerColor.SetRed(red);
            }

            /// <summary>
            /// Set all colorsets' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colorsets.</param>
            public void SetGreen(byte green)
            {
                LegsBackStabilizerColor.SetGreen(green);
                LegsUpperRightStabilizerColor.SetGreen(green);
                LegsUpperLeftStabilizerColor.SetGreen(green);
                LegsUpperRightBackStabilizerColor.SetGreen(green);
                LegsUpperLeftBackStabilizerColor.SetGreen(green);
                LegsMiddleRightStabilizerColor.SetGreen(green);
                LegsMiddleLeftStabilizerColor.SetGreen(green);
                LegsMiddleRightBackStabilizerColor.SetGreen(green);
                LegsMiddleLeftBackStabilizerColor.SetGreen(green);
                LegsLowerRightStabilizerColor.SetGreen(green);
                LegsLowerLeftStabilizerColor.SetGreen(green);
                LegsLowerRightBackStabilizerColor.SetGreen(green);
                LegsLowerLeftBackStabilizerColor.SetGreen(green);
            }

            /// <summary>
            /// Set all colorsets' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colorsets.</param>
            public void SetBlue(byte blue)
            {
                LegsBackStabilizerColor.SetBlue(blue);
                LegsUpperRightStabilizerColor.SetBlue(blue);
                LegsUpperLeftStabilizerColor.SetBlue(blue);
                LegsUpperRightBackStabilizerColor.SetBlue(blue);
                LegsUpperLeftBackStabilizerColor.SetBlue(blue);
                LegsMiddleRightStabilizerColor.SetBlue(blue);
                LegsMiddleLeftStabilizerColor.SetBlue(blue);
                LegsMiddleRightBackStabilizerColor.SetBlue(blue);
                LegsMiddleLeftBackStabilizerColor.SetBlue(blue);
                LegsLowerRightStabilizerColor.SetBlue(blue);
                LegsLowerLeftStabilizerColor.SetBlue(blue);
                LegsLowerRightBackStabilizerColor.SetBlue(blue);
                LegsLowerLeftBackStabilizerColor.SetBlue(blue);
            }

            /// <summary>
            /// Set all colorsets' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colorsets.</param>
            public void SetAlpha(byte alpha)
            {
                LegsBackStabilizerColor.SetAlpha(alpha);
                LegsUpperRightStabilizerColor.SetAlpha(alpha);
                LegsUpperLeftStabilizerColor.SetAlpha(alpha);
                LegsUpperRightBackStabilizerColor.SetAlpha(alpha);
                LegsUpperLeftBackStabilizerColor.SetAlpha(alpha);
                LegsMiddleRightStabilizerColor.SetAlpha(alpha);
                LegsMiddleLeftStabilizerColor.SetAlpha(alpha);
                LegsMiddleRightBackStabilizerColor.SetAlpha(alpha);
                LegsMiddleLeftBackStabilizerColor.SetAlpha(alpha);
                LegsLowerRightStabilizerColor.SetAlpha(alpha);
                LegsLowerLeftStabilizerColor.SetAlpha(alpha);
                LegsLowerRightBackStabilizerColor.SetAlpha(alpha);
                LegsLowerLeftBackStabilizerColor.SetAlpha(alpha);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                LegsBackStabilizerColor.SetMain(color);
                LegsUpperRightStabilizerColor.SetMain(color);
                LegsUpperLeftStabilizerColor.SetMain(color);
                LegsUpperRightBackStabilizerColor.SetMain(color);
                LegsUpperLeftBackStabilizerColor.SetMain(color);
                LegsMiddleRightStabilizerColor.SetMain(color);
                LegsMiddleLeftStabilizerColor.SetMain(color);
                LegsMiddleRightBackStabilizerColor.SetMain(color);
                LegsMiddleLeftBackStabilizerColor.SetMain(color);
                LegsLowerRightStabilizerColor.SetMain(color);
                LegsLowerLeftStabilizerColor.SetMain(color);
                LegsLowerRightBackStabilizerColor.SetMain(color);
                LegsLowerLeftBackStabilizerColor.SetMain(color);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                LegsBackStabilizerColor.SetSub(color);
                LegsUpperRightStabilizerColor.SetSub(color);
                LegsUpperLeftStabilizerColor.SetSub(color);
                LegsUpperRightBackStabilizerColor.SetSub(color);
                LegsUpperLeftBackStabilizerColor.SetSub(color);
                LegsMiddleRightStabilizerColor.SetSub(color);
                LegsMiddleLeftStabilizerColor.SetSub(color);
                LegsMiddleRightBackStabilizerColor.SetSub(color);
                LegsMiddleLeftBackStabilizerColor.SetSub(color);
                LegsLowerRightStabilizerColor.SetSub(color);
                LegsLowerLeftStabilizerColor.SetSub(color);
                LegsLowerRightBackStabilizerColor.SetSub(color);
                LegsLowerLeftBackStabilizerColor.SetSub(color);
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                LegsBackStabilizerColor.SetSupport(color);
                LegsUpperRightStabilizerColor.SetSupport(color);
                LegsUpperLeftStabilizerColor.SetSupport(color);
                LegsUpperRightBackStabilizerColor.SetSupport(color);
                LegsUpperLeftBackStabilizerColor.SetSupport(color);
                LegsMiddleRightStabilizerColor.SetSupport(color);
                LegsMiddleLeftStabilizerColor.SetSupport(color);
                LegsMiddleRightBackStabilizerColor.SetSupport(color);
                LegsMiddleLeftBackStabilizerColor.SetSupport(color);
                LegsLowerRightStabilizerColor.SetSupport(color);
                LegsLowerLeftStabilizerColor.SetSupport(color);
                LegsLowerRightBackStabilizerColor.SetSupport(color);
                LegsLowerLeftBackStabilizerColor.SetSupport(color);
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                LegsBackStabilizerColor.SetOptional(color);
                LegsUpperRightStabilizerColor.SetOptional(color);
                LegsUpperLeftStabilizerColor.SetOptional(color);
                LegsUpperRightBackStabilizerColor.SetOptional(color);
                LegsUpperLeftBackStabilizerColor.SetOptional(color);
                LegsMiddleRightStabilizerColor.SetOptional(color);
                LegsMiddleLeftStabilizerColor.SetOptional(color);
                LegsMiddleRightBackStabilizerColor.SetOptional(color);
                LegsMiddleLeftBackStabilizerColor.SetOptional(color);
                LegsLowerRightStabilizerColor.SetOptional(color);
                LegsLowerLeftStabilizerColor.SetOptional(color);
                LegsLowerRightBackStabilizerColor.SetOptional(color);
                LegsLowerLeftBackStabilizerColor.SetOptional(color);
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                LegsBackStabilizerColor.SetJoint(color);
                LegsUpperRightStabilizerColor.SetJoint(color);
                LegsUpperLeftStabilizerColor.SetJoint(color);
                LegsUpperRightBackStabilizerColor.SetJoint(color);
                LegsUpperLeftBackStabilizerColor.SetJoint(color);
                LegsMiddleRightStabilizerColor.SetJoint(color);
                LegsMiddleLeftStabilizerColor.SetJoint(color);
                LegsMiddleRightBackStabilizerColor.SetJoint(color);
                LegsMiddleLeftBackStabilizerColor.SetJoint(color);
                LegsLowerRightStabilizerColor.SetJoint(color);
                LegsLowerLeftStabilizerColor.SetJoint(color);
                LegsLowerRightBackStabilizerColor.SetJoint(color);
                LegsLowerLeftBackStabilizerColor.SetJoint(color);
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                LegsBackStabilizerColor.SetDevice(color);
                LegsUpperRightStabilizerColor.SetDevice(color);
                LegsUpperLeftStabilizerColor.SetDevice(color);
                LegsUpperRightBackStabilizerColor.SetDevice(color);
                LegsUpperLeftBackStabilizerColor.SetDevice(color);
                LegsMiddleRightStabilizerColor.SetDevice(color);
                LegsMiddleLeftStabilizerColor.SetDevice(color);
                LegsMiddleRightBackStabilizerColor.SetDevice(color);
                LegsMiddleLeftBackStabilizerColor.SetDevice(color);
                LegsLowerRightStabilizerColor.SetDevice(color);
                LegsLowerLeftStabilizerColor.SetDevice(color);
                LegsLowerRightBackStabilizerColor.SetDevice(color);
                LegsLowerLeftBackStabilizerColor.SetDevice(color);
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Randomize all colorsets.
            /// </summary>
            public void Randomize()
            {
                LegsBackStabilizerColor.Randomize();
                LegsUpperRightStabilizerColor.Randomize();
                LegsUpperLeftStabilizerColor.Randomize();
                LegsUpperRightBackStabilizerColor.Randomize();
                LegsUpperLeftBackStabilizerColor.Randomize();
                LegsMiddleRightStabilizerColor.Randomize();
                LegsMiddleLeftStabilizerColor.Randomize();
                LegsMiddleRightBackStabilizerColor.Randomize();
                LegsMiddleLeftBackStabilizerColor.Randomize();
                LegsLowerRightStabilizerColor.Randomize();
                LegsLowerLeftStabilizerColor.Randomize();
                LegsLowerRightBackStabilizerColor.Randomize();
                LegsLowerLeftBackStabilizerColor.Randomize();
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

            /// <summary>
            /// Randomize all pattern selections.
            /// </summary>
            public void Randomize()
            {
                All = RandomColorPattern();
                RandomizeFrame();
                RandomizeUnits();
                RandomizeStabilizers();
            }

            /// <summary>
            /// Randomize all frame pattern selections.
            /// </summary>
            public void RandomizeFrame()
            {
                AllFrames = RandomColorPattern();
                Head = RandomColorPattern();
                Core = RandomColorPattern();
                ArmRight = RandomColorPattern();
                ArmLeft = RandomColorPattern();
                Legs = RandomColorPattern();
            }

            /// <summary>
            /// Randomize all unit pattern selections.
            /// </summary>
            public void RandomizeUnits()
            {
                AllUnits = RandomColorPattern();
                ArmUnitRight = RandomColorPattern();
                ArmUnitLeft = RandomColorPattern();
                BackUnitRight = RandomColorPattern();
                BackUnitLeft = RandomColorPattern();
                ShoulderUnit = RandomColorPattern();
                HangerUnitRight = RandomColorPattern();
                HangerUnitLeft = RandomColorPattern();
            }

            /// <summary>
            /// Randomize all stabilizer pattern selections.
            /// </summary>
            public void RandomizeStabilizers()
            {
                AllStabilizers = RandomColorPattern();
                RandomizeHeadStabilizers();
                RandomizeCoreStabilizers();
                RandomizeArmStabilizers();
                RandomizeLegStabilizers();
            }

            /// <summary>
            /// Randomize all head stabilizer pattern selections.
            /// </summary>
            public void RandomizeHeadStabilizers()
            {
                AllHeadStabilizers = RandomColorPattern();
                HeadTopStabilizer = RandomColorPattern();
                HeadRightStabilizer = RandomColorPattern();
                HeadLeftStabilizer = RandomColorPattern();
            }

            /// <summary>
            /// Randomize all core stabilizer pattern selections.
            /// </summary>
            public void RandomizeCoreStabilizers()
            {
                AllCoreStabilizers = RandomColorPattern();
                CoreUpperRightStabilizer = RandomColorPattern();
                CoreUpperLeftStabilizer = RandomColorPattern();
                CoreLowerRightStabilizer = RandomColorPattern();
                CoreLowerLeftStabilizer = RandomColorPattern();
            }

            /// <summary>
            /// Randomize all arm stabilizer pattern selections.
            /// </summary>
            public void RandomizeArmStabilizers()
            {
                AllArmStabilizers = RandomColorPattern();
                ArmRightStabilizer = RandomColorPattern();
                ArmLeftStabilizer = RandomColorPattern();
            }

            /// <summary>
            /// Randomize all leg stabilizer pattern selections.
            /// </summary>
            public void RandomizeLegStabilizers()
            {
                AllLegStabilizers = RandomColorPattern();
                LegsBackStabilizer = RandomColorPattern();
                LegsUpperRightStabilizer = RandomColorPattern();
                LegsUpperLeftStabilizer = RandomColorPattern();
                LegsMiddleRightStabilizer = RandomColorPattern();
                LegsMiddleLeftStabilizer = RandomColorPattern();
                LegsLowerRightStabilizer = RandomColorPattern();
                LegsLowerLeftStabilizer = RandomColorPattern();
            }

            /// <summary>
            /// Get a random byte then cast it to a color pattern and return that.
            /// </summary>
            /// <returns>A random color pattern.</returns>
            public static ColorPattern RandomColorPattern()
            {
                return (ColorPattern)RandomByteInRange(0, 11);
            }

            /// <summary>
            /// Get a random byte in the given range.
            /// </summary>
            /// <param name="min">The minimum value.</param>
            /// <param name="max">The maximum value.</param>
            /// <returns>A random byte.</returns>
            public static byte RandomByteInRange(byte min, byte max)
            {
                byte[] bytes = new byte[1];
                new Random().NextBytes(bytes);
                return (byte)((bytes[0] * (min - max)) + min);
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
            public Color Main { get; set; }

            /// <summary>
            /// The sub coloring in this color set.
            /// </summary>
            public Color Sub { get; set; }

            /// <summary>
            /// The support coloring in this color set.
            /// </summary>
            public Color Support { get; set; }

            /// <summary>
            /// The optional coloring in this color set.
            /// </summary>
            public Color Optional { get; set; }

            /// <summary>
            /// The joint coloring in this color set.
            /// </summary>
            public Color Joint { get; set; }

            /// <summary>
            /// The device coloring in this color set.
            /// </summary>
            public Color Device { get; set; }

            /// <summary>
            /// Read a new color set from a stream.
            /// </summary>
            public ColorSet(BinaryReaderEx br)
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
            /// Create a new copy of an existing colorset.
            /// </summary>
            /// <param name="colorset">A colorset.</param>
            public ColorSet(ColorSet colorset)
            {
                Main = colorset.Main;
                Sub = colorset.Sub;
                Support = colorset.Support;
                Optional = colorset.Optional;
                Joint = colorset.Joint;
                Device = colorset.Device;
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
            /// Set this colorset to all the colors from another colorset.
            /// </summary>
            /// <param name="colorset">A colorset to get colors from.</param>
            public void SetColorSet(ColorSet colorset)
            {
                Main = colorset.Main;
                Sub = colorset.Sub;
                Support = colorset.Support;
                Optional = colorset.Optional;
                Joint = colorset.Joint;
                Device = colorset.Device;
            }

            /// <summary>
            /// Set this color set's colors all to the selected color.
            /// </summary>
            /// <param name="color">A color in RGBA order.</param>
            public void SetColor(Color color)
            {
                Main = color;
                Sub = color;
                Support = color;
                Optional = color;
                Joint = color;
                Device = color;
            }

            /// <summary>
            /// Set all colors' red value to the given red value.
            /// </summary>
            /// <param name="red">The red value to set on all colors.</param>
            public void SetRed(byte red)
            {
                Main = Color.FromArgb(Main.A, red, Main.G, Main.B);
                Sub = Color.FromArgb(Sub.A, red, Sub.G, Sub.B);
                Support = Color.FromArgb(Support.A, red, Support.G, Support.B);
                Optional = Color.FromArgb(Optional.A, red, Optional.G, Optional.B);
                Joint = Color.FromArgb(Joint.A, red, Joint.G, Joint.B);
                Device = Color.FromArgb(Device.A, red, Device.G, Device.B);
            }

            /// <summary>
            /// Set all colors' green value to the given green value.
            /// </summary>
            /// <param name="green">The green value to set on all colors.</param>
            public void SetGreen(byte green)
            {
                Main = Color.FromArgb(Main.A, Main.R, green, Main.B);
                Sub = Color.FromArgb(Sub.A, Sub.R, green, Sub.B);
                Support = Color.FromArgb(Support.A, Support.R, green, Support.B);
                Optional = Color.FromArgb(Optional.A, Optional.R, green, Optional.B);
                Joint = Color.FromArgb(Joint.A, Joint.R, green, Joint.B);
                Device = Color.FromArgb(Device.A, Device.R, green, Device.B);
            }

            /// <summary>
            /// Set all colors' blue value to the given blue value.
            /// </summary>
            /// <param name="blue">The blue value to set on all colors.</param>
            public void SetBlue(byte blue)
            {
                Main = Color.FromArgb(Main.A, Main.R, Main.G, blue);
                Sub = Color.FromArgb(Sub.A, Sub.R, Main.G, blue);
                Support = Color.FromArgb(Support.A, Support.R, Main.G, blue);
                Optional = Color.FromArgb(Optional.A, Optional.R, Main.G, blue);
                Joint = Color.FromArgb(Joint.A, Joint.R, Main.G, blue);
                Device = Color.FromArgb(Device.A, Device.R, Main.G, blue);
            }

            /// <summary>
            /// Set all colors' alpha value to the given alpha value.
            /// </summary>
            /// <param name="alpha">The alpha value to set on all colors.</param>
            public void SetAlpha(byte alpha)
            {
                Main = Color.FromArgb(alpha, Main.R, Main.G, Main.B);
                Sub = Color.FromArgb(alpha, Sub.R, Main.G, Sub.B);
                Support = Color.FromArgb(alpha, Support.R, Main.G, Support.B);
                Optional = Color.FromArgb(alpha, Optional.R, Main.G, Optional.B);
                Joint = Color.FromArgb(alpha, Joint.R, Main.G, Joint.B);
                Device = Color.FromArgb(alpha, Device.R, Main.G, Device.B);
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetMain(Color color)
            {
                Main = color;
            }

            /// <summary>
            /// Set the Main color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetMain(byte red, byte green, byte blue, byte alpha)
            {
                SetMain(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSub(Color color)
            {
                Sub = color;
            }

            /// <summary>
            /// Set the Sub color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSub(byte red, byte green, byte blue, byte alpha)
            {
                SetSub(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetSupport(Color color)
            {
                Support = color;
            }

            /// <summary>
            /// Set the Support color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetSupport(byte red, byte green, byte blue, byte alpha)
            {
                SetSupport(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetOptional(Color color)
            {
                Optional = color;
            }

            /// <summary>
            /// Set the Optional color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetOptional(byte red, byte green, byte blue, byte alpha)
            {
                SetOptional(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetJoint(Color color)
            {
                Joint = color;
            }

            /// <summary>
            /// Set the Joint color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetJoint(byte red, byte green, byte blue, byte alpha)
            {
                SetJoint(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="color">The color to set.</param>
            public void SetDevice(Color color)
            {
                Device = color;
            }

            /// <summary>
            /// Set the Device color.
            /// </summary>
            /// <param name="red">The red value to set.</param>
            /// <param name="green">The green value to set.</param>
            /// <param name="blue">The blue value to set.</param>
            /// <param name="alpha">The alpha value to set.</param>
            public void SetDevice(byte red, byte green, byte blue, byte alpha)
            {
                SetDevice(Color.FromArgb(alpha, red, green, blue));
            }

            /// <summary>
            /// Set all the colors in this color set's values to 0.
            /// </summary>
            public void SetColorSetEmpty()
            {
                Main = GetEmptyColor();
                Sub = GetEmptyColor();
                Support = GetEmptyColor();
                Optional = GetEmptyColor();
                Joint = GetEmptyColor();
                Device = GetEmptyColor();
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
            /// Get a color with all values set to 0.
            /// </summary>
            /// <returns>A color with all values set to 0.</returns>
            public static Color GetEmptyColor()
            {
                return Color.FromArgb(0, 0, 0, 0);
            }

            /// <summary>
            /// Get a colorset with all colors empty.
            /// </summary>
            /// <returns>A colorset with all colors set empty.</returns>
            public static ColorSet GetEmptyColorSet()
            {
                return new ColorSet(GetEmptyColor());
            }

            /// <summary>
            /// Get a random colorset.
            /// </summary>
            /// <returns>A random colorset.</returns>
            public static ColorSet GetRandomColorSet()
            {
                return new ColorSet(GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor(), GetRandomColor());
            }

            /// <summary>
            /// Get all colors in this colorset.
            /// </summary>
            /// <returns></returns>
            public List<Color> GetColors()
            {
                return new List<Color> { Main, Sub, Support, Optional, Joint };
            }

            /// <summary>
            /// Get all of the colors in a list of colorsets.
            /// </summary>
            /// <param name="colorsets">A list of colorsets.</param>
            /// <returns>A list of colors.</returns>
            public static List<Color> GetColors(List<ColorSet> colorsets)
            {
                var colors = new List<Color>();
                foreach (var colorset in colorsets)
                    colors.AddRange(colorset.GetColors());
                return colors;
            }

            /// <summary>
            /// Randomize the colors in this colorset.
            /// </summary>
            public void Randomize()
            {
                this = GetRandomColorSet();
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
