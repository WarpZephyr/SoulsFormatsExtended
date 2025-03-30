using System.Collections.Generic;
using System.Numerics;

namespace SoulsFormats
{
    public partial class AP2
    {
        /// <summary>
        /// An animation.
        /// </summary>
        public class TodAnimation
        {
            /// <summary>
            /// The key frames in the animation.
            /// </summary>
            public List<TodKeyFrame> TodKeyFrames { get; set; }
        }

        /// <summary>
        /// A key frame in an animation representing a point in time at which to exchange transformation information for the owning dummy object.
        /// </summary>
        public struct TodKeyFrame
        {
            /// <summary>
            /// The order of time this key frame appears at.
            /// </summary>
            public int KeyTime { get; set; }

            /// <summary>
            /// The position to move to.
            /// </summary>
            public Vector4 Position { get; set; }

            /// <summary>
            /// The in tangent of the position curve.
            /// </summary>
            public Vector4 PositionInTangent { get; set; }

            /// <summary>
            /// The out tangent of the position curve.
            /// </summary>
            public Vector4 PositionOutTangent { get; set; }

            /// <summary>
            /// The angle to rotate at.
            /// </summary>
            public Vector4 Angle { get; set; }

            /// <summary>
            /// The in tangent of the angle curve.
            /// </summary>
            public Vector4 AngleInTangent { get; set; }

            /// <summary>
            /// The out tangent of the angle curve.
            /// </summary>
            public Vector4 AngleOutTangent { get; set; }

            /// <summary>
            /// The scale to size to.
            /// </summary>
            public Vector4 Scale { get; set; }
        }
    }
}
