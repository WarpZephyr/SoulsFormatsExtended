﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// Common classes for <see cref="FLVER0"/> and <see cref="FLVER2"/>.
    /// </summary>
    public static partial class FLVER
    {
        /// <summary>
        /// A single point in a mesh.
        /// </summary>
        public class Vertex
        {
            /// <summary>
            /// Where the vertex is.
            /// </summary>
            public Vector3 Position;

            /// <summary>
            /// Weight of the vertex's attachment to bones; must be 4 length.
            /// </summary>
            public VertexBoneWeights BoneWeights;

            /// <summary>
            /// Bones the vertex is weighted to, indexing the parent mesh's bone indices; must be 4 length.
            /// </summary>
            public VertexBoneIndices BoneIndices;

            /// <summary>
            /// Vector pointing away from the surface.
            /// </summary>
            public Vector3 Normal;

            /// <summary>
            /// Fourth component of the normal, read without transforming into a float; used as a bone index for binding to a single bone.
            /// </summary>
            public int NormalW;

            /// <summary>
            /// Texture coordinates of the vertex.
            /// </summary>
            public List<Vector3> UVs;

            /// <summary>
            /// Vector pointing perpendicular to the normal.
            /// </summary>
            public List<Vector4> Tangents;

            /// <summary>
            /// Vector pointing perpendicular to the normal and tangent.
            /// </summary>
            public Vector4 Bitangent;

            /// <summary>
            /// Data used for alpha, blending, etc.
            /// </summary>
            public List<VertexColor> Colors;

            private Queue<Vector3> uvQueue;
            private Queue<Vector4> tangentQueue;
            private Queue<VertexColor> colorQueue;

            /// <summary>
            /// Create a Vertex with null or empty values.
            /// </summary>
            public Vertex(int uvCapacity = 0, int tangentCapacity = 0, int colorCapacity = 0)
            {
                UVs = new List<Vector3>(uvCapacity);
                Tangents = new List<Vector4>(tangentCapacity);
                Colors = new List<VertexColor>(colorCapacity);
            }

            /// <summary>
            /// Creates a new Vertex with values copied from another.
            /// </summary>
            public Vertex(Vertex clone)
            {
                Position = clone.Position;
                BoneWeights = clone.BoneWeights;
                BoneIndices = clone.BoneIndices;
                Normal = clone.Normal;
                UVs = new List<Vector3>(clone.UVs);
                Tangents = new List<Vector4>(clone.Tangents);
                Bitangent = clone.Bitangent;
                Colors = new List<VertexColor>(clone.Colors);
            }

            /// <summary>
            /// Must be called before writing any buffers. Queues list types so they will be split across buffers properly.
            /// </summary>
            internal void PrepareWrite()
            {
                uvQueue = new Queue<Vector3>(UVs);
                tangentQueue = new Queue<Vector4>(Tangents);
                colorQueue = new Queue<VertexColor>(Colors);
            }

            /// <summary>
            /// Should be called after writing all buffers. Throws out queues to free memory.
            /// </summary>
            internal void FinishWrite()
            {
                uvQueue = null;
                tangentQueue = null;
                colorQueue = null;
            }

            internal void Read(BinaryReaderEx br, List<LayoutMember> layout, float uvFactor)
            {
                foreach (LayoutMember member in layout)
                {
                    if (member.Semantic == LayoutSemantic.Position)
                    {
                        if (member.Type == LayoutType.Float3)
                        {
                            Position = br.ReadVector3();
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            Position = br.ReadVector3();
                            br.AssertSingle(0);
                        }
                        else if (member.Type == LayoutType.EdgeCompressed)
                        {
                            throw new NotSupportedException("Edge vertex buffer decompression is not done here.");
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.BoneWeights)
                    {
                        if (member.Type == LayoutType.Color)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneWeights[i] = br.ReadSByte() / 127f;
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneWeights[i] = br.ReadByte() / 255f;
                        }
                        else if (member.Type == LayoutType.Short4)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneWeights[i] = br.ReadInt16() / 32767f;
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneWeights[i] = br.ReadInt16() / 32767f;
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.BoneIndices)
                    {
                        if (member.Type == LayoutType.UByte4)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneIndices[i] = br.ReadByte();
                        }
                        else if (member.Type == LayoutType.UShort4)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneIndices[i] = br.ReadUInt16();
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneIndices[i] = br.ReadByte();
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            for (int i = 0; i < 4; i++)
                                BoneIndices[i] = br.ReadByte();
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Normal)
                    {
                        if (member.Type == LayoutType.Float3)
                        {
                            Normal = br.ReadVector3();
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            Normal = br.ReadVector3();
                            float w = br.ReadSingle();
                            NormalW = (int)w;
                            if (w != NormalW)
                                throw new InvalidDataException($"Float4 Normal W was not a whole number: {w}");
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            Normal = ReadByteNormXYZ(br);
                            NormalW = br.ReadByte();
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            Normal = ReadByteNormXYZ(br);
                            NormalW = br.ReadByte();
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            NormalW = br.ReadByte();
                            Normal = ReadSByteNormZYX(br);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            Normal = ReadByteNormXYZ(br);
                            NormalW = br.ReadByte();
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            Normal = ReadShortNormXYZ(br);
                            NormalW = br.ReadInt16();
                        }
                        else if (member.Type == LayoutType.Half_4)
                        {
                            Normal = ReadUShortNormXYZ(br);
                            NormalW = br.ReadInt16();
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            Normal = ReadByteNormXYZ(br);
                            NormalW = br.ReadByte();
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.UV)
                    {
                        if (member.Type == LayoutType.Float2)
                        {
                            UVs.Add(new Vector3(br.ReadVector2(), 0));
                        }
                        else if (member.Type == LayoutType.Float3)
                        {
                            UVs.Add(br.ReadVector3());
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            UVs.Add(new Vector3(br.ReadVector2(), 0));
                            UVs.Add(new Vector3(br.ReadVector2(), 0));
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.Short2)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.Short4)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), 0) / uvFactor);
                        }
                        else if (member.Type == LayoutType.Half_4)
                        {
                            UVs.Add(new Vector3(br.ReadInt16(), br.ReadInt16(), br.ReadInt16()) / uvFactor);
                            br.AssertInt16(0);
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Tangent)
                    {
                        if (member.Type == LayoutType.Float4)
                        {
                            Tangents.Add(br.ReadVector4());
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            Tangents.Add(ReadByteNormXYZW(br));
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            Tangents.Add(ReadByteNormXYZW(br));
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            Tangents.Add(ReadByteNormXYZW(br));
                        }
                        else if (member.Type == LayoutType.Byte4Norm)
                        {
                            Tangents.Add(ReadByteNormXYZW(br));
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            Tangents.Add(ReadShortNormXYZW(br));
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            Tangents.Add(ReadByteNormXYZW(br));
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Bitangent)
                    {
                        if (member.Type == LayoutType.Color)
                        {
                            Bitangent = ReadByteNormXYZW(br);
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            Bitangent = ReadByteNormXYZW(br);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            Bitangent = ReadByteNormXYZW(br);
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            Bitangent = ReadByteNormXYZW(br);
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.VertexColor)
                    {
                        if (member.Type == LayoutType.Float4)
                        {
                            Colors.Add(VertexColor.ReadFloatRGBA(br));
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            // Definitely RGBA in DeS
                            Colors.Add(VertexColor.ReadByteRGBA(br));
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            // Definitely RGBA in DS1
                            Colors.Add(VertexColor.ReadByteRGBA(br));
                        }
                        else
                            throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                    }
                    else
                        throw new NotImplementedException($"Read not implemented for {member.Type} {member.Semantic}.");
                }
            }

            #region Read Helpers
            private static float ReadByteNorm(BinaryReaderEx br)
                => (br.ReadByte() - 127) / 127f;

            private static Vector3 ReadByteNormXYZ(BinaryReaderEx br)
                => new Vector3(ReadByteNorm(br), ReadByteNorm(br), ReadByteNorm(br));

            private static Vector4 ReadByteNormXYZW(BinaryReaderEx br)
                => new Vector4(ReadByteNorm(br), ReadByteNorm(br), ReadByteNorm(br), ReadByteNorm(br));

            private static float ReadSByteNorm(BinaryReaderEx br)
                => br.ReadSByte() / 127f;

            private static Vector3 ReadSByteNormZYX(BinaryReaderEx br)
            {
                float z = ReadSByteNorm(br);
                float y = ReadSByteNorm(br);
                float x = ReadSByteNorm(br);
                return new Vector3(x, y, z);
            }

            private static float ReadShortNorm(BinaryReaderEx br)
                => br.ReadInt16() / 32767f;

            private static Vector3 ReadShortNormXYZ(BinaryReaderEx br)
                => new Vector3(ReadShortNorm(br), ReadShortNorm(br), ReadShortNorm(br));

            private static Vector4 ReadShortNormXYZW(BinaryReaderEx br)
                => new Vector4(ReadShortNorm(br), ReadShortNorm(br), ReadShortNorm(br), ReadShortNorm(br));

            private static float ReadUShortNorm(BinaryReaderEx br)
                => (br.ReadUInt16() - 32767) / 32767f;

            private static Vector3 ReadUShortNormXYZ(BinaryReaderEx br)
                => new Vector3(ReadUShortNorm(br), ReadUShortNorm(br), ReadUShortNorm(br));

            /// <summary>
            /// Read an edge compressed fixed-point float3 value.
            /// </summary>
            /// <param name="br">The stream reader.</param>
            /// <param name="multiplier">The multipler for the value.</param>
            /// <param name="offset">The offset for the value.</param>
            /// <returns>The decompressed float3 value.</returns>
            internal static Vector3 ReadFIXEDc3(BinaryReaderEx br, Vector4 multiplier, Vector4 offset)
                => new Vector3((br.ReadUInt16() * multiplier.X) + offset.X, (br.ReadUInt16() * multiplier.Y) + offset.Y, (br.ReadUInt16() * multiplier.Z) + offset.Z);
            #endregion

            internal void Write(BinaryWriterEx bw, List<LayoutMember> layout, float uvFactor)
            {
                foreach (LayoutMember member in layout)
                {
                    if (member.Semantic == LayoutSemantic.Position)
                    {
                        if (member.Type == LayoutType.Float3)
                        {
                            bw.WriteVector3(Position);
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            bw.WriteVector3(Position);
                            bw.WriteSingle(0);
                        }
                        else if (member.Type == LayoutType.EdgeCompressed)
                        {
                            throw new NotSupportedException("Edge vertex compression is not supported.");
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.BoneWeights)
                    {
                        if (member.Type == LayoutType.Color)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteSByte((sbyte)Math.Round(BoneWeights[i] * 127));
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteByte((byte)Math.Round(BoneWeights[i] * 255));
                        }
                        else if (member.Type == LayoutType.Short4)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteInt16((short)Math.Round(BoneWeights[i] * 32767));
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteInt16((short)Math.Round(BoneWeights[i] * 32767));
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.BoneIndices)
                    {
                        if (member.Type == LayoutType.UByte4)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteByte((byte)BoneIndices[i]);
                        }
                        else if (member.Type == LayoutType.UShort4)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteUInt16((ushort)BoneIndices[i]);
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteByte((byte)BoneIndices[i]);
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            for (int i = 0; i < 4; i++)
                                bw.WriteByte((byte)BoneIndices[i]);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Normal)
                    {
                        if (member.Type == LayoutType.Float3)
                        {
                            bw.WriteVector3(Normal);
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            bw.WriteVector3(Normal);
                            bw.WriteSingle(NormalW);
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            WriteByteNormXYZ(bw, Normal);
                            bw.WriteByte((byte)NormalW);
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            WriteByteNormXYZ(bw, Normal);
                            bw.WriteByte((byte)NormalW);
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            bw.WriteByte((byte)NormalW);
                            WriteSByteNormZYX(bw, Normal);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            WriteByteNormXYZ(bw, Normal);
                            bw.WriteByte((byte)NormalW);
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            WriteShortNormXYZ(bw, Normal);
                            bw.WriteInt16((short)NormalW);
                        }
                        else if (member.Type == LayoutType.Half_4)
                        {
                            WriteUShortNormXYZ(bw, Normal);
                            bw.WriteInt16((short)NormalW);
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            WriteByteNormXYZ(bw, Normal);
                            bw.WriteByte((byte)NormalW);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.UV)
                    {
                        Vector3 uv = uvQueue.Dequeue() * uvFactor;
                        if (member.Type == LayoutType.Float2)
                        {
                            bw.WriteSingle(uv.X);
                            bw.WriteSingle(uv.Y);
                        }
                        else if (member.Type == LayoutType.Float3)
                        {
                            bw.WriteVector3(uv);
                        }
                        else if (member.Type == LayoutType.Float4)
                        {
                            bw.WriteSingle(uv.X);
                            bw.WriteSingle(uv.Y);

                            uv = uvQueue.Dequeue() * uvFactor;
                            bw.WriteSingle(uv.X);
                            bw.WriteSingle(uv.Y);
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.Byte4)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.Short2)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.Short4)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));

                            uv = uvQueue.Dequeue() * uvFactor;
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                        }
                        else if (member.Type == LayoutType.Half_4)
                        {
                            bw.WriteInt16((short)Math.Round(uv.X));
                            bw.WriteInt16((short)Math.Round(uv.Y));
                            bw.WriteInt16((short)Math.Round(uv.Z));
                            bw.WriteInt16(0);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Tangent)
                    {
                        Vector4 tangent = tangentQueue.Dequeue();
                        if (member.Type == LayoutType.Float4)
                        {
                            bw.WriteVector4(tangent);
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            WriteByteNormXYZW(bw, tangent);
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            WriteByteNormXYZW(bw, tangent);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            WriteByteNormXYZW(bw, tangent);
                        }
                        else if (member.Type == LayoutType.Byte4Norm)
                        {
                            WriteByteNormXYZW(bw, tangent);
                        }
                        else if (member.Type == LayoutType.Short4Norm)
                        {
                            WriteShortNormXYZW(bw, tangent);
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            WriteByteNormXYZW(bw, tangent);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.Bitangent)
                    {
                        if (member.Type == LayoutType.Color)
                        {
                            WriteByteNormXYZW(bw, Bitangent);
                        }
                        else if (member.Type == LayoutType.UByte4)
                        {
                            WriteByteNormXYZW(bw, Bitangent);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            WriteByteNormXYZW(bw, Bitangent);
                        }
                        else if (member.Type == LayoutType.Byte4E)
                        {
                            WriteByteNormXYZW(bw, Bitangent);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else if (member.Semantic == LayoutSemantic.VertexColor)
                    {
                        VertexColor color = colorQueue.Dequeue();
                        if (member.Type == LayoutType.Float4)
                        {
                            color.WriteFloatRGBA(bw);
                        }
                        else if (member.Type == LayoutType.Color)
                        {
                            color.WriteByteRGBA(bw);
                        }
                        else if (member.Type == LayoutType.UByte4Norm)
                        {
                            color.WriteByteRGBA(bw);
                        }
                        else
                            throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                    }
                    else
                        throw new NotImplementedException($"Write not implemented for {member.Type} {member.Semantic}.");
                }
            }

            #region Write Helpers
            private static void WriteByteNorm(BinaryWriterEx bw, float value)
                => bw.WriteByte((byte)Math.Round(value * 127 + 127));

            private static void WriteByteNormXYZ(BinaryWriterEx bw, Vector3 value)
            {
                WriteByteNorm(bw, value.X);
                WriteByteNorm(bw, value.Y);
                WriteByteNorm(bw, value.Z);
            }

            private static void WriteByteNormXYZW(BinaryWriterEx bw, Vector4 value)
            {
                WriteByteNorm(bw, value.X);
                WriteByteNorm(bw, value.Y);
                WriteByteNorm(bw, value.Z);
                WriteByteNorm(bw, value.W);
            }

            private static void WriteSByteNorm(BinaryWriterEx bw, float value)
                => bw.WriteSByte((sbyte)Math.Round(value * 127));

            private static void WriteSByteNormZYX(BinaryWriterEx bw, Vector3 value)
            {
                WriteSByteNorm(bw, value.Z);
                WriteSByteNorm(bw, value.Y);
                WriteSByteNorm(bw, value.X);
            }

            private static void WriteShortNorm(BinaryWriterEx bw, float value)
                => bw.WriteInt16((short)Math.Round(value * 32767));

            private static void WriteShortNormXYZ(BinaryWriterEx bw, Vector3 value)
            {
                WriteShortNorm(bw, value.X);
                WriteShortNorm(bw, value.Y);
                WriteShortNorm(bw, value.Z);
            }

            private static void WriteShortNormXYZW(BinaryWriterEx bw, Vector4 value)
            {
                WriteShortNorm(bw, value.X);
                WriteShortNorm(bw, value.Y);
                WriteShortNorm(bw, value.Z);
                WriteShortNorm(bw, value.W);
            }

            private static void WriteUShortNorm(BinaryWriterEx bw, float value)
                => bw.WriteUInt16((ushort)Math.Round(value * 32767 + 32767));

            private static void WriteUShortNormXYZ(BinaryWriterEx bw, Vector3 value)
            {
                WriteUShortNorm(bw, value.X);
                WriteUShortNorm(bw, value.Y);
                WriteUShortNorm(bw, value.Z);
            }
            #endregion
        }
    }
}
