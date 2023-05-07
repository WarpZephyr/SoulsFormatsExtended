using static SoulsFormats.AcPartsFA;

namespace SoulsFormats
{
    /// <summary>
    /// An interface for booster parts in ACPARTS.
    /// </summary>
    public interface IBooster
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        PartComponent PartComponent { get; set; }

        /// <summary>
        /// Horizontal Booster stats.
        /// </summary>
        BoosterComponent HorizontalBoost { get; set; }
    }
}
