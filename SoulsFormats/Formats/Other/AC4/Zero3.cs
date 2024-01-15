using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SoulsFormats.AC4
{
    /// <summary>
    /// A multi-file container format used in Armored Core 4.
    /// </summary>
    public class Zero3
    {
        /// <summary>
        /// Files in this container.
        /// </summary>
        public List<File> Files { get; }

        /// <summary>
        /// Returns whether the file appears to be a .000 header.
        /// </summary>
        public static bool Is(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                var br = new BinaryReaderEx(true, fs);
                return Is(br);
            }
        }

        /// <summary>
        /// Returns whether the file appears to be a .000 header.
        /// </summary>
        public static bool Is(byte[] bytes)
        {
            var br = new BinaryReaderEx(true, bytes);
            return Is(br);
        }

        /// <summary>
        /// Returns whether the file appears to be a .000 header.
        /// </summary>
        public static bool Is(BinaryReaderEx br)
        {
            if (BND3.IsRead(br.GetBytes(0, (int)br.Length), out BND3 bnd3))
            {
                foreach (var file in bnd3.Files)
                {
                    if (file.Name.EndsWith(".000"))
                    {
                        return Is(file.Bytes);
                    }
                }
            }

            if (br.Length < 0x50 || br.GetInt32(4) != 0x10 || br.GetInt32(8) != 0x10 || br.GetInt32(0xC) != 0x800000)
                return false;

            for (int i = 0; i < 16; i++)
            {
                if (br.GetInt32(0x10 + i * 4) != 0)
                    return false;
            }

            return true;
        }

        private static BinaryReaderEx FindZero3InBND3(BND3 bnd3, string indexString)
        {
            foreach (var file in bnd3.Files)
            {
                if (file.Name.EndsWith(indexString))
                {
                    return new BinaryReaderEx(true, file.Bytes);
                }
            }

            throw new InvalidDataException("File was a BND3 but did not contain a valid Zero3.");
        }

        /// <summary>
        /// Read file data from the given .000 header file and associated data files.
        /// </summary>
        public static Zero3 Read(string path)
        {
            var containers = new List<BinaryReaderEx>();
            int index = 0;
            string indexString = index.ToString("D3");
            string containerPath = Path.ChangeExtension(path, indexString);

            while (System.IO.File.Exists(containerPath))
            {
                containers.Add(new BinaryReaderEx(true, System.IO.File.OpenRead(containerPath)));
                index++;
                indexString = index.ToString("D3");
                containerPath = Path.ChangeExtension(path, indexString);
            }

            var result = new Zero3(containers[0], containers);
            foreach (BinaryReaderEx br in containers)
                br.Stream.Close();
            return result;
        }

        /// <summary>
        /// Read file data from the given .000 header file and associated data files.
        /// </summary>
        public static Zero3 ReadFromPacked(string path)
        {
            var containers = new List<BinaryReaderEx>();
            int index = 0;
            string indexString = index.ToString("D3");
            string containerPath = Path.ChangeExtension(path, indexString);

            while (System.IO.File.Exists(containerPath))
            {
                BinaryReaderEx br;
                if (BND3.IsRead(containerPath, out BND3 bnd3))
                {
                    br = FindZero3InBND3(bnd3, indexString);
                }
                else
                {
                    throw new InvalidDataException("File was not packed into a BND3.");
                }

                containers.Add(br);
                index++;
                indexString = index.ToString("D3");
                containerPath = Path.ChangeExtension(path, indexString);
            }

            var result = new Zero3(containers[0], containers);
            foreach (BinaryReaderEx br in containers)
                br.Stream.Close();
            return result;
        }

        /// <summary>
        /// Write file data to a .000 header file and associated data files.
        /// </summary>
        /// <param name="path">The folder to write the files to.</param>
        /// <param name="name">The name of the files, excluding extension.</param>
        /// <param name="zero3">The Zero3 to write to these files.</param>
        /// <param name="packBndPath">The path to a BND containing the proper flags to repack these Zero3 in if applicable.</param>
        public static void Write(string path, string name, Zero3 zero3, string packBndPath = null)
        {
            var bws = new List<BinaryWriterEx>();
            bws[0] = new BinaryWriterEx(true);
            WriteHeader(bws[0], zero3);
            for (int i = 0; i < zero3.Files.Count; i++)
            {
                var file = zero3.Files[i];
                var bw = bws[file.ContainerIndex];
                if (bw == null)
                    bws[file.ContainerIndex] = new BinaryWriterEx(true);
                
                bws[0].FillUInt32($"FileOffset_{i}", (uint)bw.Position * 0x10);
                bw.WriteBytes(file.Bytes);
            }

            for (int i = 0; i < bws.Count; i++)
            {
                string fileName = $"{name}.{string.Format("D3", i)}";
                string outpath = $"{path}\\{fileName}";

                if (packBndPath != null)
                {
                    WriteBND(bws[i], outpath, packBndPath);
                    return;
                }

                System.IO.File.WriteAllBytes(outpath, bws[i].FinishBytes());
            }
        }

        private static void WriteHeader(BinaryWriterEx bw, Zero3 zero3)
        {
            bw.WriteInt32(zero3.Files.Count);
            bw.WriteInt32(0x10);
            bw.WriteInt32(0x10);
            bw.WriteInt32(0x800000);
            bw.WritePattern(0x40, 0x00);
            for (int i = 0; i < zero3.Files.Count; i++)
            {
                zero3.Files[i].Write(bw, i);
            }
        }

        private static void WriteBND(BinaryWriterEx bw, string outpath, string packBndPath)
        {
            if (!BND3.Is(packBndPath))
                throw new System.Exception("Specified pack BND path is not a BND3.");

            BND3 packBND = BND3.Read(packBndPath);

            if (packBND.Files.Count < 1)
                throw new System.Exception("Pack BND3 must have at least one file to get flags from while repacking.");
            if (packBND.Files.Count > 1)
                packBND.Files.RemoveRange(1, packBND.Files.Count - 1);

            packBND.Files[0].Bytes = bw.FinishBytes();
            System.IO.File.WriteAllBytes(outpath, packBND.Write());
        }

        internal Zero3(BinaryReaderEx br, List<BinaryReaderEx> containers)
        {
            br.BigEndian = true;

            int fileCount = br.ReadInt32();
            br.AssertInt32(0x10);
            br.AssertInt32(0x10);
            br.AssertInt32(0x800000); // Max file size (8 MB)
            br.AssertPattern(0x40, 0x00);

            Files = new List<File>(fileCount);
            for (int i = 0; i < fileCount; i++)
                Files.Add(new File(br, containers));
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

            internal File(BinaryReaderEx br, List<BinaryReaderEx> containers)
            {
                Name = br.ReadFixStr(0x40);
                ContainerIndex = br.ReadInt32();
                uint fileOffset = br.ReadUInt32(); // Multiply by 0x10
                br.ReadInt32(); // Padded file size; multiply by 0x10
                int fileSize = br.ReadInt32();
                Bytes = containers[ContainerIndex].GetBytes(fileOffset * 0x10, fileSize);
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteFixStr(Name, 0x40);
                bw.WriteInt32(ContainerIndex);
                bw.ReserveUInt32($"FileOffset_{index}");
                bw.WriteUInt32((uint)Bytes.Length * 0x10);
                bw.WriteInt32(Bytes.Length);
            }
        }
    }
}
