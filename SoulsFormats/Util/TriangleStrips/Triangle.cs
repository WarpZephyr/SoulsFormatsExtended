

namespace SoulsFormats
{
    internal struct Triangle
    {
        internal int IndexA;
        internal int IndexB;
        internal int IndexC;

        internal Triangle(int indexA, int indexB, int indexC)
        {
            IndexA = indexA;
            IndexB = indexB;
            IndexC = indexC;
        }

        internal int[] GetBuffer()
        {
            return new int[3] { IndexA, IndexB, IndexC };
        }
    }
}
