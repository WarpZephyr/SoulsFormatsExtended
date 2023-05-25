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
        /// The currently applied PARAMDBP.
        /// </summary>
        public PARAMDBP AppliedParamDBP { get; private set; }

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
        public DBPPARAM() { }

        /// <summary>
        /// Create a DBPPARAM using its DBP.
        /// </summary>
        /// <param name="dbp">The DBP of this DBPPARAM.</param>
        public DBPPARAM(PARAMDBP dbp)
        {
            AppliedParamDBP = dbp;
            var cells = new Cell[dbp.Fields.Count];
            for (int i = 0; i < dbp.Fields.Count; i++)
            {
                PARAMDBP.Field field = dbp.Fields[i];
                cells[i] = new Cell(field, field.Default);
            }
            Cells = cells;
        }

        /// <summary>
        /// Set the reader of the DBPPARAM for use when applying the PARAMDBP.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;
            CellReader = br;
        }

        /// <summary>
        /// Serialize DBPPARAM cell values to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            foreach (var cell in Cells)
            {
                switch (cell.Dbp.DisplayType)
                {
                    case PARAMDBP.Field.FieldType.s8:
                        bw.WriteSByte(Convert.ToSByte(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.u8:
                        bw.WriteByte(Convert.ToByte(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.s16:
                        bw.WriteInt16(Convert.ToInt16(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.u16:
                        bw.WriteUInt16(Convert.ToUInt16(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.s32:
                        bw.WriteInt32(Convert.ToInt32(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.u32:
                        bw.WriteUInt32(Convert.ToUInt32(cell.Value));
                        break;
                    case PARAMDBP.Field.FieldType.f32:
                        bw.WriteSingle(Convert.ToSingle(cell.Value));
                        break;
                    default:
                        throw new NotImplementedException($"Field Type: {cell.Dbp.DisplayType} invalid or not implemented.");
                }
            }
        }

        /// <summary>
        /// Apply a PARAMDBP to this DBPPARAM, validating size along the way.
        /// </summary>
        /// <param name="dbp">The PARAMDBP to apply.</param>
        public void ApplyParamDbp(PARAMDBP dbp)
        {
            int dbpParamSize = dbp.CalculateParamSize();
            if (CellReader.Length < dbpParamSize)
                throw new InvalidDataException("Param is too small to match the selected DBP to read it.");
            if (CellReader.Length > dbpParamSize)
                throw new InvalidDataException("Param is too large to match the selected DBP to read it.");
            AppliedParamDBP = dbp;

            var cells = new Cell[dbp.Fields.Count];
            for (int i = 0; i < dbp.Fields.Count; i++)
            {
                PARAMDBP.Field field = dbp.Fields[i];
                object value = ReadCellValue(CellReader, field.DisplayType);
                cells[i] = new Cell(field, value);
            }
            Cells = cells;
        }

        /// <summary>
        /// Read the value of a cell using its field type.
        /// </summary>
        /// <param name="br">A BinaryReaderEx stream representing the DBPPARAM.</param>
        /// <param name="type">The field type of the cell.</param>
        /// <returns>The value of the cell as an object.</returns>
        /// <exception cref="NotImplementedException">If the provided field type does is not supported or does not exist.</exception>
        private object ReadCellValue(BinaryReaderEx br, PARAMDBP.Field.FieldType type)
        {
            object value;
            switch (type)
            {
                case PARAMDBP.Field.FieldType.s8:
                    value = br.ReadSByte();
                    break;
                case PARAMDBP.Field.FieldType.u8:
                    value = br.ReadByte();
                    break;
                case PARAMDBP.Field.FieldType.s16:
                    value = br.ReadInt16();
                    break;
                case PARAMDBP.Field.FieldType.u16:
                    value = br.ReadUInt16();
                    break;
                case PARAMDBP.Field.FieldType.s32:
                    value = br.ReadInt32();
                    break;
                case PARAMDBP.Field.FieldType.u32:
                    value = br.ReadUInt32();
                    break;
                case PARAMDBP.Field.FieldType.f32:
                    value = br.ReadSingle();
                    break;
                default:
                    throw new NotImplementedException($"Field Type: {type} invalid or not implemented.");
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
                        case PARAMDBP.Field.FieldType.s8: _Value = Convert.ToSByte(value); break;
                        case PARAMDBP.Field.FieldType.u8: _Value = Convert.ToByte(value); break;
                        case PARAMDBP.Field.FieldType.s16: _Value = Convert.ToInt16(value); break;
                        case PARAMDBP.Field.FieldType.u16: _Value = Convert.ToUInt16(value); break;
                        case PARAMDBP.Field.FieldType.s32: _Value = Convert.ToInt32(value); break;
                        case PARAMDBP.Field.FieldType.u32: _Value = Convert.ToUInt32(value); break;
                        case PARAMDBP.Field.FieldType.f32: _Value = Convert.ToSingle(value); break;
                        default:
                            throw new NotImplementedException($"Conversion not specified for type {Dbp.DisplayType}");
                    }
                }
            }

            /// <summary>
            /// The value of this cell.
            /// </summary>
            private object _Value;

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
                return $"{Dbp.DisplayType} {Dbp.Description} = {Value}";
            }
        }
    }
}