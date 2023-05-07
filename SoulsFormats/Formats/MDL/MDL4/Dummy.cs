using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// "Dummy polygons" in this MDL4.
        /// </summary>
        public class Dummy
        {
            /// <summary>
            /// Vector indicating the dummy point's forward direction.
            /// </summary>
            public Vector3 Forward;

            /// <summary>
            /// Vector indicating the dummy point's upward direction.
            /// </summary>
            public Vector3 Upward;

            /// <summary>
            /// Unknown; Order of colors also unknown.
            /// </summary>
            public Color Color;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short ID;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk1E;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk20;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short Unk22;

            /// <summary>
            /// Reads a dummy from an MDL4 file.
            /// </summary>
            internal Dummy(BinaryReaderEx br)
            {
                Forward = br.ReadVector3();
                Upward = br.ReadVector3();
                Color = br.ReadARGB(); // Unknown order
                ID = br.ReadInt16();
                Unk1E = br.ReadInt16();
                Unk20 = br.ReadInt16();
                Unk22 = br.ReadInt16();
                br.AssertInt32(0);
                br.AssertInt32(0);
                br.AssertInt32(0);
            }

            /// <summary>
            /// Writes a dummy to an MDL4 file.
            /// </summary>
            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteVector3(Forward);
                bw.WriteVector3(Upward);
                bw.WriteARGB(Color); // Unknown order
                bw.WriteInt16(ID);
                bw.WriteInt16(Unk1E);
                bw.WriteInt16(Unk20);
                bw.WriteInt16(Unk22);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
            }
        }
    }
}
