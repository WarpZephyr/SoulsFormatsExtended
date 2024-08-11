using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    /// <summary>
    /// A texture container in Murakumo: Renegade Mech Pursuit.
    /// </summary>
    public class DDL : SoulsFile<DDL>
    {
        /// <summary>
        /// The name of the container.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The version?
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The DDS entries.
        /// </summary>
        public List<Entry> DDSEntries { get; set; }

        /// <summary>
        /// The PRM entries.
        /// </summary>
        public List<Entry> PRMEntries { get; set; }

        /// <summary>
        /// Whether or not data appears to be a <see cref="DDL"/>.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            return br.Length > 4 && br.GetASCII(0, 4) == "DDL ";
        }

        /// <summary>
        /// Read a <see cref="DDL"/>.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            br.AssertASCII("DDL ");
            Version = br.AssertInt32(20000);
            br.AssertInt32(0);
            int nameOffset = br.ReadInt32();
            int sectionCount = br.ReadInt32();
            int sectionOffset = br.ReadInt32();
            br.Position = nameOffset;
            Name = br.ReadShiftJIS();

            br.Position = sectionOffset;
            for (int i = 0; i < sectionCount; i++)
            {
                string sectionMagic = br.ReadASCII(4);
                int count = br.ReadInt32();
                int offset = br.ReadInt32();
                br.AssertInt32(0);

                long pos = br.Position;
                br.Position = offset;
                switch (sectionMagic)
                {
                    case "DDS ":
                        DDSEntries = new List<Entry>(count);
                        for (int j = 0; j < count; j++)
                        {
                            DDSEntries.Add(new Entry(br));
                        }
                        break;
                    case "INFO":
                        PRMEntries = new List<Entry>(count);
                        for (int j = 0; j < count; j++)
                        {
                            PRMEntries.Add(new Entry(br));
                        }
                        break;
                    default:
                        throw new NotSupportedException($"Unknown section type: {sectionMagic}");
                }
                br.Position = pos;
            }
        }

        /// <summary>
        /// An entry in a <see cref="DDL"/> container.
        /// </summary>
        public class Entry
        {
            /// <summary>
            /// The name in the <see cref="Entry"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The data in the <see cref="Entry"/>.
            /// </summary>
            public byte[] Bytes { get; set; }

            internal Entry(BinaryReaderEx br)
            {
                int nameOffset = br.ReadInt32();
                int offset = br.ReadInt32();
                int size = br.ReadInt32();

                long pos = br.Position;
                br.Position = nameOffset;
                Name = br.ReadShiftJIS();

                br.Position = offset;
                Bytes = br.ReadBytes(size);

                br.Position = pos;
            }
        }
    }
}
