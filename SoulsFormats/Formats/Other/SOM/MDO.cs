using System.Collections.Generic;

namespace SoulsFormats.SOM
{
    /// <summary>
    /// A model format used in Sword of Moonlight for basic models like items.
    /// </summary>
    public partial class MDO : SoulsFile<MDO>
    {
        /// <summary>
        /// Paths to textures used by this model.
        /// </summary>
        public List<string> Textures;

        /// <summary>
        /// Unknown.
        /// </summary>
        public List<Unk1> Unk1s;

        /// <summary>
        /// Individual chunks of the model.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// Reads MDO data from a BinaryReaderEx.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = false;

            int textureCount = br.ReadInt32();
            Textures = new List<string>(textureCount);
            for (int i = 0; i < textureCount; i++)
                Textures.Add(br.ReadShiftJIS());
            br.Pad(4);

            int unk1Count = br.ReadInt32();
            Unk1s = new List<Unk1>(unk1Count);
            for (int i = 0; i < unk1Count; i++)
                Unk1s.Add(new Unk1(br));

            for (int i = 0; i < 12; i++)
                br.AssertInt32(0);

            int meshCount = br.ReadInt32();
            Meshes = new List<Mesh>(meshCount);
            for (int i = 0; i < meshCount; i++)
                Meshes.Add(new Mesh(br));
        }
    }
}
