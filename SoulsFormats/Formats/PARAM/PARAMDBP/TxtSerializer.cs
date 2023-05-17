using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoulsFormats
{
    /// <summary>
    /// A DBP param containing several fields.
    /// </summary>
    public partial class PARAMDBP : SoulsFile<PARAMDBP>
    {
        /// <summary>
        /// Serializes PARAMDBP to TXT.
        /// </summary>
        public class TxtSerializer
        {
            /// <summary>
            /// Serialize a PARAMDBP to TXT.
            /// </summary>
            /// <param name="dbp">A PARAMDBP to serialize to TXT.</param>
            /// <param name="outPath">The path to create a new TXT file on containing the serialized PARAMDBP.</param>
            public static void Serialize(PARAMDBP dbp, string outPath)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("DBP fields list is null, cannot serialize nothing.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("DBP fields list has no fields, cannot serialize nothing.");

                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                List<string> lines = new List<string>();
                for (int i = 0; i < dbp.Fields.Count; i++)
                {
                    Field field = dbp.Fields[i];
                    lines.Add($"[{i}]");
                    lines.Add($"Description: {field.Description}");
                    lines.Add($"DisplayType: {field.FieldTypeToString()}");
                    lines.Add($"DisplayFormat: {field.DisplayFormat}");
                    lines.Add($"Default: {field.Default}");
                    lines.Add($"Increment: {field.Increment}");
                    lines.Add($"Minimum: {field.Minimum}");
                    lines.Add($"Maximum: {field.Maximum}");
                }
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                File.WriteAllLines(outPath, lines, encoding);
            }

            /// <summary>
            /// Serialize a DBPPARAM that has an AppliedParamDbp to TXT.
            /// </summary>
            /// <param name="param">A DBPPARAM to serialize to TXT.</param>
            /// <param name="outPath">The path to create a new TXT file on containing the serialized DBPPARAM.</param>
            public static void Serialize(DBPPARAM param, string outPath)
            {
                if (param.Cells == null)
                    throw new InvalidDataException("Param cells list is null, DBP may not be applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells, DBP may not be applied.");

                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                List<string> lines = new List<string>();
                for (int i = 0; i < param.Cells.Count; i++)
                    lines.Add($"{param.Cells[i]}");
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                File.WriteAllLines(outPath, lines, encoding);
            }

            /// <summary>
            /// Serialize only the descriptions in a PARAMDBP to TXT.
            /// </summary>
            /// <param name="dbp">A PARAMDBP with descriptions to serialize to TXT.</param>
            /// <param name="outPath">The path to create a new TXT file on containing the serialized PARAMDBP descriptions.</param>
            public static void SerializeDescriptions(PARAMDBP dbp, string outPath)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("DBP fields list is null, cannot serialize nothing.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("DBP fields list has no fields, cannot serialize nothing.");

                string[] descriptions = new string[dbp.Fields.Count];
                for (int i = 0; i < dbp.Fields.Count; )
                {
                    descriptions[i] = dbp.Fields[i].Description;
                }
                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                File.WriteAllLines(outPath, descriptions, encoding);
            }

            /// <summary>
            /// Deserialize a TXT DBP to a PARAMDBP.
            /// </summary>
            /// <param name="path">A path to a TXT file with DBP data.</param>
            /// <returns>A new PARAMDBP.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static PARAMDBP Deserialize(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path must point to a TXT file, not a Directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("The TXT file to deserialize was not found.");

                var encoding = Encoding.GetEncoding(932); // Shift-JIS
                string[] lines = File.ReadAllLines(path, encoding);
                if (lines.Length == 0)
                    throw new InvalidDataException("TXT file is empty, nothing to deserialize.");

                var dbp = new PARAMDBP();
                dbp.Fields = new List<Field>();
                for (int i = 0; i < lines.Length; i++)
                {
                    // Skip the "[i]" line.
                    i++;

                    // Create a new field.
                    Field field = new Field();

                    // Skip to the colon delimiter and past its whitespace, then get the value of and advance after each line in the entry.
                    var description = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var type = Field.GetFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2)); i++;
                    var format = lines[i].Substring(lines[i].IndexOf(":") + 2); i++;
                    var @default = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var increment = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var minimum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type); i++;
                    var maximum = Field.ConvertToFieldType(lines[i].Substring(lines[i].IndexOf(":") + 2), type);

                    // Set the field values.
                    field.Description = description;
                    field.DisplayType = type;
                    field.DisplayFormat = format;
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
            /// Deserialize a TXT DBP directly to a new DBP file.
            /// </summary>
            /// <param name="path">A path to a TXT file with DBP data.</param>
            /// <param name="outPath">The path to create a new DBP on, if a directory is found it will be placed inside of it, if outPath is null file will be placed in path directory instead.</param>
            public static void DeserializeToPath(string path, string outPath = null)
            {
                // Validation checks.
                if (Directory.Exists(path))
                    throw new InvalidDataException("TXT path must be a TXT file, not a Directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("TXT file to deserialize into DBP file could not be found.");

                // outPath options check.
                if (outPath == null)
                    outPath = $"{Path.GetDirectoryName(path)}\\{Path.GetFileNameWithoutExtension(path)}.dbp";
                if (Directory.Exists(outPath))
                    outPath = $"{outPath}\\{Path.GetFileNameWithoutExtension(path)}.dbp";

                // Make the outPath directory if it does not exist.
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                // Deserialize and write the DBP.
                var dbp = Deserialize(path);
                dbp.Write(outPath);
            }

            /// <summary>
            /// Replace only the descriptions in a PARAMDBP with a TXT file.
            /// </summary>
            /// <param name="dbp">A PARAMDBP to replace the descriptions of.</param>
            /// <param name="path">The path to a TXT file containing serialized PARAMDBP descriptions.</param>
            public static void DeserializeDescriptions(PARAMDBP dbp, string path)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("DBP fields list is null, cannot serialize nothing.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("DBP fields list has no fields, cannot serialize nothing.");
                if (!File.Exists(path))
                    throw new InvalidDataException("Cannot find Descriptions TXT, cannot serialize nothing.");

                string[] descriptions = File.ReadAllLines(path);

                if (descriptions == null)
                    throw new InvalidDataException("Descriptions was null, cannot serialize nothing.");
                if (descriptions.Length == 0)
                    throw new InvalidDataException("Description TXT was found empty, cannot serialize nothing.");
                if (descriptions.Length != dbp.Fields.Count)
                    throw new InvalidDataException("The number of descriptions does not match the number of fields.");

                for (int i = 0; i < dbp.Fields.Count;)
                {
                    dbp.Fields[i].Description = descriptions[i];
                }
            }
        }
    }
}
