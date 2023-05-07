using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SoulsFormats
{
    public partial class FLVER2
    {
        public partial class FaceSet
        {
            // This is basically shitcode which makes a lot of dubious assumptions.
            // If I ever intend to implement edge writing this will require much more investigation.
            private static class EdgeIndexCompression
            {
                [DllImport("EdgeIndexDecompressor.dll")]
                private static extern void DecompressIndexes_C_Standalone(uint numIndexes, byte[] indexes);

                public static List<int> ReadEdgeIndexGroup(BinaryReaderEx br, int indexCount)
                {
                    long start = br.Position;
                    short memberCount = br.ReadInt16();
                    br.ReadInt16();
                    br.ReadInt32();
                    br.AssertInt32(0);
                    br.ReadInt32();

                    var indices = new List<int>(indexCount);
                    for (int i = 0; i < memberCount; i++)
                    {
                        int dataLength = br.ReadInt32();
                        int dataOffset = br.ReadInt32();
                        br.AssertInt32(0);
                        br.AssertInt32(0);
                        br.ReadInt16();
                        br.ReadInt16();
                        ushort baseIndex = br.ReadUInt16();
                        br.ReadInt16();
                        br.ReadInt32();
                        br.ReadInt32();
                        br.AssertInt32(0);
                        br.AssertInt32(0);
                        br.AssertInt32(0);
                        br.AssertInt32(0);
                        br.ReadInt16();
                        br.ReadInt16();
                        br.ReadInt16();
                        br.ReadInt16();
                        br.ReadInt16();
                        ushort memberIndexCount = br.ReadUInt16();
                        int maxStep = br.AssertInt32(-1);

                        // I feel like one of those fields should be the required buffer size, but none I tried worked
                        byte[] buffer = new byte[memberIndexCount * 32];
                        br.GetBytes(start + dataOffset, buffer, 0, dataLength);

                        //List<int> decodedIndices = InverseWatermarkTransform(encodedIndices, maxStep);
                        //List<ushort> decompressedIndices = DecompressIndices(decodedIndices).ConvertAll(delegate (int num) {
                        //    return (ushort)num;
                        //});

                        DecompressIndexes_C_Standalone(memberIndexCount, buffer);

                        var brBuffer = new BinaryReaderEx(true, buffer);
                        for (int j = 0; j < memberIndexCount; j++)
                            indices.Add(baseIndex + brBuffer.ReadUInt16());
                    }
                    return indices;
                }

                /// <summary>
                /// Decodes encoded indices.
                /// </summary>
                /// <param name="encoded_indices">A list of encoded indices.</param>
                /// <param name="max_step">The max step to decode the high watermark.</param>
                /// <returns>Decoded indices.</returns>
                public static List<int> InverseWatermarkTransform(List<int> encoded_indices, int max_step)
                {
                    if (max_step == 0x0000ffff)
                    {
                        return encoded_indices;
                    }

                    List<int> out_indices = new List<int>();

                    int hi = max_step - 1;
                    foreach (int v in encoded_indices)
                    {
                        int decV = hi - v;
                        out_indices.Add(decV);
                        hi = Math.Max(hi, decV + max_step);
                    }

                    return out_indices;
                }

                /// <summary>
                /// Decompresses decoded indices.
                /// </summary>
                /// <param name="indices">A list of indices.</param>
                /// <param name="invertNormals">Whether or not to invert normals.</param>
                /// <returns>Decompressed indices.</returns>
                public static List<int> DecompressIndices(List<int> indices, bool invertNormals = true)
                {
                    List<int> out_indices = new List<int>();

                    //Nova normals seem to require this. Maybe Star Ocean faces worked different.
                    if (invertNormals)
                    {
                        for (int i = 0; i < indices.Count;)
                        {
                            int a = indices[i++];
                            int b = indices[i++];
                            int c = indices[i++];

                            bool isDegen = a == b || b == c || a == c;
                            if (!isDegen)
                            {
                                out_indices.Add(b); out_indices.Add(a); out_indices.Add(c);
                            }

                            if (a < b && i < indices.Count)
                            {
                                int d = indices[i++];

                                isDegen = a == b || b == d || a == d;
                                if (!isDegen)
                                {
                                    out_indices.Add(d); out_indices.Add(a); out_indices.Add(b);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < indices.Count;)
                        {
                            int a = indices[i++];
                            int b = indices[i++];
                            int c = indices[i++];

                            bool isDegen = a == b || b == c || a == c;
                            if (!isDegen)
                            {
                                out_indices.Add(a); out_indices.Add(b); out_indices.Add(c);
                            }

                            if (a < b && i < indices.Count)
                            {
                                int d = indices[i++];
                                isDegen = a == b || b == d || a == d;
                                if (!isDegen)
                                {
                                    out_indices.Add(a); out_indices.Add(d); out_indices.Add(b);
                                }
                            }
                        }
                    }

                    return out_indices;
                }
            }
        }
    }
}
