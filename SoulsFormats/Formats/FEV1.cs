namespace SoulsFormats
{
    /// <summary>
    /// FMOD events.
    /// </summary>
    public class FEV1 : SoulsFile<FEV1>
    {
        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            return br.GetASCII(br.Position, 4) == "FEV1";
        }
    }
}
