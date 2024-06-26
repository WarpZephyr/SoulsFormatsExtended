﻿using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// An on-demand reader for <see cref="BND3"/> containers.
    /// </summary>
    public class BND3Reader : BinderReader, IBND3
    {
        /// <summary>
        /// Unknown; always 0 except in DeS where it's occasionally 0x80000000 (probably a byte).
        /// </summary>
        public int Unk18 { get; set; }

        /// <summary>
        /// Whether or not to write the file headers end value or 0.<br/>
        /// Some Binders have this as 0 and require it to be as such for some reason.
        /// </summary>
        public bool WriteFileHeadersEnd { get; set; }

        /// <summary>
        /// Type of compression used, if any.
        /// </summary>
        public DCX.Type Compression { get; set; }

        /// <summary>
        /// Reads a <see cref="BND3"/> from the given path, decompressing if necessary.
        /// </summary>
        public BND3Reader(string path)
        {
            var br = new BinaryReaderEx(false, path);
            Read(br);
        }

        /// <summary>
        /// Reads a <see cref="BND3"/> from the given bytes, decompressing if necessary.
        /// </summary>
        public BND3Reader(byte[] bytes)
        {
            var br = new BinaryReaderEx(false, bytes);
            Read(br);
        }

        /// <summary>
        /// Reads a <see cref="BND3"/> from the given <see cref="Stream"/>, decompressing if necessary.
        /// </summary>
        public BND3Reader(Stream stream)
        {
            var br = new BinaryReaderEx(false, stream, true);
            Read(br);
        }

        /// <summary>
        /// Get file headers for a <see cref="BND3"/> from the given path.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(string path)
        {
            var reader = new BND3Reader(path);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BND3"/> from the given bytes.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(byte[] bytes)
        {
            var reader = new BND3Reader(bytes);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        /// <summary>
        /// Get file headers for a <see cref="BND3"/> from the given <see cref="Stream"/>.
        /// </summary>
        public static List<BinderFileHeader> GetFileHeaders(Stream stream)
        {
            var reader = new BND3Reader(stream);
            reader.DataBR.Dispose();
            return reader.Files;
        }

        private void Read(BinaryReaderEx br)
        {
            br = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type compression);
            Compression = compression;
            Files = BND3.ReadHeader(this, br);
            DataBR = br;
        }
    }
}
