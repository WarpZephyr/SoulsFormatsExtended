using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// An action to execute along with any parameters required to do so.
        /// </summary>
        public class Action : FXSerializable
        {
            /// <summary>
            /// The ID of the action to execute.
            /// </summary>
            [XmlAttribute]
            public int ID { get; set; }

            /// <summary>
            /// A list of parameters to pass to the action to be executed.
            /// </summary>
            public ParamList ParamList { get; set; }

            /// <summary>
            /// Create a new <see cref="Action"/>.
            /// </summary>
            public Action()
            {
                ParamList = new ParamList();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableAction";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Action(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                ID = br.ReadInt32();
                ParamList = new ParamList(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                ParamList.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(ID);
                ParamList.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Action)}({ID}, {ParamList})";
        }
    }
}
