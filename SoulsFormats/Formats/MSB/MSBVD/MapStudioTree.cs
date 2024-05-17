﻿using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SoulsFormats
{
    public partial class MSBVD
    {
        /// <summary>
        /// A hierarchy of Axis-Aligned Bounding Boxes used in calculations such as drawing and collision.
        /// </summary>
        public class MapStudioTree : IMapStudioTree
        {
            /// <summary>
            /// Unknown; probably some kind of version number.
            /// </summary>
            public int Version { get; set; }

            /// <summary>
            /// The Name or Type of the Param.
            /// </summary>
            private protected string Name = "MAPSTUDIO_TREE_ST";

            /// <summary>
            /// The root node of the Bounding Volume Hierarchy.<br/>
            /// Set to null when not calculated yet.
            /// </summary>
            public TreeNode RootNode { get; set; }
            IMapStudioTree.ITreeNode IMapStudioTree.RootNode => RootNode;

            /// <summary>
            /// Create a new <see cref="MapStudioTree"/>.
            /// </summary>
            public MapStudioTree()
            {
                // Set node null as we need to calculate that later
                RootNode = null;
                Version = 10001002;
            }

            internal TreeNode Read(BinaryReaderEx br)
            {
                Version = br.ReadInt32();
                int nameOffset = br.ReadInt32();
                int offsetCount = br.ReadInt32();
                int rootNodeOffset = br.ReadInt32();
                br.Skip((offsetCount - 2) * 4); // Entry Offsets
                int nextParamOffset = br.ReadInt32();

                string name = br.GetASCII(nameOffset);
                if (name != Name)
                    throw new InvalidDataException($"Expected param \"{Name}\", got param \"{name}\"");

                if (offsetCount - 1 != 0)
                {
                    br.Position = rootNodeOffset;
                    RootNode = new TreeNode(br);
                    br.Position = nextParamOffset;
                }
                else
                {
                    RootNode = null;
                }
                return RootNode;
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteInt32(Version);
                bw.ReserveInt32("ParamNameOffset");
                int count = RootNode.GetNodeCount();
                bw.WriteInt32(count + 1);
                for (int i = 0; i < count; i++)
                {
                    bw.ReserveInt32($"OffsetTreeNode_{i}");
                }
                bw.ReserveInt32("NextParamOffset");

                bw.FillInt32("ParamNameOffset", (int)bw.Position);
                bw.WriteASCII(Name, true);
                bw.Pad(4);

                int index = 0;
                RootNode.Write(bw, ref index);
            }
        }

        /// <summary>
        /// A node in the Bounding Volume Hierarchy.
        /// </summary>
        public class TreeNode : IMapStudioTree.ITreeNode
        {
            /// <summary>
            /// The <see cref="BoundingBox"/> for this node.
            /// </summary>
            public BoundingBox Bounds { get; set; }
            IMapStudioTree.ITreeNode.IBoundingBox IMapStudioTree.ITreeNode.Bounds => Bounds;

            /// <summary>
            /// The first child node of this node.
            /// </summary>
            public TreeNode LeftChild { get; set; }
            IMapStudioTree.ITreeNode IMapStudioTree.ITreeNode.LeftChild => LeftChild;

            /// <summary>
            /// The second child node of this node.
            /// </summary>
            public TreeNode RightChild { get; set; }
            IMapStudioTree.ITreeNode IMapStudioTree.ITreeNode.RightChild => RightChild;

            /// <summary>
            /// The parts this node contains.
            /// </summary>
            public List<short> PartIndices { get; set; }
            IList<short> IMapStudioTree.ITreeNode.PartIndices => PartIndices;

            /// <summary>
            /// Create a new <see cref="TreeNode"/> with the given bounding information.
            /// </summary>
            /// <param name="min">The minimum extent of the bounding box.</param>
            /// <param name="max">The maximum extent of the bounding box.</param>
            public TreeNode(Vector3 min, Vector3 max)
            {
                PartIndices = new List<short>();
                Bounds = new BoundingBox(min, max);
                LeftChild = null;
                RightChild = null;
            }

            internal TreeNode(BinaryReaderEx br)
            {
                long start = br.Position;
                Vector3 minimum = br.ReadVector3();
                int leftChildOffset = br.ReadInt32();
                Vector3 maximum = br.ReadVector3();
                br.AssertInt32(0); // Unknown
                Vector3 origin = br.ReadVector3();
                int rightChildOffset = br.ReadInt32();
                float radius = br.ReadSingle();
                Bounds = new BoundingBox(minimum, maximum, origin, radius);

                int partIndexCount = br.ReadInt32();
                PartIndices = new List<short>(br.ReadInt16s(partIndexCount));

                if (leftChildOffset > 0)
                {
                    br.Position = start + leftChildOffset;
                    LeftChild = new TreeNode(br);
                }

                if (rightChildOffset > 0)
                {
                    br.Position = start + rightChildOffset;
                    RightChild = new TreeNode(br);
                }
            }

            internal void Write(BinaryWriterEx bw, ref int index)
            {
                long start = bw.Position;
                bw.FillInt32($"OffsetTreeNode_{index}", (int)start);
                string fillStr1 = $"OffsetTreeNodeLeftChild_{index}";
                string fillStr2 = $"OffsetTreeNodeRightChild_{index}";

                bw.WriteVector3(Bounds.Minimum);
                bw.ReserveInt32(fillStr1);
                bw.WriteVector3(Bounds.Maximum);
                bw.WriteInt32(0); // Unknown
                bw.WriteVector3(Bounds.Origin);
                bw.ReserveInt32(fillStr2);
                bw.WriteSingle(Bounds.Radius);
                bw.WriteInt32(PartIndices.Count);
                for (int i = 0; i < PartIndices.Count; i++)
                {
                    bw.WriteInt16(PartIndices[i]);
                }
                bw.Pad(0x10);
                index += 1;

                if (LeftChild != null)
                {
                    bw.FillInt32(fillStr1, (int)(bw.Position - start));
                    LeftChild.Write(bw, ref index);
                }
                else
                {
                    bw.FillInt32(fillStr1, 0);
                }

                if (RightChild != null)
                {
                    bw.FillInt32(fillStr2, (int)(bw.Position - start));
                    RightChild.Write(bw, ref index);
                }
                else
                {
                    bw.FillInt32(fillStr2, 0);
                }
            }

            /// <summary>
            /// Get the total node count starting from this node.
            /// </summary>
            /// <returns>The total node count.</returns>
            public int GetNodeCount()
            {
                int count = 1;
                if (LeftChild != null)
                {
                    count += LeftChild.GetNodeCount();
                }

                if (RightChild != null)
                {
                    count += RightChild.GetNodeCount();
                }

                return count;
            }

            /// <summary>
            /// A bounding box used in various calculations between entities.
            /// </summary>
            public struct BoundingBox : IMapStudioTree.ITreeNode.IBoundingBox
            {
                /// <summary>
                /// The minimum extent of the bounding box.
                /// </summary>
                public Vector3 Minimum;
                readonly Vector3 IMapStudioTree.ITreeNode.IBoundingBox.Minimum => Minimum;

                /// <summary>
                /// The maximum extent of the bounding box.
                /// </summary>
                public Vector3 Maximum;
                readonly Vector3 IMapStudioTree.ITreeNode.IBoundingBox.Maximum => Maximum;

                /// <summary>
                /// The origin of the bounding box, calculated from the minimum and maximum extent.
                /// </summary>
                public Vector3 Origin;

                readonly Vector3 IMapStudioTree.ITreeNode.IBoundingBox.Origin => Origin;

                /// <summary>
                /// The distance between the furthest vertex of the bounding box and origin.
                /// </summary>
                // Does not seem entirely accurate, but is very close, might just be precision differences.
                public float Radius;
                readonly float IMapStudioTree.ITreeNode.IBoundingBox.Radius => Radius;

                /// <summary>
                /// Create a new bounding box with each value already calculated.
                /// </summary>
                /// <param name="min">The minimum extent of the bounding box.</param>
                /// <param name="max">The maximum extent of the bounding box.</param>
                /// <param name="origin">The center of the bounding box.</param>
                /// <param name="radius">The distance between the furthest vertex of the bounding box and origin.</param>
                internal BoundingBox(Vector3 min, Vector3 max, Vector3 origin, float radius)
                {
                    Minimum = min;
                    Maximum = max;
                    Origin = origin;
                    Radius = radius;
                }

                /// <summary>
                /// Create a new bounding box with a minimum and maximum extent.
                /// </summary>
                /// <param name="min">The minimum extent of the bounding box.</param>
                /// <param name="max">The maximum extent of the bounding box.</param>
                public BoundingBox(Vector3 min, Vector3 max)
                {
                    Minimum = min;
                    Maximum = max;
                    Origin = new Vector3((min.X + max.X) / 2, (min.Y + max.Y) / 2, (min.Z + max.Z) / 2);

                    // Get the position of each point on the bounding box
                    Vector3 vec1 = new Vector3(Minimum.X, Minimum.Y, Maximum.Z);
                    Vector3 vec2 = new Vector3(Maximum.X, Minimum.Y, Maximum.Z);
                    Vector3 vec3 = new Vector3(Minimum.X, Maximum.Y, Maximum.Z);
                    Vector3 vec4 = new Vector3(Maximum.X, Maximum.Y, Maximum.Z);
                    Vector3 vec5 = new Vector3(Minimum.X, Minimum.Y, Minimum.Z);
                    Vector3 vec6 = new Vector3(Maximum.X, Minimum.Y, Minimum.Z);
                    Vector3 vec7 = new Vector3(Minimum.X, Maximum.Y, Minimum.Z);
                    Vector3 vec8 = new Vector3(Maximum.X, Maximum.Y, Minimum.Z);

                    // Calculate the distances of each point from the origin
                    float[] distances = new float[8];
                    distances[0] = Vector3.Distance(Origin, vec1);
                    distances[1] = Vector3.Distance(Origin, vec2);
                    distances[2] = Vector3.Distance(Origin, vec3);
                    distances[3] = Vector3.Distance(Origin, vec4);
                    distances[4] = Vector3.Distance(Origin, vec5);
                    distances[5] = Vector3.Distance(Origin, vec6);
                    distances[6] = Vector3.Distance(Origin, vec7);
                    distances[7] = Vector3.Distance(Origin, vec8);

                    // Find the max distance value
                    float maxDistance = distances[0];
                    for (int i = 1; i < 7; i++)
                    {
                        float distance = distances[i];
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                        }
                    }

                    Radius = maxDistance;
                }
            }
        }
    }
}