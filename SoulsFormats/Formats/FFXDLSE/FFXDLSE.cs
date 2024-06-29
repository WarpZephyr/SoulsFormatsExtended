using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SoulsFormats
{
    /// <summary>
    /// An SFX configuration format used in DeS, ACV, ACVD, and DS2; DeS is not supported. Extension: .ffx
    /// </summary>
    public partial class FFXDLSE : SoulsFile<FFXDLSE>
    {
        /// <summary>
        /// The effect in the <see cref="FFXDLSE"/>.
        /// </summary>
        public FXEffect Effect { get; set; }

        /// <summary>
        /// Create a new <see cref="FFXDLSE"/>.
        /// </summary>
        public FFXDLSE()
        {
            Effect = new FXEffect();
        }

        #region SoulsFile

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            return br.Length >= 4 && br.GetASCII(0, 4) == "DLsE";
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;
            br.AssertASCII("DLsE");
            br.AssertByte(1);
            br.AssertByte(3);
            br.AssertByte(0);
            br.AssertByte(0);
            br.AssertInt32(0);
            br.AssertInt32(0);
            br.AssertByte(0);
            br.AssertInt32(1);
            short classNameCount = br.ReadInt16();

            var classNames = new List<string>(classNameCount);
            for (int i = 0; i < classNameCount; i++)
            {
                int length = br.ReadInt32();
                classNames.Add(br.ReadASCII(length));
            }

            Effect = new FXEffect(br, classNames);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            var classNames = new List<string>();
            Effect.AddClassNames(classNames);

            bw.BigEndian = false;
            bw.WriteASCII("DLsE");
            bw.WriteByte(1);
            bw.WriteByte(3);
            bw.WriteByte(0);
            bw.WriteByte(0);
            bw.WriteInt32(0);
            bw.WriteInt32(0);
            bw.WriteByte(0);
            bw.WriteInt32(1);
            bw.WriteInt16((short)classNames.Count);

            foreach (string className in classNames)
            {
                bw.WriteInt32(className.Length);
                bw.WriteASCII(className);
            }

            Effect.Write(bw, classNames);
        }

        #endregion

        #region XML Serialization

        /// <summary>
        /// An <see cref="XmlSerializer"/> for FFX.
        /// </summary>
        private static XmlSerializer _ffxSerializer;

        /// <summary>
        /// An <see cref="XmlSerializer"/> for states.
        /// </summary>
        private static XmlSerializer _stateSerializer;

        /// <summary>
        /// An <see cref="XmlSerializer"/> for parameters.
        /// </summary>
        private static XmlSerializer _paramSerializer;

        /// <summary>
        /// An <see cref="XmlSerializer"/> for FFX.
        /// </summary>
        private static XmlSerializer FFXSerializer => _ffxSerializer ?? MakeSerializers(0);

        /// <summary>
        /// An <see cref="XmlSerializer"/> for states.
        /// </summary>
        private static XmlSerializer StateSerializer => _stateSerializer ?? MakeSerializers(1);

        /// <summary>
        /// An <see cref="XmlSerializer"/> for parameters.
        /// </summary>
        private static XmlSerializer ParamSerializer => _paramSerializer ?? MakeSerializers(2);

        /// <summary>
        /// Make an <see cref="XmlSerializer"/> if it doesn't already exist.
        /// </summary>
        /// <param name="returnIndex">The index of which type to use for the serializer.</param>
        /// <returns>An <see cref="XmlSerializer"/>.</returns>
        private static XmlSerializer MakeSerializers(int returnIndex)
        {
            XmlSerializer[] serializers = XmlSerializer.FromTypes(
                new Type[] { typeof(FFXDLSE), typeof(State), typeof(Param) });

            _ffxSerializer = serializers[0];
            _stateSerializer = serializers[1];
            _paramSerializer = serializers[2];
            return serializers[returnIndex];
        }

        /// <summary>
        /// Deserialize an <see cref="FFXDLSE"/> xml.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to deserialize from.</param>
        /// <returns>A <see cref="FFXDLSE"/>.</returns>
        public static FFXDLSE XmlDeserialize(Stream stream)
            => (FFXDLSE)FFXSerializer.Deserialize(stream);

        /// <summary>
        /// Deserialize an <see cref="FFXDLSE"/> xml.
        /// </summary>
        /// <param name="textReader">The <see cref="TextReader"/> to deserialize from.</param>
        /// <returns>A <see cref="FFXDLSE"/>.</returns>
        public static FFXDLSE XmlDeserialize(TextReader textReader)
            => (FFXDLSE)FFXSerializer.Deserialize(textReader);

        /// <summary>
        /// Deserialize an <see cref="FFXDLSE"/> xml.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> to deserialize from.</param>
        /// <returns>A <see cref="FFXDLSE"/>.</returns>
        public static FFXDLSE XmlDeserialize(XmlReader xmlReader)
            => (FFXDLSE)FFXSerializer.Deserialize(xmlReader);

        /// <summary>
        /// Serialize an <see cref="FFXDLSE"/> to xml.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        public void XmlSerialize(Stream stream)
            => FFXSerializer.Serialize(stream, this);

        /// <summary>
        /// Serialize an <see cref="FFXDLSE"/> to xml.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to serialize to.</param>
        public void XmlSerialize(TextWriter textWriter)
            => FFXSerializer.Serialize(textWriter, this);

        /// <summary>
        /// Serialize an <see cref="FFXDLSE"/> to xml.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to serialize to.</param>
        public void XmlSerialize(XmlWriter xmlWriter)
            => FFXSerializer.Serialize(xmlWriter, this);

        #endregion

        #region DLVector

        /// <summary>
        /// A simplified dantelion array.
        /// </summary>
        private static class DLVector
        {
            /// <summary>
            /// Read a <see cref="DLVector"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the class name to.</param>
            /// <returns>A list of <see cref="int"/>.</returns>
            public static List<int> Read(BinaryReaderEx br, List<string> classNames)
            {
                br.AssertInt16((short)(classNames.IndexOf("DLVector") + 1));
                int count = br.ReadInt32();
                return new List<int>(br.ReadInt32s(count));
            }

            /// <summary>
            /// Adds class names if they weren't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            public static void AddClassNames(List<string> classNames)
            {
                if (!classNames.Contains("DLVector"))
                {
                    classNames.Add("DLVector");
                }
            }

            /// <summary>
            /// Writes this <see cref="DLVector"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of classes names to get a type number.</param>
            /// <param name="vector">An array of <see cref="int"/>.</param>
            public static void Write(BinaryWriterEx bw, List<string> classNames, List<int> vector)
            {
                bw.WriteInt16((short)(classNames.IndexOf("DLVector") + 1));
                bw.WriteInt32(vector.Count);
                bw.WriteInt32s(vector);
            }
        }

        #endregion

        #region FXSerializable

        /// <summary>
        /// An object that is serializable.
        /// </summary>
        public abstract class FXSerializable
        {
            /// <summary>
            /// The name of the object type when serialized.
            /// </summary>
            internal abstract string ClassName { get; }

            /// <summary>
            /// The version of the object type when serialized.
            /// </summary>
            internal abstract int Version { get; }

            /// <summary>
            /// Create a new <see cref="FXSerializable"/>.
            /// </summary>
            internal FXSerializable() { }

            /// <summary>
            /// Read a <see cref="FXSerializable"/> type from a <see cref="Stream"/>.
            /// </summary>
            /// /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the <see cref="ClassName"/> to.</param>
            internal FXSerializable(BinaryReaderEx br, List<string> classNames)
            {
                long start = br.Position;
                //br.AssertInt16((short)(classNames.IndexOf(ClassName) + 1));
                br.ReadInt16();
                br.AssertInt32(Version);
                int length = br.ReadInt32();
                Deserialize(br, classNames);
                if (br.Position != start + length)
                    throw new InvalidDataException("Failed to read all object data (or read too much of it).");
            }

            /// <summary>
            /// Deserialize a <see cref="FXSerializable"/> type from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the <see cref="ClassName"/> to.</param>
            protected internal abstract void Deserialize(BinaryReaderEx br, List<string> classNames);

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal virtual void AddClassNames(List<string> classNames)
            {
                if (!classNames.Contains(ClassName))
                {
                    classNames.Add(ClassName);
                }
            }

            /// <summary>
            /// Write this as an <see cref="FXSerializable"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get a type number.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                long start = bw.Position;
                bw.WriteInt16((short)(classNames.IndexOf(ClassName) + 1));
                bw.WriteInt32(Version);
                bw.ReserveInt32($"{start:X}Length");
                Serialize(bw, classNames);
                bw.FillInt32($"{start:X}Length", (int)(bw.Position - start));
            }

            /// <summary>
            /// Serialize this as an <see cref="FXSerializable"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of classes names to get a type number.</param>
            protected internal abstract void Serialize(BinaryWriterEx bw, List<string> classNames);
        }

        #endregion
    }
}
