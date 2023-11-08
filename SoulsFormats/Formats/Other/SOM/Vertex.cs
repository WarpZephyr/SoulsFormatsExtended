using System.Numerics;

namespace SoulsFormats.SOM
{
    public partial class MDO
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
            /// Vector pointing away from the surface.
            /// </summary>
            public Vector3 Normal;

            /// <summary>
            /// Texture coordinate of the vertex.
            /// </summary>
            public Vector2 UV;

            internal Vertex(BinaryReaderEx br)
            {
                Position = br.ReadVector3();
                Normal = br.ReadVector3();
                UV = br.ReadVector2();
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteVector3(Position);
                bw.WriteVector3(Normal);
                bw.WriteVector2(UV);
            }
        }
    }
}
