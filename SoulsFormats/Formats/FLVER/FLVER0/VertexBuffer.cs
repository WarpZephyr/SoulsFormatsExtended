using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class FLVER0
    {
        /// <summary>
        /// Temp comment.
        /// </summary>
        public class VertexBuffer
        {
            /// <summary>
            /// Temp comment.
            /// </summary>
            public int LayoutIndex;

            /// <summary>
            /// Temp comment.
            /// </summary>
            public int BufferLength;

            /// <summary>
            /// Temp comment.
            /// </summary>
            public int BufferOffset;

            /// <summary>
            /// Temp comment.
            /// </summary>
            public VertexBuffer() { }

            internal VertexBuffer(BinaryReaderEx br)
            {
                LayoutIndex = br.ReadInt32();
                BufferLength = br.ReadInt32();
                BufferOffset = br.ReadInt32();
                br.AssertInt32(0);
            }
            internal static List<VertexBuffer> ReadVertexBuffers(BinaryReaderEx br)
            {
                int bufferCount = br.ReadInt32();
                int buffersOffset = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);

                var buffers = new List<VertexBuffer>(bufferCount);
                br.StepIn(buffersOffset);
                {
                    for (int i = 0; i < bufferCount; i++)
                        buffers.Add(new VertexBuffer(br));
                }
                br.StepOut();
                return buffers;
            }

            internal void WriteVertexBuffers(BinaryWriterEx bw, int bufferCount, int meshIndex)
            {
                for (int i = 0; i < bufferCount; i++)
                {
                    bw.WriteInt32(LayoutIndex);
                    bw.ReserveInt32($"BufferLength1_{meshIndex}");
                    bw.ReserveInt32($"BufferOffset1_{meshIndex}");
                    bw.WriteInt32(0);
                }
            }
        }
    }
}
