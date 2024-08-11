using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoulsFormats.AC4
{
    /// <summary>
    /// A multi-file container format used in Armored Core 4 and Murakumo: Renegade Mech Pursuit.
    /// </summary>
    public class Zero3
    {
        /// <summary>
        /// Different versions of <see cref="Zero3"/>.
        /// </summary>
        public enum FormatVersion
        {
            /// <summary>
            /// The version used in Murakumo: Renegade Mech Pursuit.
            /// </summary>
            Murakumo,

            /// <summary>
            /// The version used in Armored Core 4.
            /// </summary>
            ArmoredCore4
        }

        /// <summary>
        /// The format version of the <see cref="Zero3"/>.
        /// </summary>
        public FormatVersion Version { get; set; }

        /// <summary>
        /// Files in this container.
        /// </summary>
        public List<File> Files { get; }

        internal Zero3(BinaryReaderEx br, List<BinaryReaderEx> containers, FormatVersion version)
        {
            Version = version;

            int fileCount = br.ReadInt32();
            if (version == FormatVersion.ArmoredCore4)
            {
                br.AssertInt32(0x10);
                br.AssertInt32(0x10);
                br.AssertInt32(0x800000);
                br.AssertPattern(0x40, 0x00);
            }
            else if (version == FormatVersion.Murakumo)
            {
                br.AssertPattern(0x3C, 0x00);
            }
            else
            {
                throw new NotSupportedException($"Unknown Format version: {version}");
            }
            
            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
                Files.Add(new File(br, containers, version));
        }

        /// <summary>
        /// Read file data from the given .000 header file and associated data files.
        /// </summary>
        /// <param name="path">The path to a <see cref="Zero3"/> container file.</param>
        /// <returns>A new <see cref="Zero3"/>.</returns>
        /// <exception cref="FileNotFoundException">The first container could not be found.</exception>
        public static Zero3 Read(string path)
        {
            // Setup containers list and check for .000 path.
            var containers = new List<BinaryReaderEx>();
            int index = 0;
            string containerPath = Path.ChangeExtension(path, index.ToString("D3"));
            if (!System.IO.File.Exists(containerPath))
            {
                throw new FileNotFoundException($"Could not find header .000 path for: {path}");
            }

            // Check first container and its version
            bool bigEndian = true;
            FormatVersion version;

            // Unwrap first container if in binder
            BinaryReaderEx br = UnwrapContainer(FormatVersion.ArmoredCore4, new BinaryReaderEx(bigEndian, System.IO.File.OpenRead(containerPath)));
            containers.Add(br);

            // Check version
            if (br.GetInt32(0x4) == 0x10 &&
                br.GetInt32(0x8) == 0x10 &&
                br.GetInt32(0xC) == 0x800000)
            {
                version = FormatVersion.ArmoredCore4;
            }
            else
            {
                br.BigEndian = bigEndian = false;
                version = FormatVersion.Murakumo;
            }

            // Grab remaining containers
            containerPath = Path.ChangeExtension(path, (++index).ToString("D3"));
            while (System.IO.File.Exists(containerPath))
            {
                br = UnwrapContainer(version, new BinaryReaderEx(bigEndian, System.IO.File.OpenRead(containerPath)));
                containers.Add(br);
                containerPath = Path.ChangeExtension(path, (++index).ToString("D3"));
            }

            // Read containers then dispose
            var result = new Zero3(containers[0], containers, version);
            foreach (BinaryReaderEx container in containers)
                container.Dispose();

            return result;
        }

        /// <summary>
        /// Read file data from the given list of byte arrays..
        /// </summary>
        /// <param name="bytesList">A list of byte arrays containing <see cref="Zero3"/> containers.</param>
        /// <returns>A new <see cref="Zero3"/>.</returns>
        /// <exception cref="ArgumentException">The list of byte arrays was empty.</exception>
        public static Zero3 Read(IList<byte[]> bytesList)
        {
            if (bytesList.Count < 1)
            {
                throw new ArgumentException("No containers to read in the list.");
            }

            // Setup containers list
            var containers = new List<BinaryReaderEx>();

            // Check first container and its version
            bool bigEndian = true;
            FormatVersion version;

            // Unwrap first container if in binder
            BinaryReaderEx br = UnwrapContainer(FormatVersion.ArmoredCore4, new BinaryReaderEx(bigEndian, bytesList[0]));
            containers.Add(br);

            // Check version
            if (br.GetInt32(0x4) == 0x10 &&
                br.GetInt32(0x8) == 0x10 &&
                br.GetInt32(0xC) == 0x800000)
            {
                version = FormatVersion.ArmoredCore4;
            }
            else
            {
                br.BigEndian = bigEndian = false;
                version = FormatVersion.Murakumo;
            }

            // Grab remaining containers
            foreach (var bytes in bytesList)
            {
                br = UnwrapContainer(version, new BinaryReaderEx(bigEndian, bytes));
                containers.Add(br);
            }

            // Read containers then dispose
            var result = new Zero3(containers[0], containers, version);
            foreach (BinaryReaderEx container in containers)
                container.Dispose();

            return result;
        }

        private List<BinaryWriterEx> WriteInternal()
        {
            bool bigEndian = Version != FormatVersion.Murakumo && (Version == FormatVersion.ArmoredCore4 ? true :
                                        throw new NotSupportedException($"Unknown version: {Version}"));

            var containers = new List<BinaryWriterEx>
            {
                new BinaryWriterEx(bigEndian)
            };

            // Write header
            var bw = containers[0];
            bw.WriteInt32(Files.Count);
            if (Version == FormatVersion.Murakumo)
            {
                bw.WritePattern(0x3C, 0x00);
            }
            else if (Version == FormatVersion.ArmoredCore4)
            {
                bw.WriteInt32(0x10);
                bw.WriteInt32(0x10);
                bw.WriteInt32(0x800000);
                bw.WritePattern(0x40, 0x00);
            }
            else
            {
                throw new NotSupportedException($"Unknown Format version: {Version}");
            }

            for (int i = 0; i < Files.Count; i++)
            {
                Files[i].Write(bw, Version, i);
            }

            if (Version == FormatVersion.Murakumo && Files.Any(x => x.ContainerIndex == 0))
            {
                bw.Pad(0x10000);
            }

            // Write files
            for (int i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                while (containers.Count < Files[i].ContainerIndex)
                {
                    containers.Add(new BinaryWriterEx(bigEndian));
                }

                bw = containers[file.ContainerIndex];
                containers[0].FillUInt32($"FileOffset_{i}", (uint)bw.Position / 0x10);
                bw.WriteBytes(file.Bytes);
                bw.Pad(0x10);
            }

            return containers;
        }

        /// <summary>
        /// Write file data to a .000 header file and associated data files.
        /// </summary>
        /// <param name="path">The path to write the header to; Used to determine other paths as well.</param>
        public void Write(string path)
        {
            List<BinaryWriterEx> containers = WriteInternal();
            for (int i = 0; i < containers.Count; i++)
            {
                string outpath = $"{path}.{string.Format("D3", i)}";
                System.IO.File.WriteAllBytes(outpath, containers[i].FinishBytes());
            }
        }

        /// <summary>
        /// Write data to each container in separate byte arrays.
        /// </summary>
        /// <returns>A list of byte arrays containing each container in order.</returns>
        public List<byte[]> Write()
        {
            List<BinaryWriterEx> containers = WriteInternal();
            List<byte[]> bytesList = new List<byte[]>(containers.Count);
            for (int i = 0; i < containers.Count; i++)
            {
                bytesList.Add(containers[i].FinishBytes());
            }
            return bytesList;
        }

        private static BinaryReaderEx UnwrapContainer(FormatVersion version, BinaryReaderEx br)
        {
            // If wrapped in a BND3
            if (version == FormatVersion.ArmoredCore4 && br.GetASCII(0, 4) == "BND3")
            {
                // Read the underlying Zero3
                var cbr = new BinaryReaderEx(br.BigEndian, BND3.ReadInternal(br).Files[0].Bytes);

                // Dispose of the BND3 reader
                br.Dispose();
                return cbr;
            }

            return br;
        }

        /// <summary>
        /// A generic file in a Zero3 container.
        /// </summary>
        public class File
        {
            /// <summary>
            /// Name of the file; maximum 0x40 characters.
            /// </summary>
            public string Name;

            /// <summary>
            /// Raw file data.
            /// </summary>
            public byte[] Bytes;

            /// <summary>
            /// The index of the container this file is in.
            /// </summary>
            public int ContainerIndex;

            internal File(BinaryReaderEx br, List<BinaryReaderEx> containers, FormatVersion version)
            {
                if (version == FormatVersion.Murakumo)
                {
                    Name = br.ReadFixStr(0x30);
                }
                else if (version == FormatVersion.ArmoredCore4)
                {
                    Name = br.ReadFixStr(0x40);
                }
                else
                {
                    throw new NotSupportedException($"Unknown Format version: {version}");
                }

                ContainerIndex = br.ReadInt32();
                uint fileOffset = br.ReadUInt32(); // Multiply by 0x10
                br.ReadInt32(); // Padded file size; multiply by 0x10
                int fileSize = br.ReadInt32();
                if (ContainerIndex > containers.Count)
                {
                    throw new IndexOutOfRangeException($"A file had an out of range container index of {ContainerIndex}: {Name}");
                }

                Bytes = containers[ContainerIndex].GetBytes(fileOffset * 0x10, fileSize);
            }

            internal void Write(BinaryWriterEx bw, FormatVersion version, int index)
            {
                if (version == FormatVersion.Murakumo)
                {
                    bw.WriteFixStr(Name, 0x30);
                }
                else if (version == FormatVersion.ArmoredCore4)
                {
                    bw.WriteFixStr(Name, 0x40);
                }

                bw.WriteInt32(ContainerIndex);
                bw.ReserveUInt32($"FileOffset_{index}");
                bw.WriteUInt32((uint)(Bytes.Length + (Bytes.Length % 0x10)) / 0x10); // Padded file size, Align to 0x10, then divide by 0x10 for the format.
                bw.WriteUInt32((uint)Bytes.Length);
            }
        }
    }
}
