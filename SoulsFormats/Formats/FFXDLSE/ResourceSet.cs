using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        public class ResourceSet : FXSerializable
        {
            /// <summary>
            /// Unknown; A resource ID list of some kind?
            /// </summary>
            public List<int> Vector1 { get; set; }

            /// <summary>
            /// Unknown; A resource ID list of some kind?
            /// </summary>
            public List<int> Vector2 { get; set; }

            /// <summary>
            /// Unknown; A resource ID list of some kind?
            /// </summary>
            public List<int> Vector3 { get; set; }

            /// <summary>
            /// Unknown; A resource ID list of some kind?
            /// </summary>
            public List<int> Vector4 { get; set; }

            /// <summary>
            /// Unknown; A resource ID list of some kind?
            /// </summary>
            public List<int> Vector5 { get; set; }

            /// <summary>
            /// Create a new <see cref="ResourceSet"/>.
            /// </summary>
            public ResourceSet()
            {
                Vector1 = new List<int>();
                Vector2 = new List<int>();
                Vector3 = new List<int>();
                Vector4 = new List<int>();
                Vector5 = new List<int>();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXResourceSet";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal ResourceSet(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                Vector1 = DLVector.Read(br, classNames);
                Vector2 = DLVector.Read(br, classNames);
                Vector3 = DLVector.Read(br, classNames);
                Vector4 = DLVector.Read(br, classNames);
                Vector5 = DLVector.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                DLVector.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                DLVector.Write(bw, classNames, Vector1);
                DLVector.Write(bw, classNames, Vector2);
                DLVector.Write(bw, classNames, Vector3);
                DLVector.Write(bw, classNames, Vector4);
                DLVector.Write(bw, classNames, Vector5);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(ResourceSet)}(" +
                $"DLVector({{{string.Join(",", Vector1)}}})," +
                $"DLVector({{{string.Join(",", Vector2)}}}), " +
                $"DLVector({{{string.Join(",", Vector3)}}}), " +
                $"DLVector({{{string.Join(",", Vector4)}}}), " +
                $"DLVector({{{string.Join(",", Vector5)}}}))";
        }
    }
}
