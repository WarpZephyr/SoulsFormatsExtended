﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SoulsFormats
{
    /// <summary>
    /// A general-purpose configuration file used throughout the series.
    /// </summary>
    public partial class PARAM : SoulsFile<PARAM>
    {
        /// <summary>
        /// Whether the file is big-endian; true for PS3/360 files, false otherwise.
        /// </summary>
        public bool BigEndian { get; set; }

        /// <summary>
        /// Flags indicating format of the file.
        /// </summary>
        public FormatFlags1 Format2D { get; set; }

        /// <summary>
        /// More flags indicating format of the file.
        /// </summary>
        public FormatFlags2 Format2E { get; set; }

        /// <summary>
        /// Originally matched the paramdef for version 101, but since is always 0 or 0xFF.
        /// </summary>
        public byte ParamdefFormatVersion { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public short Unk06 { get; set; }

        /// <summary>
        /// Indicates a revision of the row data structure.
        /// </summary>
        public short ParamdefDataVersion { get; set; }

        /// <summary>
        /// Identifies corresponding params and paramdefs.
        /// </summary>
        public string ParamType { get; set; }

        /// <summary>
        /// Automatically determined based on spacing of row offsets; -1 if param had no rows.
        /// </summary>
        public long DetectedRowSize { get; private set; }

        /// <summary>
        /// The rows of this param; must be loaded with PARAM.ApplyParamdef() before cells can be used.
        /// </summary>
        public List<Row> Rows { get; set; }

        /// <summary>
        /// The current applied PARAMDEF.
        /// </summary>
        public PARAMDEF AppliedParamdef { get; private set; }

        /// <summary>
        /// Whether or not rows do not support names.<br/>
        /// This applies to Chromehounds.
        /// </summary>
        public bool UnnamedRows { get; set; }

        /// <summary>
        /// Whether or not rows are headerless.<br/>
        /// This applies to Armored Core Formula Front.
        /// </summary>
        public bool HeaderlessRows { get; set; }

        private Dictionary<string, long> StringOffsetDictionary;
        private BinaryReaderEx RowReader;

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.Position = 0x2C;
            br.BigEndian = BigEndian = br.AssertByte(0, 0xFF) == 0xFF;
            Format2D = (FormatFlags1)br.ReadByte();
            Format2E = (FormatFlags2)br.ReadByte();
            ParamdefFormatVersion = br.ReadByte();
            br.Position = 0;

            // Make a private copy of the file to read row data from later
            byte[] copy = br.GetBytes(0, (int)br.Stream.Length);
            RowReader = new BinaryReaderEx(BigEndian, copy);

            // The strings offset in the header is highly unreliable; only use it as a last resort
            long actualStringsOffset = 0;
            long stringsOffset = br.ReadUInt32();
            long dataStartHeader = -1;
            if (Format2D.HasFlag(FormatFlags1.Flag01) && Format2D.HasFlag(FormatFlags1.IntDataOffset) || Format2D.HasFlag(FormatFlags1.LongDataOffset))
            {
                br.AssertInt16(0);
            }
            else
            {
                dataStartHeader = br.ReadUInt16();
            }

            Unk06 = br.ReadInt16();
            ParamdefDataVersion = br.ReadInt16();
            ushort rowCount = br.ReadUInt16();
            long paramTypeOffset = 0;
            if (Format2D.HasFlag(FormatFlags1.OffsetParamType))
            {
                br.AssertInt32(0);
                paramTypeOffset = br.ReadInt64();
                br.AssertPattern(0x14, 0x00);

                // ParamType itself will be checked after rows.
                actualStringsOffset = paramTypeOffset;
            }
            else
            {
                ParamType = br.ReadFixStr(0x20);
            }

            br.Skip(4); // Format
            if (Format2D.HasFlag(FormatFlags1.Flag01) && Format2D.HasFlag(FormatFlags1.IntDataOffset))
            {
                dataStartHeader = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
            }
            else if (Format2D.HasFlag(FormatFlags1.LongDataOffset))
            {
                dataStartHeader = br.ReadInt64();
                br.AssertInt64(0);
            }

            // Run some heuristics to detect nameless and headerless rows
            long rowsStart = br.Position;
            long GetRowDataOffset(long position)
            {
                long rowDataOffset;
                if (Format2D.HasFlag(FormatFlags1.LongDataOffset))
                {
                    rowDataOffset = br.GetInt64(position + 8);
                }
                else
                {
                    rowDataOffset = br.GetUInt32(position + 4);
                }

                return rowDataOffset;
            }

            // Detect if row header has no name offset
            long rowDataOffset1 = GetRowDataOffset(rowsStart);
            long rowsSize = rowDataOffset1 - rowsStart;
            int rowHeaderSize = 12;
            if (rowsSize < (rowCount * rowHeaderSize))
            {
                UnnamedRows = true;
                rowHeaderSize = 8;
            }

            // Detect if rows have no header at all
            if (dataStartHeader != -1)
            {
                if (rowsStart == dataStartHeader || (rowsStart + rowHeaderSize) > dataStartHeader)
                {
                    HeaderlessRows = true;
                    UnnamedRows = true;
                }
            }

            long dataStart = 0;
            Rows = new List<Row>(rowCount);
            if (HeaderlessRows)
            {
                dataStart = rowsStart;
                DetectedRowSize = (br.Length - rowsStart) / rowCount;
                long rowOffset = rowsStart;
                for (int i = 0; i < rowCount; i++)
                {
                    Rows.Add(new Row(i, rowOffset));
                    rowOffset += DetectedRowSize;
                }
            }
            else
            {
                for (int i = 0; i < rowCount; i++)
                    Rows.Add(new Row(br, this, ref actualStringsOffset));

                if (Rows.Count > 1)
                    DetectedRowSize = Rows[1].DataOffset - Rows[0].DataOffset;
                else if (Rows.Count == 1)
                    DetectedRowSize = (actualStringsOffset == 0 ? stringsOffset : actualStringsOffset) - Rows[0].DataOffset;
                else
                    DetectedRowSize = -1;

                if (Rows.Count > 0)
                {
                    dataStart = Rows.Min(row => row.DataOffset);
                }
            }

            if (Format2D.HasFlag(FormatFlags1.OffsetParamType))
            {
                // Check if ParamTypeOffset is valid.
                if (paramTypeOffset == dataStart + (rowCount * DetectedRowSize))
                {
                    ParamType = br.GetASCII(paramTypeOffset);
                }
                else
                {
                    ParamType = string.Empty;
                }
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            if (AppliedParamdef == null)
                throw new InvalidOperationException("Params cannot be written without applying a paramdef.");

            bw.BigEndian = BigEndian;

            bw.ReserveUInt32("StringsOffset");
            if (Format2D.HasFlag(FormatFlags1.Flag01) && Format2D.HasFlag(FormatFlags1.IntDataOffset) || Format2D.HasFlag(FormatFlags1.LongDataOffset))
            {
                bw.WriteInt16(0);
            }
            else
            {
                bw.ReserveUInt16("DataStart");
            }
            bw.WriteInt16(Unk06);
            bw.WriteInt16(ParamdefDataVersion);
            bw.WriteUInt16((ushort)Rows.Count);
            if (Format2D.HasFlag(FormatFlags1.OffsetParamType))
            {
                bw.WriteInt32(0);
                bw.ReserveInt64("ParamTypeOffset");
                bw.WritePattern(0x14, 0x00);
            }
            else
            {
                // This padding heuristic isn't completely accurate, not that it matters
                byte padding = (byte)(Format2D.HasFlag(FormatFlags1.Flag01) ? 0x20 : 0x00);
                if (HeaderlessRows)
                {
                    padding = 0x20;
                }

                bw.WriteFixStr(ParamType, 0x20, padding);
            }
            bw.WriteByte((byte)(BigEndian ? 0xFF : 0x00));
            bw.WriteByte((byte)Format2D);
            bw.WriteByte((byte)Format2E);
            bw.WriteByte(ParamdefFormatVersion);
            if (Format2D.HasFlag(FormatFlags1.Flag01) && Format2D.HasFlag(FormatFlags1.IntDataOffset))
            {
                bw.ReserveUInt32("DataStart");
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
            }
            else if (Format2D.HasFlag(FormatFlags1.LongDataOffset))
            {
                bw.ReserveInt64("DataStart");
                bw.WriteInt64(0);
            }

            if (!HeaderlessRows)
                for (int i = 0; i < Rows.Count; i++)
                    Rows[i].WriteHeader(bw, this, i);

            // This is probably pretty stupid
            if (Format2D == FormatFlags1.Flag01)
                bw.WritePattern(0x20, 0x00);

            if (Format2D.HasFlag(FormatFlags1.Flag01) && Format2D.HasFlag(FormatFlags1.IntDataOffset))
                bw.FillUInt32("DataStart", (uint)bw.Position);
            else if (Format2D.HasFlag(FormatFlags1.LongDataOffset))
                bw.FillInt64("DataStart", bw.Position);
            else
                bw.FillUInt16("DataStart", (ushort)bw.Position);

            for (int i = 0; i < Rows.Count; i++)
                Rows[i].WriteCells(bw, this, i);

            bw.FillUInt32("StringsOffset", (uint)bw.Position);

            if (Format2D.HasFlag(FormatFlags1.OffsetParamType))
            {
                bw.FillInt64("ParamTypeOffset", bw.Position);
                bw.WriteASCII(ParamType, true);
            }
            
            StringOffsetDictionary = new Dictionary<string, long>() 
            {
                {"", bw.Position}
            };

            if (!UnnamedRows && !HeaderlessRows)
            {
                bw.WriteInt16(0); // null string
                for (int i = 0; i < Rows.Count; i++)
                    Rows[i].WriteName(bw, this, i);

                // DeS and BB sometimes (but not always) include some useless padding here
                bw.WriteInt16(0); // useless padding at the end
            }
        }

        /// <summary>
        /// Interprets row data according to the given paramdef and stores it for later writing.
        /// </summary>
        public void ApplyParamdef(PARAMDEF paramdef)
        {
            AppliedParamdef = paramdef;
            foreach (Row row in Rows)
                row.ReadCells(RowReader, AppliedParamdef);
        }

        /// <summary>
        /// Applies a paramdef only if its param type, data version, and row size match this param's. Returns true if applied.
        /// </summary>
        public bool ApplyParamdefCarefully(PARAMDEF paramdef)
        {
            if (ParamType == paramdef.ParamType && ParamdefDataVersion == paramdef.DataVersion
                && (DetectedRowSize == -1 || DetectedRowSize == paramdef.GetRowSize()))
            {
                ApplyParamdef(paramdef);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Applies the first paramdef in the sequence whose param type, data version, and row size match this param's, if any. Returns true if applied. 
        /// </summary>
        public bool ApplyParamdefCarefully(IEnumerable<PARAMDEF> paramdefs)
        {
            foreach (PARAMDEF paramdef in paramdefs)
            {
                if (ApplyParamdefCarefully(paramdef))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Applies a paramdef only if its param type, and data version match this param's. Returns true if applied.
        /// </summary>
        public bool ApplyParamdefSomewhatCarefully(PARAMDEF paramdef)
        {
            // Using headerless rows as a heuristic here may be a bad idea
            if ((ParamType == paramdef.ParamType || string.IsNullOrEmpty(ParamType)) && (HeaderlessRows || (ParamdefDataVersion == paramdef.DataVersion)))
            {
                ApplyParamdef(paramdef);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Applies the first paramdef in the sequence whose param type, and data version match this param's, if any. Returns true if applied. 
        /// </summary>
        public bool ApplyParamdefSomewhatCarefully(IEnumerable<PARAMDEF> paramdefs)
        {
            foreach (PARAMDEF paramdef in paramdefs)
            {
                if (ApplyParamdefSomewhatCarefully(paramdef))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the first row with the given ID, or null if not found.
        /// </summary>
        public Row this[int id] => Rows.Find(row => row.ID == id);

        /// <summary>
        /// Returns a string representation of the PARAM.
        /// </summary>
        public override string ToString()
        {
            return $"{ParamType} v{ParamdefDataVersion} [{Rows.Count}]";
        }

        /// <summary>
        /// First set of flags indicating file format; highly speculative.
        /// </summary>
        [Flags]
        public enum FormatFlags1 : byte
        {
            /// <summary>
            /// No flags set.
            /// </summary>
            None = 0,

            /// <summary>
            /// Unknown.
            /// </summary>
            Flag01 = 0b0000_0001,

            /// <summary>
            /// Expanded header with 32-bit data offset.
            /// </summary>
            IntDataOffset = 0b0000_0010,

            /// <summary>
            /// Expanded header with 64-bit data offset.
            /// </summary>
            LongDataOffset = 0b0000_0100,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag08 = 0b0000_1000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag10 = 0b0001_0000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag20 = 0b0010_0000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag40 = 0b0100_0000,

            /// <summary>
            /// Param type string is written separately instead of fixed-width in the header.
            /// </summary>
            OffsetParamType = 0b1000_0000,
        }

        /// <summary>
        /// Second set of flags indicating file format; highly speculative.
        /// </summary>
        [Flags]
        public enum FormatFlags2 : byte
        {
            /// <summary>
            /// No flags set.
            /// </summary>
            None = 0,

            /// <summary>
            /// Row names are written as UTF-16.
            /// </summary>
            UnicodeRowNames = 0b0000_0001,

            /// <summary>
            /// Unknown.
            /// </summary>
            Flag02 = 0b0000_0010,

            /// <summary>
            /// Unknown.
            /// </summary>
            Flag04 = 0b0000_0100,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag08 = 0b0000_1000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag10 = 0b0001_0000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag20 = 0b0010_0000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag40 = 0b0100_0000,

            /// <summary>
            /// Unused?
            /// </summary>
            Flag80 = 0b1000_0000,
        }
    }
}
