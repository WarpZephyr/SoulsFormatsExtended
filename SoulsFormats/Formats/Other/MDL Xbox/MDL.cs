using System.Collections.Generic;

namespace SoulsFormats.Other
{
    /// <summary>
    /// A 3D model format used in Xbox games. Extension: .mdl
    /// </summary>
    public partial class MDL : SoulsFile<MDL>
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk0C;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk10;

        /// <summary>
        /// Unknown.
        /// </summary>
        public int Unk14;

        /// <summary>
        /// Individual chunks of the model.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// The vertex indices within the model.
        /// </summary>
        public ushort[] Indices;

        /// <summary>
        /// The first set of vertices.
        /// </summary>
        public List<Vertex> VerticesA;

        /// <summary>
        /// The second set of vertices.
        /// </summary>
        public List<Vertex> VerticesB;

        /// <summary>
        /// The third set of vertices.
        /// </summary>
        public List<Vertex> VerticesC;

        /// <summary>
        /// The fourth set of vertices.
        /// </summary>
        public List<VertexD> VerticesD;

        /// <summary>
        /// Unknown.
        /// </summary>
        public List<Struct7> Struct7s;

        /// <summary>
        /// The materials in this model.
        /// </summary>
        public List<Material> Materials;

        /// <summary>
        /// The names of textures this model uses.
        /// </summary>
        public List<string> Textures;

        /// <summary>
        /// Returns true if a file appears to be data of this type.
        /// </summary>
        protected override bool Is(BinaryReaderEx br)
        {
            if (br.Length < 4)
                return false;

            string magic = br.GetASCII(4, 4);
            return magic == "MDL ";
        }

        /// <summary>
        /// Deserialize file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            br.ReadInt32(); // File size
            br.AssertASCII("MDL ");
            br.AssertInt16(1);
            br.AssertInt16(1);
            Unk0C = br.ReadInt32();
            Unk10 = br.ReadInt32();
            Unk14 = br.ReadInt32();

            int meshCount = br.ReadInt32();
            int indexCount = br.ReadInt32();
            int vertexCountA = br.ReadInt32();
            int vertexCountB = br.ReadInt32();
            int vertexCountC = br.ReadInt32();
            int vertexCountD = br.ReadInt32();
            int count7 = br.ReadInt32();
            int materialCount = br.ReadInt32();
            int textureCount = br.ReadInt32();

            int meshesOffset = br.ReadInt32();
            int indicesOffset = br.ReadInt32();
            int verticesOffsetA = br.ReadInt32();
            int verticesOffsetB = br.ReadInt32();
            int verticesOffsetC = br.ReadInt32();
            int verticesOffsetD = br.ReadInt32();
            int offset7 = br.ReadInt32();
            int materialsOffset = br.ReadInt32();
            int texturesOffset = br.ReadInt32();

            br.Position = meshesOffset;
            Meshes = new List<Mesh>();
            for (int i = 0; i < meshCount; i++)
                Meshes.Add(new Mesh(br));

            Indices = br.GetUInt16s(indicesOffset, indexCount);

            br.Position = verticesOffsetA;
            VerticesA = new List<Vertex>(vertexCountA);
            for (int i = 0; i < vertexCountA; i++)
                VerticesA.Add(new Vertex(br, VertexFormat.A));

            br.Position = verticesOffsetB;
            VerticesB = new List<Vertex>(vertexCountB);
            for (int i = 0; i < vertexCountB; i++)
                VerticesB.Add(new Vertex(br, VertexFormat.B));

            br.Position = verticesOffsetC;
            VerticesC = new List<Vertex>(vertexCountC);
            for (int i = 0; i < vertexCountC; i++)
                VerticesC.Add(new Vertex(br, VertexFormat.C));

            br.Position = verticesOffsetD;
            VerticesD = new List<VertexD>(vertexCountD);
            for (int i = 0; i < vertexCountD; i++)
                VerticesD.Add(new VertexD(br));

            br.Position = offset7;
            Struct7s = new List<Struct7>(count7);
            for (int i = 0; i < count7; i++)
                Struct7s.Add(new Struct7(br));

            br.Position = materialsOffset;
            Materials = new List<Material>(materialCount);
            for (int i = 0; i < materialCount; i++)
                Materials.Add(new Material(br));

            br.Position = texturesOffset;
            Textures = new List<string>(textureCount);
            for (int i = 0; i < textureCount; i++)
                Textures.Add(br.ReadShiftJIS());
        }
    }
}
