using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A list of parameters.
        /// </summary>
        public class ParamList : FXSerializable, IXmlSerializable
        {
            /// <summary>
            /// Unknown; Usually the same as the number of parameters, but not always.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// The parameters in the list.
            /// </summary>
            public List<Param> Params { get; set; }

            /// <summary>
            /// Create a new <see cref="ParamList"/>.
            /// </summary>
            public ParamList()
            {
                Params = new List<Param>();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableParamList";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 2;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal ParamList(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                int paramCount = br.ReadInt32();
                Unk04 = br.ReadInt32();
                Params = new List<Param>(paramCount);
                for (int i = 0; i < paramCount; i++)
                {
                    Params.Add(Param.Read(br, classNames));
                }
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (Param param in Params)
                {
                    param.AddClassNames(classNames);
                }
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(Params.Count);
                bw.WriteInt32(Unk04);
                foreach (Param param in Params)
                {
                    param.Write(bw, classNames);
                }
            }

            #endregion

            #region IXmlSerializable

            XmlSchema IXmlSerializable.GetSchema() => null;

            void IXmlSerializable.ReadXml(XmlReader reader)
            {
                reader.MoveToContent();
                bool empty = reader.IsEmptyElement;
                Unk04 = int.Parse(reader.GetAttribute(nameof(Unk04)));
                reader.ReadStartElement();

                if (!empty)
                {
                    while (reader.IsStartElement(nameof(Param)))
                        Params.Add((Param)ParamSerializer.Deserialize(reader));
                    reader.ReadEndElement();
                }
            }

            void IXmlSerializable.WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString(nameof(Unk04), Unk04.ToString());
                for (int i = 0; i < Params.Count; i++)
                {
                    //writer.WriteComment($" {i} ");
                    ParamSerializer.Serialize(writer, Params[i]);
                }
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(ParamList)}({Unk04}, {{{string.Join(",", Params)}}})";
        }
    }
}
