using System.Text;

namespace SoulsFormats
{
    /// <summary>
    /// Encoding for various text related things in SoulsFormats.
    /// </summary>
    internal static class SFEncoding
    {
        /// <summary>
        /// ASCII encoding.
        /// </summary>
        public static readonly Encoding ASCII;

        /// <summary>
        /// UTF8 encoding.
        /// </summary>
        public static readonly Encoding UTF8;

        /// <summary>
        /// Shift-JIS encoding.
        /// </summary>
        public static readonly Encoding ShiftJIS;

        /// <summary>
        /// UTF16 or Unicode encoding.
        /// </summary>
        public static readonly Encoding UTF16;

        /// <summary>
        /// UTF16 or Unicode encoding BigEndian version.
        /// </summary>
        public static readonly Encoding UTF16BE;

        static SFEncoding()
        {
#if NETSTANDARD
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            ASCII = Encoding.ASCII;
            UTF8 = Encoding.UTF8;
            ShiftJIS = Encoding.GetEncoding("shift-jis");
            UTF16 = Encoding.Unicode;
            UTF16BE = Encoding.BigEndianUnicode;
        }
    }
}
