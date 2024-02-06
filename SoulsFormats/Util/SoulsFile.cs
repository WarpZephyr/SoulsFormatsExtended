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
            using (BinaryReaderEx br = new BinaryReaderEx(false, stream))
            {
                return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
            }
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

            using (MemoryStream ms = new MemoryStream(bytes, false))
            using (BinaryReaderEx br = new BinaryReaderEx(false, ms))
            {
                return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
            }
        }

        /// <summary>
        /// Returns true if the file appears to be a file of this type.
        /// </summary>
        public static bool Is(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs.Length == 0)
                {
                    return false;
                }

                using (BinaryReaderEx br = new BinaryReaderEx(false, fs))
                {
                    return new TFormat().Is(SFUtil.GetDecompressedBinaryReader(br, out _));
                }
            }
        }

        /// <summary>
        /// Loads file data from a BinaryReaderEx.
        /// </summary>
        protected virtual void Read(BinaryReaderEx br) => throw new NotImplementedException($"{nameof(Read)} is not implemented for this format.");

        /// <summary>
        /// Loads a file from a stream, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(Stream stream)
        {
            TFormat file = new TFormat();
            using (BinaryReaderEx br = SFUtil.GetDecompressedBinaryReader(new BinaryReaderEx(false, stream), out DCX.Type compression))
            {
                file.Compression = compression;
                file.Read(br);
            }
            return file;
        }

        /// <summary>
        /// Loads a file from a byte array, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(byte[] bytes)
        {
            TFormat file = new TFormat();
            using (MemoryStream ms = new MemoryStream(bytes, false))
            using (BinaryReaderEx br = SFUtil.GetDecompressedBinaryReader(new BinaryReaderEx(false, ms), out DCX.Type compression))
            {
                file.Compression = compression;
                file.Read(br);
            }
            return file;
        }

        /// <summary>
        /// Loads a file from the specified path, automatically decompressing it if necessary.
        /// </summary>
        public static TFormat Read(string path)
        {
            TFormat file = new TFormat();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReaderEx br = SFUtil.GetDecompressedBinaryReader(new BinaryReaderEx(false, fs), out DCX.Type compression))
            {
                file.Compression = compression;
                file.Read(br);
            }
            return file;
        }

        /// <summary>
        /// Returns whether the <see cref="BinaryReaderEx"/> appears to be a file of this type and reads it if so, automatically decompressing it if necessary.
        /// </summary>
        private static bool IsRead(BinaryReaderEx br, out TFormat file)
        {
            var dummy = new TFormat();
            using (br = SFUtil.GetDecompressedBinaryReader(br, out DCX.Type compression))
            {
                if (dummy.Is(br))
                {
                    br.Position = 0;
                    dummy.Compression = compression;
                    dummy.Read(br);
                    file = dummy;
                    return true;
                }
            }

            file = null;
            return false;
        }

        /// <summary>
        /// Returns whether the stream appears to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(Stream stream, out TFormat file)
        {
            using (BinaryReaderEx br = new BinaryReaderEx(false, stream))
            {
                return IsRead(br, out file);
            }
        }

        /// <summary>
        /// Returns whether the bytes appear to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(byte[] bytes, out TFormat file)
        {
            using (MemoryStream ms = new MemoryStream(bytes, false))
            using (BinaryReaderEx br = new BinaryReaderEx(false, ms))
            {
                return IsRead(br, out file);
            }
        }

        /// <summary>
        /// Returns whether the file appears to be a file of this type and reads it if so.
        /// </summary>
        public static bool IsRead(string path, out TFormat file)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReaderEx br = new BinaryReaderEx(false, fs))
            {
                return IsRead(br, out file);
            }
        }

        /// <summary>
        /// Writes file data to a <see cref="BinaryWriterEx"/>.
        /// </summary>
        protected virtual void Write(BinaryWriterEx bw) => throw new NotImplementedException($"{nameof(Write)} is not implemented for this format.");

        /// <summary>
        /// Writes file data to a <see cref="BinaryWriterEx"/>, compressing it afterwards if specified.
        /// </summary>
        private void Write(BinaryWriterEx bw, DCX.Type compression)
        {
            if (compression == DCX.Type.None)
            {
                Write(bw);
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryWriterEx bwd = new BinaryWriterEx(false, ms);
                    Write(bwd);
                    DCX.Compress(bwd.FinishBytes(), bw, compression);
                }
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

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriterEx bw = new BinaryWriterEx(false, ms);
                Write(bw, compression);
                return bw.FinishBytes();
            }
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
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096))
            using (BinaryWriterEx bw = new BinaryWriterEx(false, fs))
            {
                Write(bw, compression);
                bw.Finish();
            }
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
