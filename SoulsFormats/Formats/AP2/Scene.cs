using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    public partial class AP2
    {
        /// <summary>
        /// A scene containing materials and objects.
        /// </summary>
        public class Scene
        {
            /// <summary>
            /// The path to the 3dsMax file for this scene.
            /// </summary>
            public string MaxFilePath { get; set; }

            /// <summary>
            /// The start frame of the animation of the scene.
            /// </summary>
            public int AnimationStartFrame { get; set; }

            /// <summary>
            /// The end frame of the animation of the scene.
            /// </summary>
            public int AnimationEndFrame { get; set; }

            /// <summary>
            /// The scene background color.
            /// </summary>
            public Vector3 SceneBackgroundColor { get; set; }

            /// <summary>
            /// The scene ambient color.
            /// </summary>
            public Vector3 SceneAmbientColor { get; set; }

            /// <summary>
            /// An extension that identifies the minimum vertex of the object list.
            /// </summary>
            public Vector3 ExObjectListVertexMin { get; set; }

            /// <summary>
            /// An extension that identifies the maximum vertex of the object list.
            /// </summary>
            public Vector3 ExObjectListVertexMax { get; set; }

            /// <summary>
            /// An extension that identifies the average vertex of the object list.
            /// </summary>
            public Vector4 ExObjectListVertexAverage { get; set; }

            /// <summary>
            /// Materials in the scene.
            /// </summary>
            public List<Material> Materials { get; set; }

            /// <summary>
            /// Objects in the scene.
            /// </summary>
            public List<Object> Objects { get; set; }
        }

        /// <summary>
        /// The different types of object supported by the scene.
        /// </summary>
        public enum ObjectType
        {
            /// <summary>
            /// A dummy object containing animation information.
            /// </summary>
            Dummy,

            /// <summary>
            /// Geometry representing points in 3d space that make up a model.
            /// </summary>
            Geometry
        }

        /// <summary>
        /// An object in the scene.
        /// </summary>
        public class Object
        {
            /// <summary>
            /// The name of the object.
            /// </summary>
            public string ObjectName { get; set; }

            /// <summary>
            /// The object type.
            /// </summary>
            public ObjectType ObjectType { get; set; }

            /// <summary>
            /// The ID of this object, set to -1 to ignore.
            /// </summary>
            public int ObjectID { get; set; }

            /// <summary>
            /// The ID of the next sibling of this object, leave as 0 to ignore.
            /// </summary>
            public int SiblingObjectID { get; set; }

            /// <summary>
            /// The name of the next sibling of this object.
            /// </summary>
            public string SiblingObjectName { get; set; }

            /// <summary>
            /// This object's position relative to it's parent.
            /// </summary>
            public Vector4 RelativePosition { get; set; }

            /// <summary>
            /// This object's angle relative to it's parent.
            /// </summary>
            public Vector4 RelativeAngle { get; set; }

            /// <summary>
            /// This object's scale relative to it's parent.
            /// </summary>
            public Vector4 RelativeScale { get; set; }

            /// <summary>
            /// This object's position relative to the world.
            /// </summary>
            public Vector4 WorldPosition { get; set; }

            /// <summary>
            /// This object's angle relative to the world.
            /// </summary>
            public Vector4 WorldAngle { get; set; }

            /// <summary>
            /// This object's scale relative to the world.
            /// </summary>
            public Vector4 WorldScale { get; set; }

            /// <summary>
            /// Animation information for this object, only valid on dummy objects.
            /// </summary>
            public TodAnimation TodAnimation { get; set; }

            /// <summary>
            /// The geometry information for this object, only valid on geometry objects.
            /// </summary>
            public Geometry Geometry { get; set; }
        }
    }
}
