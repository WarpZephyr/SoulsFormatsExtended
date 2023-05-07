using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsFormats
{
    public partial class MSBFA
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        private class MapStudioTree : Param<Tree>
        {
            internal override int Version => throw new NotImplementedException();

            /// <summary>
            /// The name string indicating the param type this is.
            /// </summary>
            internal override string Name => "MAPSTUDIO_TREE_ST";

            /// <summary>
            /// A list of Trees in this MapStudioTree.
            /// </summary>
            public List<Tree> Trees { get; set; }

            /// <summary>
            /// Create a new MapStudioTree with a list of empty Trees.
            /// </summary>
            public MapStudioTree()
            {
                Trees = new List<Tree>();
            }

            /// <summary>
            /// Read a Tree entry in a MapStudioTree.
            /// </summary>
            internal override Tree ReadEntry(BinaryReaderEx br)
            {
                return Trees.EchoAdd(new Tree(br));
            }

            /// <summary>
            /// Get the Tree entries in a MapStudioTree.
            /// </summary>
            public override List<Tree> GetEntries()
            {
                return Trees;
            }
        }

        /// <summary>
        /// Unknown.
        /// </summary>
        public class Tree : Entry
        {
            /// <summary>
            /// Creates a Tree with default values.
            /// </summary>
            public Tree()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Creates a deep copy of the tree.
            /// </summary>
            public Tree DeepCopy()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Read a Tree from a BinaryReaderEx.
            /// </summary>
            internal Tree(BinaryReaderEx br)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Write a Tree to a BinaryWriterEx.
            /// </summary>
            internal override void Write(BinaryWriterEx bw, int id)
            {
                throw new NotImplementedException();
            }
        }
    }
}
