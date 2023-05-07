using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats.Other
{
    public partial class MDL : SoulsFile<MDL>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class Bone
        {
            public Vector3 Translation;
            public Vector3 Rotation;
            public Vector3 Scale;
            public int ParentIndex;
            public int ChildIndex;
            public int NextSiblingIndex;
            public int PreviousSiblingIndex;
            public List<Faceset> FacesetsA;
            public List<Faceset> FacesetsB;
            public List<FacesetC> FacesetsC;
            public List<FacesetC> FacesetsD;
            public int Unk54;
            public Vector3 BoundingBoxMin;
            public Vector3 BoundingBoxMax;
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
                int facesetCountC = br.ReadInt32();
                int facesetCountD = br.ReadInt32();
                int facesetsOffsetA = br.ReadInt32();
                int facesetsOffsetB = br.ReadInt32();
                int facesetsOffsetC = br.ReadInt32();
                int facesetsOffsetD = br.ReadInt32();
                Unk54 = br.ReadInt32();
                BoundingBoxMin = br.ReadVector3();
                BoundingBoxMax = br.ReadVector3();
                Unk70 = br.ReadInt16s(10);
                br.AssertPattern(0xC, 0x00);

                br.StepIn(facesetsOffsetA);
                {
                    FacesetsA = new List<Faceset>(facesetCountA);
                    for (int i = 0; i < facesetCountA; i++)
                        FacesetsA.Add(new Faceset(br));
                }
                br.StepOut();

                br.StepIn(facesetsOffsetB);
                {
                    FacesetsB = new List<Faceset>(facesetCountB);
                    for (int i = 0; i < facesetCountB; i++)
                        FacesetsB.Add(new Faceset(br));
                }
                br.StepOut();

                br.StepIn(facesetsOffsetC);
                {
                    FacesetsC = new List<FacesetC>(facesetCountC);
                    for (int i = 0; i < facesetCountC; i++)
                        FacesetsC.Add(new FacesetC(br));
                }
                br.StepOut();

                br.StepIn(facesetsOffsetD);
                {
                    FacesetsD = new List<FacesetC>(facesetCountD);
                    for (int i = 0; i < facesetCountD; i++)
                        FacesetsD.Add(new FacesetC(br));
                }
                br.StepOut();
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
