namespace SoulsFormats
{
    /// <summary>
    /// A model format used in Armored Core Formula Front on the PSP version. It probably stands for Playstation Portable Model.
    /// </summary>
    public class PPM : SoulsFile<PPM>
    {
        /// <summary>
        /// Returns true if the data appears to be a PPM model file.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            br.BigEndian = false;
            if (br.Length < 184)
            {
                return false;
            }

            return br.ReadASCII(4) == "PPM ";
        }
    }
}
