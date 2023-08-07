using System.Collections.Generic;
using System.IO;

namespace SoulsFormats.AC3
{
    /// <summary>
    /// The different versions of this iteration of BND0.
    /// </summary>
    public enum NameVersion : byte
    {
        /// <summary>
        /// Files in this BND have no name, only an ID.
        /// </summary>
        Nameless = 0,

        /// <summary>
        /// Files in this BND have names immediately after the file entries with no offset to the names.
        /// </summary>
        NamesNoOffset = 1,

        /// <summary>
        /// Files in this BND have paths immediately after the file entries with no offset to the paths. 
        /// </summary>
        Paths = 2,

        /// <summary>
        /// Files in this BND have names with an offset to them, the first being a path.
        /// </summary>
        NamesOffset = 3
    }

    /// <summary>
    /// A BND file from Armored Core Nine Breaker, Armored Core Last Raven, and Metal Wolf Chaos.
    /// </summary>
    public class BND0 : SoulsFile<BND0>
    {
        /// <summary>
        /// Files in this BND.
        /// </summary>
        public List<File> Files;

        /// <summary>
        /// Unknown; 0xD3 or 0xCA.
        /// Something similar seen in Kuon BND0 Unk04.
        /// Is likely a format indicator.
        /// </summary>
        public int Unk08;

        /// <summary>
        /// Version number identifying several ways of handling names in the archive.
        /// </summary>
        public NameVersion Version;

        /// <summary>
        /// Unknown; Was found set to 1 on files extracted from memory.
        /// Usually set to 0.
        /// </summary>
        public byte Unk1B;

        /// <summary>
        /// The root path of all files;
        /// Only present if NameVersion is NamesOffset, will be null and ignored otherwise.
        /// </summary>
        public string Path;

        /// <summary>
        /// The alignment of each file in the BND0 determining how much padding is added.
        /// </summary>
        public ushort Alignment;

        /// <summary>
        /// Creates a new blank BND0.
        /// </summary>
        public BND0(){}

        /// <summary>
        /// Creates a new empty BND0 with the specified options.
        /// </summary>
        public BND0(int unk08, NameVersion version, byte unk1B, ushort alignment = 2048, string path = null)
        {
            Files = new List<File>();
            Unk08 = unk08;
            Version = version;
            Unk1B = unk1B;
            Alignment = alignment;
            Path = path;
        }

        /// <summary>
        /// Creates a new BND0 with the specified list of files and options.
        /// </summary>
        public BND0(List<File> files, int unk08, NameVersion version, byte unk1B, ushort alignment = 2048, string path = null)
        {
            Files = files;
            Unk08 = unk08;
            Version = version;
            Unk1B = unk1B;
            Alignment = alignment;
            Path = path;
        }

        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 32)
                return false;

            string magic = br.GetASCII(0, 3); br.Position = 4;
            uint unk04 = br.ReadUInt32();
            int unk08 = br.ReadInt32();
            int fileSize = br.ReadInt32();
            int fileCount = br.ReadInt32();
            int namesOffset = br.ReadInt32();
            ushort alignment = br.ReadUInt16();
            byte version = br.ReadByte();
            byte unk1B = br.ReadByte();
            uint unk1C = br.ReadUInt32();

            if (magic != "BND"
             || unk04 != 0xFFFF
             || fileSize != br.Length
             || fileCount * 16 > fileSize
             || namesOffset > br.Length
             || alignment > br.Length
             || version > 5
             || version < 0
             || unk1C != 0
             )
                return false;
            else if (unk08 == 0xD3 || unk08 == 0xCA) return true;
            else return false;
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            br.AssertASCII("BND\0");
            br.AssertUInt32(0xFFFF);
            Unk08 = br.AssertInt32(0xD3, 0xCA);
            int fileSize = br.ReadInt32();
            int fileCount = br.ReadInt32();
            int namesOffset = br.ReadInt32();
            Alignment = br.ReadUInt16();
            Version = br.ReadEnum8<NameVersion>();
            Unk1B = br.ReadByte();

            br.AssertUInt32(0);

            if (Version == NameVersion.NamesOffset)
            {
                br.StepIn(namesOffset);
                Path = br.ReadShiftJIS();
                br.StepOut();
            }

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
            {
                Files.Add(new File(br, Version, Path));
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = false;

            bw.WriteASCII("BND\0");
            bw.WriteUInt32(0xFFFF);
            bw.WriteInt32(Unk08);
            bw.ReserveInt32("fileSize");
            bw.WriteInt32(Files.Count);
            bw.ReserveInt32("namesOffset");
            bw.WriteUInt16(Alignment);
            bw.WriteByte((byte)Version);
            bw.WriteByte(Unk1B);
            bw.WriteUInt32(0);

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Write(bw, Version);
            }

