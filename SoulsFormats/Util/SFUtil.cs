﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SoulsFormats
{
    /// <summary>
    /// Miscellaneous utility functions for SoulsFormats, mostly for internal use.
    /// </summary>
    public static class SFUtil
    {
        /// <summary>
        /// The prime for computing 32-bit FromSoftware path hashes.
        /// </summary>
        public const uint FROM_HASH_PRIME32 = 37u;

        /// <summary>
        /// The prime for computing 64-bit FromSoftware path hashes.
        /// </summary>
        public const ulong FROM_HASH_PRIME64 = 0x85ul;

        /// <summary>
        /// Guesses the path a file should be in based on it's extension.
        /// </summary>
        /// <returns></returns>
        public static string GuessFolder(byte[] bytes, string ext)
        {
            switch (ext)
            {
                case ".aisd": return "_unknown/aisd/";
                case ".bdt": return "bind/";
                case ".bhd": return "bind/";
                case ".dds": return "image/texture/dds/";
                case ".ffx": return "sfx/";
                case ".drb": return "lang/menu/";
                case ".edf": return "_unknown/edf/";
                case ".eld": return "_unknown/eld/";
                case ".entryfilelist": return "_unknown/entryfilelist/";
                case ".esd": return "state/";
                case ".evd": return "script/evd/";
                case ".fev": return "sound/";
                case ".flver": return "model/";
                case ".fsb": return "sound/";
                case ".gfx": return "_unknown/gfx/";
                case ".itl": return "_unknown/itl/";
                case ".lc": return "script/lua/compiled/";
                case ".lua": return "script/lua/";
                case ".msb": return "model/map/";
                case ".mtd": return "material/";
                case ".nfd": return "_unknown/nfd/";
                case ".param": return "param/";
                case ".png": return "image/";
                case ".sib": return "model/map/sib/";
                case ".tae": return "tae/";
                case ".tdf": return "_unknown/tdf/";
                case ".tpf": return "image/texture/tpf/";
                case ".blf": return "system/";
                case ".xml": return "text/xml/";
                case ".ani": return "bind/motion/ani/";
                case ".nva": return "model/map/ch_nav/";
                case ".hnav": return "model/map/ch_nav/";
                case ".htr": return "model/map/ch_nav/";
                case ".fmg": return "lang/";
                case ".luainfo": return "script/lua/info/";
                case ".anc": return "bind/motion/anc/";
                case ".ane": return "bind/motion/ane/";
                case ".mqb": return "movie/mqb/";
                case ".pam": return "movie/";
                case ".fxr": return "sfx/fxr/";
                case ".clm": return "_unknown/clm/";
                case ".acb": return "_unknown/acb/";
                case ".properties": return "system/";
                case ".ini": return "system/";
                case ".pem": return "system/";
                case ".txt": return "system/";
            }

            if (ext == ".bnd")
            {
                IBinder bnd = ReadBinder(bytes) ?? throw new InvalidDataException("Extension was guessed to be BND, but it failed to be read as a BND.");
                string folder = GuessFolderBinder(bnd);
                return folder;
            }

            if (ext.EndsWith(".dcx"))
            {
                byte[] decompressedBytes = DCX.Decompress(bytes);
                string bindExt = GuessExtension(decompressedBytes);
                string folder = GuessFolder(decompressedBytes, bindExt);
                return $"{folder}/dcx/";
            }

            return "_unknown/";
        }

        private static string GuessFolderBinder(IBinder bnd)
        {
            if (bnd.Files.Count == 0)
            {
                return "bind/empty/";
            }

            string nowload = bnd.Files[0].Name.Substring(bnd.Files[0].Name.LastIndexOf("\\") + 1);

            string folder;
            if (bnd.Files.NamesContains("event\\mission\\", "model\\cam\\cam_a.bnd", "mission\\battlefeild", "model\\map\\ch_nav\\battlefeild_", "mission\\mission_"))
                folder = "mission/";
            else if (bnd.Files.NamesStartsWiths("a0") && bnd.Files.NamesEndsWiths(".ani"))
                folder = "model/motion/";
            else if (bnd.Files.NamesStartsWiths("b") && bnd.Files.NamesEndsWiths(".flv", ".flver", ".tpf"))
                folder = "model/break/";
            else if (bnd.Files.NamesStartsWiths("e") && bnd.Files.NamesEndsWiths(".flv", ".flver", ".tpf"))
                folder = "model/ene/";
            else if (nowload.Length == 6 && nowload.EndsWith(".tpf"))
                folder = "lang/nowload/";
            else
            {
                string bindExt = GuessExtension(bnd.Files[0].Bytes);
                folder = GuessFolder(bnd.Files[0].Bytes, bindExt);
            }
            return $"bind/{folder}";
        }

        private static bool NamesContains(this List<BinderFile> files, params string[] strs)
        {
            foreach (BinderFile file in files)
                foreach (string str in strs)
                    if (file.Name.Contains(str))
                        return true;
            return false;
        }

        private static bool NamesStartsWiths(this List<BinderFile> files, params string[] strs)
        {
            foreach (BinderFile file in files)
                foreach (string str in strs)
                    if (file.Name.StartsWith(str))
                        return true;
            return false;
        }

        private static bool NamesEndsWiths(this List<BinderFile> files, params string[] strs)
        {
            foreach (BinderFile file in files)
                foreach (string str in strs)
                    if (file.Name.EndsWith(str))
                        return true;
            return false;
        }

        private static IBinder ReadBinder(byte[] bytes)
        {
            if (BND3.IsRead(bytes, out BND3 bnd3)) return bnd3;
            else if (BND4.IsRead(bytes, out BND4 bnd4)) return bnd4;
            else return null;
        }

        /// <summary>
        /// Guesses the extension of a file based on its contents.
        /// </summary>
        public static string GuessExtension(byte[] bytes, bool bigEndian = false)
        {
            bool dcx = false;
            if (DCX.Is(bytes))
            {
                dcx = true;
                bytes = DCX.Decompress(bytes);
            }

            bool checkMsb(BinaryReaderEx br)
            {
                if (br.Length < 8)
                    return false;

                int offset = br.GetInt32(4);
                if (offset > br.Length || offset < 0)
                {
                    byte[] offsetbytes = BitConverter.GetBytes(offset);
                    Array.Reverse(offsetbytes);
                    offset = BitConverter.ToInt32(offsetbytes, 0);
                }

                if (offset < 0 || offset >= br.Length - 1)
                    return false;

                try
                {
                    return br.GetASCII(offset) == "MODEL_PARAM_ST";
                }
                catch
                {
                    return false;
                }
            }

            bool checkParam(BinaryReaderEx br)
            {
                if (br.Length < 0x2C)
                    return false;

                string param = br.GetASCII(0xC, 0x20);
                return Regex.IsMatch(param, "^[^\0]+\0 *$");
            }

            bool checkTdf(BinaryReaderEx br)
            {
                if (br.Length < 4)
                    return false;

                if (br.GetASCII(0, 1) != "\"")
                    return false;

                for (int i = 1; i < br.Length; i++)
                {
                    if (br.GetASCII(i, 1) == "\"")
                    {
                        return i < br.Length - 2 && br.GetASCII(i + 1, 2) == "\r\n";
                    }
                }
                return false;
            }

            string ext = "";
            using (var ms = new MemoryStream(bytes))
            {
                var br = new BinaryReaderEx(bigEndian, ms);
                string magic = null;
                if (br.Length >= 4)
                    magic = br.ReadASCII(4);

                if (magic == "AISD")
                    ext = ".aisd";
                else if (magic == "BDF3" || magic == "BDF4")
                    ext = ".bdt";
                else if (magic == "BHF3" || magic == "BHF4")
                    ext = ".bhd";
                else if (magic == "BND3" || magic == "BND4")
                    ext = ".bnd";
                else if (magic == "DDS ")
                    ext = ".dds";
                // ESD or FFX
                else if (magic != null && magic.ToUpper() == "DLSE")
                    ext = ".ffx";
                else if (magic == "\0BRD" || magic == "DRB\0")
                    ext = ".drb";
                else if (magic == "EDF\0")
                    ext = ".edf";
                else if (magic == "ELD\0")
                    ext = ".eld";
                else if (magic == "ENFL")
                    ext = ".entryfilelist";
                else if (magic != null && magic.ToUpper() == "FSSL")
                    ext = ".esd";
                else if (magic == "EVD\0")
                    ext = ".evd";
                else if (br.Length >= 3 && br.GetASCII(0, 3) == "FEV" || br.Length >= 0x10 && br.GetASCII(8, 8) == "FEV FMT ")
                    ext = ".fev";
                else if (br.Length >= 6 && br.GetASCII(0, 6) == "FLVER\0")
                    ext = ".flver";
                else if (br.Length >= 3 && br.GetASCII(0, 3) == "FSB")
                    ext = ".fsb";
                else if (br.Length >= 3 && br.GetASCII(0, 3) == "GFX")
                    ext = ".gfx";
                else if (br.Length >= 0x19 && br.GetASCII(0xC, 0xE) == "ITLIMITER_INFO")
                    ext = ".itl";
                else if (br.Length >= 5 && br.GetASCII(1, 4) == "LuaP")
                    ext = ".lc";
                else if (br.Length >= 4 && br.GetASCII(1, 3) == "Lua")
                    ext = ".lua";
                else if (checkMsb(br))
                    ext = ".msb";
                else if (br.Length >= 0x30 && br.GetASCII(0x2C, 4) == "MTD ")
                    ext = ".mtd";
                else if (magic == "DFPN")
                    ext = ".nfd";
                else if (checkParam(br))
                    ext = ".param";
                else if (br.Length >= 4 && br.GetASCII(1, 3) == "PNG")
                    ext = ".png";
                else if (br.Length >= 0x2C && br.GetASCII(0x28, 4) == "SIB ")
                    ext = ".sib";
                else if (magic == "TAE ")
                    ext = ".tae";
                else if (checkTdf(br))
                    ext = ".tdf";
                else if (magic == "TPF\0")
                    ext = ".tpf";
                else if (magic == "#BOM" || magic == "TEXT")
                    ext = ".txt";
                else if (magic == "BLF\0")
                    ext = ".blf";
                else if (br.Length >= 5 && br.GetASCII(0, 5) == "<?xml")
                    ext = ".xml";
                else if (magic == "NVMA")
                    ext = ".nva";
                else if (magic == "HNAV")
                    ext = ".hnav";
                else if (magic == "HTR\0")
                    ext = ".ht";
                else if (magic == "LUAI")
                    ext = ".luainfo";
                else if (br.Length > 4 && br.Length >= br.GetInt32(0) + 8 && br.GetInt32(0) > 0 && br.GetASCII(br.GetInt32(0), 8) == "#ANIEDIT")
                    ext = ".anc";
                else if (magic == "ANE\0")
                    ext = ".ane";
                else if (magic == "MQB ")
                    ext = ".mqb";
                else if (br.Length >= 4 && br.GetASCII(0, 3) == "PAM")
                    ext = ".pam";
                else if (magic == "FXR\0")
                    ext = ".fxr";
                else if (magic == "CLM2")
                    ext = ".clm";
                else if (magic == "ACB\0")
                    ext = ".acb";
                // This is pretty sketchy
                else if (br.Length >= 0xC && br.GetByte(0) == 0 && br.GetByte(3) == 0 && br.GetInt32(4) == br.Length && br.GetInt16(0xA) == 0 || FMG.TryRead(bytes, out _))
                    ext = ".fmg";
            }

            if (dcx)
                return ext + ".dcx";
            else
                return ext;
        }

        /// <summary>
        /// Reverses the order of bits in a byte, probably very inefficiently.
        /// </summary>
        public static byte ReverseBits(byte value)
        {
            return (byte)(
                ((value & 0b00000001) << 7) |
                ((value & 0b00000010) << 5) |
                ((value & 0b00000100) << 3) |
                ((value & 0b00001000) << 1) |
                ((value & 0b00010000) >> 1) |
                ((value & 0b00100000) >> 3) |
                ((value & 0b01000000) >> 5) |
                ((value & 0b10000000) >> 7)
                );
        }

        /// <summary>
        /// Makes a backup of a file if not already found, and returns the backed-up path.
        /// </summary>
        public static string Backup(string file, bool overwrite = false)
        {
            string bak = file + ".bak";
            if (overwrite || !File.Exists(bak))
                File.Copy(file, bak, overwrite);
            return bak;
        }

        /// <summary>
        /// Returns the extension of the specified file path, removing .dcx if present.
        /// </summary>
        public static string GetRealExtension(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension == ".dcx")
                extension = Path.GetExtension(Path.GetFileNameWithoutExtension(path));
            return extension;
        }

        /// <summary>
        /// Returns the file name of the specified path, removing both .dcx if present and the actual extension.
        /// </summary>
        public static string GetRealFileName(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (Path.GetExtension(path) == ".dcx")
                name = Path.GetFileNameWithoutExtension(name);
            return name;
        }

        /// <summary>
        /// Decompresses data and returns a new <see cref="BinaryReaderEx"/> if necessary.
        /// </summary>
        public static BinaryReaderEx GetDecompressedBinaryReader(BinaryReaderEx br, out DCX.Type compression)
        {
            if (DCX.Is(br))
            {
                byte[] bytes = DCX.Decompress(br, out compression);
                return new BinaryReaderEx(false, bytes);
            }
            else
            {
                compression = DCX.Type.None;
                return br;
            }
        }

        /// <summary>
        /// Computes a 32-bit hash for paths used in FromSoftware games.
        /// </summary>
        public static uint ComputeFromPathHash32(string text)
        {
            string hashable = text.Trim().ToLowerInvariant().Replace('\\', '/');
            if (!hashable.StartsWith("/"))
            {
                hashable = '/' + hashable;
            }

            return hashable.Aggregate(0u, (i, c) => i * FROM_HASH_PRIME32 + c);
        }

        /// <summary>
        /// Computes a 64-bit hash for paths used in FromSoftware games.
        /// </summary>
        public static ulong ComputeFromPathHash64(string text)
        {
            string hashable = text.Trim().ToLowerInvariant().Replace('\\', '/');
            if (!hashable.StartsWith("/"))
            {
                hashable = '/' + hashable;
            }

            return hashable.Aggregate(0ul, (i, c) => i * FROM_HASH_PRIME64 + c);
        }

        /// <summary>
        /// Computes a 32-bit or 64-bit hash for paths used in FromSoftware games.
        /// </summary>
        public static ulong ComputeFromPathHash(string text, bool bit64 = false)
        {
            string hashable = text.Trim().ToLowerInvariant().Replace('\\', '/');
            if (!hashable.StartsWith("/"))
            {
                hashable = '/' + hashable;
            }

            return bit64 ? hashable.Aggregate(0ul, (i, c) => i * FROM_HASH_PRIME64 + c) : hashable.Aggregate(0u, (i, c) => i * FROM_HASH_PRIME32 + c);
        }

        /// <summary>
        /// Determines whether a number is prime or not.
        /// </summary>
        public static bool IsPrime(uint candidate)
        {
            if (candidate < 2)
                return false;
            if (candidate == 2)
                return true;
            if (candidate % 2 == 0)
                return false;

            for (int i = 3; i * i <= candidate; i += 2)
            {
                if (candidate % i == 0)
                    return false;
            }

            return true;
        }

        private static readonly Regex timestampRx = new Regex(@"(\d\d)(\w)(\d+)(\w)(\d+)");

        /// <summary>
        /// Converts a BND/BXF timestamp string to a DateTime object.
        /// </summary>
        public static DateTime BinderTimestampToDate(string timestamp)
        {
            Match match = timestampRx.Match(timestamp);
            if (!match.Success)
                throw new InvalidDataException("Unrecognized timestamp format.");

            int year = Int32.Parse(match.Groups[1].Value) + 2000;
            int month = match.Groups[2].Value[0] - 'A';
            int day = Int32.Parse(match.Groups[3].Value);
            int hour = match.Groups[4].Value[0] - 'A';
            int minute = Int32.Parse(match.Groups[5].Value);

            return new DateTime(year, month, day, hour, minute, 0);
        }

        /// <summary>
        /// Converts a DateTime object to a BND/BXF timestamp string.
        /// </summary>
        public static string DateToBinderTimestamp(DateTime dateTime)
        {
            int year = dateTime.Year - 2000;
            if (year < 0 || year > 99)
                throw new InvalidDataException("BND timestamp year must be between 2000 and 2099 inclusive.");

            char month = (char)(dateTime.Month + 'A');
            int day = dateTime.Day;
            char hour = (char)(dateTime.Hour + 'A');
            int minute = dateTime.Minute;

            return $"{year:D2}{month}{day}{hour}{minute}".PadRight(8, '\0');
        }

        /// <summary>
        /// Compresses data and writes it to a BinaryWriterEx with Zlib wrapper.
        /// </summary>
        public static int WriteZlib(BinaryWriterEx bw, byte formatByte, byte[] input)
        {
            long start = bw.Position;
            bw.WriteByte(0x78);
            bw.WriteByte(formatByte);

            var deflateStream = new DeflateStream(bw.Stream, CompressionMode.Compress, true);
            deflateStream.Write(input, 0, input.Length);
            deflateStream.Dispose();

            uint adler = Adler32(input);
            if (!bw.BigEndian)
            {
                adler = EndianHelper.ReverseEndianness(adler);
            }

            bw.WriteUInt32(adler);
            return (int)(bw.Position - start);
        }

        /// <summary>
        /// Reads a Zlib block from a BinaryReaderEx and returns the uncompressed data.
        /// </summary>
        public static byte[] ReadZlib(BinaryReaderEx br, int compressedSize)
        {
            br.AssertByte(0x78);
            br.AssertByte(0x01, 0x5E, 0x9C, 0xDA);
            return DecompressZlibBytes(br.ReadBytes(compressedSize - 2));
        }

        /// <summary>
        /// Decompresses zlib starting at the current position in a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <param name="compressedSize">The size of the compressed data including the 2 byte zlib header.</param>
        /// <returns>Decompressed zlib data.</returns>
        /// <exception cref="EndOfStreamException">Cannot read beyond the end of the stream.</exception>
        /// <exception cref="InvalidDataException">A valid zlib header could not be detected.</exception>
        /// <exception cref="Exception">Did not read the expected number of compressed bytes from the <see cref="Stream"/>.</exception>
        public static byte[] DecompressZlib(Stream stream, int compressedSize)
        {
            var cmf = stream.ReadByte();
            var flg = stream.ReadByte();

            if (cmf == -1 || flg == -1)
            {
                throw new EndOfStreamException("Cannot read beyond the end of the stream.");
            }

            if (cmf != 0x78)
            {
                throw new InvalidDataException("Zlib header could not be detected.");
            }

            if (flg != 0x01 && flg != 0x5E && flg != 0x9C && flg != 0xDA)
            {
                throw new InvalidDataException("Valid zlib compression level could not be detected.");
            }

            byte[] bytes = new byte[compressedSize - 2];
            if (stream.Read(bytes, 0, bytes.Length) < bytes.Length)
            {
                throw new Exception("Could not read the expected number of bytes.");
            }
            return DecompressZlibBytes(bytes);
        }

        /// <summary>
        /// Decompresses zlib bytes coming after a zlib header.
        /// </summary>
        /// <param name="compressedBytes">Compressed bytes not including the zlib header.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] DecompressZlibBytes(byte[] compressedBytes)
        {
            using var decompressedStream = new MemoryStream();
            using var compressedStream = new MemoryStream(compressedBytes);
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress, true);
            deflateStream.CopyTo(decompressedStream);
            return decompressedStream.ToArray();
        }

        /// <summary>
        /// Computes an Adler32 checksum used by Zlib.
        /// </summary>
        public static uint Adler32(byte[] data)
        {
            uint s1 = 1;
            uint s2 = 0;

            foreach (byte b in data)
            {
                s1 = (s1 + b) % 65521;
                s2 = (s2 + s1) % 65521;
            }

            return (s2 << 16) | s1;
        }

        /// <summary>
        /// Concatenates multiple collections into one list.
        /// </summary>
        public static List<T> ConcatAll<T>(params IEnumerable<T>[] lists)
        {
            IEnumerable<T> all = new List<T>();
            foreach (IEnumerable<T> list in lists)
                all = all.Concat(list);
            return all.ToList();
        }

        /// <summary>
        /// Convert a list to a dictionary with indices as keys.
        /// </summary>
        public static Dictionary<int, T> Dictionize<T>(List<T> items)
        {
            var dict = new Dictionary<int, T>(items.Count);
            for (int i = 0; i < items.Count; i++)
                dict[i] = items[i];
            return dict;
        }

        /// <summary>
        /// Converts a hex string in format "AA BB CC DD" to a byte array.
        /// </summary>
        public static byte[] ParseHexString(string str)
        {
            string[] strings = str.Split(' ');
            byte[] bytes = new byte[strings.Length];
            for (int i = 0; i < strings.Length; i++)
                bytes[i] = Convert.ToByte(strings[i], 16);
            return bytes;
        }

        /// <summary>
        /// Returns a copy of the key used for encrypting original DS2 save files on PC.
        /// </summary>
        public static byte[] GetDS2SaveKey()
        {
            return (byte[])ds2SaveKey.Clone();
        }
        private static byte[] ds2SaveKey = ParseHexString("B7 FD 46 3E 4A 9C 11 02 DF 17 39 E5 F3 B2 A5 0F");

        /// <summary>
        /// Returns a copy of the key used for encrypting DS2 SotFS save files on PC.
        /// </summary>
        public static byte[] GetScholarSaveKey()
        {
            return (byte[])scholarSaveKey.Clone();
        }
        private static byte[] scholarSaveKey = ParseHexString("59 9F 9B 69 96 40 A5 52 36 EE 2D 70 83 5E C7 44");

        /// <summary>
        /// Returns a copy of the key used for encrypting DS3 save files on PC.
        /// </summary>
        public static byte[] GetDS3SaveKey()
        {
            return (byte[])ds3SaveKey.Clone();
        }
        private static byte[] ds3SaveKey = ParseHexString("FD 46 4D 69 5E 69 A3 9A 10 E3 19 A7 AC E8 B7 FA");

        /// <summary>
        /// Decrypts a file from a DS2/DS3 SL2. Do not remove the hash and IV before calling.
        /// </summary>
        public static byte[] DecryptSL2File(byte[] encrypted, byte[] key)
        {
            // Just leaving this here for documentation
            //byte[] hash = new byte[16];
            //Buffer.BlockCopy(encrypted, 0, hash, 0, 16);

            byte[] iv = new byte[16];
            Buffer.BlockCopy(encrypted, 16, iv, 0, 16);

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                // PKCS7-style padding is used, but they don't include the minimum padding
                // so it can't be stripped safely
                aes.Padding = PaddingMode.None;
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (var encStream = new MemoryStream(encrypted, 32, encrypted.Length - 32))
                using (var cryptoStream = new CryptoStream(encStream, decryptor, CryptoStreamMode.Read))
                using (var decStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(decStream);
                    return decStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Encrypts a file for a DS2/DS3 SL2. Result includes the hash and IV.
        /// </summary>
        public static byte[] EncryptSL2File(byte[] decrypted, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.None;
                aes.Key = key;
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor();
                using (var decStream = new MemoryStream(decrypted))
                using (var cryptoStream = new CryptoStream(decStream, encryptor, CryptoStreamMode.Read))
                using (var encStream = new MemoryStream())
                using (var md5 = MD5.Create())
                {
                    encStream.Write(aes.IV, 0, 16);
                    cryptoStream.CopyTo(encStream);
                    byte[] encrypted = new byte[encStream.Length + 16];
                    encStream.Position = 0;
                    encStream.Read(encrypted, 16, (int)encStream.Length);
                    byte[] hash = md5.ComputeHash(encrypted, 16, encrypted.Length - 16);
                    Buffer.BlockCopy(hash, 0, encrypted, 0, 16);
                    return encrypted;
                }
            }
        }

        private static byte[] ds3RegulationKey = SFEncoding.ASCII.GetBytes("ds3#jn/8_7(rsY9pg55GFN7VFL#+3n/)");

        /// <summary>
        /// Decrypts and unpacks DS3's regulation BND4 from the specified path.
        /// </summary>
        public static BND4 DecryptDS3Regulation(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            bytes = DecryptByteArray(ds3RegulationKey, bytes);
            return BND4.Read(bytes);
        }

        /// <summary>
        /// Repacks and encrypts DS3's regulation BND4 to the specified path.
        /// </summary>
        public static void EncryptDS3Regulation(string path, BND4 bnd)
        {
            byte[] bytes = bnd.Write();
            bytes = EncryptByteArray(ds3RegulationKey, bytes);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, bytes);
        }

        private static byte[] EncryptByteArray(byte[] key, byte[] secret)
        {
            using (MemoryStream ms = new MemoryStream())
            using (AesManaged cryptor = new AesManaged())
            {
                cryptor.Mode = CipherMode.CBC;
                cryptor.Padding = PaddingMode.PKCS7;
                cryptor.KeySize = 256;
                cryptor.BlockSize = 128;

                byte[] iv = cryptor.IV;

                using (CryptoStream cs = new CryptoStream(ms, cryptor.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(secret, 0, secret.Length);
                }
                byte[] encryptedContent = ms.ToArray();

                byte[] result = new byte[iv.Length + encryptedContent.Length];

                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

                return result;
            }
        }

        private static byte[] DecryptByteArray(byte[] key, byte[] secret)
        {
            byte[] iv = new byte[16];
            byte[] encryptedContent = new byte[secret.Length - 16];

            Buffer.BlockCopy(secret, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(secret, iv.Length, encryptedContent, 0, encryptedContent.Length);

            using (MemoryStream ms = new MemoryStream())
            using (AesManaged cryptor = new AesManaged())
            {
                cryptor.Mode = CipherMode.CBC;
                cryptor.Padding = PaddingMode.None;
                cryptor.KeySize = 256;
                cryptor.BlockSize = 128;

                using (CryptoStream cs = new CryptoStream(ms, cryptor.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedContent, 0, encryptedContent.Length);
                }
                return ms.ToArray();
            }
        }

        internal static byte[] To4Bit(byte value)
        {
            byte[] values = new byte[2];
            values[0] = (byte)((byte)(value & 0b1111_0000) >> 4);
            values[1] = (byte)(value & 0b0000_1111);
            return values;
        }
    }
}
