using System.Collections.Generic;
using System.Drawing;

namespace SoulsFormats
{
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
