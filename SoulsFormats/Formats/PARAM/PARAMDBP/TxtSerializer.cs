using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoulsFormats
{
    public partial class PARAMDBP : SoulsFile<PARAMDBP>
    {
        /// <summary>
        /// Serializes params and dbps to txt.
        /// </summary>
        public class TxtSerializer
        {
            #region Serialize

            /// <summary>
            /// Serialize a dbp to a txt file.
            /// </summary>
            /// <param name="dbp">A dbp to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized dbp.</param>
            public static void Serialize(PARAMDBP dbp, string path)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields were null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp had no fields.");

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                List<string> lines = new List<string>();
                for (int i = 0; i < dbp.Fields.Count; i++)
                {
                    Field field = dbp.Fields[i];
                    lines.Add($"[{i}]");
                    lines.Add($"Description   : {(field.Description == "" ? "%NULL%" : field.Description)}");
                    lines.Add($"DisplayFormat : {field.DisplayFormat}");
                    lines.Add($"DisplayType   : {field.DisplayType}");
                    lines.Add($"Default       : {field.Default}");
                    lines.Add($"Increment     : {field.Increment}");
                    lines.Add($"Minimum       : {field.Minimum}");
                    lines.Add($"Maximum       : {field.Maximum}");
                }
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.txt";
                File.WriteAllLines($"{directory}\\{name}", lines, encoding);
            }

            /// <summary>
            /// Serialize a param that has an applied dbp to a txt file.
            /// </summary>
            /// <param name="param">A param to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized param.</param>
            public static void Serialize(DBPPARAM param, string path)
            {
                if (!param.DbpApplied)
                    throw new InvalidDataException("Dbp has not been applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells.");

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                List<string> lines = new List<string>();
                for (int i = 0; i < param.Cells.Count; i++)
                {
                    var cell = param.Cells[i];
                    lines.Add($"[{i}]");
                    lines.Add($"Description   : {(cell.Description == "" ? "%NULL%" : cell.Description)}");
                    lines.Add($"DisplayFormat : {cell.DisplayFormat}");
                    lines.Add($"DisplayType   : {cell.DisplayType}");
                    lines.Add($"Default       : {cell.Default}");
                    lines.Add($"Increment     : {cell.Increment}");
                    lines.Add($"Minimum       : {cell.Minimum}");
                    lines.Add($"Maximum       : {cell.Maximum}");
                    lines.Add($"Value         : {cell.Value}");
                }
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.txt";
                File.WriteAllLines($"{directory}\\{name}", lines, encoding);
            }

            /// <summary>
            /// Serialize only the descriptions in a dbp to a txt file.
            /// </summary>
            /// <param name="dbp">A dbp with descriptions to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized dbp descriptions.</param>
            public static void SerializeDescriptions(PARAMDBP dbp, string path)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields are null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp has no fields.");

                string[] descriptions = new string[dbp.Fields.Count];
                for (int i = 0; i < dbp.Fields.Count; i++)
                    descriptions[i] = dbp.Fields[i].Description == "" ? "%NULL%" : dbp.Fields[i].Description;
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.descriptions.txt";
                File.WriteAllLines($"{directory}\\{name}", descriptions, encoding);
            }

            /// <summary>
            /// Serialize only the values in a param to a txt file.
            /// </summary>
            /// <param name="param">A param with values to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized param values.</param>
            public static void SerializeValues(DBPPARAM param, string path)
            {
                if (!param.DbpApplied)
                    throw new InvalidDataException("Dbp has not been applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells.");

                string[] values = new string[param.Cells.Count];
                for (int i = 0; i < param.Cells.Count; i++)
                    values[i] = param.Cells[i].Value.ToString();
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.values.txt";
                File.WriteAllLines($"{directory}\\{name}", values, encoding);
            }

            /// <summary>
            /// Serialize a dbp to a param txt file.
            /// </summary>
            /// <param name="dbp">A dbp to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized data.</param>
            public static void SerializeToParam(PARAMDBP dbp, string path)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields are null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp has no fields.");
                Serialize(new DBPPARAM(dbp), $"{path}.txt");
            }

            /// <summary>
            /// Serialize a param that has an applied dbp to a dbp txt file.
            /// </summary>
            /// <param name="param">A param to serialize to txt.</param>
            /// <param name="path">The path to create a new txt file on containing the serialized data.</param>
            public static void SerializeToDbp(DBPPARAM param, string path)
            {
                if (!param.DbpApplied)
                    throw new InvalidDataException("Dbp has not been applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells.");
                Serialize(param.AppliedParamDbp, $"{Path.GetFileNameWithoutExtension(path)}.txt");
            }

            #endregion Serialize

            #region Deserialize

            /// <summary>
            /// Deserialize a txt file to a dbp.
            /// </summary>
            /// <param name="path">A path to a txt file with dbp data.</param>
            /// <returns>A new dbp.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static PARAMDBP DeserializeDbp(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string[] lines = File.ReadAllLines(path, encoding);
                if (lines.Length == 0)
                    throw new InvalidDataException("File is empty.");
                if (lines.Length % 8 != 0)
                    throw new InvalidDataException("File has an invalid line count.");

                var dbp = new PARAMDBP();
                for (int i = 0; i < lines.Length; i++)
                {
                    // Skip the "[i]" line.
                    i++;

                    // Create a new field.
                    Field field = new Field();

                    // Skip to the colon delimiter and past its whitespace, then get the value of and advance after each line in the entry.
                    var description = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var format = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var type = Field.GetDisplayType(lines[i].Substring(lines[i].IndexOf(":") + 2)); i++;
                    var @default = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var increment = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var minimum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var maximum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type);

                    // Set the field values.
                    field.Description = description;
                    field.DisplayFormat = format;
                    field.DisplayType = type;
                    field.Default = @default;
                    field.Increment = increment;
                    field.Minimum = minimum;
                    field.Maximum = maximum;

                    // Add the field.
                    dbp.Fields.Add(field);
                }
                return dbp;
            }

            /// <summary>
            /// Deserialize a txt file to a param.
            /// </summary>
            /// <param name="path">A path to a txt file with param data.</param>
            /// <returns>A new param.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static DBPPARAM DeserializeParam(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string[] lines = File.ReadAllLines(path, encoding);
                if (lines.Length == 0)
                    throw new InvalidDataException("File is empty.");
                if (lines.Length % 9 != 0)
                    throw new InvalidDataException("File has an invalid line count.");

                var dbp = new PARAMDBP();
                var paramValues = new List<object>();
                for (int i = 0; i < lines.Length; i++)
                {
                    // Skip the "[i]" line.
                    i++;

                    // Create a new field.
                    Field field = new Field();

                    // Skip to the colon delimiter and past its whitespace, then get the value of and advance after each line in the entry.
                    var description = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var format = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var type = Field.GetDisplayType(lines[i].Substring(lines[i].IndexOf(":") + 2)); i++;
                    var @default = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var increment = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var minimum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var maximum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var value = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type);

                    // Set the field values.
                    field.Description = description;
                    field.DisplayFormat = format;
                    field.DisplayType = type;
                    field.Default = @default;
                    field.Increment = increment;
                    field.Minimum = minimum;
                    field.Maximum = maximum;
                    paramValues.Add(value);

                    // Add the field.
                    dbp.Fields.Add(field);
                }

                var param = new DBPPARAM(dbp);
                for (int i = 0; i < paramValues.Count; i++)
                    param.Cells[i].Value = paramValues[i];

                return param;
            }

            /// <summary>
            /// Replace only the descriptions in a dbp with a txt file.
            /// </summary>
            /// <param name="dbp">A dbp to replace the descriptions of.</param>
            /// <param name="path">The path to a txt file containing serialized dbp descriptions.</param>
            public static void DeserializeDescriptions(PARAMDBP dbp, string path)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields are null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp has no fields.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                string[] descriptions = File.ReadAllLines(path);

                if (descriptions == null)
                    throw new InvalidDataException("Descriptions were null.");
                if (descriptions.Length == 0)
                    throw new InvalidDataException("Descriptions were empty.");
                if (descriptions.Length != dbp.Fields.Count)
                    throw new InvalidDataException("The number of descriptions does not match the number of fields.");

                for (int i = 0; i < dbp.Fields.Count;)
                    dbp.Fields[i].Description = descriptions[i];
            }

            /// <summary>
            /// Deserialize a txt file directly to a new dbp file.
            /// </summary>
            /// <param name="path">A path to a txt file with dbp data.</param>
            /// <param name="outPath">The path to create a new dbp on, if a directory is found it will be placed inside of it, if outPath is null file will be placed in path directory instead.</param>
            public static void DeserializeDbpToPath(string path, string outPath = null)
            {
                // Validation checks.
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                // outPath options check.
                if (outPath == null)
                    outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.dbp";
                if (Directory.Exists(outPath))
                    outPath = $"{outPath}\\{Path.GetFileNameWithoutExtension(path)}.dbp";

                // Make the outPath directory if it does not exist.
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                // Deserialize and write the DBP.
                var dbp = DeserializeDbp(path);
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.dbp";
                dbp.Write($"{directory}\\{name}");
            }

            /// <summary>
            /// Deserialize a txt file directly to a new param file.
            /// </summary>
            /// <param name="path">A path to a txt file with param data.</param>
            /// <param name="outPath">The path to create a new param on, if a directory is found it will be placed inside of it, if outPath is null file will be placed in path directory instead.</param>
            public static void DeserializeParamToPath(string path, string outPath = null)
            {
                // Validation checks.
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                // outPath options check.
                if (outPath == null)
                    outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.bin";
                if (Directory.Exists(outPath))
                    outPath = $"{Path.GetFileNameWithoutExtension(outPath)}\\{Path.GetFileNameWithoutExtension(path)}.bin";

                // Make the outPath directory if it does not exist.
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                // Deserialize and write the DBPPARAM.
                var param = DeserializeParam(path);
                string directory = Path.GetDirectoryName(path);
                string name = $"{Path.GetFileNameWithoutExtension(path)}.bin";
                param.Write($"{directory}\\{name}");
            }

            /// <summary>
            /// Deserialize a txt param to a dbp.
            /// </summary>
            /// <param name="path">A path to a txt file with param data.</param>
            /// <returns>A new dbp.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static PARAMDBP DeserializeDbpFromParam(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");
                return DeserializeParam(path).AppliedParamDbp;
            }

            /// <summary>
            /// Deserialize a txt dbp to a param.
            /// </summary>
            /// <param name="path">A path to a txt file with param data.</param>
            /// <returns>A new param.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static DBPPARAM DeserializeParamFromDbp(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");
                return new DBPPARAM(DeserializeDbp(path));
            }

            #endregion Deserialize

            /// <summary>
            /// Attempt to detect if a txt file is a serialized param or serialized dbp.
            /// Returns null if neither was detected.
            /// </summary>
            /// <param name="path">A path to a txt file with param or dbp data.</param>
            /// <returns>The detected serialized type or null.</returns>
            public static string DetectSerializedType(string path)
            {
                if (Directory.Exists(path))
                    return null;
                if (!File.Exists(path))
                    return null;

                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string[] lines = File.ReadAllLines(path, encoding);
                if (lines.Length == 0)
                    return null;
                if (lines.Length % 9 != 0)
                    return "Param";
                else if (lines.Length % 8 != 0)
                    return "Dbp";
                return null;
            }
        }
    }
}