﻿using System;

namespace SoulsFormats
{
    public static partial class FLVER
    {
        /// <summary>
        /// Represents one property of a vertex.
        /// </summary>
        public class LayoutMember
        {
            /// <summary>
            /// The index of this member into the current layout.<br/>
            /// Used to combine members into a single layout.
            /// </summary>
            public int GroupIndex { get; set; }

            /// <summary>
            /// Format used to store this member.
            /// </summary>
            public LayoutType Type { get; set; }

            /// <summary>
            /// Vertex property being stored.
            /// </summary>
            public LayoutSemantic Semantic { get; set; }

            /// <summary>
            /// For semantics that may appear more than once such as UVs, which one this member is.
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// The size of this member's ValueType, in bytes.
            /// </summary>
            public int Size
            {
                get
                {
                    switch (Type)
                    {
                        case LayoutType.EdgeCompressed:
                            return 1;

                        case LayoutType.Byte4A:
                        case LayoutType.Byte4B:
                        case LayoutType.Short2toFloat2:
                        case LayoutType.Byte4C:
                        case LayoutType.Byte4D:
                        case LayoutType.UV:
                        case LayoutType.Byte4E:
                            return 4;

                        case LayoutType.Float2:
                        case LayoutType.UVPair:
                        case LayoutType.ShortBoneIndices:
                        case LayoutType.Short4toFloat4A:
                        case LayoutType.Short4toFloat4B:
                            return 8;

                        case LayoutType.Float3:
                            return 12;

                        case LayoutType.Float4:
                            return 16;

                        default:
                            throw new NotImplementedException($"No size defined for buffer layout type: {Type}");
                    }
                }
            }

            /// <summary>
            /// Creates a LayoutMember with the specified values.
            /// </summary>
            public LayoutMember(LayoutType type, LayoutSemantic semantic, int index = 0, int groupIndex = 0)
            {
                GroupIndex = groupIndex;
                Type = type;
                Semantic = semantic;
                Index = index;
            }

            /// <summary>
            /// Clone an existing LayoutMember.
            /// </summary>
            public LayoutMember(LayoutMember layoutMember)
            {
                GroupIndex = layoutMember.GroupIndex;
                Type = layoutMember.Type;
                Semantic = layoutMember.Semantic;
                Index = layoutMember.Index;
            }

            internal LayoutMember(BinaryReaderEx br, int structOffset)
            {
                GroupIndex = br.ReadInt32();
                br.AssertInt32(structOffset);
                Type = br.ReadEnum32<LayoutType>();
                Semantic = br.ReadEnum32<LayoutSemantic>();
                Index = br.ReadInt32();
            }

            internal void Write(BinaryWriterEx bw, int structOffset)
            {
                bw.WriteInt32(GroupIndex);
                bw.WriteInt32(structOffset);
                bw.WriteUInt32((uint)Type);
                bw.WriteUInt32((uint)Semantic);
                bw.WriteInt32(Index);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int hashCode = 19;
                hashCode += GroupIndex.GetHashCode() * 37;
                hashCode += Type.GetHashCode() * 37;
                hashCode += Semantic.GetHashCode() * 37;
                hashCode += Index.GetHashCode() * 37;
                return hashCode;
            }

            /// <summary>
            /// Returns the value type and semantic of this member.
            /// </summary>
            public override string ToString()
            {
                return $"{Type}: {Semantic}";
            }
        }

        /// <summary>
        /// Format of a vertex property.
        /// </summary>
        public enum LayoutType : uint
        {
            /// <summary>
            /// Two single-precision floats.
            /// </summary>
            Float2 = 0x01,

            /// <summary>
            /// Three single-precision floats.
            /// </summary>
            Float3 = 0x02,

            /// <summary>
            /// Four single-precision floats.
            /// </summary>
            Float4 = 0x03,

            /// <summary>
            /// Four bytes or two shorts?
            /// </summary>
            Byte4A = 0x10,

            /// <summary>
            /// Four bytes or two shorts?
            /// </summary>
            Byte4B = 0x11,

            /// <summary>
            /// Four bytes or two shorts?
            /// </summary>
            Short2toFloat2 = 0x12,

            /// <summary>
            /// Four bytes.
            /// </summary>
            Byte4C = 0x13,

            /// <summary>
            /// Four bytes.
            /// </summary>
            Byte4D = 0x14,

            /// <summary>
            /// Two shorts.
            /// </summary>
            UV = 0x15,

            /// <summary>
            /// Two shorts and two shorts.
            /// </summary>
            UVPair = 0x16,

            /// <summary>
            /// Four shorts, maybe unsigned?
            /// </summary>
            ShortBoneIndices = 0x18,

            /// <summary>
            /// Four shorts.
            /// </summary>
            Short4toFloat4A = 0x1A,

            /// <summary>
            /// Unknown.
            /// </summary>
            Short4toFloat4B = 0x2E,

            /// <summary>
            /// Unknown.
            /// </summary>
            Byte4E = 0x2F,

            /// <summary>
            /// Edge compression specified by edge members in face sets.
            /// </summary>
            EdgeCompressed = 0xF0,
        }

        /// <summary>
        /// Property of a vertex.
        /// </summary>
        public enum LayoutSemantic : uint
        {
            /// <summary>
            /// Location of the vertex.
            /// </summary>
            Position = 0,

            /// <summary>
            /// Weight of the vertex's attachment to bones.
            /// </summary>
            BoneWeights = 1,

            /// <summary>
            /// Bones the vertex is weighted to, indexing the parent mesh's bone indices.
            /// </summary>
            BoneIndices = 2,

            /// <summary>
            /// Orientation of the vertex.
            /// </summary>
            Normal = 3,

            /// <summary>
            /// Texture coordinates of the vertex.
            /// </summary>
            UV = 5,

            /// <summary>
            /// Vector pointing perpendicular to the normal.
            /// </summary>
            Tangent = 6,

            /// <summary>
            /// Vector pointing perpendicular to the normal and tangent.
            /// </summary>
            Bitangent = 7,

            /// <summary>
            /// Data used for blending, alpha, etc.
            /// </summary>
            VertexColor = 10,
        }
    }
}
