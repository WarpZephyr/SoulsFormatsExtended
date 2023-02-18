using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class FLVER0
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class Material : IFlverMaterial
        {
            /// <summary>
            /// Identifies the mesh that uses this material, may include keywords that determine hideable parts.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Virtual path to an MTD file.
            /// </summary>
            public string MTD { get; set; }

            public List<Texture> Textures { get; set; }
            IReadOnlyList<IFlverTexture> IFlverMaterial.Textures => Textures;

            public List<BufferLayout> Layouts { get; set; }

            /// <summary>
            /// Creates a new Material with null or default values.
            /// </summary>
            public Material()
            {
                Name = "";
                MTD = "";
                Textures = new List<Texture>();
                Layouts = new List<BufferLayout>();
            }

            /// <summary>
            /// Creates a new Material with the given values and an empty texture list.
            /// </summary>
            public Material(string name, string mtd)
            {
                Name = name;
                MTD = mtd;
                Textures = new List<Texture>();
                Layouts = new List<BufferLayout>();
            }

            internal Material(BinaryReaderEx br, FLVER0 flv)
            {
                int nameOffset = br.ReadInt32();
                int mtdOffset = br.ReadInt32();
                int texturesOffset = br.ReadInt32();
                int layoutsOffset = br.ReadInt32();
                br.ReadInt32(); // Data length from name offset to end of buffer layouts
                int layoutHeaderOffset = br.ReadInt32();
                br.AssertInt32(0);
                br.AssertInt32(0);

                Name = flv.Header.Unicode ? br.GetUTF16(nameOffset) : br.GetShiftJIS(nameOffset);
                MTD = flv.Header.Unicode ? br.GetUTF16(mtdOffset) : br.GetShiftJIS(mtdOffset);

                br.StepIn(texturesOffset);
                {
                    byte textureCount = br.ReadByte();
                    br.AssertByte(0);
                    br.AssertByte(0);
                    br.AssertByte(0);
                    br.AssertInt32(0);
                    br.AssertInt32(0);
                    br.AssertInt32(0);

                    Textures = new List<Texture>(textureCount);
                    for (int i = 0; i < textureCount; i++)
                        Textures.Add(new Texture(br, flv));
                }
                br.StepOut();

                if (layoutHeaderOffset != 0)
                {
                    br.StepIn(layoutHeaderOffset);
                    {
                        int layoutCount = br.ReadInt32();
                        br.AssertInt32((int)br.Position + 0xC);
                        br.AssertInt32(0);
                        br.AssertInt32(0);
                        Layouts = new List<BufferLayout>(layoutCount);
                        for (int i = 0; i < layoutCount; i++)
                        {
                            int layoutOffset = br.ReadInt32();
                            br.StepIn(layoutOffset);
                            {
                                Layouts.Add(new BufferLayout(br));
                            }
                            br.StepOut();
                        }
                    }
                    br.StepOut();
                }
                else
                {
                    Layouts = new List<BufferLayout>(1);
                    br.StepIn(layoutsOffset);
                    {
                        Layouts.Add(new BufferLayout(br));
                    }
                    br.StepOut();
                }
            }

            internal void Write(BinaryWriterEx bw, int index)
            {
                bw.ReserveInt32($"nameOffset{index}");
                bw.ReserveInt32($"mtdOffset{index}");
                bw.ReserveInt32($"texturesOffset{index}");
                bw.ReserveInt32($"layoutsOffset{index}");
                bw.ReserveInt32($"dataLength{index}"); //TEMPSHIT // Data length from name offset to end of buffer layouts
                bw.ReserveInt32($"layoutHeaderOffset{index}");
                bw.WriteInt32(0);
                bw.WriteInt32(0);
            }

            internal void WriteStrings(BinaryWriterEx bw, bool Unicode, int index)
            {
                bw.FillInt32($"nameOffset{index}", (int)bw.Position);
                if (Unicode)
                    bw.WriteUTF16(Name, true);
                else
                    bw.WriteShiftJIS(Name, true);

                bw.FillInt32($"mtdOffset{index}", (int)bw.Position);
                if (Unicode)
                    bw.WriteUTF16(MTD, true);
                else
                    bw.WriteShiftJIS(MTD, true);
            }

            internal void WriteTextures(BinaryWriterEx bw, bool Unicode, int index)
            {
                bw.FillInt32($"texturesOffset{index}", (int)bw.Position);
                bw.WriteByte((byte)Textures.Count);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                for (int i = 0; i < Textures.Count; i++)
                    Textures[i].Write(bw, i);
                for (int i = 0; i < Textures.Count; i++)
                    Textures[i].WriteStrings(bw, Unicode, i);
            }

            internal void WriteLayouts(BinaryWriterEx bw, int index)
            {
                bw.FillInt32($"layoutHeaderOffset{index}", (int)bw.Position);
                bw.WriteInt32(Layouts.Count);
                bw.ReserveInt32($"layoutOffsetsOffset{index}");
                bw.WriteInt32(0);
                bw.WriteInt32(0);
                bw.FillInt32($"layoutOffsetsOffset{index}", (int)bw.Position);
                int layoutsOffset = 0;
                for (int i = 0; i < Layouts.Count; i++)
                {
                    bw.ReserveInt32($"layoutOffset{index}");
                    bw.FillInt32($"layoutOffset{index}", (int)bw.Position);
                    if (i == 0)
                        layoutsOffset = (int)bw.Position;
                    bw.WriteInt16((short)Layouts[i].Count);
                    bw.WriteInt16((short)Layouts[i].Size);//tempshit structSize
                    bw.WriteInt32(0);
                    bw.WriteInt32(0);
                    bw.WriteInt32(0);
                    int memberSize = 0;
                    for (int m = 0; m < Layouts[i].Count; m++)
                    {
                        Layouts[i][m].Write(bw, memberSize);
                        memberSize += Layouts[i][m].Size;
                    }
                }
                bw.FillInt32($"layoutsOffset{index}", layoutsOffset);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
