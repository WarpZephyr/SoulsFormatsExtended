using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A state map containing states that can trigger one another based on evaluated triggers.
        /// </summary>
        public class StateMap : FXSerializable, IXmlSerializable
        {
            /// <summary>
            /// A list of states in the <see cref="StateMap"/>.
            /// </summary>
            public List<State> States { get; set; }

            /// <summary>
            /// Create a new <see cref="StateMap"/>.
            /// </summary>
            public StateMap()
            {
                States = new List<State>();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableStateMap";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal StateMap(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                int stateCount = br.ReadInt32();
                States = new List<State>(stateCount);
                for (int i = 0; i < stateCount; i++)
                    States.Add(new State(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (State state in States)
                    state.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(States.Count);
                foreach (State state in States)
                    state.Write(bw, classNames);
            }

            #endregion

            #region IXmlSerializable

            XmlSchema IXmlSerializable.GetSchema() => null;

            void IXmlSerializable.ReadXml(XmlReader reader)
            {
                reader.MoveToContent();
                bool empty = reader.IsEmptyElement;
                reader.ReadStartElement();

                if (!empty)
                {
                    while (reader.IsStartElement(nameof(State)))
                        States.Add((State)StateSerializer.Deserialize(reader));
                    reader.ReadEndElement();
                }
            }

            void IXmlSerializable.WriteXml(XmlWriter writer)
            {
                for (int i = 0; i < States.Count; i++)
                {
                    writer.WriteComment($" State {i} ");
                    StateSerializer.Serialize(writer, States[i]);
                }
            }

            #endregion
        }
    }
}
