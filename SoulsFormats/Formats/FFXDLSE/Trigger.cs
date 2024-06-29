using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A trigger that if activated by an evaluator, can move the state machine to another state.
        /// </summary>
        public class Trigger : FXSerializable
        {
            /// <summary>
            /// The <see cref="State"/> to jump to in the <see cref="StateMap"/> if triggered.
            /// </summary>
            [XmlAttribute]
            public int StateIndex { get; set; }

            /// <summary>
            /// Evaluates conditions that may activate the <see cref="Trigger"/>.
            /// </summary>
            public Evaluatable Evaluator { get; set; }

            /// <summary>
            /// Create a new <see cref="Trigger"/>.
            /// </summary>
            public Trigger() { }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableTrigger";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Trigger(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                StateIndex = br.ReadInt32();
                Evaluator = Evaluatable.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Evaluator.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(StateIndex);
                Evaluator.Write(bw, classNames);
            }

            #endregion
        }
    }
}
