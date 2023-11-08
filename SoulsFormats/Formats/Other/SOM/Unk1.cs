namespace SoulsFormats.SOM
{
    public partial class MDO
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        public class Unk1
        {
            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk00;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk04;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk08;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk0C;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk10;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk14;

            /// <summary>
            /// Unknown.
            /// </summary>
            public float Unk18;

            internal Unk1(BinaryReaderEx br)
            {
                Unk00 = br.ReadSingle();
                Unk04 = br.ReadSingle();
                Unk08 = br.ReadSingle();
                Unk0C = br.ReadSingle();
                Unk10 = br.ReadSingle();
                Unk14 = br.ReadSingle();
                Unk18 = br.ReadSingle();
                br.AssertInt32(0);
            }
        }
    }
}
