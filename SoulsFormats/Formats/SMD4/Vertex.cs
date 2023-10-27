using System.Numerics;

namespace SoulsFormats
{
    public partial class SMD4
    {
        /// <summary>
        /// A single point in a mesh.
        /// </summary>
        public class Vertex
        {
            /// <summary>
            /// Where the vertex is.
            /// </summary>
            public Vector3 Position { get; set; }

            /// <summary>
            /// The index of the bone the vertex is weighted to.
            /// </summary>
            public short BoneIndex { get; set; }

            /// <summary>
            /// Create a new Vertex with default values.
            /// </summary>
            public Vertex(){}

            /// <summary>
            /// Clone an existing Vertex.
            /// </summary>
            public Vertex(Vertex vertex)
            {
                Position = vertex.Position;
                BoneIndex = vertex.BoneIndex;
            }

            /// <summary>
            /// Read a Vertex from a stream.
            /// </summary>
            internal Vertex(BinaryReaderEx br)
            {
                Position = br.ReadVector3();
                BoneIndex = br.ReadInt16();
                br.AssertInt16(0);
            }

            /// <summary>
            /// Write a Vertex to a stream.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteVector3(Position);
                bw.WriteInt16(BoneIndex);
                bw.WriteInt16(0);
            }
        }
    }
}
