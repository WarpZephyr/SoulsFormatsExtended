using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    public partial class FLVER2
    {
        /// <summary>
        /// A grouping of <see cref="EdgeIndexBuffer"/>.
        /// </summary>
        internal class EdgeIndexBuffers : List<EdgeIndexBuffer>
        {
            /// <summary>
            /// Read an <see cref="EdgeIndexBuffers"/> from a stream.
            /// </summary>
            /// <param name="br">The stream reader..</param>
            /// <param name="faceIndices">A list to add decompressed face indices to.</param>
            /// <exception cref="InvalidDataException">The member index buffer length was invalid.</exception>
            internal EdgeIndexBuffers(BinaryReaderEx br, List<int> faceIndices)
            {
                long start = br.Position;
                short indexBufferCount = br.ReadInt16();
                br.ReadInt16();
                br.ReadInt16();
                br.ReadByte();
                bool unk07 = br.ReadBoolean();
                br.AssertInt32(0); // Members offset?
                int edgeIndexBufferLength = br.ReadInt32(); // The length of all edge index buffer data plus padding.

                if (!unk07 && edgeIndexBufferLength != 0)
                {
                    throw new InvalidDataException($"{nameof(edgeIndexBufferLength)} must be 0 when {nameof(unk07)} is false.");
                }

                // Read each buffer header (they come before the buffers).
                for (int i = 0; i < indexBufferCount; i++)
                {
                    Add(new EdgeIndexBuffer(br));
                }

                // Decompress each index buffer.
                for (int i = 0; i < indexBufferCount; i++)
                {
                    this[i].GetFaceIndices(br, start, faceIndices);
                }
            }

            /// <summary>
            /// Read all edge vertex buffers referenced by this group.
            /// </summary>
            /// <param name="br">The stream reader.</param>
            /// <returns>A list of <see cref="EdgeVertexBuffer"/>.</returns>
            internal List<EdgeVertexBuffer> ReadEdgeVertexBuffers(BinaryReaderEx br)
            {
                var edgeVertexBuffers = new List<EdgeVertexBuffer>();

                long start = br.Position;
                foreach (var indexBuffer in this)
                {
                    edgeVertexBuffers.Add(indexBuffer.ReadEdgeVertexBuffer(br, start));
                }

                return edgeVertexBuffers;
            }
        }
    }
}
