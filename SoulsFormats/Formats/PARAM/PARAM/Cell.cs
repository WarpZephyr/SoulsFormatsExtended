using System;

namespace SoulsFormats
{
    public partial class PARAM
    {
        /// <summary>
        /// One cell in one row in a param.
        /// </summary>
        public class Cell
        {
            /// <summary>
            /// The paramdef field that describes this cell.
            /// </summary>
            public PARAMDEF.Field Def { get; }

            /// <summary>
            /// The value of this cell.
            /// </summary>
            public object Value
            {
                get => value;
                set
                {
                    if (value == null)
                        throw new NullReferenceException($"Cell value may not be null.");

                    switch (Def.DisplayType)
                    {
                        case DefType.s8: this.value = Convert.ToSByte(value); break;
                        case DefType.u8: this.value = Convert.ToByte(value); break;
                        case DefType.s16: this.value = Convert.ToInt16(value); break;
                        case DefType.u16: this.value = Convert.ToUInt16(value); break;
                        case DefType.s32: this.value = Convert.ToInt32(value); break;
                        case DefType.u32: this.value = Convert.ToUInt32(value); break;
                        case DefType.f32: this.value = Convert.ToSingle(value); break;
                        case DefType.b32: this.value = Convert.ToInt32(value); break;
                        case DefType.angle32: this.value = Convert.ToSingle(value); break;
                        case DefType.f64: this.value = Convert.ToDouble(value); break;
                        case DefType.fixstr: this.value = Convert.ToString(value); break;
                        case DefType.fixstrW: this.value = Convert.ToString(value); break;
                        case DefType.dummy8:
                            if (Def.BitSize == -1)
                                this.value = (byte[])value;
                            else
                                this.value = Convert.ToByte(value);
                            break;

                        default:
                            throw new NotImplementedException($"Conversion not specified for type {Def.DisplayType}");
                    }
                }
            }
            private object value;

            /// <summary>
            /// The display type of the cell.
            /// </summary>
            public DefType DisplayType => Def.DisplayType;

            /// <summary>
            /// The display name of the cell.
            /// </summary>
            public string DisplayName => Def.DisplayName;

            /// <summary>
            /// The internal name of the cell.
            /// </summary>
            public string InternalName => Def.InternalName;

            /// <summary>
            /// An optional description of the cell.
            /// </summary>
            public string Description => Def.Description;

            /// <summary>
            /// A string for formatting the value of the cell.
            /// </summary>
            public string DisplayFormat => Def.DisplayFormat;

            /// <summary>
            /// The default value of the cell.
            /// </summary>
            public object Default => Def.Default;

            /// <summary>
            /// How much the cell's value increments by.
            /// </summary>
            public object Increment => Def.Increment;

            /// <summary>
            /// The minimum value of the cell.
            /// </summary>
            public object Minimum => Def.Minimum;

            /// <summary>
            /// The maximum value of the cell.
            /// </summary>
            public object Maximum => Def.Maximum;

            internal Cell(PARAMDEF.Field def, object value)
            {
                Def = def;
                Value = value;
            }

            /// <summary>
            /// Make a new cell from a cell.
            /// </summary>
            /// <param name="clone">The cell to copy.</param>
            public Cell(Cell clone)
            {
                Def = clone.Def;
                Value = clone.Value;
            }

            /// <summary>
            /// Returns a string representation of the cell.
            /// </summary>
            public override string ToString()
            {
                return $"{Def.DisplayType} {Def.InternalName} = {Value}";
            }
        }
    }
}
