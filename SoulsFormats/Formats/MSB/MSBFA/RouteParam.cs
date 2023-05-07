using System;
using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    public partial class MSBFA
    {
        /// <summary>
        /// A section containing routes. May be routes AI can follow from how names describe them.
        /// </summary>
        public class RouteParam : Param<Route>
        {
            internal override int Version => throw new NotImplementedException();
            internal override string Name => "ROUTE_PARAM_ST";

            /// <summary>
            /// The routes in this section.
            /// </summary>
            public List<Route> Routes { get; set; }

            /// <summary>
            /// Creates a new RouteParam with no routes.
            /// </summary>
            public RouteParam()
            {
                Routes = new List<Route>();
            }

            /// <summary>
            /// Returns every route in the order they will be written.
            /// </summary>
            public override List<Route> GetEntries()
            {
                return Routes;
            }

            internal override Route ReadEntry(BinaryReaderEx br)
            {
                return Routes.EchoAdd(new Route(br));
            }
        }

        /// <summary>
        /// Unknown. May be routes AI can follow from how names describe them.
        /// </summary>
        public class Route : NamedEntry
        {
            /// <summary>
            /// The name of this route.
            /// </summary>
            public override string Name { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk04 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk08 { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk0C { get; set; }

            /// <summary>
            /// Unknown; Assumed to be ID from how it is unique to each route.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Unknown.
            /// </summary>
            public int Unk14 { get; set; }

            /// <summary>
            /// Creates a new Route with default values.
            /// </summary>
            public Route()
            {
                Name = "NewRoute";
            }

            /// <summary>
            /// Creates a deep copy of the route.
            /// </summary>
            public Route DeepCopy()
            {
                return (Route)MemberwiseClone();
            }

            internal Route(BinaryReaderEx br)
            {
                long start = br.Position;

                int nameOffset = br.ReadInt32();
                Unk04 = br.ReadInt32();
                Unk08 = br.ReadInt32();
                Unk0C = br.ReadInt32();
                ID = br.ReadInt32();
                Unk14 = br.ReadInt32();
                br.AssertPattern(0x68, 0);

                if (nameOffset == 0)
                    throw new InvalidDataException($"{nameof(nameOffset)} must not be 0.");

                br.Position = start + nameOffset;
                Name = br.ReadShiftJIS();
            }

            internal override void Write(BinaryWriterEx bw, int id)
            {
                long start = bw.Position;

                bw.ReserveInt32("NameOffset");
                bw.WriteInt32(Unk04);
                bw.WriteInt32(Unk08);
                bw.WriteInt32(Unk0C);
                bw.WriteInt32(ID);
                bw.WriteInt32(Unk14);
                bw.WritePattern(0x68, 0);

                bw.FillInt32("NameOffset", (int)bw.Position - (int)start);
                bw.WriteShiftJIS(Name, true);
            }

            /// <summary>
            /// Returns the name, ID, and values of this route.
            /// </summary>
            public override string ToString()
            {
                return $"{Name} ID: {ID} ({Unk04}, {Unk08}, {Unk0C}, {Unk14})";
            }
        }
    }
}
