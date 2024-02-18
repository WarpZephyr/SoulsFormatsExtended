using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class FLVER2
    {
        public partial class FaceSet
        {
            private static class EdgeIndexCompression
            {
                public static List<int> ReadEdgeIndexGroup(BinaryReaderEx br, int indexCount)
                {
                    long start = br.Position;
                    short memberCount = br.ReadInt16();
                    br.ReadInt16();
                    br.ReadInt16();
                    br.ReadByte();
                    br.ReadByte();
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

                        // The base index is where this member's indexes start off from, add it to each decompressed index
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
                        br.AssertInt32(-1);

                        long returnPos = br.Position;
                        br.Position = start + dataOffset;
                        ushort[] indexes = DecompressIndexes(br, memberIndexCount);
                        for (int j = 0; j < memberIndexCount; j++)
                            indices.Add(indexes[j] + baseIndex);
                        br.Position = returnPos;
                        
                    }
                    return indices;
                }

                private static ushort[] DecompressIndexes(BinaryReaderEx br, uint numIndexes)
                {
                    ushort numIndexesInNBitStream = br.ReadUInt16(); // The number of delta compressed indexes, each taking bitsPerIndex number of bits.
                    ushort baseDelta = br.ReadUInt16(); // The base delta, which was the minimum index after delta compressing which must be subtracted from each delta compressed index first.
                    ushort num1Bytes = br.ReadUInt16(); // The number of bytes taken by the 1 bit stream for sequential indexes.
                    byte bitsPerIndex = br.ReadByte(); // How many bits each delta index takes up.
                    br.AssertByte(0); // Padding, should always be 0.

                    // There are 8 bits per byte, so multiply the number of 1 bits bytes by 8.
                    uint num1Bits = (uint)(num1Bytes * 8);

                    // There are 3 indexes per triangle so divide the number of indexes by 3
                    uint numTriangles = numIndexes / 3;

                    // There are 2 bits per triangle, so double the number of triangles in the calculation
                    // This is the number of triangle configurations, which are two bits each.
                    // It must be padded to 8 bits, which I think adding 7 is supposed to do? Seen that in a python script.
                    uint num2Bits = (numTriangles + numTriangles) + 7;

                    // There are 8 bits per byte, so divide the number of 2 bits by 8.
                    uint num2Bytes = num2Bits / 8;

                    // Multiply the number of n bit indexes by the number of bits per index to get the number of n bits
                    // It must be padded to 8 bits, which I think adding 7 is supposed to do? Seen that in a python script.
                    uint numNBits = (uint)(numIndexesInNBitStream * bitsPerIndex) + 7;

                    // There are 8 bits per byte, so divide the number of n bits by 8.
                    uint numNBytes = numNBits / 8;

                    // Read raw bytes
                    var bit1Bytes = br.ReadBytes(num1Bytes);
                    var bit2Bytes = br.ReadBytes((int)num2Bytes);
                    var nbitBytes = br.ReadBytes((int)numNBytes);

                    // Read actual data from bytes (Needs better implementation later)
                    var bit1Table = bit1Bytes.ToBinary().FromBinary(1);
                    var bit2Table = bit2Bytes.ToBinary().FromBinary(2);
                    var deltaIndexes = nbitBytes.ToBinary().FromBinaryToUInt16(bitsPerIndex);

                    // Decompress delta indexes in place.
                    DecompressDeltaIndexes(deltaIndexes, baseDelta);

                    // Add back the sequential indexes using the 1 bit stream.
                    ushort[] newIndexes = DecompressSequentialIndexes(deltaIndexes, bit1Table);

                    // Sort the triangle configurations given by the 2 bit stream.
                    // The following is each configuration by bits,
                    // Each number is a previous index, and N means the next New index.
                    // 00 = 1 0 N
                    // 01 = 0 2 N
                    // 10 = 2 1 N
                    // 11 = N N N
                    int currentIndex = 0;
                    var result = new List<ushort>((int)numIndexes);
                    ushort[] triangleIndexes = new ushort[3];
                    for (int i = 0; i < numTriangles; i++)
                    {
                        var configValue = bit2Table[i];
                        switch (configValue)
                        {
                            case 0:
                                triangleIndexes[1] = triangleIndexes[2];
                                triangleIndexes[2] = newIndexes[currentIndex];
                                currentIndex++;
                                break;
                            case 1:
                                triangleIndexes[0] = triangleIndexes[2];
                                triangleIndexes[2] = newIndexes[currentIndex];
                                currentIndex++;
                                break;
                            case 2:
                                ushort tempIndex = triangleIndexes[0];
                                triangleIndexes[0] = triangleIndexes[1];
                                triangleIndexes[1] = tempIndex;
                                triangleIndexes[2] = newIndexes[currentIndex];
                                currentIndex++;
                                break;
                            case 3:
                                triangleIndexes[0] = newIndexes[currentIndex];
                                currentIndex++;
                                triangleIndexes[1] = newIndexes[currentIndex];
                                currentIndex++;
                                triangleIndexes[2] = newIndexes[currentIndex];
                                currentIndex++;
                                break;
                            default:
                                throw new Exception($"Unknown config value: {configValue}");
                        }
                        result.Add(triangleIndexes[0]);
                        result.Add(triangleIndexes[1]);
                        result.Add(triangleIndexes[2]);
                    }

                    // Finished
                    return result.ToArray();
                }

                internal static void DecompressDeltaIndexes(ushort[] indexes, ushort baseDelta)
                {
                    for (int i = 0; i < indexes.Length; i++)
                    {
                        indexes[i] -= baseDelta;
                    }

                    for (int i = 8; i < indexes.Length; i++)
                    {
                        indexes[i] = (ushort)(indexes[i] + indexes[i - 8]);
                    }
                }

                internal static ushort[] DecompressSequentialIndexes(ushort[] indexes, byte[] bitStream)
                {
                    ushort[] newIndexes = new ushort[bitStream.Length];

                    ushort increment = 0;
                    int indexesIndex = 0;
                    for (int i = 0; i < bitStream.Length; i++)
                    {
                        if (bitStream[i] == 1)
                        {
                            newIndexes[i] = indexes[indexesIndex];
                            indexesIndex++;
                        }
                        else
                        {
                            newIndexes[i] = increment;
                            increment++;
                        }
                    }
                    return newIndexes;
                }
            }
        }
    }
}