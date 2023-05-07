using static SoulsFormats.AcPartsFA;

namespace SoulsFormats
{
    /// <summary>
    /// An interface for parts in acparts.
    /// </summary>
    public interface IPart
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        PartComponent PartComponent { get; set; }
    }
}
