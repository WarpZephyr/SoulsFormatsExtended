namespace SoulsFormats
{
    /// <summary>
    /// Supported primitive field types in paramdefs.
    /// </summary>
    public enum DefType
    {
        /// <summary>
        /// Signed 1-byte integer.
        /// </summary>
        s8,

        /// <summary>
        /// Unsigned 1-byte integer.
        /// </summary>
        u8,

        /// <summary>
        /// Signed 2-byte integer.
        /// </summary>
        s16,

        /// <summary>
        /// Unsigned 2-byte integer.
        /// </summary>
        u16,

        /// <summary>
        /// Signed 4-byte integer.
        /// </summary>
        s32,

        /// <summary>
        /// Unsigned 4-byte integer.
        /// </summary>
        u32,

        /// <summary>
        /// Single-precision floating point value.
        /// </summary>
        f32,

        /// <summary>
        /// 4-byte integer representing a boolean.
        /// </summary>
        b32,

        /// <summary>
        /// Single-precision floating point value representing an angle.
        /// </summary>
        angle32,

        /// <summary>
        /// Double-precision floating point value.
        /// </summary>
        f64,

        /// <summary>
        /// Byte or array of bytes used for padding or placeholding.
        /// </summary>
        dummy8,

        /// <summary>
        /// Fixed-width Shift-JIS string.
        /// </summary>
        fixstr,

        /// <summary>
        /// Fixed-width UTF-16 string.
        /// </summary>
        fixstrW,
    }
}
