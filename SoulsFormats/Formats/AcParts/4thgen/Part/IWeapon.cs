using static SoulsFormats.AcParts4.Component;

namespace SoulsFormats
{
    /// <summary>
    /// An interface for parts with weapon stats in ACPARTS.
    /// </summary>
    public interface IWeapon
    {
        /// <summary>
        /// A Component which contains common stats across all parts.
        /// </summary>
        PartComponent PartComponent { get; set; }

        /// <summary>
        /// A Component which contains stats for weapons.
        /// </summary>
        WeaponComponent WeaponComponent { get; set; }
    }
}