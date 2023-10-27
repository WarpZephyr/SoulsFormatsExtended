using System.Collections.Generic;

namespace SoulsFormats
{
    internal class TriangleStripCollection
    {
        internal int PRIMITIVE_RESTART_CONSTANT;
        internal List<TriangleStrip> TriangleStrips;

        internal int FaceCount
        {
            get
            {
                int count = 0;
                foreach (TriangleStrip strip in TriangleStrips)
                {
                    count += strip.FaceCount;
                }
                return count;
            }
        }

        internal int StripCount => TriangleStrips.Count;

        internal TriangleStripCollection(IList<int> indexBuffer, int primitiveRestartConstant)
        {
            TriangleStrips = new List<TriangleStrip>();
            PRIMITIVE_RESTART_CONSTANT = primitiveRestartConstant;

            List<int> stripBuffer = new List<int>();
            for (int i = 0; i < indexBuffer.Count; i++)
            {
                if (indexBuffer[i] != PRIMITIVE_RESTART_CONSTANT)
                {
                    stripBuffer.Add(indexBuffer[i]);
                }
                else
                {
                    TriangleStrips.Add(new TriangleStrip(stripBuffer, PRIMITIVE_RESTART_CONSTANT));
                    stripBuffer = new List<int>();
                }
            }
        }

        internal TriangleStripCollection(IList<ushort> indexBuffer, int primitiveRestartConstant)
        {
            TriangleStrips = new List<TriangleStrip>();
            PRIMITIVE_RESTART_CONSTANT = primitiveRestartConstant;

            List<int> stripBuffer = new List<int>();
            for (int i = 0; i < indexBuffer.Count; i++)
            {
                if (indexBuffer[i] != PRIMITIVE_RESTART_CONSTANT)
                {
                    stripBuffer.Add(indexBuffer[i]);
                }
                else
                {
                    TriangleStrips.Add(new TriangleStrip(stripBuffer, PRIMITIVE_RESTART_CONSTANT));
                    stripBuffer = new List<int>();
                }
            }
        }

        internal int[] GetBuffer()
        {
            List<int> buffer = new List<int>();
            for (int i = 0; i < TriangleStrips.Count; i++)
            {
                if (i == TriangleStrips.Count - 1)
                {
                    buffer.AddRange(TriangleStrips[i].GetBuffer(true));
                }
                else
                {
                    buffer.AddRange(TriangleStrips[i].GetBuffer(false));
                }
            }
            return buffer.ToArray();
        }

        internal ushort[] GetBufferUShort()
        {
            var buffer = GetBuffer();
            var newBuffer = new ushort[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                newBuffer[i] = (ushort)buffer[i];
            }
            return newBuffer;
        }

        internal List<int[]> GetFaces()
        {
            List<int[]> faces = new List<int[]>();
            foreach (var strip in TriangleStrips)
            {
                faces.AddRange(strip.GetFaces());
            }
            return faces;
        }
    }
}
