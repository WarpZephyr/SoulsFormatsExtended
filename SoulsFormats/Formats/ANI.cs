using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// Armored Core For Answer animations.
    /// </summary>
    public class ANI : SoulsFile<ANI>
    {
        /// <summary>
        /// The bone entries in this ANI file.
        /// </summary>
        public List<AnimEntry> AnimEntries { get; set; }

        /// <summary>
        /// A positions array the AnimEntries' FrameData if applicable use.
        /// </summary>
        public Vector3[] Positions { get; set; }

        /// <summary>
        /// A rotations array the AnimEntries' FrameData if applicable use.
        /// </summary>
        public Vector3[] Rotations { get; set; }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;
            int unk00 = br.AssertInt32(0x20051014);
            int unk04 = br.AssertInt32(0);
            int keyFrameCount = br.ReadInt32();
            int animEntryOffset = br.ReadInt32();
            int animEntryCount = br.ReadInt32();
            int framePositionOffset = br.ReadInt32();
            int frameRotationOffset = br.ReadInt32();
            int framePositionCount = br.ReadInt32();
            int frameRotationCount = br.ReadInt32();
            int dataSize = br.ReadInt32();

            if (!(dataSize == br.Length || dataSize < br.Length))
                throw new InvalidDataException("Data size value was greater than file size.");

            if (dataSize < br.Length)
            {
                br.StepIn(dataSize);
                br.AssertPattern((int)br.Length - dataSize, 0);
                br.StepOut();
            }

            int unk28 = br.AssertInt32(0);
            int unk2C = br.AssertInt32(1);
            byte unk30 = br.AssertByte(1);
            byte unk31 = br.AssertByte(1);

            br.AssertPattern(70, 0);

            Positions = new Vector3[framePositionCount];
            Rotations = new Vector3[frameRotationCount];

            br.StepIn(framePositionOffset);
            for (int i = 0; i < framePositionCount; i++)
                Positions[i] = br.ReadVector3();
            br.StepOut();

            br.StepIn(frameRotationOffset);
            for (int i = 0; i < framePositionCount; i++)
                Rotations[i] = ReadVector3Short(br);
            br.StepOut();

            br.StepIn(animEntryOffset);
            for (int i = 0; i < animEntryCount; i++)
                AnimEntries.Add(new AnimEntry(br));
            br.StepOut();
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            bw.WriteInt32(0x20051014);
            bw.WriteInt32(0);
            bw.WriteInt32(GetMaxKeyFrameValue());
            bw.WriteInt32(120); // AnimEntryOffset
            bw.WriteInt32(AnimEntries.Count);
            bw.ReserveInt32("FramePositionsOffset");
            bw.ReserveInt32("FrameRotationsOffset");
            bw.WriteInt32(Positions.Length);
            bw.WriteInt32(Rotations.Length);
            bw.ReserveInt32("DataSize");
            bw.WriteInt32(0);
            bw.WriteInt32(1);
            bw.WriteByte(1);
            bw.WriteByte(1);
            bw.WritePattern(70, 0);

            for (int i = 0; i < AnimEntries.Count; i++)
                AnimEntries[i].Write(bw, i);

            for (int i = 0; i < AnimEntries.Count; i++)
            {
                bw.FillInt32($"BoneNameOffset_{i}", (int)bw.Position);
                bw.WriteShiftJIS(AnimEntries[i].BoneName, true);
                if (AnimEntries[i].HasAnimationData)
                {
                    bw.FillInt32($"AnimDataOffset_{i}", (int)bw.Position);
                    AnimEntries[i].AnimationData.Write(bw);
                }
            }

            bw.FillInt32("FramePositionsOffset", (int)bw.Position);
            foreach (var position in Positions)
                bw.WriteVector3(position);
            bw.FillInt32("FrameRotationsOffset", (int)bw.Position);
            foreach (var rotation in Rotations)
                WriteVector3Short(bw, rotation);
            bw.FillInt32("DataSize", (int)bw.Position);
        }

        /// <summary>
        /// Checks whether the data appears to be a file of this format.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            br.BigEndian = true;
            if (br.Length < 64)
                return false;

            return br.ReadInt32() == 0x20051014;
        }

        /// <summary>
        /// Reads three shorts which are divided by 1000.0f into floats to get a Vector3.
        /// </summary>
        /// <param name="br">A BinaryReaderEx.</param>
        /// <returns>A Vector3.</returns>
        private Vector3 ReadVector3Short(BinaryReaderEx br)
        {
            return new Vector3(br.ReadInt16() / 1000.0f, br.ReadInt16() / 1000.0f, br.ReadInt16() / 1000.0f);
        }

        /// <summary>
        /// Write a Vector3 into 3 shorts by multipying its coordinates by 1000.
        /// </summary>
        /// <param name="bw">A BinaryWriterEx.</param>
        /// <param name="vector">The Vector3 to write.</param>
        private void WriteVector3Short(BinaryWriterEx bw, Vector3 vector)
        {
            bw.WriteInt16((short)(vector.X * 1000));
            bw.WriteInt16((short)(vector.Y * 1000));
            bw.WriteInt16((short)(vector.Z * 1000));
        }

        /// <summary>
        /// Gets the max key frame value from the AnimEntries for writing.
        /// </summary>
        /// <returns>The max key frame value.</returns>
        private int GetMaxKeyFrameValue()
        {
            int value = 0;
            foreach (var entry in AnimEntries)
                if(entry.HasAnimationData)
                    foreach (var frame in entry.AnimationData.Frames)
                        if (frame.KeyFrame > value)
                            value = frame.KeyFrame;
            return value;
        }

        /// <summary>
        /// An entry for animating a bone.
        /// </summary>
        public class AnimEntry
        {
            /// <summary>
            /// Whether or not the entry has AnimData.
            /// </summary>
            public bool HasAnimationData { get => AnimationData != null; }

            /// <summary>
            /// Unknown, always 0, 1, or 2.
            /// </summary>
            public int Unk04 { get; set; }

            /// <summary>
            /// The index of this entry.
            /// </summary>
            public short BoneIndex { get; set; }

            /// <summary>
            /// The index of another entry, connection unknown.
            /// </summary>
            public short Unk08Index { get; set; }

            /// <summary>
            /// The index of the parent entry.
            /// </summary>
            public short ParentIndex { get; set; }

            /// <summary>
            /// The index of the child entry.
            /// </summary>
            public short ChildIndex { get; set; }

            /// <summary>
            /// The index of the next sibling entry.
            /// </summary>
            public short NextSiblingIndex { get; set; }

            /// <summary>
            /// Unknown, always seems to be -1, but not all files have been tested yet.
            /// </summary>
            public short Unk12Index { get; set; }

            /// <summary>
            /// Where the bone will be moved when animating.
            /// </summary>
            public Vector3 BoneTranslation { get; set; }

            /// <summary>
            /// How the bone will rotate when animating.
            /// </summary>
            public Vector3 BoneRotation { get; set; }

            /// <summary>
            /// The size the bone will be when animating.
            /// </summary>
            public Vector3 BoneScale { get; set; }

            /// <summary>
            /// The name of the bone this entry is for.
            /// </summary>
            public string BoneName { get; set; }

            /// <summary>
            /// Data holding indices for positions and rotations.
            /// </summary>
            public AnimData AnimationData { get; set; }

            /// <summary>
            /// Read an AnimEntry from a stream.
            /// </summary>
            /// <param name="br">A BinaryReaderEx.</param>
            internal AnimEntry(BinaryReaderEx br)
            {
                int boneNameOffset = br.ReadInt32();

                if (boneNameOffset < 1)
                    throw new InvalidDataException("Entry must have a bone name.");

                BoneName = br.GetShiftJIS(boneNameOffset);
                Unk04 = br.AssertInt16(0, 1, 2);
                BoneIndex = br.ReadInt16();
                Unk08Index = br.ReadInt16();
                ParentIndex = br.ReadInt16();
                ChildIndex = br.ReadInt16();
                NextSiblingIndex = br.ReadInt16();
                Unk12Index = br.ReadInt16();
                BoneTranslation = br.ReadVector3();
                BoneRotation = br.ReadVector3();
                BoneScale = br.ReadVector3();
                int animDataOffset = br.ReadInt32();

                if (animDataOffset > 0)
                    AnimationData = new AnimData(br);
            }

            /// <summary>
            /// Write an animation entry to a stream.
            /// </summary>
            /// <param name="bw">A BinaryWriterEx.</param>
            /// <param name="index">The index of the entry for reserving data offsets.</param>
            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.ReserveInt32($"BoneNameOffset_{index}");
                bw.WriteInt32(Unk04);
                bw.WriteInt16(BoneIndex);
                bw.WriteInt16(Unk08Index);
                bw.WriteInt16(ParentIndex);
                bw.WriteInt16(ChildIndex);
                bw.WriteInt16(NextSiblingIndex);
                bw.WriteInt16(Unk12Index);
                bw.WriteVector3(BoneTranslation);
                bw.WriteVector3(BoneRotation);
                bw.WriteVector3(BoneScale);
                bw.WritePattern(184, 0);

                if (HasAnimationData)
                    bw.ReserveInt32($"AnimDataOffset_{index}");
            }

            /// <summary>
            /// Data holding indices for positions and rotations.
            /// </summary>
            public class AnimData
            {
                /// <summary>
                /// Unknown, seems to determine whether position and rotation indices are present, and in how many bytes.
                /// </summary>
                public enum AnimType : int
                {
                    /// <summary>
                    /// Position and rotation indices stored as bytes.
                    /// </summary>
                    PosRotBytes = 1,

                    /// <summary>
                    /// Position and rotation indices stored as shorts.
                    /// </summary>
                    PosRotShorts = 2,

                    /// <summary>
                    /// Rotation indices stored as shorts.
                    /// </summary>
                    RotShorts = 4
                }

                /// <summary>
                /// Unknown, seems to determine whether position and rotation indices are present, and in how many bytes.
                /// </summary>
                public AnimType AnimationType { get; set; }

                /// <summary>
                /// Unknown, assumed to be a Vector3 rotation.
                /// </summary>
                public Vector3 Unk10 { get; set; }

                /// <summary>
                /// Unknown, assumed to be a Vector3 rotation.
                /// </summary>
                public Vector3 Unk20 { get; set; }

                /// <summary>
                /// The data for frames in this AnimData.
                /// </summary>
                public List<FrameData> Frames { get; set; }

                /// <summary>
                /// Read AnimData from a stream.
                /// </summary>
                /// <param name="br">A BinaryReaderEx.</param>
                internal AnimData(BinaryReaderEx br)
                {
                    int frameDataOffset = br.ReadInt32();
                    if (frameDataOffset < br.Position + 36)
                        throw new InvalidDataException("FrameData offset must not be behind AnimData.");
                    int frameCount = br.ReadInt32();
                    AnimationType = br.ReadEnum32<AnimType>();
                    Unk10 = br.ReadVector3();
                    Unk20 = br.ReadVector3();
                    br.AssertInt32(0);

                    br.Position = frameDataOffset;
                    Frames = new List<FrameData>();
                    for (int i = 0; i < frameCount; i++)
                        Frames.Add(new FrameData(br, AnimationType));
                }

                /// <summary>
                /// Write AnimData to a stream.
                /// </summary>
                /// <param name="bw">A BinaryWriterEx.</param>
                internal void Write(BinaryWriterEx bw)
                {
                    bw.WriteInt32((int)bw.Position + 36);
                    bw.WriteInt32(Frames.Count);
                    bw.WriteInt32((int)AnimationType);
                    bw.WriteVector3(Unk10);
                    bw.WriteVector3(Unk20);
                    bw.WriteInt32(0);

                    foreach (var frame in Frames)
                        frame.Write(bw, AnimationType);
                }

                /// <summary>
                /// Data for frames.
                /// </summary>
                public class FrameData
                {
                    /// <summary>
                    /// The current key frame value?
                    /// </summary>
                    public short KeyFrame { get; set; }

                    /// <summary>
                    /// The index of the position in the positions array.
                    /// </summary>
                    public short PositionIndex { get; set; }

                    /// <summary>
                    /// An unknown index.
                    /// </summary>
                    public short UnkIndex2 { get; set; }

                    /// <summary>
                    /// An unknown index.
                    /// </summary>
                    public short UnkIndex3 { get; set; }

                    /// <summary>
                    /// The index of the rotation in the rotations array.
                    /// </summary>
                    public short RotationIndex { get; set; }

                    /// <summary>
                    /// An unknown index.
                    /// </summary>
                    public short UnkIndex5 { get; set; }

                    /// <summary>
                    /// An unknown index.
                    /// </summary>
                    public short UnkIndex6 { get; set; }

                    /// <summary>
                    /// An unknown index.
                    /// </summary>
                    public short UnkIndex7 { get; set; }

                    /// <summary>
                    /// Read FrameData from a stream.
                    /// </summary>
                    /// <param name="br">A BinaryReaderEx.</param>
                    /// <param name="animationType">The animation type.</param>
                    internal FrameData(BinaryReaderEx br, AnimType animationType)
                    {
                        KeyFrame = br.ReadInt16();
                        switch (animationType)
                        {
                            case AnimType.PosRotBytes:
                                PositionIndex = br.ReadByte();
                                UnkIndex2 = br.ReadByte();
                                UnkIndex3 = br.ReadByte();
                                RotationIndex = br.ReadByte();
                                UnkIndex5 = br.ReadByte();
                                UnkIndex6 = br.ReadByte();
                                break;
                            case AnimType.PosRotShorts:
                                PositionIndex = br.ReadInt16();
                                UnkIndex2 = br.ReadInt16();
                                UnkIndex3 = br.ReadInt16();
                                RotationIndex = br.ReadInt16();
                                UnkIndex5 = br.ReadInt16();
                                UnkIndex6 = br.ReadInt16();
                                UnkIndex7 = br.ReadInt16();
                                break;
                            case AnimType.RotShorts:
                                RotationIndex = br.ReadInt16();
                                UnkIndex5 = br.ReadInt16();
                                UnkIndex6 = br.ReadInt16();
                                break;
                            default:
                                throw new NotImplementedException($"AnimType \"{animationType}\" has not been implemented in index reading.");
                        }
                    }

                    /// <summary>
                    /// Write FrameData to a stream.
                    /// </summary>
                    /// <param name="bw">A BinaryWriterEx.</param>
                    /// <param name="animationType">The animation type.</param>
                    internal void Write(BinaryWriterEx bw, AnimType animationType)
                    {
                        bw.WriteInt32(KeyFrame);
                        switch (animationType)
                        {
                            case AnimType.PosRotBytes:
                                if (PositionIndex < 0 || UnkIndex2 < 0 || UnkIndex3 < 0 || RotationIndex < 0 || UnkIndex5 < 0 || UnkIndex6 < 0
                                 || PositionIndex > 255 || UnkIndex2 > 255 || UnkIndex3 > 255 || RotationIndex > 255 || UnkIndex5 > 255 || UnkIndex6 > 255)
                                    throw new InvalidCastException("Indices must be able to fit within an unsigned byte for this AnimType.");

                                bw.WriteByte((byte)PositionIndex);
                                bw.WriteByte((byte)UnkIndex2);
                                bw.WriteByte((byte)UnkIndex3);
                                bw.WriteByte((byte)RotationIndex);
                                bw.WriteByte((byte)UnkIndex5);
                                bw.WriteByte((byte)UnkIndex6);
                                break;
                            case AnimType.PosRotShorts:
                                bw.WriteInt16(PositionIndex);
                                bw.WriteInt16(UnkIndex2);
                                bw.WriteInt16(UnkIndex3);
                                bw.WriteInt16(RotationIndex);
                                bw.WriteInt16(UnkIndex5);
                                bw.WriteInt16(UnkIndex6);
                                bw.WriteInt16(UnkIndex7);
                                break;
                            case AnimType.RotShorts:
                                bw.WriteInt16(RotationIndex);
                                bw.WriteInt16(UnkIndex5);
                                bw.WriteInt16(UnkIndex6);
                                break;
                            default:
                                throw new NotImplementedException($"AnimType \"{animationType}\" has not been implemented in index writing.");
                        }
                    }

                    /// <summary>
                    /// Get the position of this frame.
                    /// </summary>
                    /// <param name="positions">The positions array from the ANI itself.</param>
                    /// <returns>The position of this frame.</returns>
                    public Vector3 GetPosition(Vector3[] positions)
                    {
                        return positions[PositionIndex];
                    }

                    /// <summary>
                    /// Get the rotation of this frame.
                    /// </summary>
                    /// <param name="rotations">The rotations array from the ANI itself.</param>
                    /// <returns>The rotation of this frame.</returns>
                    public Vector3 GetRotation(Vector3[] rotations)
                    {
                        return rotations[RotationIndex];
                    }
                }
            }
        }
    }
}
