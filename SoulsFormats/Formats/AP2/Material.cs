using System.Numerics;

namespace SoulsFormats
{
    public partial class AP2
    {
        /// <summary>
        /// The supported alpha blend operations in materials.
        /// </summary>
        public enum AlphaBlendOperation
        {
            Add
        }

        /// <summary>
        /// The supported alpha blend functions in materials.
        /// </summary>
        public enum AlphaBlendFunction
        {
            SourceColor,
            SourceAlpha,
            OneMinusSourceAlpha
        }

        /// <summary>
        /// A material representing lighting information.
        /// </summary>
        public class Material
        {
            /// <summary>
            /// The name of the material.
            /// </summary>
            public string MaterialName { get; set; }

            /// <summary>
            /// The color.
            /// </summary>
            public Vector4 Color { get; set; }

            /// <summary>
            /// The ambient color.
            /// </summary>
            public Vector3 AmbientColor { get; set; }

            /// <summary>
            /// How much self illumination is present.
            /// </summary>
            public float SelfIllumination { get; set; }

            /// <summary>
            /// The specular color.
            /// </summary>
            public Vector3 SpecularColor { get; set; }

            /// <summary>
            /// The specular level.
            /// </summary>
            public float SpecularLevel { get; set; }

            /// <summary>
            /// The specular exponent.
            /// </summary>
            public float SpecularExponent { get; set; }

            /// <summary>
            /// The shininess strength.
            /// </summary>
            public float ShininessStrength { get; set; }

            /// <summary>
            /// The shininess.
            /// </summary>
            public float Shininess { get; set; }

            /// <summary>
            /// Whether or not back faces are culled.
            /// </summary>
            public bool TwoSided { get; set; }

            /// <summary>
            /// The shading type.
            /// </summary>
            public int ShadingTypeIIP { get; set; }

            /// <summary>
            /// The reflection amount.
            /// </summary>
            public float ReflectionAmount { get; set; }

            /// <summary>
            /// The refraction amount.
            /// </summary>
            public float RefractionAmount { get; set; }

            /// <summary>
            /// The target platform the alpha blend is being used on.
            /// </summary>
            public string AlphaBlendTargetType { get; set; }

            /// <summary>
            /// Whether or not to use alpha blend.
            /// </summary>
            public bool AlphaBlendUse { get; set; }

            /// <summary>
            /// The alpha blend operation.
            /// </summary>
            public AlphaBlendOperation AlphaBlendOperation { get; set; }

            /// <summary>
            /// The first alpha blend function.
            /// </summary>
            public string AlphaBlendFunctionA { get; set; }

            /// <summary>
            /// The second alpha blend function.
            /// </summary>
            public string AlphaBlendFunctionB { get; set; }

            /// <summary>
            /// The first alpha blend fix color.
            /// </summary>
            public Vector3 AlphaBlendFixColorA { get; set; }

            /// <summary>
            /// The second alpha blend fix color.
            /// </summary>
            public Vector3 AlphaBlendFixColorB { get; set; }

            /// <summary>
            /// The diffuse channel ID.
            /// </summary>
            public int DiffuseChannelID { get; set; }

            /// <summary>
            /// The diffuse texture path.
            /// </summary>
            public string DiffuseTexture { get; set; }
        }
    }
}
