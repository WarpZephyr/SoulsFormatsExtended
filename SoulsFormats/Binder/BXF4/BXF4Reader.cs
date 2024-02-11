using System.Collections.Generic;
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
            using var brHeader = new BinaryReaderEx(false, bhdPath);
            var brData = new BinaryReaderEx(false, bdtPath);
            Read(brHeader, brData);
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header path and data bytes.
        /// </summary>
        public BXF4Reader(string bhdPath, byte[] bdtBytes)
        {
            using var brHeader = new BinaryReaderEx(false, bhdPath);
            var brData = new BinaryReaderEx(false, bdtBytes);
            Read(brHeader, brData);
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header bytes and data path.
        /// </summary>
        public BXF4Reader(byte[] bhdBytes, string bdtPath)
        {
            using var brHeader = new BinaryReaderEx(false, bhdBytes);
            var brData = new BinaryReaderEx(false, bdtPath);
            Read(brHeader, brData);
        }

        /// <summary>
        /// Reads a <see cref="BXF4"/> from the given header and data bytes.
        /// </summary>
        public BXF4Reader(byte[] bhdBytes, byte[] bdtBytes)
        {
            using var brHeader = new BinaryReaderEx(false, bhdBytes);
            var brData = new BinaryReaderEx(false, bdtBytes);
            Read(brHeader, brData);
        }

        /// <summary>
        /// Reads only the files of a BHF4 header.
        /// </summary>
        private BXF4Reader(BinaryReaderEx br)
        {
            using BinaryReaderEx brd = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type _);
            Files = BXF4.ReadBHFHeader(this, brd);
        }

        /// <summary>
        /// Get file headers for a <see cref="BXF4"/> from the given path.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(string path)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, path);
            return new BXF4Reader(br).Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BXF4"/> from the given bytes.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(byte[] bytes)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return new BXF4Reader(br).Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BXF4"/> from the given <see cref="Stream"/>.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(Stream stream)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, stream, true);
            return new BXF4Reader(br).Files;
        }

        private void Read(BinaryReaderEx brHeader, BinaryReaderEx brData)
        {
            using BinaryReaderEx brHeaderDecompressed = SFUtil.GetDecompressedBinaryReader(brHeader, out DCX.Type _);
            BinaryReaderEx brDataDecompressed = SFUtil.GetDecompressedBinaryReader(brData, out DCX.Type dataCompression);
            if (dataCompression != DCX.Type.None)
            {
                brData.Dispose();
            }

            BXF4.ReadBDFHeader(brDataDecompressed);
            Files = BXF4.ReadBHFHeader(this, brHeaderDecompressed);
            DataBR = brDataDecompressed;
        }
    }
}
