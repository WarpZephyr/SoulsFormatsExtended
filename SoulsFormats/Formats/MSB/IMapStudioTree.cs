using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// A hierarchy of Axis-Aligned Bounding Boxes used in various calculations.
    /// </summary>
    public interface IMapStudioTree
    {
        /// <summary>
        /// The root node of the Bounding Volume Hierarchy.<br/>
        /// Set to null when not calculated yet.
        /// </summary>
        public ITreeNode RootNode { get; }

        /// <summary>
        /// A node in the Bounding Volume Hierarchy.
        /// </summary>
        public interface ITreeNode
        {
            /// <summary>
            /// The Bounding Box for this node.
            /// </summary>
            public IBoundingBox Bounds { get; }

            /// <summary>
            /// The first child node of this node.
            /// </summary>
            public ITreeNode LeftChild { get; }

            /// <summary>
            /// The second child node of this node.
            /// </summary>
            public ITreeNode RightChild { get; }

            /// <summary>
            /// The parts this node contains.
            /// </summary>
            public IList<short> PartIndices { get; }

            /// <summary>
            /// Get the total node count starting from this node.
            /// </summary>
            /// <returns>The total node count.</returns>
            public int GetNodeCount();

            /// <summary>
            /// A bounding box used in various calculations between entities.
            /// </summary>
            public interface IBoundingBox
            {
                /// <summary>
                /// The minimum extent of the bounding box.
                /// </summary>
                public Vector3 Minimum { get; }

                /// <summary>
                /// The maximum extent of the bounding box.
                /// </summary>
                public Vector3 Maximum { get; }

                /// <summary>
                /// The origin of the bounding box, calculated from the minimum and maximum extent.
                /// </summary>
                public Vector3 Origin { get; }

                /// <summary>
                /// The distance between the furthest vertex of the bounding box and origin.
                /// </summary>
                // Does not seem entirely accurate, but is very close, might just be precision differences.
                public float Radius { get; }
            }
        }
    }
}
