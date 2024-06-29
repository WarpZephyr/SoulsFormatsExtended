using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats.Other
{
    public partial class MDL : SoulsFile<MDL>
    {
        /// <summary>
        /// A joint available for vertices to be attached to.<br/>
        /// Also holds meshes/facesets?
        /// </summary>
        public class Bone
        {
            /// <summary>
            /// Translation of this bone.
            /// </summary>
            public Vector3 Translation;

            /// <summary>
            /// Rotation of this bone.
            /// </summary>
            public Vector3 Rotation;

            /// <summary>
            /// Scale of this bone.
            /// </summary>
            public Vector3 Scale;

            /// <summary>
            /// Index of the parent in this FLVER's bone collection, or -1 for none.
            /// </summary>
            public int ParentIndex;

            /// <summary>
            /// Index of the first child in this FLVER's bone collection, or -1 for none.
            /// </summary>
            public int ChildIndex;

            /// <summary>
            /// Index of the next child of this bone's parent, or -1 for none.
            /// </summary>
            public int NextSiblingIndex;

            /// <summary>
            /// Index of the previous child of this bone's parent, or -1 for none.
            /// </summary>
            public int PreviousSiblingIndex;

            /// <summary>
            /// A list of meshes/facesets?
            /// </summary>
            public List<Mesh> FacesetsA;

            /// <summary>
            /// A list of meshes/facesets?
            /// </summary>
            public List<Mesh> FacesetsB;

            /// <summary>
            /// Groups of lists of meshes/facesets?
            /// </summary>
            public List<MeshGroup> FacesetGroupsA;

            /// <summary>
            /// Groups of lists of meshes/facesets?
            /// </summary>
            public List<MeshGroup> FacesetGroupsB;

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk54;

            /// <summary>
            /// Minimum extent of the vertices weighted to this bone.
            /// </summary>
            public Vector3 BoundingBoxMin;

            /// <summary>
            /// Maximum extent of the vertices weighted to this bone.
            /// </summary>
            public Vector3 BoundingBoxMax;

            /// <summary>
            /// Unknown.
            /// </summary>
            public short[] Unk70;

            internal Bone(BinaryReaderEx br)
            {
                Translation = br.ReadVector3();
                Rotation = br.ReadVector3();
                Scale = br.ReadVector3();
                ParentIndex = br.ReadInt32();
                ChildIndex = br.ReadInt32();
                NextSiblingIndex = br.ReadInt32();
                PreviousSiblingIndex = br.ReadInt32();
                int facesetCountA = br.ReadInt32();
                int facesetCountB = br.ReadInt32();
                int facesetGroupCountA = br.ReadInt32();
                int facesetGroupCountB = br.ReadInt32();
                int facesetsOffsetA = br.ReadInt32();
                int facesetsOffsetB = br.ReadInt32();
                int facesetGroupsOffsetA = br.ReadInt32();
                int facesetGroupsOffsetB = br.ReadInt32();
                Unk54 = br.ReadInt32();
                BoundingBoxMin = br.ReadVector3();
                BoundingBoxMax = br.ReadVector3();
                Unk70 = br.ReadInt16s(10);
                br.AssertPattern(0xC, 0x00);

                br.StepIn(facesetsOffsetA);
                {
                    FacesetsA = new List<Mesh>(facesetCountA);
                    for (int i = 0; i < facesetCountA; i++)
                        FacesetsA.Add(new Mesh(br));
                }
                br.StepOut();

                br.StepIn(facesetsOffsetB);
                {
                    FacesetsB = new List<Mesh>(facesetCountB);
                    for (int i = 0; i < facesetCountB; i++)
                        FacesetsB.Add(new Mesh(br));
                }
                br.StepOut();

                br.StepIn(facesetGroupsOffsetA);
                {
                    FacesetGroupsA = new List<MeshGroup>(facesetGroupCountA);
                    for (int i = 0; i < facesetGroupCountA; i++)
                        FacesetGroupsA.Add(new MeshGroup(br));
                }
                br.StepOut();

                br.StepIn(facesetGroupsOffsetB);
                {
                    FacesetGroupsB = new List<MeshGroup>(facesetGroupCountB);
                    for (int i = 0; i < facesetGroupCountB; i++)
                        FacesetGroupsB.Add(new MeshGroup(br));
                }
                br.StepOut();
            }
        }
    }
}
