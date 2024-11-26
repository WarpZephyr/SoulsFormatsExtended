using System;
using System.Numerics;

namespace SoulsFormats
{
    public partial class FLVER
    {
        /// <summary>
        /// A joint available for vertices to be attached to.
        /// </summary>
        public class Node
        {
            /// <summary>
            /// A set of flags denoting the properties of a node.
            /// </summary>
            [Flags]
            public enum NodeFlags
            {
                /// <summary>
                /// Is disconnected from the node hierarchy (should not appear in combination with other flags)
                /// </summary>
                Disabled = 1,

                /// <summary>
                /// A dummy poly references this node using either <see cref="Dummy.AttachBoneIndex"/> or <see cref="Dummy.ParentBoneIndex"/>
                /// </summary>
                DummyOwner = 1 << 1,

                /// <summary>
                /// This node represents a mesh.
                /// </summary>
                Mesh = 1 << 2,

                /// <summary>
                /// This node represents a bone, i.e. it can be used to transform vertices.
                /// </summary>
                Bone = 1 << 3
            }

            /// <summary>
            /// The name of this <see cref="Node"/>.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The translation of this <see cref="Node"/>.
            /// </summary>
            public Vector3 Translation { get; set; }

            /// <summary>
            /// The rotation of this <see cref="Node"/>; euler radians in XZY order.
            /// </summary>
            public Vector3 Rotation { get; set; }

            /// <summary>
            /// The scale of this <see cref="Node"/>.
            /// </summary>
            public Vector3 Scale { get; set; }

            /// <summary>
            /// The minimum extent of the vertices weighted to this <see cref="Node"/>.
            /// </summary>
            public Vector3 BoundingBoxMin { get; set; }

            /// <summary>
            /// The maximum extent of the vertices weighted to this <see cref="Node"/>.
            /// </summary>
            public Vector3 BoundingBoxMax { get; set; }

            /// <summary>
            /// The index of the parent of this <see cref="Node"/>, or -1 for none.
            /// </summary>
            public short ParentIndex { get; set; }

            /// <summary>
            /// The index of the first child of this <see cref="Node"/>, or -1 for none.
            /// </summary>
            public short FirstChildIndex { get; set; }

            /// <summary>
            /// The index of the next child of the parent of this <see cref="Node"/>, or -1 for none.
            /// </summary>
            public short NextSiblingIndex { get; set; }

            /// <summary>
            /// The index of the previous child of the parent of this <see cref="Node"/>, or -1 for none.
            /// </summary>
            public short PreviousSiblingIndex { get; set; }

            /// <inheritdoc cref="NodeFlags"/>
            public NodeFlags Flags { get; set; }

            /// <summary>
            /// Create a new <see cref="Node"/> with default values.
            /// </summary>
            public Node()
            {
                Name = string.Empty;
                ParentIndex = -1;
                FirstChildIndex = -1;
                NextSiblingIndex = -1;
                PreviousSiblingIndex = -1;
                Scale = Vector3.One;
            }

            /// <summary>
            /// Clone an existing <see cref="Node"/>.
            /// </summary>
            public Node(Node node)
            {
                Name = node.Name;
                ParentIndex = node.ParentIndex;
                FirstChildIndex = node.FirstChildIndex;
                NextSiblingIndex = node.NextSiblingIndex;
                PreviousSiblingIndex = node.PreviousSiblingIndex;
                Translation = node.Translation;
                Rotation = node.Rotation;
                Scale = node.Scale;
                BoundingBoxMin = node.BoundingBoxMin;
                BoundingBoxMax = node.BoundingBoxMax;
            }

            /// <summary>
            /// Read a <see cref="Node"/> from a stream.
            /// </summary>
            /// <param name="br">The stream reader.</param>
            /// <param name="unicode">Whether or not strings are in unicode.</param>
            internal Node(BinaryReaderEx br, bool unicode)
            {
                Translation = br.ReadVector3();
                int nameOffset = br.ReadInt32();
                Rotation = br.ReadVector3();
                ParentIndex = br.ReadInt16();
                FirstChildIndex = br.ReadInt16();
                Scale = br.ReadVector3();
                NextSiblingIndex = br.ReadInt16();
                PreviousSiblingIndex = br.ReadInt16();
                BoundingBoxMin = br.ReadVector3();
                Flags = (NodeFlags)br.ReadInt32();
                BoundingBoxMax = br.ReadVector3();
                br.AssertPattern(0x34, 0x00);

                if (unicode)
                    Name = br.GetUTF16(nameOffset);
                else
                    Name = br.GetShiftJIS(nameOffset);
            }

            /// <summary>
            /// Write this <see cref="Node"/> to a stream.
            /// </summary>
            /// <param name="bw">The stream writer.</param>
            /// <param name="index">The index of the written data.</param>
            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.WriteVector3(Translation);
                bw.ReserveInt32($"NodeNameOffset_{index}");
                bw.WriteVector3(Rotation);
                bw.WriteInt16(ParentIndex);
                bw.WriteInt16(FirstChildIndex);
                bw.WriteVector3(Scale);
                bw.WriteInt16(NextSiblingIndex);
                bw.WriteInt16(PreviousSiblingIndex);
                bw.WriteVector3(BoundingBoxMin);
                bw.WriteInt32((int)Flags);
                bw.WriteVector3(BoundingBoxMax);
                bw.WritePattern(0x34, 0x00);
            }

            /// <summary>
            /// Writes strings for this <see cref="Node"/> according to the specified settings.
            /// </summary>
            /// <param name="bw">The stream writer.</param>
            /// <param name="unicode">Whether or not to write in unicode.</param>
            /// <param name="index">The index of the written data.</param>
            internal void WriteStrings(BinaryWriterEx bw, bool unicode, int index)
            {
                bw.FillInt32($"NodeNameOffset_{index}", (int)bw.Position);
                if (unicode)
                    bw.WriteUTF16(Name, true);
                else
                    bw.WriteShiftJIS(Name, true);
            }

            /// <summary>
            /// Creates a transformation matrix from the scale, rotation, and translation of this <see cref="Node"/>.
            /// </summary>
            public Matrix4x4 ComputeLocalTransform()
            {
                return Matrix4x4.CreateScale(Scale)
                    * Matrix4x4.CreateRotationX(Rotation.X)
                    * Matrix4x4.CreateRotationZ(Rotation.Z)
                    * Matrix4x4.CreateRotationY(Rotation.Y)
                    * Matrix4x4.CreateTranslation(Translation);
            }

            /// <summary>
            /// Returns a string representation of this <see cref="Node"/>.
            /// </summary>
            public override string ToString()
            {
                return Name;
            }
        }
    }
}
