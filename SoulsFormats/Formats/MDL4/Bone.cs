using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoulsFormats
{
    /// <summary>
    /// A 3D model format used in early PS3/X360 games. Extension: .mdl
    /// </summary>
    public partial class MDL4
    {
        /// <summary>
        /// A joint available for vertices to be attached to.
        /// </summary>
        public class Bone
        {
            /// <summary>
            /// Corresponds to the name of a bone in the parent skeleton, if present.
            /// </summary>
            public string Name;

            /// <summary>
            /// Translation of this bone.
            /// </summary>
            public Vector3 Translation;

            /// <summary>
            /// Rotation of this bone; euler radians in XZY order.
            /// </summary>
            public Vector3 Rotation;

            /// <summary>
            /// Scale of this bone.
            /// </summary>
            public Vector3 Scale;

            /// <summary>
            /// Minimum extent of the vertices weighted to this bone.
            /// </summary>
            public Vector3 BoundingBoxMin;

            /// <summary>
            /// Maximum extent of the vertices weighted to this bone.
            /// </summary>
            public Vector3 BoundingBoxMax;

            /// <summary>
            /// Index of the parent in this FLVER's bone collection, or -1 for none.
            /// </summary>
            public short ParentIndex;

            /// <summary>
            /// Index of the first child in this FLVER's bone collection, or -1 for none.
            /// </summary>
            public short ChildIndex;

            /// <summary>
            /// Index of the next child of this bone's parent, or -1 for none.
            /// </summary>
            public short NextSiblingIndex;

            /// <summary>
            /// Index of the previous child of this bone's parent, or -1 for none.
            /// </summary>
            public short PreviousSiblingIndex;

            /// <summary>
            /// An unknown set of indices.
            /// </summary>
            public short[] UnkIndices;

            /// <summary>
            /// Reads a bone from an BinaryReaderEx.
            /// </summary>
            internal Bone(BinaryReaderEx br)
            {
                Name = br.ReadFixStr(0x20);
                Translation = br.ReadVector3();
                Rotation = br.ReadVector3();
                Scale = br.ReadVector3();
                BoundingBoxMin = br.ReadVector3();
                BoundingBoxMax = br.ReadVector3();
                ParentIndex = br.ReadInt16();
                ChildIndex = br.ReadInt16();
                NextSiblingIndex = br.ReadInt16();
                PreviousSiblingIndex = br.ReadInt16();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
                UnkIndices = br.ReadInt16s(16);
            }

            /// <summary>
            /// Writes a bone to a BinaryWriterEx.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteFixStr(Name, 0x20);
                bw.WriteVector3(Translation);
                bw.WriteVector3(Rotation);
                bw.WriteVector3(Scale);
                bw.WriteVector3(BoundingBoxMin);
                bw.WriteVector3(BoundingBoxMax);
                bw.WriteInt16(ParentIndex);
                bw.WriteInt16(ChildIndex);
                bw.WriteInt16(NextSiblingIndex);
                bw.WriteInt16(PreviousSiblingIndex);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt16s(UnkIndices);
            }

            /// <summary>
            /// Creates a transformation matrix from the scale, rotation, and translation of the bone.
            /// </summary>
            public Matrix4x4 ComputeLocalTransform()
            {
                return Matrix4x4.CreateScale(Scale)
                    * Matrix4x4.CreateRotationX(Rotation.X)
                    * Matrix4x4.CreateRotationZ(Rotation.Z)
                    * Matrix4x4.CreateRotationY(Rotation.Y)
                    * Matrix4x4.CreateTranslation(Translation);
            }
        }
    }
}