            switch (Version)
            {
                case NameVersion.Nameless:
                    bw.FillInt32("namesOffset", 0);
                    break;
                case NameVersion.NamesNoOffset:
                    bw.FillInt32("namesOffset", 0);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{Files[i].ID}", (int)bw.Position);
                        bw.WriteShiftJIS(Files[i].Name, true);
                    }
                    break;
                case NameVersion.Paths:
                    bw.FillInt32("namesOffset", 0);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{Files[i].ID}", (int)bw.Position);
                        bw.WriteShiftJIS($"K:\\{Files[i].Name}", true); // Not sure if other drive letters are used
                    }
                    break;
                case NameVersion.NamesOffset:
                    bw.FillInt32("namesOffset", (int)bw.Position);
                    bw.WriteShiftJIS(Path, true);
                    for (int i = 0; i < Files.Count; i++)
                    {
                        bw.FillInt32($"nameOffset_{Files[i].ID}", (int)bw.Position);
                        bw.WriteShiftJIS(Files[i].Name, true);
                    }
                    break;
                default:
                    throw new InvalidDataException("Name version type is invalid or not yet supported.");
            }

            for (int i = 0; i < Files.Count; i++)
            {
                bw.Pad(Alignment);
                bw.FillInt32($"fileOffset_{Files[i].ID}", (int)bw.Position);
                bw.WriteBytes(Files[i].Bytes);
            }

            bw.FillInt32($"fileSize", (int)bw.Length);
        }

        /// <summary>
        /// A file in a BND0.
        /// </summary>
        public class File
        {
            /// <summary>
            /// ID of this file.
            /// </summary>
            public int ID;

            /// <summary>
            /// Name of this file.
            /// Will be set to ID if name does not exist.
            /// </summary>
            public string Name;

            /// <summary>
            /// The raw data of this file.
            /// </summary>
            public byte[] Bytes;

            /// <summary>
            /// Creates a new blank File.
            /// </summary>
            public File(){}

            /// <summary>
            /// Creates a new file with an ID.
            /// </summary>
            public File(int id)
            {
                ID = id;
            }

            /// <summary>
            /// Creates a new file with a name.
            /// </summary>
            /// <param name="name"></param>
            public File(string name)
            {
                Name = name;
            }

            /// <summary>
            /// Creates a new file with bytes.
            /// </summary>
            public File(byte[] bytes)
            {
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a new File with an ID and bytes.
            /// </summary>
            public File(int id, byte[] bytes)
            {
                ID = id;
                Bytes = bytes;
            }

            /// <summary>
            /// Creates a new File with an id, name, and bytes.
            /// </summary>
            public File(int id, string name, byte[] bytes)
            {
                ID = id;
                Name = name;
                Bytes = bytes;
            }

            internal File(BinaryReaderEx br, NameVersion version, string path)
            {
                ID = br.ReadInt32();
                int offset = br.ReadInt32();
                int size = br.ReadInt32();
                int nameOffset = br.ReadInt32();

                string name;
                switch (version)
                {
                    case NameVersion.Nameless:
                        Name = $"{ID}";
                        break;
                    case NameVersion.NamesNoOffset:
                        Name = br.GetShiftJIS(nameOffset);
                        break;
                    case NameVersion.Paths:
                        name = br.GetShiftJIS(nameOffset);
                        name = name.Substring(2);
                        Name = name;
                        break;
                    case NameVersion.NamesOffset:
                        string truncatedPath = path.Substring(2);
                        name = truncatedPath.Substring(0, truncatedPath.Length - 1) + br.GetShiftJIS(nameOffset);
                        Name = name;
                        break;
                    default:
                        throw new InvalidDataException("Name version type is invalid or not yet supported.");
                }

                Bytes = br.GetBytes(offset, size);
            }

            /// <summary>
            /// Serializes file data to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw, NameVersion version)
            {
                bw.WriteInt32(ID);
                bw.ReserveInt32($"fileOffset_{ID}");
                bw.WriteInt32(Bytes.Length);

                if (version == NameVersion.Nameless) bw.WriteInt32(0);
                else bw.ReserveInt32($"nameOffset_{ID}");
            }
        }
    }
}
