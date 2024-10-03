using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A parameter that holds a value or a reference to something.
        /// </summary>
        #region XmlInclude
        [
            XmlInclude(typeof(Param1)),
            XmlInclude(typeof(Param2)),
            XmlInclude(typeof(Param5)),
            XmlInclude(typeof(Param6)),
            XmlInclude(typeof(Param7)),
            XmlInclude(typeof(Param9)),
            XmlInclude(typeof(Param11)),
            XmlInclude(typeof(Param12)),
            XmlInclude(typeof(Param13)),
            XmlInclude(typeof(Param15)),
            XmlInclude(typeof(Param17)),
            XmlInclude(typeof(Param18)),
            XmlInclude(typeof(Param19)),
            XmlInclude(typeof(Param20)),
            XmlInclude(typeof(Param21)),
            XmlInclude(typeof(Param37)),
            XmlInclude(typeof(Param38)),
            XmlInclude(typeof(Param40)),
            XmlInclude(typeof(Param41)),
            XmlInclude(typeof(Param44)),
            XmlInclude(typeof(Param45)),
            XmlInclude(typeof(Param46)),
            XmlInclude(typeof(Param47)),
            XmlInclude(typeof(Param59)),
            XmlInclude(typeof(Param60)),
            XmlInclude(typeof(Param66)),
            XmlInclude(typeof(Param68)),
            XmlInclude(typeof(Param69)),
            XmlInclude(typeof(Param70)),
            XmlInclude(typeof(Param71)),
            XmlInclude(typeof(Param79)),
            XmlInclude(typeof(Param81)),
            XmlInclude(typeof(Param82)),
            XmlInclude(typeof(Param83)),
            XmlInclude(typeof(Param84)),
            XmlInclude(typeof(Param85)),
            XmlInclude(typeof(Param87)),
            ]
        #endregion
        public abstract class Param : FXSerializable
        {
            /// <summary>
            /// The type value identifying what kind of <see cref="Param"/> this is.
            /// </summary>
            internal abstract int Type { get; }

            /// <summary>
            /// Create a new <see cref="Param"/>.
            /// </summary>
            public Param() { }

            /// <summary>
            /// Read a <see cref="Param"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the <see cref="ClassName"/> to.</param>
            /// <returns>A <see cref="Param"/>.</returns>
            /// <exception cref="NotImplementedException">The read <see cref="Type"/> is not implemented.</exception>
            internal static Param Read(BinaryReaderEx br, List<string> classNames)
            {
                // Don't @ me.
                int type = br.GetInt32(br.Position + 0xA);
                switch (type)
                {
                    case 1: return new Param1(br, classNames);
                    case 2: return new Param2(br, classNames);
                    case 3: return new Param3(br, classNames);
                    case 5: return new Param5(br, classNames);
                    case 6: return new Param6(br, classNames);
                    case 7: return new Param7(br, classNames);
                    case 9: return new Param9(br, classNames);
                    case 11: return new Param11(br, classNames);
                    case 12: return new Param12(br, classNames);
                    case 13: return new Param13(br, classNames);
                    case 15: return new Param15(br, classNames);
                    case 17: return new Param17(br, classNames);
                    case 18: return new Param18(br, classNames);
                    case 19: return new Param19(br, classNames);
                    case 20: return new Param20(br, classNames);
                    case 21: return new Param21(br, classNames);
                    case 37: return new Param37(br, classNames);
                    case 38: return new Param38(br, classNames);
                    case 40: return new Param40(br, classNames);
                    case 41: return new Param41(br, classNames);
                    case 44: return new Param44(br, classNames);
                    case 45: return new Param45(br, classNames);
                    case 46: return new Param46(br, classNames);
                    case 47: return new Param47(br, classNames);
                    case 59: return new Param59(br, classNames);
                    case 60: return new Param60(br, classNames);
                    case 66: return new Param66(br, classNames);
                    case 68: return new Param68(br, classNames);
                    case 69: return new Param69(br, classNames);
                    case 70: return new Param70(br, classNames);
                    case 71: return new Param71(br, classNames);
                    case 79: return new Param79(br, classNames);
                    case 81: return new Param81(br, classNames);
                    case 82: return new Param82(br, classNames);
                    case 83: return new Param83(br, classNames);
                    case 84: return new Param84(br, classNames);
                    case 85: return new Param85(br, classNames);
                    case 87: return new Param87(br, classNames);

                    default:
                        throw new NotImplementedException($"Unimplemented param type: {type}");
                }
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableParam";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 2;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                br.AssertInt32(Type);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(Type);
            }

            #endregion
        }

        #region Param Types

        /// <summary>
        /// A parameter that holds an <see cref="int"/>.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param1 : Param
        {
            /// <summary>
            /// An <see cref="int"/> value.
            /// </summary>
            [XmlAttribute]
            public int Int { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param1() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param1(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Int = PrimitiveInt.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveInt.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveInt.Write(bw, classNames, Int);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param1)}({Int})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="int"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param2 : Param
        {
            /// <summary>
            /// A list of <see cref="int"/> values.
            /// </summary>
            public List<int> Ints { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param2()
            {
                Ints = new List<int>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 2;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param2(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                Ints = new List<int>(count);
                for (int i = 0; i < count; i++)
                    Ints.Add(PrimitiveInt.Read(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveInt.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Ints.Count);
                foreach (int value in Ints)
                    PrimitiveInt.Write(bw, classNames, value);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param2)}({{{string.Join(",", Ints)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickInt"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param3 : Param
        {
            /// <summary>
            /// A list of <see cref="TickInt"/> values.
            /// </summary>
            public List<TickInt> TickInts { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param3()
            {
                TickInts = new List<TickInt>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param3(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickInts = new List<TickInt>(count);
                for (int i = 0; i < count; i++)
                    TickInts.Add(new TickInt(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickInt tickInt in TickInts)
                    tickInt.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickInts.Count);
                foreach (TickInt tickInt in TickInts)
                    tickInt.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param3)}({{{string.Join(",", TickInts)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickInt"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param5 : Param
        {
            /// <summary>
            /// A list of <see cref="TickInt"/> values.
            /// </summary>
            public List<TickInt> TickInts { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param5()
            {
                TickInts = new List<TickInt>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 5;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param5(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickInts = new List<TickInt>(count);
                for (int i = 0; i < count; i++)
                    TickInts.Add(new TickInt(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickInt tickInt in TickInts)
                    tickInt.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickInts.Count);
                foreach (TickInt tickInt in TickInts)
                    tickInt.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param5)}({{{string.Join(",", TickInts)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickInt"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param6 : Param
        {
            /// <summary>
            /// A list of <see cref="TickInt"/> values.
            /// </summary>
            public List<TickInt> TickInts { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param6()
            {
                TickInts = new List<TickInt>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 6;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param6(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickInts = new List<TickInt>(count);
                for (int i = 0; i < count; i++)
                    TickInts.Add(new TickInt(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickInt tickInt in TickInts)
                    tickInt.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickInts.Count);
                foreach (TickInt tickInt in TickInts)
                    tickInt.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param6)}({{{string.Join(",", TickInts)}}})";
        }

        /// <summary>
        /// A parameter that holds a <see cref="float"/>.<br/>
        /// Purpose unknown. Might be texture scale or light flicker?
        /// </summary>
        public class Param7 : Param
        {
            /// <summary>
            /// A <see cref="float"/>.
            /// </summary>
            [XmlAttribute]
            public float Float { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param7() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 7;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param7(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Float = PrimitiveFloat.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveFloat.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveFloat.Write(bw, classNames, Float);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param7)}({Float})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickFloat"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param9 : Param
        {
            /// <summary>
            /// A list of <see cref="TickFloat"/> values.
            /// </summary>
            public List<TickFloat> TickFloats { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param9()
            {
                TickFloats = new List<TickFloat>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 9;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param9(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickFloats = new List<TickFloat>(count);
                for (int i = 0; i < count; i++)
                    TickFloats.Add(new TickFloat(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickFloats.Count);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param9)}({{{string.Join(",", TickFloats)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickFloat"/> values.<br/>
        /// Might be used for size.
        /// </summary>
        public class Param11 : Param
        {
            /// <summary>
            /// A list of <see cref="TickFloat"/> values.
            /// </summary>
            public List<TickFloat> TickFloats { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param11()
            {
                TickFloats = new List<TickFloat>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 11;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param11(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickFloats = new List<TickFloat>(count);
                for (int i = 0; i < count; i++)
                    TickFloats.Add(new TickFloat(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickFloats.Count);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param11)}({{{string.Join(",", TickFloats)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickFloat"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param12 : Param
        {
            /// <summary>
            /// A list of <see cref="TickFloat"/> values.
            /// </summary>
            public List<TickFloat> TickFloats { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param12()
            {
                TickFloats = new List<TickFloat>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 12;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param12(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickFloats = new List<TickFloat>(count);
                for (int i = 0; i < count; i++)
                    TickFloats.Add(new TickFloat(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickFloats.Count);
                foreach (TickFloat tickFloat in TickFloats)
                    tickFloat.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param12)}({{{string.Join(",", TickFloats)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickFloat3"/> values.<br/>
        /// Might be used for movement.
        /// </summary>
        public class Param13 : Param
        {
            /// <summary>
            /// A list of <see cref="TickFloat3"/> values.
            /// </summary>
            public List<TickFloat3> TickFloat3s { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param13()
            {
                TickFloat3s = new List<TickFloat3>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 13;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param13(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickFloat3s = new List<TickFloat3>(count);
                for (int i = 0; i < count; i++)
                    TickFloat3s.Add(new TickFloat3(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickFloat3 tickFloat3 in TickFloat3s)
                    tickFloat3.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickFloat3s.Count);
                foreach (TickFloat3 tickFloat3 in TickFloat3s)
                    tickFloat3.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param13)}({{{string.Join(",", TickFloat3s)}}})";
        }

        /// <summary>
        /// A parameter that holds a <see cref="PrimitiveColor"/> value.<br/>
        /// Might be used as a texture color.
        /// </summary>
        public class Param15 : Param
        {
            /// <summary>
            /// A <see cref="PrimitiveColor"/>.
            /// </summary>
            public PrimitiveColor Color { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param15()
            {
                Color = new PrimitiveColor();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 15;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param15(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Color = new PrimitiveColor(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Color.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Color.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param15)}({Color})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickColor"/> values.<br/>
        /// Might be used for texture colors.
        /// </summary>
        public class Param17 : Param
        {
            /// <summary>
            /// A list of <see cref="TickColor"/> values.
            /// </summary>
            public List<TickColor> TickColors { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param17()
            {
                TickColors = new List<TickColor>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 17;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param17(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickColors = new List<TickColor>(count);
                for (int i = 0; i < count; i++)
                    TickColors.Add(new TickColor(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickColor tickColor in TickColors)
                    tickColor.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickColors.Count);
                foreach (TickColor tickColor in TickColors)
                    tickColor.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param17)}({{{string.Join(",", TickColors)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickColor"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param18 : Param
        {
            /// <summary>
            /// A list of <see cref="TickColor"/> values.
            /// </summary>
            public List<TickColor> TickColors { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param18()
            {
                TickColors = new List<TickColor>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 18;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param18(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickColors = new List<TickColor>(count);
                for (int i = 0; i < count; i++)
                    TickColors.Add(new TickColor(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickColor tickColor in TickColors)
                    tickColor.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickColors.Count);
                foreach (TickColor tickColor in TickColors)
                    tickColor.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param18)}({{{string.Join(",", TickColors)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickColor"/> values.<br/>
        /// Might be used as a light color.
        /// </summary>
        public class Param19 : Param
        {
            /// <summary>
            /// A list of <see cref="TickColor"/> values.
            /// </summary>
            public List<TickColor> TickColors { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param19()
            {
                TickColors = new List<TickColor>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 19;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param19(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickColors = new List<TickColor>(count);
                for (int i = 0; i < count; i++)
                    TickColors.Add(new TickColor(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickColor tickColor in TickColors)
                    tickColor.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickColors.Count);
                foreach (TickColor tickColor in TickColors)
                    tickColor.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param19)}({{{string.Join(",", TickColors)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickColor"/> values.<br/>
        /// Might be used for texture colors.
        /// </summary>
        public class Param20 : Param
        {
            /// <summary>
            /// A list of <see cref="TickColor"/> values.
            /// </summary>
            public List<TickColor> TickColors { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param20()
            {
                TickColors = new List<TickColor>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 20;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param20(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickColors = new List<TickColor>(count);
                for (int i = 0; i < count; i++)
                    TickColors.Add(new TickColor(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickColor tickColor in TickColors)
                    tickColor.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickColors.Count);
                foreach (TickColor tickColor in TickColors)
                    tickColor.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param20)}({{{string.Join(",", TickColors)}}})";
        }

        /// <summary>
        /// A parameter that holds an array of <see cref="TickColor3"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param21 : Param
        {
            /// <summary>
            /// A list of <see cref="TickColor3"/> values.
            /// </summary>
            public List<TickColor3> TickColor3s { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param21()
            {
                TickColor3s = new List<TickColor3>();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 21;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param21(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                int count = br.ReadInt32();
                TickColor3s = new List<TickColor3>(count);
                for (int i = 0; i < count; i++)
                    TickColor3s.Add(new TickColor3(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (TickColor3 tickColor3 in TickColor3s)
                    tickColor3.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TickColor3s.Count);
                foreach (TickColor3 tickColor3 in TickColor3s)
                    tickColor3.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param21)}({{{string.Join(",", TickColor3s)}}})";
        }

        /// <summary>
        /// A parameter that holds an effect ID and a list of parameters.<br/>
        /// Used to call effects.
        /// </summary>
        public class Param37 : Param
        {
            /// <summary>
            /// The ID of the effect to execute.
            /// </summary>
            [XmlAttribute]
            public int EffectID { get; set; }

            /// <summary>
            /// A parameter list.
            /// </summary>
            public ParamList ParamList { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param37()
            {
                ParamList = new ParamList();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 37;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param37(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                EffectID = br.ReadInt32();
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
                base.Serialize(bw, classNames);
                bw.WriteInt32(EffectID);
                ParamList.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param37)}({EffectID}, {ParamList})";
        }

        /// <summary>
        /// A parameter that holds an action ID and a list of parameters.<br/>
        /// Used to call an action.
        /// </summary>
        public class Param38 : Param
        {
            /// <summary>
            /// The ID of the action to execute.
            /// </summary>
            [XmlAttribute]
            public int ActionID { get; set; }

            /// <summary>
            /// A parameter list.<br/>
            /// Presumably to pass to the action.
            /// </summary>
            public ParamList ParamList { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param38()
            {
                ParamList = new ParamList();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 38;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param38(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                ActionID = br.ReadInt32();
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
                base.Serialize(bw, classNames);
                bw.WriteInt32(ActionID);
                ParamList.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param38)}({ActionID}, {ParamList})";
        }

        /// <summary>
        /// A parameter that holds a texture ID.<br/>
        /// Used to reference a texture.
        /// </summary>
        public class Param40 : Param
        {
            /// <summary>
            /// The ID of a texture.
            /// </summary>
            [XmlAttribute]
            public int TextureID { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param40() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 40;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param40(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                TextureID = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(TextureID);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param40)}({TextureID})";
        }

        /// <summary>
        /// A parameter that holds a value of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param41 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param41() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 41;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param41(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param41)}({Unk04})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param44 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param44() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 44;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param44(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param44)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param45 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param45() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 45;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param45(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param45)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param46 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param46() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 46;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param46(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param46)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param47 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param47() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 47;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param47(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param47)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param59 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param59() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 59;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param59(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param59)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param60 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param60() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 60;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param60(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param60)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param66 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param66() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 66;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param66(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param66)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds a sound ID.<br/>
        /// Used to reference a sound.
        /// </summary>
        public class Param68 : Param
        {
            /// <summary>
            /// The ID of a sound.
            /// </summary>
            [XmlAttribute]
            public int SoundID { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param68() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 68;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param68(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                SoundID = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(SoundID);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param68)}({SoundID})";
        }

        /// <summary>
        /// A parameter that holds a value of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param69 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param69() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 69;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param69(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param69)}({Unk04})";
        }

        /// <summary>
        /// A parameter that holds a tick value.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param70 : Param
        {
            /// <summary>
            /// A tick value.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param70() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 70;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param70(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Tick = PrimitiveTick.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveTick.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveTick.Write(bw, classNames, Tick);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param70)}({Tick})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param71 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param71() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 71;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param71(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param71)}({Unk04}, {ArgIndex})";
        }

        /// <summary>
        /// A parameter that holds two <see cref="int"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param79 : Param
        {
            /// <summary>
            /// An <see cref="int"/>.
            /// </summary>
            [XmlAttribute]
            public int Int1 { get; set; }

            /// <summary>
            /// An <see cref="int"/>.
            /// </summary>
            [XmlAttribute]
            public int Int2 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param79() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 79;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param79(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Int1 = PrimitiveInt.Read(br, classNames);
                Int2 = PrimitiveInt.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveInt.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveInt.Write(bw, classNames, Int1);
                PrimitiveInt.Write(bw, classNames, Int2);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param79)}({Int1}, {Int2})";
        }

        /// <summary>
        /// A parameter that holds two <see cref="float"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param81 : Param
        {
            /// <summary>
            /// A <see cref="float"/>.
            /// </summary>
            [XmlAttribute]
            public float Float1 { get; set; }

            /// <summary>
            /// A <see cref="float"/>.
            /// </summary>
            [XmlAttribute]
            public float Float2 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param81() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 81;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param81(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Float1 = PrimitiveFloat.Read(br, classNames);
                Float2 = PrimitiveFloat.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveFloat.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveFloat.Write(bw, classNames, Float1);
                PrimitiveFloat.Write(bw, classNames, Float2);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param81)}({Float1}, {Float2})";
        }

        /// <summary>
        /// A parameter that holds a parameter and a float.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param82 : Param
        {
            /// <summary>
            /// A parameter.
            /// </summary>
            public Param Param { get; set; }

            /// <summary>
            /// A <see cref="float"/> value.
            /// </summary>
            public float Float { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param82()
            {
                Param = new Param1();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 82;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param82(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Param = Param.Read(br, classNames);
                Float = PrimitiveFloat.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Param.AddClassNames(classNames);
                PrimitiveFloat.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Param.Write(bw, classNames);
                PrimitiveFloat.Write(bw, classNames, Float);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param82)}({Param}, {Float})";
        }

        /// <summary>
        /// A parameter that holds two <see cref="PrimitiveColor"/> values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param83 : Param
        {
            /// <summary>
            /// A <see cref="PrimitiveColor"/>.
            /// </summary>
            public PrimitiveColor Color1 { get; set; }

            /// <summary>
            /// A <see cref="PrimitiveColor"/>.
            /// </summary>
            public PrimitiveColor Color2 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param83()
            {
                Color1 = new PrimitiveColor();
                Color2 = new PrimitiveColor();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 83;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param83(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Color1 = new PrimitiveColor(br, classNames);
                Color2 = new PrimitiveColor(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Color1.AddClassNames(classNames);
                Color2.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Color1.Write(bw, classNames);
                Color2.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param83)}({Color1}, {Color2})";
        }

        /// <summary>
        /// A parameter that holds a parameter and a <see cref="PrimitiveColor"/>.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param84 : Param
        {
            /// <summary>
            /// A parameter.
            /// </summary>
            public Param Param { get; set; }

            /// <summary>
            /// A <see cref="PrimitiveColor"/>.
            /// </summary>
            public PrimitiveColor Color { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param84()
            {
                Param = new Param1();
                Color = new PrimitiveColor();
            }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 84;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param84(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Param = Param.Read(br, classNames);
                Color = new PrimitiveColor(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Param.AddClassNames(classNames);
                Color.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Param.Write(bw, classNames);
                Color.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param84)}({Param}, {Color})";
        }

        /// <summary>
        /// A parameter that holds a two tick values.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param85 : Param
        {
            /// <summary>
            /// A tick value.
            /// </summary>
            [XmlAttribute]
            public float Tick1 { get; set; }

            /// <summary>
            /// A tick value.
            /// </summary>
            [XmlAttribute]
            public float Tick2 { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param85() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 85;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param85(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Tick1 = PrimitiveTick.Read(br, classNames);
                Tick2 = PrimitiveTick.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                PrimitiveTick.AddClassName(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                PrimitiveTick.Write(bw, classNames, Tick1);
                PrimitiveTick.Write(bw, classNames, Tick2);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param85)}({Tick1}, {Tick2})";
        }

        /// <summary>
        /// A parameter that holds a value and index of some kind.<br/>
        /// Purpose unknown.
        /// </summary>
        public class Param87 : Param
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown; An index of some kind?
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Param87() { }

            #region Param

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 87;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Param87(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk04 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk04);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(Param87)}({Unk04}, {ArgIndex})";
        }

        #endregion

        #region Structures

        /// <summary>
        /// A value that has a tick and an <see cref="int"/>.
        /// </summary>
        public class TickInt
        {
            /// <summary>
            /// A value representing a tick.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// An <see cref="int"/> value.
            /// </summary>
            [XmlAttribute]
            public int Int { get; set; }

            /// <summary>
            /// Create a new <see cref="TickInt"/>.
            /// </summary>
            public TickInt() { }

            /// <summary>
            /// Create a new <see cref="TickInt"/> with the given values.
            /// </summary>
            /// <param name="tick">The value of the tick.</param>
            /// <param name="primInt">The value of the <see cref="int"/>.</param>
            public TickInt(float tick, int primInt)
            {
                Tick = tick;
                Int = primInt;
            }

            #region FXSerializable

            /// <summary>
            /// Read a <see cref="TickInt"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the underlying class names to.</param>
            internal TickInt(BinaryReaderEx br, List<string> classNames)
            {
                Tick = PrimitiveTick.Read(br, classNames);
                Int = PrimitiveInt.Read(br, classNames);
            }

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal void AddClassNames(List<string> classNames)
            {
                PrimitiveTick.AddClassName(classNames);
                PrimitiveInt.AddClassName(classNames);
            }

            /// <summary>
            /// Write this <see cref="TickInt"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get type numbers.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                PrimitiveTick.Write(bw, classNames, Tick);
                PrimitiveInt.Write(bw, classNames, Int);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(TickInt)}({Tick},{Int})";
        }

        /// <summary>
        /// A value that has a tick and a <see cref="float"/>.
        /// </summary>
        public class TickFloat
        {
            /// <summary>
            /// A value representing a tick.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// A <see cref="float"/> value.
            /// </summary>
            [XmlAttribute]
            public float Float { get; set; }

            /// <summary>
            /// Create a new <see cref="TickFloat"/>.
            /// </summary>
            public TickFloat() { }

            /// <summary>
            /// Create a new <see cref="TickFloat"/> with the given values.
            /// </summary>
            /// <param name="tick">The value of the tick.</param>
            /// <param name="primFloat">The value of the <see cref="float"/>.</param>
            public TickFloat(float tick, float primFloat)
            {
                Tick = tick;
                Float = primFloat;
            }

            #region FXSerializable

            /// <summary>
            /// Read a <see cref="TickFloat"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the underlying class names to.</param>
            internal TickFloat(BinaryReaderEx br, List<string> classNames)
            {
                Tick = PrimitiveTick.Read(br, classNames);
                Float = PrimitiveFloat.Read(br, classNames);
            }

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal void AddClassNames(List<string> classNames)
            {
                PrimitiveTick.AddClassName(classNames);
                PrimitiveFloat.AddClassName(classNames);
            }

            /// <summary>
            /// Write this <see cref="TickFloat"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get type numbers.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                PrimitiveTick.Write(bw, classNames, Tick);
                PrimitiveFloat.Write(bw, classNames, Float);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(TickFloat)}({Tick},{Float})";
        }

        /// <summary>
        /// A value that has a tick and three <see cref="float"/> values.
        /// </summary>
        public class TickFloat3
        {
            /// <summary>
            /// A value representing a tick.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// A <see cref="float"/> value.
            /// </summary>
            public float Float1 { get; set; }

            /// <summary>
            /// A <see cref="float"/> value.
            /// </summary>
            public float Float2 { get; set; }

            /// <summary>
            /// A <see cref="float"/> value.
            /// </summary>
            public float Float3 { get; set; }

            /// <summary>
            /// Create a new <see cref="TickFloat3"/>.
            /// </summary>
            public TickFloat3() { }

            /// <summary>
            /// Create a new <see cref="TickFloat3"/> with the given values.
            /// </summary>
            /// <param name="tick">The value of the tick.</param>
            /// <param name="float1">The first <see cref="float"/>.</param>
            /// <param name="float2">The second <see cref="float"/>.</param>
            /// <param name="float3">The third <see cref="float"/>.</param>
            public TickFloat3(float tick, float float1, float float2, float float3)
            {
                Tick = tick;
                Float1 = float1;
                Float2 = float2;
                Float3 = float3;
            }

            #region FXSerializable

            /// <summary>
            /// Read a <see cref="TickFloat3"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the underlying class names to.</param>
            internal TickFloat3(BinaryReaderEx br, List<string> classNames)
            {
                Tick = PrimitiveTick.Read(br, classNames);
                Float1 = PrimitiveFloat.Read(br, classNames);
                Float2 = PrimitiveFloat.Read(br, classNames);
                Float3 = PrimitiveFloat.Read(br, classNames);
            }

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal void AddClassNames(List<string> classNames)
            {
                PrimitiveTick.AddClassName(classNames);
                PrimitiveFloat.AddClassName(classNames);
            }

            /// <summary>
            /// Write this <see cref="TickFloat3"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get type numbers.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                PrimitiveTick.Write(bw, classNames, Tick);
                PrimitiveFloat.Write(bw, classNames, Float1);
                PrimitiveFloat.Write(bw, classNames, Float2);
                PrimitiveFloat.Write(bw, classNames, Float3);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(TickFloat3)}({Tick},{Float1},{Float2},{Float3})";
        }

        /// <summary>
        /// A value that has a tick and a color.
        /// </summary>
        public class TickColor
        {
            /// <summary>
            /// A value representing a tick.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// A color value.
            /// </summary>
            public PrimitiveColor Color { get; set; }

            /// <summary>
            /// Create a new <see cref="TickColor"/>.
            /// </summary>
            public TickColor()
            {
                Color = new PrimitiveColor();
            }

            /// <summary>
            /// Create a new <see cref="TickColor"/> with the given values.
            /// </summary>
            /// <param name="tick">The value of the tick.</param>
            /// <param name="color">The value of the color.</param>
            public TickColor(float tick, PrimitiveColor color)
            {
                Tick = tick;
                Color = color;
            }

            #region FXSerializable

            /// <summary>
            /// Read a <see cref="TickColor"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the underlying class names to.</param>
            internal TickColor(BinaryReaderEx br, List<string> classNames)
            {
                Tick = PrimitiveTick.Read(br, classNames);
                Color = new PrimitiveColor(br, classNames);
            }

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal void AddClassNames(List<string> classNames)
            {
                PrimitiveTick.AddClassName(classNames);
                Color.AddClassNames(classNames);
            }

            /// <summary>
            /// Write this <see cref="TickColor"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get type numbers.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                PrimitiveTick.Write(bw, classNames, Tick);
                Color.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(TickColor)}({Tick},{Color})";
        }

        /// <summary>
        /// A value that has a tick and three color values.
        /// </summary>
        public class TickColor3
        {
            /// <summary>
            /// A value representing a tick.
            /// </summary>
            [XmlAttribute]
            public float Tick { get; set; }

            /// <summary>
            /// A color value.
            /// </summary>
            public PrimitiveColor Color1 { get; set; }

            /// <summary>
            /// A color value.
            /// </summary>
            public PrimitiveColor Color2 { get; set; }

            /// <summary>
            /// A color value.
            /// </summary>
            public PrimitiveColor Color3 { get; set; }

            /// <summary>
            /// Create a new <see cref="TickColor3"/>.
            /// </summary>
            public TickColor3()
            {
                Color1 = new PrimitiveColor();
                Color2 = new PrimitiveColor();
                Color3 = new PrimitiveColor();
            }

            /// <summary>
            /// Create a new <see cref="TickColor3"/> with the given values.
            /// </summary>
            /// <param name="tick">The value of the tick.</param>
            /// <param name="color1">The first color.</param>
            /// <param name="color2">The second color.</param>
            /// <param name="color3">The third color.</param>
            public TickColor3(float tick, PrimitiveColor color1, PrimitiveColor color2, PrimitiveColor color3)
            {
                Tick = tick;
                Color1 = color1;
                Color2 = color2;
                Color3 = color3;
            }

            #region FXSerializable

            /// <summary>
            /// Read a <see cref="TickColor3"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the underlying class names to.</param>
            internal TickColor3(BinaryReaderEx br, List<string> classNames)
            {
                Tick = PrimitiveTick.Read(br, classNames);
                Color1 = new PrimitiveColor(br, classNames);
                Color2 = new PrimitiveColor(br, classNames);
                Color3 = new PrimitiveColor(br, classNames);
            }

            /// <summary>
            /// Adds this class name if it isn't already present in the class name list.
            /// </summary>
            /// <param name="classNames">A list of class names to add a class name to.</param>
            internal void AddClassNames(List<string> classNames)
            {
                PrimitiveTick.AddClassName(classNames);
                Color1.AddClassNames(classNames);
                Color2.AddClassNames(classNames);
                Color3.AddClassNames(classNames);
            }

            /// <summary>
            /// Write this <see cref="TickColor3"/> to a <see cref="Stream"/>.
            /// </summary>
            /// <param name="bw">The writer.</param>
            /// <param name="classNames">A list of class names to get type numbers.</param>
            internal void Write(BinaryWriterEx bw, List<string> classNames)
            {
                PrimitiveTick.Write(bw, classNames, Tick);
                Color1.Write(bw, classNames);
                Color2.Write(bw, classNames);
                Color3.Write(bw, classNames);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString()
                => $"{nameof(TickColor3)}({Tick},{Color1},{Color2},{Color3})";
        }

        #endregion
    }
}
