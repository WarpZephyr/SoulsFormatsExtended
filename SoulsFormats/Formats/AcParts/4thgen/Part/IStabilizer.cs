using static SoulsFormats.AcParts4.Component;

namespace SoulsFormats
{
    /// <summary>
    /// An interface for stabilizer parts in acparts.
    /// </summary>
    public interface IStabilizer
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        PartComponent PartComponent { get; set; }

        /// <summary>
        /// A Component which contains Stabilizer stats.
        /// </summary>
        StabilizerComponent StabilizerComponent { get; set; }
    }
}