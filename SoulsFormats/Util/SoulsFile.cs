using System;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// A generic From file supporting transparent DCX reading and writing.
    /// </summary>
    public abstract class SoulsFile<TFormat> : ISoulsFile where TFormat : SoulsFile<TFormat>, new()
    {
        /// <summary>
        /// The type of <see cref="DCX"/> compression to be used when writing.
        /// </summary>
        public DCX.Type Compression { get; set; } = DCX.Type.None;

        /// <summary>
        /// Returns true if the data appears to be a file of this type.
        /// </summary>
        // This should really be a static method, but interfaces do not allow static inheritance; hence the dummy objects below.
        protected virtual bool Is(BinaryReaderEx br) => throw new NotImplementedException($"{nameof(Is)} is not implemented for this format.");

        /// <summary>
        /// Returns true if the stream appears to be a file of this type.
        /// </summary>
        public static bool Is(Stream stream)
        {
            if ((stream.Length - stream.Position) == 0)
            {
                return false;
            }

            using BinaryReaderEx br = new BinaryReaderEx(false, stream, true);
            return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
        }

        /// <summary>
        /// Returns true if the bytes appear to be a file of this type.
        /// </summary>
        public static bool Is(byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                return false;
            }

            using BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
        }

        /// <summary>
        /// Returns true if the file appears to be a file of this type.
        /// </summary>
        public static bool Is(string path)
        {
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length == 0)
            {
                return false;
            }

            using BinaryReaderEx br = new BinaryReaderEx(false, fs);
            return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
        }

        /// <summary>
        /// Loads a file from a stream, automatically decompressing it if necessary.
        /// </summary>
        protected virtual void Read(BinaryReaderEx br) => throw new NotImplementedException($"{nameof(Read)} is not implemented for this format.");

        /// <summary>
        /// Loads a file from a stream, automatically decompressing it if necessary.
        /// </summary>
        internal static TFormat ReadInternal(BinaryReaderEx br)
        {
            TFormat file = new TFormat();
            file.Read(br);
            return file;
        }

        /// <summary>
        /// Loads a file from a stream, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(Stream stream)
        {
            TFormat file = new TFormat();
            using BinaryReaderEx br = SFUtil.GetDecompressedBinaryReader(new BinaryReaderEx(false, stream, true), out DCX.Type compression);
            file.Compression = compression;
            file.Read(br);
            return file;
        }

        /// <summary>
        /// Loads a file from a byte array, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(byte[] bytes)
        {
            TFormat file = new TFormat();
            using BinaryReaderEx br = SFUtil.GetDecompressedBinaryReader(new BinaryReaderEx(false, bytes), out DCX.Type compression);
            file.Compression = compression;
            file.Read(br);
            return file;
        }

        /// <summary>
        /// Loads a file from the specified path, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(string path)
        {
            TFormat file = new TFormat();
            using BinaryReaderEx br = new BinaryReaderEx(false, path);
            using BinaryReaderEx brd = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type compression);
            file.Compression = compression;
            file.Read(brd);
            return file;
        }

        /// <summary>
        /// Returns whether or not the <see cref="Stream"/> read as this format correctly.
        /// </summary>
        public static bool TryRead(Stream stream, out TFormat file)
        {
            long pos = stream.Position;
            using BinaryReaderEx br = new BinaryReaderEx(false, stream, true);
            bool result = TryRead(br, out file);
            stream.Position = pos;
            return result;
        }

        /// <summary>
        /// Returns whether or not the bytes read as this format correctly.
        /// </summary>
        public static bool TryRead(byte[] bytes, out TFormat file)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return TryRead(br, out file);
        }

        /// <summary>
        /// Returns whether or not file read as this format correctly.
        /// </summary>
        public static bool TryRead(string path, out TFormat file)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, path);
            return TryRead(br, out file);
        }

        /// <summary>
        /// Returns whether or not the data read as this format correctly.
        /// </summary>
        private static bool TryRead(BinaryReaderEx br, out TFormat file)
        {
            var dummy = new TFormat();
            using (br = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type compression))
            {
                try
                {
                    dummy.Compression = compression;
                    dummy.Read(br);
                    file = dummy;
                    return true;
                }
                catch
                {
                    file = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns whether or not the data appears to be a file of this type and reads it if so, automatically decompressing it if necessary.
        /// </summary>
        private static bool IsReadInternal(BinaryReaderEx br, out TFormat file)
        {
            var oldPos = br.Position;
            var dummy = new TFormat();
            
            if (DCX.Is(br))
            {
                byte[] bytes = DCX.Decompress(br, out DCX.Type compression);
                using var dbr = new BinaryReaderEx(false, bytes);
                if (dummy.Is(dbr))
                {
                    dbr.Position = 0;
                    dummy.Compression = compression;
                    dummy.Read(dbr);
                    file = dummy;
                    return true;
                }

                file = null;
                return false;
            }

            if (dummy.Is(br))
            {
                br.Position = 0;
                dummy.Compression = DCX.Type.None;
                dummy.Read(br);
                file = dummy;
                return true;
            }

            br.Position = oldPos;
            file = null;
            return false;
        }

        /// <summary>
        /// Returns whether the data appears to be a file of this type and reads it if so.
        /// </summary>
        // A more direct check without re-reading everything.
        public static bool IsRead(BinaryReaderEx br, out TFormat file)
            => IsReadInternal(br, out file);

        /// <summary>
        /// Returns whether the stream appears to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(Stream stream, out TFormat file)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, stream, true);
            return IsReadInternal(br, out file);
        }

        /// <summary>
        /// Returns whether the bytes appear to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(byte[] bytes, out TFormat file)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return IsReadInternal(br, out file);
        }

        /// <summary>
        /// Returns whether the file appears to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(string path, out TFormat file)
        {
            using BinaryReaderEx br = new BinaryReaderEx(false, path);
            return IsReadInternal(br, out file);
        }

        /// <summary>
        /// Writes file data to a stream.
        /// </summary>
        protected virtual void Write(BinaryWriterEx bw) => throw new NotImplementedException($"{nameof(Write)} is not implemented for this format.");

        /// <summary>
        /// Writes file data to a stream, compressing it afterwards if specified.
        /// </summary>
        private void Write(BinaryWriterEx bw, DCX.Type compression)
        {
            if (compression == DCX.Type.None)
            {
                Write(bw);
            }
            else
            {
                BinaryWriterEx bwd = new BinaryWriterEx(false);
                Write(bwd);
                DCX.Compress(bwd.FinishBytes(), bw, compression);
            }
        }

        /// <summary>
        /// Writes the file to an array of bytes, automatically compressing it if necessary.
        /// </summary>
        public byte[] Write()
        {
            return Write(Compression);
        }

        /// <summary>
        /// Writes the file to an array of bytes, compressing it as specified.
        /// </summary>
        public byte[] Write(DCX.Type compression)
        {
            if (!Validate(out Exception ex))
            {
                throw ex;
            }

            BinaryWriterEx bw = new BinaryWriterEx(false);
            Write(bw, compression);
            return bw.FinishBytes();
        }

        /// <summary>
        /// Writes the file to the specified path, automatically compressing it if necessary.
        /// </summary>
        public void Write(string path)
        {
            Write(path, Compression);
        }

        /// <summary>
        /// Writes the file to the specified path, compressing it as specified.
        /// </summary>
        public void Write(string path, DCX.Type compression)
        {
            if (!Validate(out Exception ex))
            {
                throw ex;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new NullReferenceException($"Failed to get directory name for: {path}"));
            using BinaryWriterEx bw = new BinaryWriterEx(false, path);
            Write(bw, compression);
            bw.Finish();
        }

        /// <summary>
        /// Checks the object for any fatal problems; Write will throw the returned exception on failure.
        /// </summary>
        public virtual bool Validate(out Exception ex)
        {
            ex = null;
            return true;
        }

        /// <summary>
        /// Returns whether the object is not null, otherwise setting ex to a NullReferenceException with the given message.
        /// </summary>
        protected static bool ValidateNull(object obj, string message, out Exception ex)
        {
            if (obj == null)
            {
                ex = new NullReferenceException(message);
                return false;
            }

            ex = null;
            return true;
        }

        /// <summary>
        /// Returns whether the index is in range, otherwise setting ex to an IndexOutOfRangeException with the given message.
        /// </summary>
        protected static bool ValidateIndex(long count, long index, string message, out Exception ex)
        {
            if (index < 0 || index >= count)
            {
                ex = new IndexOutOfRangeException(message);
                return false;
            }

            ex = null;
            return true;
        }
    }
}
