using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    internal class TriangleStrip
    {
        internal List<Triangle> Triangles;
        internal int PRIMITIVE_RESTART_CONSTANT;

        internal int FaceCount => Triangles.Count;

        internal TriangleStrip(IList<int> indices, int primitiveRestartConstant)
        {
            Triangles = new List<Triangle>();

            PRIMITIVE_RESTART_CONSTANT = primitiveRestartConstant;

            for (int i = 0; i < indices.Count - 2; i++)
            {

                int indexA;
                int indexB;
                int indexC;

                if (i % 2 == 1)
                {
                    indexA = indices[i + 1];
                    indexB = indices[i];
                    indexC = indices[i + 2];
                }
                else
                {
                    indexA = indices[i];
                    indexB = indices[i + 1];
                    indexC = indices[i + 2];
                }

                if (indexA == PRIMITIVE_RESTART_CONSTANT || indexB == PRIMITIVE_RESTART_CONSTANT || indexC == PRIMITIVE_RESTART_CONSTANT)
                    throw new Exception("The Primitive Restart Constant was passed as an index.");

                if (indexA != indexB && indexA != indexC && indexB != indexC)
                {
                    Triangles.Add(new Triangle(indexA, indexB, indexC));
                }
            }
        }

        internal int[] GetBuffer(bool lastRow = false)
        {
            List<int> buffer = new List<int>((Triangles.Count * 3) + 1);
            foreach (Triangle triangle in Triangles)
            {
                buffer.AddRange(triangle.GetBuffer());
            }

            if (!lastRow)
            {
                buffer.Add(PRIMITIVE_RESTART_CONSTANT);
            }

            return buffer.ToArray();
        }

        internal List<int[]> GetFaces()
        {
            List<int[]> faces = new List<int[]>();
            foreach (Triangle triangle in Triangles)
            {
                faces.Add(triangle.GetBuffer());
            }

            return faces;
        }
    }
}
