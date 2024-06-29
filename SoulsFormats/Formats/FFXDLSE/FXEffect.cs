using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// An effect.
        /// </summary>
        public class FXEffect : FXSerializable
        {
            /// <summary>
            /// The ID assigned to this effect.
            /// </summary>
            [XmlAttribute]
            public int ID { get; set; }

            /// <summary>
            /// A list of parameters.
            /// </summary>
            public ParamList ParamList1 { get; set; }

            /// <summary>
            /// A list of parameters.
            /// </summary>
            public ParamList ParamList2 { get; set; }

            /// <summary>
            /// A state map containing states that can trigger one another based on evaluated triggers.
            /// </summary>
            public StateMap StateMap { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public ResourceSet ResourceSet { get; set; }

            /// <summary>
            /// Create a new <see cref="FXEffect"/>.
            /// </summary>
            public FXEffect()
            {
                ParamList1 = new ParamList();
                ParamList2 = new ParamList();
                StateMap = new StateMap();
                ResourceSet = new ResourceSet();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableEffect";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 5;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal FXEffect(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                br.AssertInt32(0);
                ID = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(2); // MATParam list count?
                br.AssertInt16(0);
                br.AssertInt16(2); // Judging by the order of class names, this must be an always-empty DLVector
                br.AssertInt32(0);

                ParamList1 = new ParamList(br, classNames);
                ParamList2 = new ParamList(br, classNames);

                StateMap = new StateMap(br, classNames);
                ResourceSet = new ResourceSet(br, classNames);
                br.AssertByte(0);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                DLVector.AddClassNames(classNames);

                ParamList1.AddClassNames(classNames);
                ParamList2.AddClassNames(classNames);

                StateMap.AddClassNames(classNames);
                ResourceSet.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(0);
                bw.WriteInt32(ID);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(2);
                bw.WriteInt16(0);
                bw.WriteInt16(2);
                bw.WriteInt32(0);

                ParamList1.Write(bw, classNames);
                ParamList2.Write(bw, classNames);

                StateMap.Write(bw, classNames);
                ResourceSet.Write(bw, classNames);
                bw.WriteByte(0);
            }

            #endregion
        }
    }
}
