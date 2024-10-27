using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SoulsFormats
{
    public partial class FLVER2
    {
        /// <summary>
        /// A header for an edge compressed vertex buffer.
        /// </summary>
        internal class EdgeVertexBuffer
        {
            /// <summary>
            /// What to multiply components by when decompressing.
            /// </summary>
            public Vector4 Multiplier { get; set; }

            /// <summary>
            /// The offsets to add to components when decompressing.
            /// </summary>
            public Vector4 Offset { get; set; }

            /// <summary>
            /// The number of bits per bone index in <see cref="BoneIndexBytes"/>.
            /// </summary>
            public byte BoneIndexBitSize { get; set; }

            /// <summary>
            /// The vertices in this buffer. 
            /// </summary>
            public FIXEDc3[] Vertices { get; set; }

            /// <summary>
            /// The bit-packed bone indices in this buffer.
            /// </summary>
            public byte[] BoneIndexBytes { get; set; }

            /// <summary>
            /// Read a <see cref="EdgeVertexBuffer"/> from a stream.
            /// </summary>
            /// <param name="br">The stream reader.</param>
            /// <param name="vertexCount">The number of vertices in this buffer.</param>
            /// <exception cref="InvalidDataException">The length values were invalid.</exception>
            internal EdgeVertexBuffer(BinaryReaderEx br, int vertexCount)
            {
                Multiplier = br.ReadVector4();
                Offset = br.ReadVector4();
                int edgeVertexBufferLength = br.ReadInt32(); // The length of vertices + padding + header
                int edgeVertexBufferTotalLength = br.ReadInt32(); // The total length of the buffer including bone indices.
                BoneIndexBitSize = br.ReadByte();
                br.ReadByte(); // Unknown
                br.AssertUInt16(0);
                br.AssertUInt32(0);

                if (edgeVertexBufferTotalLength < edgeVertexBufferLength)
                {
                    throw new InvalidDataException($"{nameof(edgeVertexBufferTotalLength)} must have at least the length of {nameof(edgeVertexBufferLength)}");
                }

                if (BoneIndexBitSize == 0 && edgeVertexBufferLength != edgeVertexBufferTotalLength)
                {
                    throw new InvalidDataException($"{nameof(edgeVertexBufferTotalLength)} must be the same length as {nameof(edgeVertexBufferLength)} when {nameof(BoneIndexBitSize)} is {0}.");
                }

                long pos = br.Position;
                Vertices = new FIXEDc3[vertexCount];
                for (int i = 0; i < vertexCount; i++)
                {
                    Vertices[i] = new FIXEDc3(br);
                }
                br.Position = pos + (edgeVertexBufferLength - 48);

                BoneIndexBytes = br.ReadBytes(edgeVertexBufferTotalLength - edgeVertexBufferLength);
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct FIXEDc3
            {
                [FieldOffset(0)]
                public ushort X;

                [FieldOffset(2)]
                public ushort Y;

                [FieldOffset(4)]
                public ushort Z;

                internal FIXEDc3(BinaryReaderEx br)
                {
                    X = br.ReadUInt16();
                    Y = br.ReadUInt16();
                    Z = br.ReadUInt16();
                }

                public readonly Vector3 Decompress(Vector4 multiplier, Vector4 offset)
                    => new Vector3((X * multiplier.X) + offset.X, (Y * multiplier.Y) + offset.Y, (Z * multiplier.Z) + offset.Z);
            }
        }
    }
}
