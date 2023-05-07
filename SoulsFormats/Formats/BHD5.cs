using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SoulsFormats
{
    /// <summary>
    /// The header file of the dvdbnd container format used to package all game files with hashed filenames.
    /// </summary>
    public class BHD5
    {
        /// <summary>
        /// Format the file should be written in.
        /// </summary>
        public Game Format { get; set; }

        /// <summary>
        /// Whether the header is big-endian.
        /// </summary>
        public bool BigEndian { get; set; }

        /// <summary>
        /// Unknown; possibly whether crypto is allowed? Offsets are present regardless.
        /// </summary>
        public bool Unk05 { get; set; }

        /// <summary>
        /// A salt used to calculate SHA hashes for file data.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Collections of files grouped by their hash value for faster lookup.
        /// </summary>
        public List<Bucket> Buckets { get; set; }

        /// <summary>
        /// Returns true if the bytes appear to be a BHD5 header file.
        /// </summary>
        public static bool IsBHD(byte[] bytes)
        {
            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return IsBHD(SFUtil.GetDecompressedBR(br, out _));
        }

        /// <summary>
        /// Returns true if the file appears to be a BHD5 header file.
        /// </summary>
        public static bool IsBHD(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                BinaryReaderEx br = new BinaryReaderEx(false, fs);
                return IsBHD(br);
            }
        }

        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        public static bool IsBHD(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(0, 4);
            return magic == "BHD5";
        }

        /// <summary>
        /// Returns true if the bytes appear to be a BHD5 data file.
        /// </summary>
        public static bool IsBDT(byte[] bytes)
        {
            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            return IsBDT(SFUtil.GetDecompressedBR(br, out _));
        }

        /// <summary>
        /// Returns true if the file appears to be a BHD5 data file.
        /// </summary>
        public static bool IsBDT(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                BinaryReaderEx br = new BinaryReaderEx(false, fs);
                return IsBDT(br);
            }
        }

        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        public static bool IsBDT(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(0, 4);
            return magic == "BDF3";
        }

        /// <summary>
        /// Read a dvdbnd header from the given stream, formatted for the given game. Must already be decrypted, if applicable.
        /// </summary>
        public static BHD5 Read(string path, Game game)
        {
            Stream stream = File.OpenRead(path);
            return Read(stream, game);
        }

        /// <summary>
        /// Read a dvdbnd header from the given stream, formatted for the given game. Must already be decrypted, if applicable.
        /// </summary>
        public static BHD5 Read(Stream bhdStream, Game game)
        {
            var br = new BinaryReaderEx(false, bhdStream);
            return new BHD5(br, game);
        }

        /// <summary>
        /// Write a dvdbnd header to the given stream.
        /// </summary>
        public void Write(Stream bhdStream)
        {
            var bw = new BinaryWriterEx(false, bhdStream);
            Write(bw);
            bw.Finish();
        }

        /// <summary>
        /// Write a dvdbnd header to the given path.
        /// </summary>
        public void Write(string bhdPath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(bhdPath));
            using (FileStream stream = File.Create(bhdPath))
            {
                BinaryWriterEx bw = new BinaryWriterEx(false, stream);
                Write(bw);
                bw.Finish();
            }
        }

        /*
        /// <summary>
        /// Write a dvdbnd header and data to the given paths.
        /// </summary>
        public void Write(string bhdPath, string bdtPath, string bdtDataDirPath)
        {
            // Write the BHD header.
            Write(bhdPath);

            // If the data path for BDT does not exist throw.
            if (!Directory.Exists(bdtDataDirPath))
                throw new DirectoryNotFoundException("BDT data must be in specified directory to have BDT written, but no directory was found on provided path.");

            // Get the files to repack into BDT.
            string[] dirPaths = Directory.GetFiles(bdtDataDirPath, "*", SearchOption.AllDirectories);

            // Remove bdt extract directory name from path.
            for (int i = 0; i < dirPaths.Length; i++)
                dirPaths[i] = dirPaths[i].Substring(dirPaths[i].IndexOf("\\"));

            // Get the number of hashes we need to repack.
            int hashCount = HashCount();
            Console.WriteLine($"Hash count is {hashCount}");

            // Verify there are enough files to repack to begin with.
            if (dirPaths.Length < hashCount)
                throw new Exception("There is not enough files to fill all the hashes in the BHD.");

            using (var bdtStream = File.Create(bdtPath))
            {
                // Search each bucket
                foreach (var bucket in Buckets)
                {
                    Console.WriteLine($"Checking next Bucket");
                    // Search each hash in bucket
                    foreach (var hash in bucket)
                    {
                        Console.WriteLine($"Checking hash {hash.FileNameHash}");
                        bool matched = false;
                        // Search for a hash to match in each path
                        foreach (string path in dirPaths)
                        {
                            // If the file name is the hash already, use it instead of hashing it
                            if (Path.GetFileNameWithoutExtension(path) == hash.FileNameHash.ToString())
                            {
                                Console.WriteLine($"Hash {hash.FileNameHash} matched with path {path}");
                                matched = true;
                                byte[] bytes = File.ReadAllBytes(path);
                                if (bytes.Length > hash.PaddedFileSize)
                                    throw new ArgumentOutOfRangeException($"The path {path} byte length is longer than the hash PaddedFileSize, this may lead to overflowing into other files.");
                                bdtStream.Write(bytes, (int)hash.FileOffset, bytes.Length);
                            }
                            // If the path being hashed is the file name hash we have a match.
                            else if (SFUtil.FromPathHash(path) == hash.FileNameHash)
                            {
                                Console.WriteLine($"Hash {hash.FileNameHash} matched with path {path}");
                                matched = true;
                                byte[] bytes = File.ReadAllBytes(path);
                                if (bytes.Length > hash.PaddedFileSize)
                                    throw new ArgumentOutOfRangeException($"The path {path} byte length is longer than the hash PaddedFileSize, this may lead to overflowing into other files.");
                                bdtStream.Write(bytes, (int)hash.FileOffset, bytes.Length);
                            }
                        }
                        // We did not find a match for this hash.
                        if (matched == false)
                            throw new Exception($"No paths matched hash {hash.FileNameHash} in the given BDT data directory.");
                    }
                }
                bdtStream.Close();
            }
        }
        */

        /// <summary>
        /// Get the total count of all the hashes in the BHD5.
        /// </summary>
        /// <returns>The total number of hashes in the BHD5.</returns>
        public int HashCount()
        {
            int count = 0;
            foreach (var bucket in Buckets)
                foreach (var hash in bucket)
                    count++;
            return count;
        }

        /// <summary>
        /// Creates an empty BHD5.
        /// </summary>
        public BHD5(Game game)
        {
            Format = game;
            Salt = "";
            Buckets = new List<Bucket>();
        }

        private BHD5(BinaryReaderEx br, Game game)
        {
            Format = game;

            br.AssertASCII("BHD5");
            BigEndian = br.AssertSByte(0, -1) == 0;
            br.BigEndian = BigEndian;
            Unk05 = br.ReadBoolean();
            br.AssertByte(0);
            br.AssertByte(0);
            br.AssertInt32(1);
            br.ReadInt32(); // File size
            int bucketCount = br.ReadInt32();
            int bucketsOffset = br.ReadInt32();

            if (game >= Game.DarkSouls2)
            {
                int saltLength = br.ReadInt32();
                Salt = br.ReadASCII(saltLength);
                // No padding
            }

            br.Position = bucketsOffset;
            Buckets = new List<Bucket>(bucketCount);
            for (int i = 0; i < bucketCount; i++)
                Buckets.Add(new Bucket(br, game));
        }

        private void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = BigEndian;
            bw.WriteASCII("BHD5");
            bw.WriteSByte((sbyte)(BigEndian ? 0 : -1));
            bw.WriteBoolean(Unk05);
            bw.WriteByte(0);
            bw.WriteByte(0);
            bw.WriteInt32(1);
            bw.ReserveInt32("FileSize");
            bw.WriteInt32(Buckets.Count);
            bw.ReserveInt32("BucketsOffset");

            if (Format >= Game.DarkSouls2)
            {
                bw.WriteInt32(Salt.Length);
                bw.WriteASCII(Salt);
            }

            bw.FillInt32("BucketsOffset", (int)bw.Position);
            for (int i = 0; i < Buckets.Count; i++)
                Buckets[i].Write(bw, i);

            for (int i = 0; i < Buckets.Count; i++)
                Buckets[i].WriteFileHeaders(bw, Format, i);

            for (int i = 0; i < Buckets.Count; i++)
                for (int j = 0; j < Buckets[i].Count; j++)
                    Buckets[i][j].WriteHashAndKey(bw, Format, i, j);

            bw.FillInt32("FileSize", (int)bw.Position);
        }

        /// <summary>
        /// Indicates the format of a dvdbnd.
        /// </summary>
        public enum Game
        {
            /// <summary>
            /// Dark Souls 1, both PC and console versions.
            /// </summary>
            DarkSouls1,

            /// <summary>
            /// Dark Souls 2 and Scholar of the First Sin on PC.
            /// </summary>
            DarkSouls2,

            /// <summary>
            /// Dark Souls 3 on PC.
            /// </summary>
            DarkSouls3,

            /// <summary>
            /// Sekiro on PC.
            /// </summary>
            Sekiro,
        }

        /// <summary>
        /// A collection of files grouped by their hash.
        /// </summary>
        public class Bucket : List<FileHeader>
        {
            /// <summary>
            /// Creates an empty Bucket.
            /// </summary>
            public Bucket() : base() { }

            internal Bucket(BinaryReaderEx br, Game game) : base()
            {
                int fileHeaderCount = br.ReadInt32();
                int fileHeadersOffset = br.ReadInt32();
                Capacity = fileHeaderCount;

                br.StepIn(fileHeadersOffset);
                {
                    for (int i = 0; i < fileHeaderCount; i++)
                        Add(new FileHeader(br, game));
                }
                br.StepOut();
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteInt32(Count);
                bw.ReserveInt32($"FileHeadersOffset{index}");
            }

            internal void WriteFileHeaders(BinaryWriterEx bw, Game game, int index)
            {
                bw.FillInt32($"FileHeadersOffset{index}", (int)bw.Position);
                for (int i = 0; i < Count; i++)
                    this[i].Write(bw, game, index, i);
            }
        }

        /// <summary>
        /// Information about an individual file in the dvdbnd.
        /// </summary>
        public class FileHeader
        {
            /// <summary>
            /// Hash of the full file path using From's algorithm found in SFUtil.FromPathHash.
            /// </summary>
            public uint FileNameHash { get; set; }

            /// <summary>
            /// Full size of the file data in the BDT.
            /// </summary>
            public int PaddedFileSize { get; set; }

            /// <summary>
            /// File size after decryption; only included in DS3.
            /// </summary>
            public long UnpaddedFileSize { get; set; }

            /// <summary>
            /// Beginning of file data in the BDT.
            /// </summary>
            public long FileOffset { get; set; }

            /// <summary>
            /// Hashing information for this file.
            /// </summary>
            public SHAHash SHAHash { get; set; }

            /// <summary>
            /// Encryption information for this file.
            /// </summary>
            public AESKey AESKey { get; set; }

            /// <summary>
            /// Creates a FileHeader with default values.
            /// </summary>
            public FileHeader() { }

            internal FileHeader(BinaryReaderEx br, Game game)
            {
                FileNameHash = br.ReadUInt32();
                PaddedFileSize = br.ReadInt32();
                FileOffset = br.ReadInt64();

                if (game >= Game.DarkSouls2)
                {
                    long shaHashOffset = br.ReadInt64();
                    long aesKeyOffset = br.ReadInt64();

                    if (shaHashOffset != 0)
                    {
                        br.StepIn(shaHashOffset);
                        {
                            SHAHash = new SHAHash(br);
                        }
                        br.StepOut();
                    }

                    if (aesKeyOffset != 0)
                    {
                        br.StepIn(aesKeyOffset);
                        {
                            AESKey = new AESKey(br);
                        }
                        br.StepOut();
                    }
                }

                UnpaddedFileSize = -1;
                if (game >= Game.DarkSouls3)
                {
                    UnpaddedFileSize = br.ReadInt64();
                }
            }

            internal void Write(BinaryWriterEx bw, Game game, int bucketIndex, int fileIndex)
            {
                bw.WriteUInt32(FileNameHash);
                bw.WriteInt32(PaddedFileSize);
                bw.WriteInt64(FileOffset);

                if (game >= Game.DarkSouls2)
                {
                    bw.ReserveInt64($"SHAHashOffset{bucketIndex}:{fileIndex}");
                    bw.ReserveInt64($"AESKeyOffset{bucketIndex}:{fileIndex}");
                }

                if (game >= Game.DarkSouls3)
                {
                    bw.WriteInt64(UnpaddedFileSize);
                }
            }

            internal void WriteHashAndKey(BinaryWriterEx bw, Game game, int bucketIndex, int fileIndex)
            {
                if (game >= Game.DarkSouls2)
                {
                    if (SHAHash == null)
                    {
                        bw.FillInt64($"SHAHashOffset{bucketIndex}:{fileIndex}", 0);
                    }
                    else
                    {
                        bw.FillInt64($"SHAHashOffset{bucketIndex}:{fileIndex}", bw.Position);
                        SHAHash.Write(bw);
                    }

                    if (AESKey == null)
                    {
                        bw.FillInt64($"AESKeyOffset{bucketIndex}:{fileIndex}", 0);
                    }
                    else
                    {
                        bw.FillInt64($"AESKeyOffset{bucketIndex}:{fileIndex}", bw.Position);
                        AESKey.Write(bw);
                    }
                }
            }

            /// <summary>
            /// Read and decrypt (if necessary) file data from the BDT.
            /// </summary>
            public byte[] ReadFile(FileStream bdtStream)
            {
                byte[] bytes = new byte[PaddedFileSize];
                bdtStream.Position = FileOffset;
                bdtStream.Read(bytes, 0, PaddedFileSize);
                AESKey?.Decrypt(bytes);
                return bytes;
            }
        }

        /// <summary>
        /// Hash information for a file in the dvdbnd.
        /// </summary>
        public class SHAHash
        {
            /// <summary>
            /// 32-byte salted SHA hash.
            /// </summary>
            public byte[] Hash { get; set; }

            /// <summary>
            /// Hashed sections of the file.
            /// </summary>
            public List<Range> Ranges { get; set; }

            /// <summary>
            /// Creates a SHAHash with default values.
            /// </summary>
            public SHAHash()
            {
                Hash = new byte[32];
                Ranges = new List<Range>();
            }

            internal SHAHash(BinaryReaderEx br)
            {
                Hash = br.ReadBytes(32);
                int rangeCount = br.ReadInt32();
                Ranges = new List<Range>(rangeCount);
                for (int i = 0; i < rangeCount; i++)
                    Ranges.Add(new Range(br));
            }

            internal void Write(BinaryWriterEx bw)
            {
                if (Hash.Length != 32)
                    throw new InvalidDataException("SHA hash must be 32 bytes long.");

                bw.WriteBytes(Hash);
                bw.WriteInt32(Ranges.Count);
                foreach (Range range in Ranges)
                    range.Write(bw);
            }
        }

        /// <summary>
        /// Encryption information for a file in the dvdbnd.
        /// </summary>
        public class AESKey
        {
            private static AesManaged AES = new AesManaged() { Mode = CipherMode.ECB, Padding = PaddingMode.None, KeySize = 128 };

            /// <summary>
            /// 16-byte encryption key.
            /// </summary>
            public byte[] Key { get; set; }

            /// <summary>
            /// Encrypted sections of the file.
            /// </summary>
            public List<Range> Ranges { get; set; }

            /// <summary>
            /// Creates an AESKey with default values.
            /// </summary>
            public AESKey()
            {
                Key = new byte[16];
                Ranges = new List<Range>();
            }

            internal AESKey(BinaryReaderEx br)
            {
                Key = br.ReadBytes(16);
                int rangeCount = br.ReadInt32();
                Ranges = new List<Range>(rangeCount);
                for (int i = 0; i < rangeCount; i++)
                    Ranges.Add(new Range(br));
            }

            internal void Write(BinaryWriterEx bw)
            {
                if (Key.Length != 16)
                    throw new InvalidDataException("AES key must be 16 bytes long.");

                bw.WriteBytes(Key);
                bw.WriteInt32(Ranges.Count);
                foreach (Range range in Ranges)
                    range.Write(bw);
            }

            /// <summary>
            /// Decrypt file data in-place.
            /// </summary>
            public void Decrypt(byte[] bytes)
            {
                using (ICryptoTransform decryptor = AES.CreateDecryptor(Key, new byte[16]))
                {
                    foreach (Range range in Ranges.Where(r => r.StartOffset != -1 && r.EndOffset != -1 && r.StartOffset != r.EndOffset))
                    {
                        int start = (int)range.StartOffset;
                        int count = (int)(range.EndOffset - range.StartOffset);
                        decryptor.TransformBlock(bytes, start, count, bytes, start);
                    }
                }
            }
        }

        /// <summary>
        /// Indicates a hashed or encrypted section of a file.
        /// </summary>
        public struct Range
        {
            /// <summary>
            /// The beginning of the range, inclusive.
            /// </summary>
            public long StartOffset { get; set; }

            /// <summary>
            /// The end of the range, exclusive.
            /// </summary>
            public long EndOffset { get; set; }

            /// <summary>
            /// Creates a Range with the given values.
            /// </summary>
            public Range(long startOffset, long endOffset)
            {
                StartOffset = startOffset;
                EndOffset = endOffset;
            }

            internal Range(BinaryReaderEx br)
            {
                StartOffset = br.ReadInt64();
                EndOffset = br.ReadInt64();
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteInt64(StartOffset);
                bw.WriteInt64(EndOffset);
            }
        }
    }
}
