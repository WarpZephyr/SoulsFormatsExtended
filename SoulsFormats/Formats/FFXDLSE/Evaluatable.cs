using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// An object used for the evaluation of a condition or set of conditions.
        /// </summary>
        #region XmlInclude
        [
            XmlInclude(typeof(EvaluatableConstant)),
            XmlInclude(typeof(Evaluatable2)),
            XmlInclude(typeof(Evaluatable3)),
            XmlInclude(typeof(EvaluatableCurrentTick)),
            XmlInclude(typeof(EvaluatableTotalTick)),
            XmlInclude(typeof(EvaluatableAnd)),
            XmlInclude(typeof(EvaluatableOr)),
            XmlInclude(typeof(EvaluatableGE)),
            XmlInclude(typeof(EvaluatableGT)),
            XmlInclude(typeof(EvaluatableLE)),
            XmlInclude(typeof(EvaluatableLT)),
            XmlInclude(typeof(EvaluatableEQ)),
            XmlInclude(typeof(EvaluatableNE)),
            XmlInclude(typeof(EvaluatableNot)),
            XmlInclude(typeof(EvaluatableChildExists)),
            XmlInclude(typeof(EvaluatableParentExists)),
            XmlInclude(typeof(EvaluatableDistanceFromCamera)),
            XmlInclude(typeof(EvaluatableEmittersStopped)),
            ]
        #endregion
        public abstract class Evaluatable : FXSerializable
        {
            /// <summary>
            /// A value representing what kind of operation is being performed.
            /// </summary>
            internal abstract int Opcode { get; }

            /// <summary>
            /// The type of value being evaluated.<br/>
            /// int = 1<br/>
            /// float = 2<br/>
            /// bool = 3
            /// </summary>
            internal abstract int Type { get; }

            /// <summary>
            /// Create a new <see cref="Evaluatable"/>.
            /// </summary>
            public Evaluatable() { }

            /// <summary>
            /// Read an <see cref="Evaluatable"/> from a <see cref="Stream"/>.
            /// </summary>
            /// <param name="br">The reader.</param>
            /// <param name="classNames">The names of classes to compare the <see cref="ClassName"/> to</param>
            /// <returns>An <see cref="Evaluatable"/>.</returns>
            /// <exception cref="NotImplementedException">The read <see cref="Opcode"/> is not implemented.</exception>
            internal static Evaluatable Read(BinaryReaderEx br, List<string> classNames)
            {
                // Don't @ me.
                int opcode = br.GetInt32(br.Position + 0xA);
                switch (opcode)
                {
                    case 1: return new EvaluatableConstant(br, classNames);
                    case 2: return new Evaluatable2(br, classNames);
                    case 3: return new Evaluatable3(br, classNames);
                    case 4: return new EvaluatableCurrentTick(br, classNames);
                    case 5: return new EvaluatableTotalTick(br, classNames);
                    case 8: return new EvaluatableAnd(br, classNames);
                    case 9: return new EvaluatableOr(br, classNames);
                    case 10: return new EvaluatableGE(br, classNames);
                    case 11: return new EvaluatableGT(br, classNames);
                    case 12: return new EvaluatableLE(br, classNames);
                    case 13: return new EvaluatableLT(br, classNames);
                    case 14: return new EvaluatableEQ(br, classNames);
                    case 15: return new EvaluatableNE(br, classNames);
                    case 20: return new EvaluatableNot(br, classNames);
                    case 21: return new EvaluatableChildExists(br, classNames);
                    case 22: return new EvaluatableParentExists(br, classNames);
                    case 23: return new EvaluatableDistanceFromCamera(br, classNames);
                    case 24: return new EvaluatableEmittersStopped(br, classNames);

                    default:
                        throw new NotImplementedException($"Unimplemented evaluatable opcode: {opcode}");
                }
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableEvaluatable<dl_int32>";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Evaluatable(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                br.AssertInt32(Opcode);
                br.AssertInt32(Type);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(Opcode);
                bw.WriteInt32(Type);
            }

            #endregion
        }

        #region Evaluatable Types

        /// <summary>
        /// An evaluator that holds a single operand.
        /// </summary>
        public abstract class EvaluatableUnary : Evaluatable
        {
            /// <summary>
            /// An evaluator operand to evaluate.
            /// </summary>
            public Evaluatable Operand { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableUnary() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableUnary(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Operand = Evaluatable.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Operand.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Operand.Write(bw, classNames);
            }

            #endregion
        }

        /// <summary>
        /// An evaluator that compares two operands.
        /// </summary>
        public abstract class EvaluatableBinary : Evaluatable
        {
            /// <summary>
            /// The left operand.
            /// </summary>
            public Evaluatable Left { get; set; }

            /// <summary>
            /// The right operand.
            /// </summary>
            public Evaluatable Right { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableBinary() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableBinary(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Right = Evaluatable.Read(br, classNames);
                Left = Evaluatable.Read(br, classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                Left.AddClassNames(classNames);
                Right.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                Right.Write(bw, classNames);
                Left.Write(bw, classNames);
            }

            #endregion
        }

        /// <summary>
        /// An evaluator that holds a constant.
        /// </summary>
        public class EvaluatableConstant : Evaluatable
        {
            /// <summary>
            /// The value of the constant.
            /// </summary>
            [XmlAttribute]
            public int Value { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableConstant() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableConstant(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Value = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Value);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"{Value}";
        }

        /// <summary>
        /// An evaluator with an unknown purpose.
        /// </summary>
        public class Evaluatable2 : Evaluatable
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk00 { get; set; }

            /// <summary>
            /// Unknown index of some kind.
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Evaluatable2() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 2;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Evaluatable2(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk00 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk00);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"{{2: {Unk00}, {ArgIndex}}}";
        }

        /// <summary>
        /// An evaluator with an unknown purpose.<br/>
        /// Has been compared to <see cref="EvaluatableTotalTick"/>.
        /// </summary>
        public class Evaluatable3 : Evaluatable
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            [XmlAttribute]
            public int Unk00 { get; set; }

            /// <summary>
            /// Index with unknown purpose; Has indexed into <see cref="FXEffect.ParamList1"/> .
            /// </summary>
            [XmlAttribute]
            public int ArgIndex { get; set; }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public Evaluatable3() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal Evaluatable3(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                base.Deserialize(br, classNames);
                Unk00 = br.ReadInt32();
                ArgIndex = br.ReadInt32();
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                base.Serialize(bw, classNames);
                bw.WriteInt32(Unk00);
                bw.WriteInt32(ArgIndex);
            }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"{{3: {Unk00}, {ArgIndex}}}";
        }

        /// <summary>
        /// An evaluator that gets the current tick value.
        /// </summary>
        public class EvaluatableCurrentTick : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableCurrentTick() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 4;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableCurrentTick(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "CurrentTick";
        }

        /// <summary>
        /// An evaluator that gets the total tick value.
        /// </summary>
        public class EvaluatableTotalTick : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableTotalTick() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 5;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableTotalTick(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "TotalTick";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If both are true this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableAnd : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableAnd() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 8;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableAnd(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} && {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If either or both are true this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableOr : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableOr() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 9;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableOr(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} || {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If right is greater than or equal to left, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableGE : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableGE() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 10;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableGE(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} >= {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If right is greater than left, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableGT : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableGT() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 11;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableGT(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} > {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If right is less than or equal to left, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableLE : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableLE() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 12;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableLE(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} <= {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If right is less than left, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableLT : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableLT() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 13;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableLT(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} < {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If left is equal to right, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableEQ : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableEQ() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 14;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableEQ(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} == {Right})";
        }

        /// <summary>
        /// An evaluator that compares two operands;<br/>
        /// If left is not equal to right, this is true, otherwise this is false.
        /// </summary>
        public class EvaluatableNE : EvaluatableBinary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableNE() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 15;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableNE(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"({Left} != {Right})";
        }

        /// <summary>
        /// An evaluator that holds a single operand.<br/>
        /// Inverts the result of the operand.
        /// </summary>
        public class EvaluatableNot : EvaluatableUnary
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableNot() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 20;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableNot(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => $"!({Operand})";
        }

        /// <summary>
        /// An evaluator that returns true if, presumbly, a child effect exists.
        /// </summary>
        public class EvaluatableChildExists : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableChildExists() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 21;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableChildExists(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "ChildExists";
        }

        /// <summary>
        /// An evaluator that returns true if, presumbly, a parent effect exists.
        /// </summary>
        public class EvaluatableParentExists : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableParentExists() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 22;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableParentExists(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "ParentExists";
        }

        /// <summary>
        /// An evaluator that gets the distance from the camera.
        /// </summary>
        public class EvaluatableDistanceFromCamera : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableDistanceFromCamera() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 23;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableDistanceFromCamera(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "DistanceFromCamera";
        }

        /// <summary>
        /// An evaluator that returns true if the emitters have stopped.
        /// </summary>
        public class EvaluatableEmittersStopped : Evaluatable
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public EvaluatableEmittersStopped() { }

            #region Evaluatable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Opcode => 24;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Type => 3;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal EvaluatableEmittersStopped(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            #endregion

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public override string ToString() => "EmittersStopped";
        }

        #endregion
    }
}
