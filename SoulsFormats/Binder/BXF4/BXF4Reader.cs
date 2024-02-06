using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// An on-demand reader for <see cref="BXF4"/> containers.
    /// </summary>
    public class BXF4Reader : BinderReader, IBXF4
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        public bool Unk04 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public bool Unk05 { get; set; }

        /// <summary>
        /// Whether to write strings in UTF-16.
        /// </summary>
        public bool Unicode { get; set; }

        /// <summary>
        /// Indicates the presence of a filename hash table.
        /// </summary>
        public byte Extended { get; set; }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header and data paths.
        /// </summary>
        public BXF4Reader(string bhdPath, string bdtPath)
        {
            using (FileStream fsHeader = File.OpenRead(bhdPath))
            {
                FileStream fsData = File.OpenRead(bdtPath);
                var brHeader = new BinaryReaderEx(false, fsHeader);
                var brData = new BinaryReaderEx(false, fsData);
                Read(brHeader, brData);
            }
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header path and data bytes.
        /// </summary>
        public BXF4Reader(string bhdPath, byte[] bdtBytes)
        {
            using (FileStream fsHeader = File.OpenRead(bhdPath))
            {
                var msData = new MemoryStream(bdtBytes);
                var brHeader = new BinaryReaderEx(false, fsHeader);
                var brData = new BinaryReaderEx(false, msData);
                Read(brHeader, brData);
            }
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header bytes and data path.
        /// </summary>
        public BXF4Reader(byte[] bhdBytes, string bdtPath)
        {
            using (var msHeader = new MemoryStream(bhdBytes))
            {
                FileStream fsData = File.OpenRead(bdtPath);
                var brHeader = new BinaryReaderEx(false, msHeader);
                var brData = new BinaryReaderEx(false, fsData);
                Read(brHeader, brData);
            }
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header and data bytes.
        /// </summary>
        public BXF4Reader(byte[] bhdBytes, byte[] bdtBytes)
        {
            using (var msHeader = new MemoryStream(bhdBytes))
            {
                var msData = new MemoryStream(bdtBytes);
                var brHeader = new BinaryReaderEx(false, msHeader);
                var brData = new BinaryReaderEx(false, msData);
                Read(brHeader, brData);
            }
        }

        private void Read(BinaryReaderEx brHeader, BinaryReaderEx brData)
        {
            BXF4.ReadBDFHeader(brData);
            Files = BXF4.ReadBHFHeader(this, brHeader);
            DataBR = brData;
        }
    }
}
