using System.Collections.Generic;
using System.Xml;

namespace SoulsFormats
{
    /// <summary>
    /// FromSoftware's Ascii Format Model Data.
    /// </summary>
    public partial class AP2 : SoulsFile<AP2>
    {
        /// <summary>
        /// The version of the Ascii Format Model Data File.
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// The scene.
        /// </summary>
        public Scene ModelScene { get; set; }

        /// <inheritdoc/>
        protected override bool Is(BinaryReaderEx br)
        {
            return br.GetASCII(0, 39) == ";\r\n; Ascii Format Model Data File (AP2)";
        }

        /// <inheritdoc/>
        protected override void Read(BinaryReaderEx br)
        {

        }
    }
}
