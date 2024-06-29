using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A signed 32-bit value.
        /// </summary>
        public class PrimitiveInt : FXSerializable
        {
            /// <summary>
            /// The value of this <see cref="PrimitiveInt"/>.
            /// </summary>
            [XmlAttribute]
            public int Value { get; set; }

            /// <summary>
            /// Create a new <see cref="PrimitiveInt"/>.
            /// </summary>
            public PrimitiveInt() { }

            /// <summary>
            /// Create a new <see cref="PrimitiveInt"/> with a value.
            /// </summary>
            /// <param name="value">The value of the <see cref="PrimitiveInt"/>.</param>
            public PrimitiveInt(int value)
            {
                Value = value;
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializablePrimitive<dl_int32>";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal PrimitiveInt(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                Value = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(Value);
            }

            internal static int Read(BinaryReaderEx br, List<string> classNames)
                => new PrimitiveInt(br, classNames).Value;

            internal static void AddClassName(List<string> classNames)
                => new PrimitiveInt().AddClassNames(classNames);

            internal static void Write(BinaryWriterEx bw, List<string> classNames, int value)
                => new PrimitiveInt(value).Write(bw, classNames);

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(PrimitiveInt)}({Value})";
        }

        /// <summary>
        /// A signed 32-bit floating-point decimal value.
        /// </summary>
        public class PrimitiveFloat : FXSerializable
        {
            /// <summary>
            /// The value of this <see cref="PrimitiveFloat"/>.
            /// </summary>
            [XmlAttribute]
            public float Value { get; set; }

            /// <summary>
            /// Create a new <see cref="PrimitiveFloat"/>.
            /// </summary>
            public PrimitiveFloat() { }

            /// <summary>
            /// Create a new <see cref="PrimitiveFloat"/> with a value.
            /// </summary>
            /// <param name="value">The value of the <see cref="PrimitiveFloat"/>.</param>
            public PrimitiveFloat(float value)
            {
                Value = value;
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializablePrimitive<dl_float32>";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal PrimitiveFloat(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                Value = br.ReadSingle();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteSingle(Value);
            }

            internal static float Read(BinaryReaderEx br, List<string> classNames)
                => new PrimitiveFloat(br, classNames).Value;

            internal static void AddClassName(List<string> classNames)
                => new PrimitiveFloat().AddClassNames(classNames);

            internal static void Write(BinaryWriterEx bw, List<string> classNames, float value)
                => new PrimitiveFloat(value).Write(bw, classNames);

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(PrimitiveFloat)}({Value})";
        }

        /// <summary>
        /// A primitive value representing a tick.
        /// </summary>
        public class PrimitiveTick : FXSerializable
        {
            /// <summary>
            /// The value of this <see cref="PrimitiveTick"/>.
            /// </summary>
            [XmlAttribute]
            public float Value { get; set; }

            /// <summary>
            /// Create a new <see cref="PrimitiveTick"/>.
            /// </summary>
            public PrimitiveTick() { }

            /// <summary>
            /// Create a new <see cref="PrimitiveTick"/> with a value.
            /// </summary>
            /// <param name="value">The value of the <see cref="PrimitiveTick"/>.</param>
            public PrimitiveTick(float value)
            {
                Value = value;
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializablePrimitive<FXTick>";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal PrimitiveTick(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                Value = br.ReadSingle();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteSingle(Value);
            }

            internal static float Read(BinaryReaderEx br, List<string> classNames)
                => new PrimitiveTick(br, classNames).Value;

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal static void AddClassName(List<string> classNames)
                => new PrimitiveTick().AddClassNames(classNames);

            /// <summary>
            /// Serialize this as an <see cref="FXSerializable"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of classes names to get a type number.</param>
            /// <param name="value">The value to serialize.</param>
            internal static void Write(BinaryWriterEx bw, List<string> classNames, float value)
                => new PrimitiveTick(value).Write(bw, classNames);

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(PrimitiveTick)}({Value})";
        }

        /// <summary>
        /// A primitive value representing a color.
        /// </summary>
        public class PrimitiveColor : FXSerializable
        {
            /// <summary>
            /// Red.
            /// </summary>
            public float R { get; set; }

            /// <summary>
            /// Green.
            /// </summary>
            public float G { get; set; }

            /// <summary>
            /// Blue.
            /// </summary>
            public float B { get; set; }

            /// <summary>
            /// Alpha.
            /// </summary>
            public float A { get; set; }

            /// <summary>
            /// Create a new <see cref="PrimitiveColor"/>.
            /// </summary>
            public PrimitiveColor() { }

            /// <summary>
            /// Create a new <see cref="PrimitiveColor"/> with the given values.
            /// </summary>
            /// <param name="r">The red value.</param>
            /// <param name="g">The green value.</param>
            /// <param name="b">The blue value.</param>
            /// <param name="a">The alpha value.</param>
            public PrimitiveColor(float r, float g, float b, float a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializablePrimitive<FXColorRGBA>";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal PrimitiveColor(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                R = br.ReadSingle();
                G = br.ReadSingle();
                B = br.ReadSingle();
                A = br.ReadSingle();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteSingle(R);
                bw.WriteSingle(G);
                bw.WriteSingle(B);
                bw.WriteSingle(A);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(PrimitiveColor)}({R},{G},{B},{A})";
        }
    }
}
