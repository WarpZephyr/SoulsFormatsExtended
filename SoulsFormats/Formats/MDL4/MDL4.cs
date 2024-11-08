using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// A 3D model format used in early PS3/X360 games. Extension: .mdl
    /// </summary>
    public partial class MDL4 : SoulsFile<MDL4>
    {
        /// <summary>
        /// General values for this model.
        /// </summary>
        public MDLHeader Header { get; set; }

        /// <summary>
        /// Dummy polygons in this model.
        /// </summary>
        public List<Dummy> Dummies;

        /// <summary>
        /// Materials in this model, usually one per mesh.
        /// </summary>
        public List<Material> Materials;

        /// <summary>
        /// Bones used by this model, may or may not be the full skeleton.
        /// </summary>
        public List<Bone> Bones;

        /// <summary>
        /// Individual chunks of the model.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// Returns true if the data appears to be an MDL4 model.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(0, 4);
            return magic == "MDL4";
        }

        /// <summary>
        /// Reads an MDL4 from a BinaryReaderEx.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;
            br.AssertASCII("MDL4");

            Header = new MDLHeader();
            Header.Version = br.AssertInt32(0x40001, 0x40002);
            int dataOffset = br.ReadInt32();
            int dataLength = br.ReadInt32();
            int dummyCount = br.ReadInt32();
            int materialCount = br.ReadInt32();
            int boneCount = br.ReadInt32();
            int meshCount = br.ReadInt32();
            int vertexBufferCount = br.ReadInt32(); // Vertex Buffer Count Probably
            Header.BoundingBoxMin = br.ReadVector3();
            Header.BoundingBoxMax = br.ReadVector3();
            int trueFaceCount = br.ReadInt32();
            int totalFaceCount = br.ReadInt32();
            br.AssertPattern(0x3C, 0x00);

            Dummies = new List<Dummy>(dummyCount);
            for (int i = 0; i < dummyCount; i++)
                Dummies.Add(new Dummy(br));

            Materials = new List<Material>(materialCount);
            for (int i = 0; i < materialCount; i++)
                Materials.Add(new Material(br));

            Bones = new List<Bone>(boneCount);
            for (int i = 0; i < boneCount; i++)
                Bones.Add(new Bone(br));

            Meshes = new List<Mesh>(meshCount);
            for (int i = 0; i < meshCount; i++)
                Meshes.Add(new Mesh(br, dataOffset, Header.Version));
        }

        /// <summary>
        /// Writes MDL4 data to a BinaryWriterEx.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = true;
            bw.WriteASCII("MDL4");
            bw.WriteInt32(Header.Version);
            bw.ReserveInt32("DataOffset");
            bw.ReserveInt32("DataLength");
            bw.WriteInt32(Dummies.Count);
            bw.WriteInt32(Materials.Count);
            bw.WriteInt32(Bones.Count);
            bw.WriteInt32(Meshes.Count);
            bw.WriteInt32(Meshes.Count); // Vertex Buffer Count Probably
            bw.WriteVector3(Header.BoundingBoxMin);
            bw.WriteVector3(Header.BoundingBoxMax);

            int faceCount = 0;
            foreach (var mesh in Meshes)
                faceCount += mesh.GetFaceCount(true, true);

            int indexCount = faceCount * 3;
            bw.WriteInt32(faceCount); // Not entirely accurate but oh well
            bw.WriteInt32(indexCount); // Not entirely accurate but oh well

            bw.WritePattern(0x3C, 0x00);

            foreach (Dummy dummy in Dummies)
                dummy.Write(bw);

            foreach (Material material in Materials)
                material.Write(bw);

            foreach (Bone bone in Bones)
                bone.Write(bw);

            for (int i = 0; i < Meshes.Count; i++)
                Meshes[i].Write(bw, i);

            // Fill the offsets of the vertex indices and buffers
            int dataStart = (int)bw.Position;
            bw.FillInt32("DataOffset", dataStart);
            for (int i = 0; i < Meshes.Count; i++)
            {
                int vertexIndexStart = (int)bw.Position - dataStart;
                bw.FillInt32($"VertexIndicesOffset_{i}", vertexIndexStart);
                bw.WriteUInt16s(Meshes[i].Indices);
                int vertexIndexEnd = (int)bw.Position - dataStart;

                bw.FillInt32($"VertexIndicesLength_{i}", vertexIndexEnd - vertexIndexStart);

                int bufferStart = (int)bw.Position - dataStart;
                bw.FillInt32($"BufferOffset_{i}", bufferStart);
                foreach (Vertex vertex in Meshes[i].Vertices)
                    vertex.Write(bw, Header.Version, Meshes[i].VertexFormat);
                int bufferEnd = (int)bw.Position - dataStart;
                bw.FillInt32($"BufferLength_{i}", bufferEnd - bufferStart);
            }
            int dataEnd = (int)bw.Position;
            bw.FillInt32($"DataLength", dataEnd - dataStart);
        }

        /// <summary>
        /// Compute the world transform for a bone.
        /// </summary>
        /// <param name="index">The index of the bone to compute the world transform of.</param>
        /// <returns>A matrix representing the world transform of the bone.</returns>
        public Matrix4x4 ComputeBoneWorldMatrix(int index)
        {
            var bone = Bones[index];
            Matrix4x4 matrix = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                bone = Bones[bone.ParentIndex];
                matrix *= bone.ComputeLocalTransform();
            }

            return matrix;
        }

        /// <summary>
        /// Compute the world transform for a bone.
        /// </summary>
        /// <param name="bone">The bone to compute the world transform of.</param>
        /// <returns>A matrix representing the world transform of the bone.</returns>
        public Matrix4x4 ComputeBoneWorldMatrix(Bone bone)
        {
            Matrix4x4 matrix = bone.ComputeLocalTransform();
            while (bone.ParentIndex != -1)
            {
                bone = Bones[bone.ParentIndex];
                matrix *= bone.ComputeLocalTransform();
            }

            return matrix;
        }

        /// <summary>
        /// An MDL4 header containing general values for this model.
        /// </summary>
        public class MDLHeader
        {
            /// <summary>
            /// Version of the format indicating presence of various features.
            /// </summary>
            public int Version;

            /// <summary>
            /// Minimum extent of the entire model.
            /// </summary>
            public Vector3 BoundingBoxMin;

            /// <summary>
            /// Maximum extent of the entire model.
            /// </summary>
            public Vector3 BoundingBoxMax;

            /// <summary>
            /// Creates a MDLHeader with default values.
            /// </summary>
            public MDLHeader()
            {
                Version = 0x40001;
            }
        }
    }
}
