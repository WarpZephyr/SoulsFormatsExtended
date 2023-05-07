namespace SoulsFormats
{
    public partial class AcPartsFA
    {
        /// <summary>
        /// A Stabilizer on top of Head parts in an ACPARTS file.
        /// </summary>
        public class HeadTopStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Head Top Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal HeadTopStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Head Top Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the sides of Head parts in an ACPARTS file.
        /// </summary>
        public class HeadSideStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Head Side Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal HeadSideStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Head Side Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the upper sides of Core parts in an ACPARTS file.
        /// </summary>
        public class CoreUpperSideStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Creates a new Core Upper Side Stabilizer part.
            /// </summary>
            /// <param name="br">A binary reader</param>
            internal CoreUpperSideStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Core Upper Side Stabilizer part to a binary writer stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the lower sides of Core parts in an ACPARTS file.
        /// </summary>
        public class CoreLowerSideStabilizer : IPart
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Core Lower Side Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal CoreLowerSideStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Core Lower Side Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on Arm parts in an ACPARTS file
        /// </summary>
        public class ArmStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads an Arm Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal ArmStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes an Arm Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the back of Leg parts in an ACPARTS file.
        /// </summary>
        public class LegBackStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Leg Back Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal LegBackStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Leg Back Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the upper end of Leg parts in an ACPARTS file.
        /// </summary>
        public class LegUpperStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Leg Upper Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal LegUpperStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Leg Upper Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer in the middle of Leg parts in an ACPARTS file.
        /// </summary>
        public class LegMiddleStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Creates a new Leg Middle Stabilizer part.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal LegMiddleStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Leg Middle Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }

        /// <summary>
        /// A Stabilizer on the lower end of Leg parts in an ACPARTS file.
        /// </summary>
        public class LegLowerStabilizer : IPart, IStabilizer
        {
            /// <summary>
            /// A Component which contains common stats across all parts.
            /// </summary>
            public PartComponent PartComponent { get; set; }

            /// <summary>
            /// A Component which contains Stabilizer stats.
            /// </summary>
            public StabilizerComponent StabilizerComponent { get; set; }

            /// <summary>
            /// Reads a Leg Lower Stabilizer part from a stream.
            /// </summary>
            /// <param name="br">A binary reader.</param>
            internal LegLowerStabilizer(BinaryReaderEx br)
            {
                PartComponent = new PartComponent(br);
                StabilizerComponent = new StabilizerComponent(br);
            }

            /// <summary>
            /// Writes a Leg Lower Stabilizer part to a stream.
            /// </summary>
            /// <param name="bw">A binary writer.</param>
            public void Write(BinaryWriterEx bw)
            {
                PartComponent.Write(bw);
                StabilizerComponent.Write(bw);
            }
        }
    }
}
