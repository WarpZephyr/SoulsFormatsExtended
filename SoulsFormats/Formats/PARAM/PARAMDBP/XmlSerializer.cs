using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace SoulsFormats
{
    public partial class PARAMDBP : SoulsFile<PARAMDBP>
    {
        /// <summary>
        /// Serializes params and dbps to xml.
        /// </summary>
        public class XmlSerializer
        {
            #region Serialize

            /// <summary>
            /// Serialize a dbp to an xml an return a byte array.
            /// </summary>
            /// <param name="dbp">A dbp.</param>
            /// <returns>A byte array.</returns>
            public static byte[] Serialize(PARAMDBP dbp)
            {
                using (var stream = new MemoryStream())
                {
                    var xws = new XmlWriterSettings();
                    xws.Indent = true;
                    xws.Encoding = DefaultEncoding;
                    var xw = XmlWriter.Create(stream, xws);
                    Serialize(dbp, xw);
                    stream.Seek(0, SeekOrigin.Begin);
                    return stream.ToArray();
                }
            }

            /// <summary>
            /// Serialize a dbp to an xml file.
            /// </summary>
            /// <param name="dbp">A dbp to serialize to xml.</param>
            /// <param name="outPath">The path to create a new xml file on containing the serialized dbp.</param>
            public static void Serialize(PARAMDBP dbp, string outPath)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                var xws = new XmlWriterSettings();
                xws.Indent = true;
                xws.Encoding = DefaultEncoding;
                var xw = XmlWriter.Create($"{Path.GetDirectoryName(outPath)}\\{Path.GetFileNameWithoutExtension(outPath)}.xml", xws);
                Serialize(dbp, xw, $"{Path.GetFileNameWithoutExtension(outPath)}.dbp");
            }

            /// <summary>
            /// Serialize a dbp to an xml writer.
            /// </summary>
            /// <param name="dbp">A dbp.</param>
            /// <param name="xw">An xml writer.</param>
            /// <param name="dbpname">The name of the dbp if applicable.</param>
            public static void Serialize(PARAMDBP dbp, XmlWriter xw, string dbpname = null)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields were null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp had no fields.");

                xw.WriteStartElement("dbp");
                xw.WriteElementString("BigEndian", dbp.WriteBigEndian.ToString());
                xw.WriteElementString("Name", dbpname ?? "NoName");
                xw.WriteStartElement("Fields");
                foreach (var field in dbp.Fields)
                {
                    xw.WriteStartElement("Field");
                    xw.WriteElementString("DisplayName", field.DisplayName == "" ? "%NULL%" : field.DisplayName);
                    xw.WriteElementString("DisplayFormat", field.DisplayFormat);
                    xw.WriteElementString("DisplayType", field.DisplayType.ToString());
                    xw.WriteElementString("Default", field.Default.ToString());
                    xw.WriteElementString("Minimum", field.Minimum.ToString());
                    xw.WriteElementString("Maximum", field.Maximum.ToString());
                    xw.WriteElementString("Increment", field.Increment.ToString());
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndElement();
                xw.Dispose();
            }

            /// <summary>
            /// Serialize a param that has an applied dbp to an xml file.
            /// </summary>
            /// <param name="param">A param to serialize to txt.</param>
            /// <param name="outPath">The path to create a new txt file on containing the serialized param.</param>
            public static void Serialize(DBPPARAM param, string outPath)
            {
                if (!param.DbpApplied)
                    throw new InvalidDataException("Dbp has not been applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells.");

                var dbp = param.AppliedParamDbp;
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                var xws = new XmlWriterSettings();
                xws.Indent = true;
                xws.Encoding = DefaultEncoding;
                var xw = XmlWriter.Create($"{Path.GetDirectoryName(outPath)}\\{Path.GetFileNameWithoutExtension(outPath)}.xml", xws);
                xw.WriteStartElement("dbpparam");
                xw.WriteElementString("Name", $"{Path.GetFileNameWithoutExtension(outPath)}.bin");
                xw.WriteStartElement("Cells");
                foreach (var cell in param.Cells)
                {
                    xw.WriteStartElement("Cell");
                    xw.WriteElementString("DisplayName", cell.DisplayName == "" ? "%NULL%" : cell.DisplayName);
                    xw.WriteElementString("DisplayFormat", cell.DisplayFormat);
                    xw.WriteElementString("DisplayType", cell.DisplayType.ToString());
                    xw.WriteElementString("Default", cell.Default.ToString());
                    xw.WriteElementString("Minimum", cell.Minimum.ToString());
                    xw.WriteElementString("Maximum", cell.Maximum.ToString());
                    xw.WriteElementString("Increment", cell.Increment.ToString());
                    xw.WriteElementString("Value", cell.Value.ToString());
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();
                xw.WriteEndElement();
                xw.Dispose();
            }

            /// <summary>
            /// Serialize a dbp to a param xml file.
            /// </summary>
            /// <param name="dbp">A dbp to serialize to xml.</param>
            /// <param name="outPath">The path to create a new xml file on containing the serialized data.</param>
            public static void SerializeToParam(PARAMDBP dbp, string outPath)
            {
                if (dbp.Fields == null)
                    throw new InvalidDataException("Dbp fields are null.");
                if (dbp.Fields.Count == 0)
                    throw new InvalidDataException("Dbp has no fields.");
                Serialize(new DBPPARAM(dbp), outPath);
            }

            /// <summary>
            /// Serialize a param that has an applied dbp to a dbp xml file.
            /// </summary>
            /// <param name="param">A param to serialize to xml.</param>
            /// <param name="outPath">The path to create a new xml file on containing the serialized data.</param>
            public static void SerializeToDbp(DBPPARAM param, string outPath)
            {
                if (!param.DbpApplied)
                    throw new InvalidDataException("Dbp has not been applied.");
                if (param.Cells.Count == 0)
                    throw new InvalidDataException("Param has no cells.");
                Serialize(param.AppliedParamDbp, outPath);
            }

            #endregion Serialize

            #region Deserialize

            /// <summary>
            /// Deserialize an xml file from a byte array to a dbp.
            /// </summary>
            /// <param name="bytes">A byte array</param>
            /// <returns>A new dbp.</returns>
            public static PARAMDBP DeserializeDbp(byte[] bytes)
            {
                XmlDocument xml = new XmlDocument();

                using (var ms = new MemoryStream(bytes))
                {
                    xml.Load(ms);
                }

                return DeserializeDbp(xml);
            }

            /// <summary>
            /// Deserialize an xml file from a path to a dbp.
            /// </summary>
            /// <param name="path">A path to an xml file with dbp data.</param>
            /// <returns>A new dbp.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static PARAMDBP DeserializeDbp(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                XmlDocument xml = new XmlDocument();
                xml.Load(path);

                return DeserializeDbp(xml);
            }

            /// <summary>
            /// Deserialize an xml file from an xml document to dbp.
            /// </summary>
            /// <param name="xml">An xml document.</param>
            /// <returns>A new dbp.</returns>
            public static PARAMDBP DeserializeDbp(XmlDocument xml)
            {
                var dbp = new PARAMDBP();
                bool bigendian = bool.Parse(xml.SelectSingleNode("dbp/BigEndian").InnerText);
                string dbpname = xml.SelectSingleNode("dbp/Name").InnerText;

                var fieldsNode = xml.SelectSingleNode("dbp/Fields");
                foreach (XmlNode fieldNode in fieldsNode.SelectNodes("Field"))
                {
                    var field = new Field();
                    string name = fieldNode.SelectSingleNode("DisplayName").InnerText;
                    string format = fieldNode.SelectSingleNode("DisplayFormat").InnerText;
                    var type = (DefType)Enum.Parse(typeof(DefType), fieldNode.SelectSingleNode("DisplayType").InnerText);
                    object @default = Field.ConvertToDisplayType(fieldNode.SelectSingleNode("Default").InnerText, type);
                    object minimum = Field.ConvertToDisplayType(fieldNode.SelectSingleNode("Minimum").InnerText, type);
                    object maximum = Field.ConvertToDisplayType(fieldNode.SelectSingleNode("Maximum").InnerText, type);

                    object increment = Field.ConvertToDisplayType(fieldNode.SelectSingleNode("Increment").InnerText, type);

                    field.DisplayName = name;
                    field.DisplayFormat = format;
                    field.DisplayType = type;
                    field.Default = @default;
                    field.Minimum = minimum;
                    field.Maximum = maximum;
                    field.Increment = increment;
                    dbp.Fields.Add(field);
                }

                dbp.WriteBigEndian = bigendian;

                return dbp;
            }

            /// <summary>
            /// Deserialize an xml file to a param.
            /// </summary>
            /// <param name="path">A path to an xml file with param data.</param>
            /// <returns>A new param.</returns>
            /// <exception cref="InvalidDataException">Something is wrong with the provided data.</exception>
            /// <exception cref="FileNotFoundException">The provided path had no file to deserialize.</exception>
            public static DBPPARAM DeserializeParam(string path)
            {
                if (Directory.Exists(path))
                    throw new InvalidDataException("Path was a directory.");
                if (!File.Exists(path))
                    throw new FileNotFoundException("There must be a file to deserialize.");

                var dbp = new PARAMDBP();
                XmlDocument xml = new XmlDocument();
                xml.Load(path);

                string paramname = xml.SelectSingleNode("dbpparam/Name").InnerText;

                var cellsNode = xml.SelectSingleNode("dbpparam/Cells");
                var paramValues = new List<object>();
                foreach (XmlNode cellNode in cellsNode.SelectNodes("Cell"))
                {
                    var field = new Field();
                    string name = cellNode.SelectSingleNode("DisplayName").InnerText;
                    string format = cellNode.SelectSingleNode("DisplayFormat").InnerText;
                    var type = (DefType)Enum.Parse(typeof(DefType), cellNode.SelectSingleNode("DisplayType").InnerText);
                    object @default = Field.ConvertToDisplayType(cellNode.SelectSingleNode("Default").InnerText, type);
                    object minimum = Field.ConvertToDisplayType(cellNode.SelectSingleNode("Minimum").InnerText, type);
                    object maximum = Field.ConvertToDisplayType(cellNode.SelectSingleNode("Maximum").InnerText, type);
                    object increment = Field.ConvertToDisplayType(cellNode.SelectSingleNode("Increment").InnerText, type);
                    object value = Field.ConvertToDisplayType(cellNode.SelectSingleNode("Value").InnerText, type);

                    field.DisplayName = name;
                    field.DisplayFormat = format;
                    field.DisplayType = type;
                    field.Default = @default;
                    field.Minimum = minimum;
                    field.Maximum = maximum;
                    field.Increment = increment;
                    paramValues.Add(value);

                    dbp.Fields.Add(field);
                }

                var param = new DBPPARAM(dbp);
                for (int i = 0; i < paramValues.Count; i++)
                    param.Cells[i].Value = paramValues[i];

                return param;
            }

            /// <summary>
            /// Deserialize an xml file directly to a new dbp file.
            /// </summary>
            /// <param name="path">A path to an xml file with dbp data.</param>
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
                dbp.Write($"{Path.GetDirectoryName(outPath)}\\{Path.GetFileNameWithoutExtension(outPath)}.dbp");
            }

            /// <summary>
            /// Deserialize an xml file directly to a new param file.
            /// </summary>
            /// <param name="path">A path to an xml file with param data.</param>
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
                    outPath = $"{outPath}\\{Path.GetFileNameWithoutExtension(path)}.bin";

                // Make the outPath directory if it does not exist.
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                // Deserialize and write the DBPPARAM.
                var param = DeserializeParam(path);
                param.Write($"{Path.GetDirectoryName(outPath)}\\{Path.GetFileNameWithoutExtension(outPath)}.bin");
            }

            /// <summary>
            /// Deserialize an xml param to a dbp.
            /// </summary>
            /// <param name="path">A path to an xml file with param data.</param>
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
            /// Deserialize an xml dbp to a param.
            /// </summary>
            /// <param name="path">A path to an xml file with PARAM data.</param>
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
            /// Attempt to detect if an xml file is a serialized param or serialized dbp.
            /// Returns null if neither was detected.
            /// </summary>
            /// <param name="path">A path to an xml file with param or dbp data.</param>
            /// <returns>The detected serialized type or null.</returns>
            public static string DetectSerializedType(string path)
            {
                if (Directory.Exists(path))
                    return null;
                if (!File.Exists(path))
                    return null;

                XmlDocument xml = new XmlDocument();
                xml.Load(path);

                string dbp = xml.SelectSingleNode("dbp").InnerText;
                string param = xml.SelectSingleNode("dbpparam").InnerText;

                if (dbp != null)
                    return "dbp";
                else if (param != null)
                    return "param";

                return null;
            }
        }
    }
}