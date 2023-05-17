using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    /// <summary>
    /// A companion format to DBP params that describes each field present in a DBP param. Extension: .dbp
    /// </summary>
    public partial class PARAMDBP : SoulsFile<PARAMDBP>
    {
        /// <summary>
        /// The fields in this PARAMDBP.
        /// </summary>
        public List<Field> Fields;

        /// <summary>
        /// Creates a new, empty PARAMDBP.
        /// </summary>
        public PARAMDBP(){}

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType and DisplayFormat and empty Descriptions.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP.</param>
        /// <param name="displayType">The type of the value this field represents.</param>
        /// <param name="displayFormat">The string formatter for the value this field represents.</param>
        public PARAMDBP(int fieldCount, Field.FieldType displayType, string displayFormat)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayType, displayFormat));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, and Description.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP.</param>
        /// <param name="displayType">The type of the value the fields in this PARAMDBP represent.</param>
        /// <param name="displayFormat">The string formatter for the value the fields in this PARAMDBP represent.</param>
        /// <param name="description">The description to set for all fields in this PARAMDBP.</param>
        public PARAMDBP(int fieldCount, Field.FieldType displayType, string displayFormat, string description)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayType, displayFormat, description));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, Description, and value.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP.</param>
        /// <param name="displayType">The type of the value the fields in this PARAMDBP represent.</param>
        /// <param name="displayFormat">The string formatter for the value the fields in this PARAMDBP represent.</param>
        /// <param name="description">The description to set for all fields in this PARAMDBP.</param>
        /// <param name="value">The value to set all values in the fields to in this PARAMDBP.</param>
        public PARAMDBP(int fieldCount, Field.FieldType displayType, string displayFormat, string description, object value)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayType, displayFormat, description, value, value, value, value));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, Description, and specified values.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP.</param>
        /// <param name="displayType">The type of the value the fields in this PARAMDBP represent.</param>
        /// <param name="displayFormat">The string formatter for the value the fields in this PARAMDBP represent.</param>
        /// <param name="description">The description to set for all fields in this PARAMDBP.</param>
        /// <param name="default">The value to set for the default value in the fields in this PARAMDBP.</param>
        /// <param name="increment">The value to set for the increment value in the fields in this PARAMDBP.</param>
        /// <param name="minimum">The value to set for the minimum value in the fields in this PARAMDBP.</param>
        /// <param name="maximum">The value to set for the maximum value in the fields in this PARAMDBP.</param>
        public PARAMDBP(int fieldCount, Field.FieldType displayType, string displayFormat, string description, object @default, object increment, object minimum, object maximum)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayType, displayFormat, description, @default, increment, minimum, maximum));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, and Description.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP in order.</param>
        /// <param name="displayTypes">The types of the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="displayFormats">The string formatters for the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="descriptions">The descriptions to set for the fields in this PARAMDBP in order.</param>
        public PARAMDBP(int fieldCount, List<Field.FieldType> displayTypes, List<string> displayFormats, List<string> descriptions)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayTypes[i], displayFormats[i], descriptions[i]));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, Description, and values.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP in order.</param>
        /// <param name="displayTypes">The types of the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="displayFormats">The string formatters for the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="descriptions">The descriptions to set for the fields in this PARAMDBP in order.</param>
        /// <param name="values">The values to set for the fields in this PARAMDBP in order.</param>
        public PARAMDBP(int fieldCount, List<Field.FieldType> displayTypes, List<string> displayFormats, List<string> descriptions, List<object> values)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayTypes[i], displayFormats[i], descriptions[i], values[i], values[i], values[i], values[i]));
            }
        }

        /// <summary>
        /// Creates a new PARAMDBP with the specified number of fields all set to the specified DisplayType, DisplayFormat, Description, and specified values.
        /// </summary>
        /// <param name="fieldCount">The number of fields to add to this PARAMDBP in order.</param>
        /// <param name="displayTypes">The types of the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="displayFormats">The string formatters for the values the fields in this PARAMDBP represent in order.</param>
        /// <param name="descriptions">The descriptions to set for the fields in this PARAMDBP in order.</param>
        /// <param name="defaults">The default values to set for the fields in this PARAMDBP in order.</param>
        /// <param name="increments">The increment values to set for the fields in this PARAMDBP in order.</param>
        /// <param name="minimums">The minimum values to set for the fields in this PARAMDBP in order.</param>
        /// <param name="maximums">The maximum values to set for the fields in this PARAMDBP in order.</param>
        public PARAMDBP(int fieldCount, List<Field.FieldType> displayTypes, List<string> displayFormats, List<string> descriptions, List<object> defaults, List<object> increments, List<object> minimums, List<object> maximums)
        {
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(displayTypes[i], displayFormats[i], descriptions[i], defaults[i], increments[i], minimums[i], maximums[i]));
            }
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;
            int fieldCount = br.ReadInt32();
            br.AssertInt32(0);
            br.AssertInt32(0);
            br.AssertInt32(0);
            br.AssertPattern(fieldCount * 4, 0);
            Fields = new List<Field>();
            for (int i = 0; i < fieldCount; i++)
            {
                Fields.Add(new Field(br));
            }
            for (int i = 0; i < fieldCount; i++)
            {
                string description = br.ReadShiftJIS();
                if (string.IsNullOrEmpty(description))
                    description = "%NULL%";
                Fields[i].Description = description;
                Fields[i].DisplayFormat = br.ReadShiftJIS();
            }
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            bw.WriteInt32(Fields.Count);
            bw.WriteInt32(0);
            bw.WriteInt32(0);
            bw.WriteInt32(0);
            bw.WritePattern(Fields.Count * 4, 0);
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Write(bw);
            }
            for (int i = 0; i < Fields.Count; i++)
            {
                bw.WriteShiftJIS(Fields[i].Description, true);
                bw.WriteShiftJIS(Fields[i].DisplayFormat, true);
            }
        }

        /// <summary>
        /// Calculate the size of the PARAM this DBP goes to using it's fields.
        /// </summary>
        /// <returns>The size of the PARAM this DBP goes to.</returns>
        public int CalculateParamSize()
        {
            int size = 0;
            foreach (Field field in Fields)
            {
                switch (field.DisplayType)
                {
                    case Field.FieldType.s8:
                    case Field.FieldType.u8:
                        size += 1;
                        break;
                    case Field.FieldType.s16:
                    case Field.FieldType.u16:
                        size += 2;
                        break;
                    case Field.FieldType.s32:
                    case Field.FieldType.u32:
                    case Field.FieldType.f32:
                        size += 4;
                        break;
                    default:
                        throw new NotImplementedException($"Field Type: {field.DisplayType} invalid or not implemented.");
                }
            }
            return size;
        }

        /// <summary>
        /// Create a new DBPPARAM using only this DBP.
        /// </summary>
        /// <returns>A new DBPPARAM.</returns>
        public DBPPARAM CreateParam()
        {
            return new DBPPARAM(this);
        }

        /// <summary>
        /// Create a new DBPPARAM using only this DBP on the specified path.
        /// </summary>
        /// <param name="path">The path to create the DBPPARAM on.</param>
        public void CreateParam(string path)
        {
            var param = CreateParam();
            param.Write(path);
        }

        /// <summary>
        /// A field in a PARAMDBP containing values for a field, description for a field, and a string formatter for a field.
        /// </summary>
        public class Field
        {
            /// <summary>
            /// Supported primitive field types.
            /// </summary>
            public enum FieldType : uint
            {
                /// <summary>
                /// Signed 1-byte integer.
                /// </summary>
                s8 = 0,

                /// <summary>
                /// Unsigned 1-byte integer.
                /// </summary>
                u8 = 1,

                /// <summary>
                /// Signed 2-byte integer.
                /// </summary>
                s16 = 2,

                /// <summary>
                /// Unsigned 2-byte integer.
                /// </summary>
                u16 = 3,

                /// <summary>
                /// Signed 4-byte integer.
                /// </summary>
                s32 = 4,

                /// <summary>
                /// Unsigned 4-byte integer.
                /// </summary>
                u32 = 5,

                /// <summary>
                /// Single-precision floating point value.
                /// </summary>
                f32 = 6
            }

            /// <summary>
            /// Type of value to display in the editor.
            /// </summary>
            public FieldType DisplayType;

            /// <summary>
            /// The description for this field describing what it is in the DBP param.
            /// </summary>
            public string Description;

            /// <summary>
            /// The string formatter indicating how this field's values should be formatted.
            /// </summary>
            public string DisplayFormat;

            /// <summary>
            /// Default value for new fields.
            /// </summary>
            public object Default { get; set; }

            /// <summary>
            /// Amount of increase or decrease per step when scrolling in the editor.
            /// </summary>
            public object Increment { get; set; }

            /// <summary>
            /// Minimum valid value.
            /// </summary>
            public object Minimum { get; set; }

            /// <summary>
            /// Maximum valid value.
            /// </summary>
            public object Maximum { get; set; }

            /// <summary>
            /// Creates a new, empty Field.
            /// </summary>
            public Field(){}

            /// <summary>
            /// Create a new Field with default values using the specified options.
            /// </summary>
            /// <param name="displayType">The type the values in the field will be.</param>
            /// <param name="displayFormat">The display format of this field.</param>
            public Field(FieldType displayType, string displayFormat)
            {
                DisplayType = displayType;
                Description = "";
                DisplayFormat = displayFormat;
                Default = 0;
                Increment = 0;
                Minimum = 0;
                Maximum = 0;
            }

            /// <summary>
            /// Create a new Field with default values using the specified options.
            /// </summary>
            /// <param name="displayType">The type the values in the field will be.</param>
            /// <param name="description">The description of this field.</param>
            /// <param name="displayFormat">The display format of this field.</param>
            public Field(FieldType displayType, string description, string displayFormat)
            {
                DisplayType = displayType;
                Description = description;
                DisplayFormat = displayFormat;
                Default = 0;
                Increment = 0;
                Minimum = 0;
                Maximum = 0;
            }

            /// <summary>
            /// Create a new Field using the specified options.
            /// </summary>
            /// <param name="displayType">The type the values in the field will be.</param>
            /// <param name="description">The description of this field.</param>
            /// <param name="displayFormat">The display format of this field.</param>
            /// <param name="default">The default value of the value this field represents.</param>
            /// <param name="increment">The editor increment value of the value this field represents.</param>
            /// <param name="maximum">The editor maximum value of the value this field represents.</param>
            /// <param name="minimum">The editor minimum value of the value this field represents.</param>
            public Field(FieldType displayType, string description, string displayFormat, object @default, object increment, object minimum, object maximum)
            {
                DisplayType = displayType;
                Description = description;
                DisplayFormat = displayFormat;
                Default = @default;
                Increment = increment;
                Minimum = minimum;
                Maximum = maximum;
            }

            /// <summary>
            /// Read a new field from a stream.
            /// </summary>
            internal Field(BinaryReaderEx br)
            {
                DisplayType = br.ReadEnum32<FieldType>();
                br.AssertInt32(0);
                br.AssertInt32(0);
                switch (DisplayType)
                {
                    case FieldType.s8:
                        Default = br.ReadSByte();
                        Increment = br.ReadSByte();
                        Minimum = br.ReadSByte();
                        Maximum = br.ReadSByte();
                        break;
                    case FieldType.u8:
                        Default = br.ReadByte();
                        Increment = br.ReadByte();
                        Minimum = br.ReadByte();
                        Maximum = br.ReadByte();
                        break;
                    case FieldType.s16:
                        Default = br.ReadInt16();
                        Increment = br.ReadInt16();
                        Minimum = br.ReadInt16();
                        Maximum = br.ReadInt16();
                        break;
                    case FieldType.u16:
                        Default = br.ReadUInt16();
                        Increment = br.ReadUInt16();
                        Minimum = br.ReadUInt16();
                        Maximum = br.ReadUInt16();
                        break;
                    case FieldType.s32:
                        Default = br.ReadInt32();
                        Increment = br.ReadInt32();
                        Minimum = br.ReadInt32();
                        Maximum = br.ReadInt32();
                        break;
                    case FieldType.u32:
                        Default = br.ReadUInt32();
                        Increment = br.ReadUInt32();
                        Minimum = br.ReadUInt32();
                        Maximum = br.ReadUInt32();
                        break;
                    case FieldType.f32:
                        Default = br.ReadSingle();
                        Increment = br.ReadSingle();
                        Minimum = br.ReadSingle();
                        Maximum = br.ReadSingle();
                        break;
                    default:
                        throw new NotImplementedException($"Field Type: {DisplayType} invalid or not implemented.");
                }
            }

            /// <summary>
            /// Write this field to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteInt32((int)DisplayType);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                switch (DisplayType)
                {
                    case FieldType.s8:
                        bw.WriteByte((byte)Default);
                        bw.WriteByte((byte)Increment);
                        bw.WriteByte((byte)Minimum);
                        bw.WriteByte((byte)Maximum);
                        break;
                    case FieldType.u8:
                        bw.WriteByte((byte)Default);
                        bw.WriteByte((byte)Increment);
                        bw.WriteByte((byte)Minimum);
                        bw.WriteByte((byte)Maximum);
                        break;
                    case FieldType.s16:
                        bw.WriteInt16((short)Default);
                        bw.WriteInt16((short)Increment);
                        bw.WriteInt16((short)Minimum);
                        bw.WriteInt16((short)Maximum);
                        break;
                    case FieldType.u16:
                        bw.WriteUInt16((ushort)Default);
                        bw.WriteUInt16((ushort)Increment);
                        bw.WriteUInt16((ushort)Minimum);
                        bw.WriteUInt16((ushort)Maximum);
                        break;
                    case FieldType.s32:
                        bw.WriteInt32((int)Default);
                        bw.WriteInt32((int)Increment);
                        bw.WriteInt32((int)Minimum);
                        bw.WriteInt32((int)Maximum);
                        break;
                    case FieldType.u32:
                        bw.WriteUInt32((uint)Default);
                        bw.WriteUInt32((uint)Increment);
                        bw.WriteUInt32((uint)Minimum);
                        bw.WriteUInt32((uint)Maximum);
                        break;
                    case FieldType.f32:
                        bw.WriteSingle((float)Default);
                        bw.WriteSingle((float)Increment);
                        bw.WriteSingle((float)Minimum);
                        bw.WriteSingle((float)Maximum);
                        break;
                    default:
                        throw new NotImplementedException($"Field Type: {DisplayType} invalid or not implemented.");
                }
            }

            /// <summary>
            /// Convert FieldType into a string.
            /// </summary>
            /// <returns>FieldType as a string.</returns>
            public string FieldTypeToString()
            {
                switch (DisplayType)
                {
                    case FieldType.s8: return "s8";
                    case FieldType.u8: return "u8";
                    case FieldType.s16: return "s16";
                    case FieldType.u16: return "u16";
                    case FieldType.s32: return "s32";
                    case FieldType.u32: return "u32";
                    case FieldType.f32: return "f32";
                    default:
                        throw new NotImplementedException($"Field Type: {DisplayType} invalid or not implemented.");
                }
            }

            /// <summary>
            /// Get a FieldType using a string.
            /// </summary>
            /// <param name="str">A string representing a FieldType.</param>
            /// <returns>A FieldType.</returns>
            public static FieldType GetFieldType(string str)
            {
                switch (str.ToLower())
                {
                    case "0":
                    case "int8":
                    case "sbyte":
                    case "s8": return FieldType.s8;
                    case "1":
                    case "uint8":
                    case "ubyte":
                    case "byte":
                    case "u8": return FieldType.u8;
                    case "2":
                    case "int16":
                    case "short":
                    case "s16": return FieldType.s16;
                    case "3":
                    case "uint16":
                    case "ushort":
                    case "u16": return FieldType.u16;
                    case "4":
                    case "int":
                    case "int32":
                    case "s32": return FieldType.s32;
                    case "5":
                    case "uint":
                    case "uint32":
                    case "u32": return FieldType.u32;
                    case "6":
                    case "float":
                    case "float32":
                    case "f32": return FieldType.f32;
                    default:
                        throw new NotImplementedException($"Field Type: {str} invalid or not implemented.");
                }
            }

            /// <summary>
            /// Convert an object value to the specified type using a FieldType.
            /// </summary>
            /// <param name="str">A string to convert to the specified FieldType.</param>
            /// <param name="type">A FieldType.</param>
            /// <returns>An object from the provided string converted to the specified type.</returns>
            public static object ConvertToFieldType(string str, FieldType type)
            {
                switch (type)
                {
                    case FieldType.s8: return Convert.ToSByte(str);
                    case FieldType.u8: return Convert.ToByte(str);
                    case FieldType.s16: return Convert.ToInt16(str);
                    case FieldType.u16: return Convert.ToUInt16(str);
                    case FieldType.s32: return Convert.ToInt32(str);
                    case FieldType.u32: return Convert.ToUInt32(str);
                    case FieldType.f32: return Convert.ToSingle(str);
                    default:
                        throw new NotImplementedException($"Field Type: {type} invalid or not implemented.");
                }
            }

            /// <summary>
            /// Returns a string representation of this field.
            /// </summary>
            public override string ToString()
            {
                return $"DisplayType: {FieldTypeToString()}\n" +
                       $"DisplayFormat: {DisplayFormat}\n" +
                       $"Default: {Default}\n" +
                       $"Increment: {Increment}\n" +
                       $"Minimum: {Minimum}\n" +
                       $"Maximum:{Maximum}\n" +
                       $"Description: {Description}";
            }
        }
    }
}
