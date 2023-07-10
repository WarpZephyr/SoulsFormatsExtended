using System;
using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    /// <summary>
    /// A DBP param containing several fields.
    /// </summary>
    public class DBPPARAM : SoulsFile<DBPPARAM>
    {
        /// <summary>
        /// Whether or not the DBPPARAM is in big endian.
        /// </summary>
        public bool BigEndian { get; set; } = true;

        /// <summary>
        /// Whether or not a dbp is currently applied.
        /// </summary>
        public bool DbpApplied { get => AppliedParamDbp != null; }

        /// <summary>
        /// The currently applied PARAMDBP.
        /// </summary>
        public PARAMDBP AppliedParamDbp { get; private set; }

        /// <summary>
        /// Cells contained in this row. Must be loaded with PARAM.ApplyParamDbp() before use.
        /// </summary>
        public IReadOnlyList<Cell> Cells { get; private set; }

        /// <summary>
        /// The raw stream of the DBPPARAM.
        /// </summary>
        private BinaryReaderEx CellReader;

        /// <summary>
        /// Create a new, empty DBPPARAM.
        /// </summary>
        public DBPPARAM(){}

        /// <summary>
        /// Create a DBPPARAM using its DBP.
        /// </summary>
        /// <param name="dbp">The DBP of this DBPPARAM.</param>
        public DBPPARAM(PARAMDBP dbp)
        {
            AppliedParamDbp = dbp;
            var cells = new Cell[dbp.Fields.Count];
            for (int i = 0; i < dbp.Fields.Count; i++)
            {
                PARAMDBP.Field field = dbp.Fields[i];
                cells[i] = new Cell(field, field.Default);
            }
            Cells = cells;
        }

        /// <summary>
        /// Deserialize a DBPPARAM from a stream.
        /// </summary>
        /// <param name="path">The path to read the DBPPARAM from.</param>
        /// <param name="bigendian">Whether or not to read the DBPPARAM in big endian.</param>
        /// <returns>A new DBPPARAM.</returns>
        public static DBPPARAM Read(string path, bool bigendian)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                BinaryReaderEx br = new BinaryReaderEx(false, stream);
                DBPPARAM file = new DBPPARAM();
                br = SFUtil.GetDecompressedBR(br, out DCX.Type compression);
                file.Compression = compression;
                file.BigEndian = bigendian;
                file.Read(br);
                return file;
            }
        }

        /// <summary>
        /// Deserialize a DBPPARAM from a stream.
        /// </summary>
        /// <param name="bytes">The bytes to read the DBPPARAM from.</param>
        /// <param name="bigendian">Whether or not to read the DBPPARAM in big endian.</param>
        /// <returns>A new DBPPARAM.</returns>
        public static DBPPARAM Read(byte[] bytes, bool bigendian)
        {
            BinaryReaderEx br = new BinaryReaderEx(false, bytes);
            DBPPARAM file = new DBPPARAM();
            br = SFUtil.GetDecompressedBR(br, out DCX.Type compression);
            file.Compression = compression;
            file.BigEndian = bigendian;
            file.Read(br);
            return file;
        }

        /// <summary>
        /// Set the reader of the DBPPARAM for use when applying the PARAMDBP.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = BigEndian;
            byte[] copy = br.GetBytes(0, (int)br.Stream.Length);
            CellReader = new BinaryReaderEx(br.BigEndian, copy);
        }

        /// <summary>
        /// Serialize DBPPARAM cell values to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = BigEndian;
            foreach (var cell in Cells)
            {
                switch (cell.Dbp.DisplayType)
                {
                    case DefType.s8:
                        bw.WriteSByte(Convert.ToSByte(cell.Value));
                        break;
                    case DefType.u8:
                        bw.WriteByte(Convert.ToByte(cell.Value));
                        break;
                    case DefType.s16:
                        bw.WriteInt16(Convert.ToInt16(cell.Value));
                        break;
                    case DefType.u16:
                        bw.WriteUInt16(Convert.ToUInt16(cell.Value));
                        break;
                    case DefType.s32:
                        bw.WriteInt32(Convert.ToInt32(cell.Value));
                        break;
                    case DefType.u32:
                        bw.WriteUInt32(Convert.ToUInt32(cell.Value));
                        break;
                    case DefType.f32:
                        bw.WriteSingle(Convert.ToSingle(cell.Value));
                        break;
                    default:
                        throw new NotImplementedException($"Display Type: {cell.Dbp.DisplayType} invalid or not implemented.");
                }
            }
        }

        /// <summary>
        /// Apply a dbp to this param, returning true if it applied.
        /// </summary>
        /// <param name="dbp">The PARAMDBP to apply.</param>
        public bool ApplyParamDbp(PARAMDBP dbp)
        {
            int dbpParamSize = dbp.CalculateParamSize();
            if (CellReader.Length < dbpParamSize)
                return false;
            if (CellReader.Length > dbpParamSize)
                return false;
            CellReader.Position = 0;

            var cells = new Cell[dbp.Fields.Count];
            for (int i = 0; i < dbp.Fields.Count; i++)
            {
                PARAMDBP.Field field = dbp.Fields[i];
                object value = ReadCellValue(CellReader, field.DisplayType);
                cells[i] = new Cell(field, value);
            }
            Cells = cells;

            AppliedParamDbp = dbp;
            return true;
        }

        /// <summary>
        /// Apply a dbp to this param, checking a list of dbps for matches.
        /// Returns true if a match was found.
        /// </summary>
        /// <param name="dbps">The dbps to check for a match.</param>
        /// <returns>Whether or not a match was found.</returns>
        public bool ApplyParamDbp(IEnumerable<PARAMDBP> dbps)
        {
            foreach (PARAMDBP dbp in dbps)
                if (ApplyParamDbp(dbp))
                    return true;
            return false;
        }

        /// <summary>
        /// Read the value of a cell using its display type.
        /// </summary>
        /// <param name="br">A BinaryReaderEx stream representing the DBPPARAM.</param>
        /// <param name="type">The display type of the cell.</param>
        /// <returns>The value of the cell as an object.</returns>
        /// <exception cref="NotImplementedException">If the provided display type does is not supported or does not exist.</exception>
        private object ReadCellValue(BinaryReaderEx br, DefType type)
        {
            object value;
            switch (type)
            {
                case DefType.s8:
                    value = br.ReadSByte();
                    break;
                case DefType.u8:
                    value = br.ReadByte();
                    break;
                case DefType.s16:
                    value = br.ReadInt16();
                    break;
                case DefType.u16:
                    value = br.ReadUInt16();
                    break;
                case DefType.s32:
                    value = br.ReadInt32();
                    break;
                case DefType.u32:
                    value = br.ReadUInt32();
                    break;
                case DefType.f32:
                    value = br.ReadSingle();
                    break;
                default:
                    throw new NotImplementedException($"Display Type: {type} invalid or not implemented.");
            }
            return value;
        }

        /// <summary>
        /// A single value in a PARAM.
        /// </summary>
        public class Cell
        {
            /// <summary>
            /// The PARAMDBP Field that describes this cell.
            /// </summary>
            public PARAMDBP.Field Dbp { get; }

            /// <summary>
            /// The DisplayType of the cell from its dbp field as a string.
            /// </summary>
            public DefType DisplayType
            {
                get => Dbp.DisplayType;
                set => Dbp.DisplayType = value;
            }

            /// <summary>
            /// The description of the cell from its dbp field.
            /// </summary>
            public string DisplayName
            {
                get => Dbp.DisplayName;
                set => Dbp.DisplayName = value;
            }

            /// <summary>
            /// The DisplayFormat of the cell from its dbp field.
            /// </summary>
            public string DisplayFormat
            {
                get => Dbp.DisplayFormat;
                set => Dbp.DisplayFormat = value;
            }

            /// <summary>
            /// The default editor value of the cell from its dbp field.
            /// </summary>
            public object Default
            {
                get => Dbp.Default;
                set => Dbp.Default = value;
            }

            /// <summary>
            /// The maximum editor value of the cell from its dbp field.
            /// </summary>
            public object Maximum
            {
                get => Dbp.Maximum;
                set => Dbp.Maximum = value;
            }

            /// <summary>
            /// The minimum editor value of the cell from its dbp field.
            /// </summary>
            public object Minimum
            {
                get => Dbp.Minimum;
                set => Dbp.Minimum = value;
            }

            /// <summary>
            /// The increment editor value of the cell from its dbp field.
            /// </summary>
            public object Increment
            {
                get => Dbp.Increment;
                set => Dbp.Increment = value;
            }

            /// <summary>
            /// The value of this cell.
            /// </summary>
            private object _Value;

            /// <summary>
            /// The value of this cell.
            /// </summary>
            public object Value
            {
                get => _Value;
                set
                {
                    if (value == null)
                        throw new NullReferenceException($"Cell value may not be null.");

                    switch (Dbp.DisplayType)
                    {
                        case DefType.s8: _Value = Convert.ToSByte(value); break;
                        case DefType.u8: _Value = Convert.ToByte(value); break;
                        case DefType.s16: _Value = Convert.ToInt16(value); break;
                        case DefType.u16: _Value = Convert.ToUInt16(value); break;
                        case DefType.s32: _Value = Convert.ToInt32(value); break;
                        case DefType.u32: _Value = Convert.ToUInt32(value); break;
                        case DefType.f32: _Value = Convert.ToSingle(value); break;
                        default:
                            throw new NotImplementedException($"Conversion not specified for type {Dbp.DisplayType}");
                    }
                }
            }

            /// <summary>
            /// Create a new DBPPARAM cell.
            /// </summary>
            /// <param name="dbp">The DBP field to apply to this cell.</param>
            /// <param name="value">The value to set to this cell.</param>
            internal Cell(PARAMDBP.Field dbp, object value)
            {
                Dbp = dbp;
                Value = value;
            }

            /// <summary>
            /// Returns a string representation of the cell.
            /// </summary>
            public override string ToString()
            {
                return $"{Dbp.DisplayType} {Dbp.DisplayName} = {Value}";
            }
        }
    }
}