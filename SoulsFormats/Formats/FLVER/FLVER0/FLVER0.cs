using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SoulsFormats
{
    /// <summary>
    /// 3D models from Armored Core: For Answer to Another Century's Episode R. Extension: .flv, .flver
    /// </summary>
    public partial class FLVER0 : SoulsFile<FLVER0>, IFlver
    {
        /// <summary>
        /// General values for this model.
        /// </summary>
        public FLVERHeader Header { get; set; }

        /// <summary>
        /// Dummy polygons in this model.
        /// </summary>
        public List<FLVER.Dummy> Dummies { get; set; }
        IReadOnlyList<FLVER.Dummy> IFlver.Dummies => Dummies;

        /// <summary>
        /// Materials in this model, usually one per mesh.
        /// </summary>
        public List<Material> Materials { get; set; }
        IReadOnlyList<IFlverMaterial> IFlver.Materials => Materials;

        /// <summary>
        /// Bones used by this model, may or may not be the full skeleton.
        /// </summary>
        public List<FLVER.Bone> Bones { get; set; }
        IReadOnlyList<FLVER.Bone> IFlver.Bones => Bones;

        /// <summary>
        /// Individual chunks of the model.
        /// </summary>
        public List<Mesh> Meshes { get; set; }
        IReadOnlyList<IFlverMesh> IFlver.Meshes => Meshes;

        /// <summary>
        /// Creates a FLVER0 with a default header and empty lists.
        /// </summary>
        public FLVER0()
        {
            Header = new FLVERHeader();
            Dummies = new List<FLVER.Dummy>();
            Materials = new List<Material>();
            Bones = new List<FLVER.Bone>();
            Meshes = new List<Mesh>();
        }

        /// <summary>
        /// Returns true if the data appears to be a FLVER0 model.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 0xC)
                return false;

            string magic = br.ReadASCII(6);
            string endian = br.ReadASCII(2);
            if (endian == "L\0")
                br.BigEndian = false;
            else if (endian == "B\0")
                br.BigEndian = true;
            int version = br.ReadInt32();
            return magic == "FLVER\0" && version >= 0x00000 && version < 0x20000;
        }

        /// <summary>
        /// Reads FLVER0 data from a BinaryReaderEx.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.AssertASCII("FLVER\0");
            Header.BigEndian = br.AssertASCII("L\0", "B\0") == "B\0";
            br.BigEndian = Header.BigEndian;

            // 10002, 10003 - Another Century's Episode R
            Header.Version = br.AssertInt32(0x0E, 0x0F, 0x10, 0x12, 0x13, 0x14, 0x15, // 0x11 needs to be added
                0x10002, 0x10003);
            int dataOffset = br.ReadInt32();
            br.ReadInt32(); // Data length
            int dummyCount = br.ReadInt32();
            int materialCount = br.ReadInt32();
            int boneCount = br.ReadInt32();
            int meshCount = br.ReadInt32();
            br.ReadInt32(); // Vertex buffer count
            Header.BoundingBoxMin = br.ReadVector3();
            Header.BoundingBoxMax = br.ReadVector3();
            br.ReadInt32(); // Face count not including motion blur meshes or degenerate faces
            br.ReadInt32(); // Total face count
            Header.VertexIndexSize = br.AssertByte(16, 32);
            Header.Unicode = br.ReadBoolean();
            Header.Unk4A = br.ReadByte();
            Header.Unk4B = br.ReadByte();
            Header.Unk4C = br.ReadInt32();
            br.AssertInt32(0);
            br.AssertInt32(0);
            br.AssertInt32(0);
            Header.Unk5C = br.ReadByte();
            br.AssertByte(0);
            br.AssertByte(0);
            br.AssertByte(0);
            br.AssertPattern(0x20, 0x00);

            Dummies = new List<FLVER.Dummy>(dummyCount);
            for (int i = 0; i < dummyCount; i++)
                Dummies.Add(new FLVER.Dummy(br, Header.Version));

            Materials = new List<Material>(materialCount);
            for (int i = 0; i < materialCount; i++)
                Materials.Add(new Material(br, this));

            Bones = new List<FLVER.Bone>(boneCount);
            for (int i = 0; i < boneCount; i++)
                Bones.Add(new FLVER.Bone(br, Header.Unicode));

            Meshes = new List<Mesh>(meshCount);
            for (int i = 0; i < meshCount; i++)
                Meshes.Add(new Mesh(br, this, dataOffset));
        }

        /// <summary>
        /// Writes FLVER0 data to a BinaryWriterEx.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = Header.BigEndian;
            bw.WriteASCII("FLVER\0");
            bw.WriteASCII(Header.BigEndian ? "B\0" : "L\0");
            bw.WriteInt32(Header.Version);

            bw.ReserveInt32("DataOffset");
            bw.ReserveInt32("DataSize");

            bw.WriteInt32(Dummies.Count);
            bw.WriteInt32(Materials.Count);
            bw.WriteInt32(Bones.Count);
            bw.WriteInt32(Meshes.Count);
            bw.WriteInt32(Meshes.Count);
            bw.WriteVector3(Header.BoundingBoxMin);
            bw.WriteVector3(Header.BoundingBoxMax);

            int trueFaceCount = 0;
            int totalFaceCount = 0;
            foreach (Mesh mesh in Meshes)
            {
                mesh.AddFaceCounts(Header.Version, ref trueFaceCount, ref totalFaceCount);
            }
            bw.WriteInt32(trueFaceCount);
            bw.WriteInt32(totalFaceCount);
            bw.WriteByte(Header.VertexIndexSize);//tempshit
            bw.WriteBoolean(Header.Unicode);
            bw.WriteByte(Header.Unk4A);
            bw.WriteByte(Header.Unk4B);
            bw.WriteInt32(Header.Unk4C);
            bw.WriteInt32(0);
            bw.WriteInt32(0);
            bw.WriteInt32(0);
            bw.WriteByte(Header.Unk5C);
            bw.WriteByte(0);
            bw.WriteByte(0);
            bw.WriteByte(0);
            bw.WritePattern(0x20, 0x00);

            foreach (FLVER.Dummy dummy in Dummies)
                dummy.Write(bw, Header.Version);

            for (int i = 0; i < Materials.Count; i++)
                Materials[i].Write(bw, i);

            for (int i = 0; i < Bones.Count; i++)
                Bones[i].Write(bw, i);

            for (int i = 0; i < Meshes.Count; i++)
                Meshes[i].Write(bw, i);


            //Filling stuff
            for (int i = 0; i < Materials.Count; i++)
            {
                int dataStart = (int)bw.Position;
                Materials[i].WriteStrings(bw, Header.Unicode, i);
                Materials[i].WriteTextures(bw, Header.Unicode, i);
                Materials[i].WriteLayouts(bw, i);
                bw.FillInt32($"dataLength{i}", (int)bw.Position - dataStart);
            }

            for (int i = 0; i < Bones.Count; i++)
            {
                Bones[i].WriteStrings(bw, Header.Unicode, i);
            }

            for (int i = 0; i < Meshes.Count; i++)
            {
                bw.FillInt32($"vertexBuffersHeaderOffset1_{i}", (int)bw.Position);
                Meshes[i].WriteVertexBuffers1(bw, i);
            }
            bw.Pad(0x20);
            int dataOffset = (int)bw.Position;
            bw.FillInt32($"DataOffset", dataOffset);
            for (int i = 0; i < Meshes.Count; i++)
            {
                bw.FillInt32($"vertexIndicesOffset{i}", (int)bw.Position - dataOffset);
                bw.FillInt32($"vertexIndicesLength{i}", Meshes[i].VertexIndices.Count * 2);
                Meshes[i].WriteIndexes(bw, Header.VertexIndexSize);
                bw.Pad(0x20);
                bw.FillInt32($"BufferOffset1_{i}", (int)bw.Position - dataOffset);
                bw.FillInt32($"bufferDataOffset{i}", (int)bw.Position - dataOffset);
                int bufferStart = (int)bw.Position;
                Meshes[i].WriteVertices(bw, Materials, i);
                bw.FillInt32($"BufferLength1_{i}", (int)bw.Position - bufferStart);
                bw.Pad(0x20);
            }

            bw.FillInt32($"DataSize", (int)bw.Position - dataOffset);
        }

        /// <summary>
        /// An FLVER0 header containing general values for this model.
        /// </summary>
        public class FLVERHeader
        {
            /// <summary>
            /// If true FLVER will be written big-endian, if false little-endian.
            /// </summary>
            public bool BigEndian { get; set; }

            /// <summary>
            /// Version of the format indicating presence of various features.
            /// </summary>
            public int Version { get; set; }

            /// <summary>
            /// Minimum extent of the entire model.
            /// </summary>
            public Vector3 BoundingBoxMin { get; set; }

            /// <summary>
            /// Maximum extent of the entire model.
            /// </summary>
            public Vector3 BoundingBoxMax { get; set; }

            /// <summary>
            /// Idk, some shit size
            /// </summary>
            public byte VertexIndexSize { get; set; }

            /// <summary>
            /// If true strings are UTF-16, if false Shift-JIS.
            /// </summary>
            public bool Unicode { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk4A { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk4B { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk4C { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte Unk5C { get; set; }

            /// <summary>
            /// Creates a FLVERHeader with default values.
            /// </summary>
            public FLVERHeader()
            {
                BigEndian = true;
                Version = 0x00015;
                Unicode = true;
            }
        }
    }
}
