using System;
using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// A Binder2 archive.
    /// <para>Appears in Metal Wolf Chaos and the following Armored Core Games: Formula Front PSP, Nine Breaker, Last Raven.</para>
    /// <para>Build settings for a file of this format were found in Armored Core Formula Front PSP which mentioned an app named Binder2.</para>
    /// </summary>
    public class BND2 : SoulsFile<BND2>
    {
        /// <summary>
        /// An enum for the different supported file path modes.
        /// </summary>
        public enum FilePathModeEnum : byte
        {
            /// <summary>
            /// Files in this BND have no name, only an ID.
            /// </summary>
            Nameless = 0,

            /// <summary>
            /// Files in this BND have names immediately after the file entries with no offset to the names array.
            /// </summary>
            NamesNoOffset = 1,

            /// <summary>
            /// Files in this BND have paths immediately after the file entries with no offset to the paths array. 
            /// </summary>
            Paths = 2,

            /// <summary>
            /// Files in this BND have names with an offset to the names array, the first being a base directory.
            /// </summary>
            NamesOffset = 3
        }

        /// <summary>
        /// The version of this <see cref="BND2"/>.
        /// <para>Only 202 and 211 have been seen.</para>
        /// </summary>
        public int FileVersion { get; set; }

        /// <summary>
        /// The alignment of each <see cref="File"/>.
        /// <para>The bigger the aligment, the more empty bytes are added as padding. This increases the size of the archive.</para>
        /// </summary>
        public ushort AlignmentSize { get; set; }

        /// <summary>
        /// The file path mode determining how paths are handled.
        /// </summary>
        public FilePathModeEnum FilePathMode { get; set; }

        /// <summary>
        /// Unknown; Was found set to 1 on files extracted from memory.
        /// Usually set to 0.
        /// </summary>
        public byte Unk1B { get; set; }

        /// <summary>
        /// The base directory of all files.
        /// <para>Only used when <see cref="FilePathModeEnum.NamesOffset"/> is set on <see cref="FilePathMode"/>.</para>
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// The files in this <see cref="BND2"/>.
        /// </summary>
        public List<File> Files { get; set; }

        /// <summary>
        /// Creates a <see cref="BND2"/>.
        /// </summary>
        public BND2()
        {
            FileVersion = 211;
            AlignmentSize = 2048;
            FilePathMode = FilePathModeEnum.NamesNoOffset;
            Unk1B = 0;
            BaseDirectory = string.Empty;
            Files = new List<File>();
        }

        /// <summary>
        /// Creates a <see cref="BND2"/> with the specified version.
        /// </summary>
        public BND2(int version)
        {
            FileVersion = version;
            AlignmentSize = 2048;
            FilePathMode = FilePathModeEnum.NamesNoOffset;
            Unk1B = 0;
            BaseDirectory = string.Empty;
            Files = new List<File>();
        }

        /// <summary>
        /// Creates a <see cref="BND2"/> with the specified <see cref="FilePathMode"/>.
        /// </summary>
        public BND2(FilePathModeEnum filePathMode)
        {
            FileVersion = 211;
            AlignmentSize = 2048;
            FilePathMode = filePathMode;
            Unk1B = 0;
            BaseDirectory = string.Empty;
            Files = new List<File>();
        }

        /// <summary>
        /// Creates a <see cref="BND2"/> with the specified version and <see cref="FilePathMode"/>.
        /// </summary>
        public BND2(int version, FilePathModeEnum filePathMode)
        {
            FileVersion = version;
            AlignmentSize = 2048;
            FilePathMode = filePathMode;
            Unk1B = 0;
            BaseDirectory = string.Empty;
            Files = new List<File>();
        }

        /// <summary>
        /// Returns true if the data appears to be a <see cref="BND2"/>.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 32)
                return false;

            string magic = br.ReadASCII(4);
            uint unk04 = br.ReadUInt32();
            int fileVersion = br.ReadInt32();
            br.Position += 8; // File Size, File Num
            int namesOffset = br.ReadInt32();
            ushort alignmentSize = br.ReadUInt16();
            byte filePathMode = br.ReadByte();
            byte unk1B = br.ReadByte();
            uint unk1C = br.ReadUInt32();

            // All file path modes except for one have namesOffset set to 0.
            bool validNamesOffset;
            switch (filePathMode)
            {
                case 0:
                case 1:
                case 2:
                    validNamesOffset = namesOffset <= br.Length && namesOffset == 0;
                    break;
                case 3:
                    validNamesOffset = namesOffset <= br.Length;
                    break;
                default:
                    // File path mode was invalid
                    return false;
            }

            bool validMagic = magic == "BND\0";
            bool expectedUnk04 = unk04 == 0xFFFF;
            bool expectedAligmentSize = alignmentSize % 2 == 0;
            bool expectedFileVersion = fileVersion >= 202 && fileVersion <= 211;
            bool expectedUnk1B = unk1B == 0 || unk1B == 1;
            bool expectedUnk1C = unk1C == 0;
            return validMagic && expectedUnk04 && expectedFileVersion && validNamesOffset && expectedAligmentSize && expectedUnk1B && expectedUnk1C;
        }

        /// <summary>
        /// Reads a <see cref="BND2"/> from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            br.AssertASCII("BND\0");
            br.AssertUInt32(0xFFFF);
            FileVersion = br.AssertInt32(202, 211); // Versions between 202 and 211 not seen.
            br.Position += 4; // File Size
            int fileCount = br.ReadInt32();
            int namesOffset = br.ReadInt32();
            AlignmentSize = br.ReadUInt16();
            FilePathMode = br.ReadEnum8<FilePathModeEnum>();
            Unk1B = br.AssertByte(0,1);
            br.AssertUInt32(0);

            if (FilePathMode == FilePathModeEnum.NamesOffset)
            {
                br.StepIn(namesOffset);
                BaseDirectory = br.ReadShiftJIS();
                br.StepOut();
            }

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(new File(br, FilePathMode));
            }
        }

        /// <summary>
        /// Writes this <see cref="BND2"/> to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;

            bw.WriteASCII("BND\0");
            bw.WriteUInt32(0xFFFF);
            bw.WriteInt32(FileVersion);
            bw.ReserveInt32("fileSize");
            bw.WriteInt32(Files.Count);
            bw.ReserveInt32("namesOffset");
            bw.WriteUInt16(AlignmentSize);
            bw.WriteByte((byte)FilePathMode);
            bw.WriteByte(Unk1B);
            bw.WriteUInt32(0);

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Write(bw, FilePathMode, i);
            }

            switch (FilePathMode)
            {
                case FilePathModeEnum.Nameless:
                    bw.FillInt32("namesOffset", 0);
                    break;
                case FilePathModeEnum.NamesNoOffset:
                    bw.FillInt32("namesOffset", 0);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{i}", (int)bw.Position);
                        bw.WriteShiftJIS(Files[i].Name, true);
                    }
                    break;
                case FilePathModeEnum.Paths:
                    bw.FillInt32("namesOffset", 0);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{i}", (int)bw.Position);

                        string name = Files[i].Name;
                        if (!Path.IsPathRooted(name))
                        {
                            name = Path.Combine("K:\\", name);
                        }
                        bw.WriteShiftJIS(name, true);
                    }
                    break;
                case FilePathModeEnum.NamesOffset:
                    bw.FillInt32("namesOffset", (int)bw.Position);
                    bw.WriteShiftJIS(BaseDirectory, true);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{i}", (int)bw.Position);
                        bw.WriteShiftJIS(Files[i].Name, true);
                    }
                    break;
                default:
                    throw new NotSupportedException($"{nameof(FilePathMode)} {FilePathMode} is not supported.");
            }

            for (int i = 0; i < Files.Count; i++)
            {
                bw.Pad(AlignmentSize);
                bw.FillInt32($"fileOffset_{i}", (int)bw.Position);
                bw.WriteBytes(Files[i].Bytes);
            }

            bw.FillInt32($"fileSize", (int)bw.Length);
        }

        /// <summary>
        /// A <see cref="File"/> in a <see cref="BND2"/>.
        /// </summary>
        public class File
        {
            /// <summary>
            /// The ID of this <see cref="File"/>.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// The name of this <see cref="File"/>.
            /// <para>Will be set to ID if name does not exist.</para>
            /// <para>Will be a path with a drive letter if <see cref="FilePathModeEnum.Paths"/> is set.</para>
            /// <para>Will need <see cref="BaseDirectory"/> added as the base directory if <see cref="FilePathModeEnum.NamesOffset"/> is set.</para>
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The raw data of this <see cref="File"/>.
            /// </summary>
            public byte[] Bytes { get; set; }

            /// <summary>
            /// Creates a <see cref="File"/>.
            /// </summary>
            public File()
            {
                ID = -1;
                Name = string.Empty;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID.
            /// </summary>
            public File(int id)
            {
                ID = id;
                Name = string.Empty;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with a name.
            /// </summary>
            public File(string name)
            {
                ID = -1;
                Name = name;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with bytes.
            /// </summary>
            public File(byte[] bytes)
            {
                ID = -1;
                Name = string.Empty;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID and name.
            /// </summary>
            public File(int id, string name)
            {
                ID = id;
                Name = name;
                Bytes = Array.Empty<byte>();
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an ID and bytes.
            /// </summary>
            public File(int id, byte[] bytes)
            {
                ID = id;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with a name and bytes.
            /// </summary>
            public File(string name, byte[] bytes)
            {
                ID = -1;
                Name = name;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a <see cref="File"/> with an id, name, and bytes.
            /// </summary>
            public File(int id, string name, byte[] bytes)
            {
                ID = id;
                Name = name;
                Bytes = bytes;
            }

            /// <summary>
            /// Reads a <see cref="File"/> from a stream.
            /// </summary>
            internal File(BinaryReaderEx br, FilePathModeEnum filePathMode)
            {
                ID = br.ReadInt32();
                int offset = br.ReadInt32();
                int size = br.ReadInt32();
                int nameOffset = br.ReadInt32();

                switch (filePathMode)
                {
                    case FilePathModeEnum.Nameless:
                        Name = ID.ToString();
                        break;
                    case FilePathModeEnum.NamesNoOffset:
                    case FilePathModeEnum.Paths:
                    case FilePathModeEnum.NamesOffset:
                        Name = br.GetShiftJIS(nameOffset);
                        break;
                    default:
                        throw new NotSupportedException($"{nameof(filePathMode)} {filePathMode} is not supported.");
                }

                Bytes = br.GetBytes(offset, size);
            }

            /// <summary>
            /// Writes this <see cref="File"/> entry to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw, FilePathModeEnum filePathMode, int index)
            {
                bw.WriteInt32(ID);
                bw.ReserveInt32($"fileOffset_{index}");
                bw.WriteInt32(Bytes.Length);

                if (filePathMode == FilePathModeEnum.Nameless)
                {
                    bw.WriteInt32(0);
                }
                else
                {
                    bw.ReserveInt32($"nameOffset_{index}");
                }
            }
        }
    }
}
