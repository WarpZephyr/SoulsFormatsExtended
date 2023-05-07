namespace SoulsFormats
{
    /// <summary>
    /// FMOD sound banks.
    /// </summary>
    public class FSB4 : SoulsFile<FSB4>
    {
        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(0, 4);
            return magic == "FSB4";
        }
    }
}
