using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    public partial class AP2
    {
        /// <summary>
        /// Geometry representing points in 3d space that make up a model.
        /// </summary>
        public class Geometry
        {
            /// <summary>
            /// An extension that identifies the minimum vertex of the primitive list.
            /// </summary>
            public Vector3 ExPrimitiveListVertexMin { get; set; }

            /// <summary>
            /// An extension that identifies the maximum vertex of the primitive list.
            /// </summary>
            public Vector3 ExPrimitiveListVertexMax { get; set; }

            /// <summary>
            /// An extension that identifies the average vertex of the primitive list.
            /// </summary>
            public Vector4 ExPrimitiveListVertexAverage { get; set; }

            /// <summary>
            /// The primitives making up geometry.
            /// </summary>
            public List<Primitive> Primitives { get; set; }
        }

        /// <summary>
        /// The different supported primtives.
        /// </summary>
        public enum PrimitiveType
        {
            Strip
        }

        /// <summary>
        /// A primitive representing part of geometry.
        /// </summary>
        public class Primitive
        {
            /// <summary>
            /// The index of the material of this primitive.
            /// </summary>
            public int PrimitiveMaterial { get; set; }

            /// <summary>
            /// The direction of the primitive.
            /// </summary>
            public int PrimitiveDirection { get; set; }

            /// <summary>
            /// The primitive type.
            /// </summary>
            public PrimitiveType PrimitiveType { get; set; }

            /// <summary>
            /// An extension that identifies the minimum vertex.
            /// </summary>
            public Vector3 ExPrimitiveVertexMin { get; set; }

            /// <summary>
            /// An extension that identifies the maximum vertex.
            /// </summary>
            public Vector3 ExPrimitiveVertexMax { get; set; }

            /// <summary>
            /// An extension that identifies the average vertex.
            /// </summary>
            public Vector4 ExPrimitiveVertexAverage { get; set; }

            /// <summary>
            /// The primitive nodes making up face information for this primitive.
            /// </summary>
            public List<PrimitiveNode> PrimitiveNodes { get; set; }

            /// <summary>
            /// The position information for this primitive.
            /// </summary>
            public List<Vertex> Vertices { get; set; }

            /// <summary>
            /// The normal information for nodes in this primitive.
            /// </summary>
            public List<Vector4> Normals { get; set; }

            /// <summary>
            /// The color information for nodes in this primitive.
            /// </summary>
            public List<Vector4> Colors { get; set; }

            /// <summary>
            /// The texture coordinates for nodes in this primitive.
            /// </summary>
            public List<Vector2> TextureCoordinates { get; set; }
        }

        /// <summary>
        /// A node representing one point in 3d space for faces.
        /// </summary>
        public struct PrimitiveNode
        {
            /// <summary>
            /// The index of the vertex for this node.
            /// </summary>
            public int VertexID { get; set; }

            /// <summary>
            /// The index of the normal for this node.
            /// </summary>
            public int NormalID { get; set; }

            /// <summary>
            /// The index of the texture coordinate for this node.
            /// </summary>
            public int TextureCoordinateID { get; set; }

            /// <summary>
            /// An unknown extension.
            /// </summary>
            public int ExAdc { get; set; }

            /// <summary>
            /// An unknown extension. Direction?
            /// </summary>
            public int ExDir { get; set; }
        }

        /// <summary>
        /// A vertex representing a position in 3d space.
        /// </summary>
        public struct Vertex
        {
            /// <summary>
            /// The position of the vertex.
            /// </summary>
            public Vector4 Position { get; set; }

            /// <summary>
            /// An extension representing the index of the object to connect this vertex to.
            /// </summary>
            public int ExConnectObjectID { get; set; }

            /// <summary>
            /// An extension representing the level to connect to.
            /// </summary>
            public int ExConnectLevel { get; set; }

            /// <summary>
            /// An extension representing the index of the matrix to transform this vertex by.
            /// </summary>
            public int ExConnectMatrixID { get; set; }

            /// <summary>
            /// An extension representing the default coordinate plane of this vertex.
            /// </summary>
            public Vector3 ExDefaultXYZ { get; set; }

            /// <summary>
            /// An extension representing a level-of-detail version of this vertex.
            /// </summary>
            public Vector3 ExLodVertex { get; set; }
        }
    }
}
