using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace SoulsFormats
{
    /// <summary>
    /// An on-demand reader for <see cref="BND4"/> containers.
    /// </summary>
    public class BND4Reader : BinderReader, IBND4
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
        /// Whether to encode filenames as UTF-8 or Shift JIS.
        /// </summary>
        public bool Unicode { get; set; }

        /// <summary>
        /// Indicates presence of filename hash table.
        /// </summary>
        public byte Extended { get; set; }

        /// <summary>
        /// Type of compression used, if any.
        /// </summary>
        public DCX.Type Compression { get; set; }

        /// <summary>
        /// Reads a <see cref="BND4"/> from the given path, decompressing if necessary.
        /// </summary>
        public BND4Reader(string path)
        {
            var br = new BinaryReaderEx(false, path);
            Read(br);
        }

        /// <summary>
        /// Reads a <see cref="BND4"/> from the given bytes, decompressing if necessary.
        /// </summary>
        public BND4Reader(byte[] bytes)
        {
            var br = new BinaryReaderEx(false, bytes);
            Read(br);
        }

        /// <summary>
        /// Reads a <see cref="BND4"/> from the given <see cref="Stream"/>, decompressing if necessary.
        /// </summary>
        public BND4Reader(Stream stream)
        {
            var br = new BinaryReaderEx(false, stream, true);
            Read(br);
        }

        /// <summary>
        /// Get file headers for a <see cref="BND4"/> from the given path.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(string path)
        {
            var reader = new BND4Reader(path);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BND4"/> from the given bytes.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(byte[] bytes)
        {
            var reader = new BND4Reader(bytes);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BND4"/> from the given <see cref="Stream"/>.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(Stream stream)
        {
            var reader = new BND4Reader(stream);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        private void Read(BinaryReaderEx br)
        {
            br = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type compression);
            Compression = compression;
            Files = BND4.ReadHeader(this, br);
            DataBR = br;
        }
    }
}
