using System;
using System.Collections.Generic;
using System.IO;

namespace SoulsFormats
{
    public partial class MSBFA
    {
        /// <summary>
        /// A section containing layers. Purpose unknown.
        /// Not present in system MSB files as indicated by version number on all params.
        /// </summary>
        public class LayerParam : Param<Layer>
        {
            internal override int Version => throw new NotImplementedException();
            internal override string Name => "LAYER_PARAM_ST";

            /// <summary>
            /// The layers in this section.
            /// </summary>
            public List<Layer> Layers { get; set; }

            /// <summary>
            /// Creates a new LayerParam with no layers.
            /// </summary>
            public LayerParam()
            {
                Layers = new List<Layer>();
            }

            /// <summary>
            /// Returns every layer in the order they will be written.
            /// </summary>
            public override List<Layer> GetEntries()
            {
                return Layers;
            }

            internal override Layer ReadEntry(BinaryReaderEx br)
            {
                return Layers.EchoAdd(new Layer(br));
            }
        }
        /// <summary>
        /// Unknown; seems to be related to different difficulty options in missions.
        /// </summary>
        public class Layer : NamedEntry
        {
            /// <summary>
            /// The name of this layer. Usually "normal", "hard", or variations of "temp". Always in lowercase, never the same name as another layer.
            /// </summary>
            public override string Name { get; set; }

            /// <summary>
            /// Unknown; Some kind of ID or index unique to each layer. Can be out of order.
            /// </summary>
            public int LayerID { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk08 { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk0C { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk10 { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk14 { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk18 { get; set; }

            /// <summary>
            /// Unknown; seems to always be 0.
            /// </summary>
            public int Unk1C { get; set; }

            /// <summary>
            /// Creates a Layer with default values.
            /// </summary>
            public Layer()
            {
                Name = "newlayer";
            }

            /// <summary>
            /// Creates a deep copy of the layer.
            /// </summary>
            public Layer DeepCopy()
            {
                return (Layer)MemberwiseClone();
            }

            internal Layer(BinaryReaderEx br)
            {
                long start = br.Position;

                int nameOffset = br.ReadInt32();
                LayerID = br.ReadInt32();
                Unk08 = br.ReadInt32();
                Unk0C = br.ReadInt32();
                Unk10 = br.ReadInt32();
                Unk14 = br.ReadInt32();
                Unk18 = br.ReadInt32();
                Unk1C = br.ReadInt32();

                if (nameOffset == 0)
                    throw new InvalidDataException($"{nameof(nameOffset)} must not be 0.");

                br.Position = start + nameOffset;
                Name = br.ReadShiftJIS();
            }

            internal override void Write(BinaryWriterEx bw, int id)
            {
                long start = bw.Position;

                bw.ReserveInt32("NameOffset");
                bw.WriteInt32(LayerID);
                bw.WriteInt32(Unk08);
                bw.WriteInt32(Unk0C);
                bw.WriteInt32(Unk10);
                bw.WriteInt32(Unk14);
                bw.WriteInt32(Unk18);
                bw.WriteInt32(Unk1C);

                bw.FillInt32("NameOffset", (int)bw.Position - (int)start);
                bw.WriteShiftJIS(Name, true);
            }

            /// <summary>
            /// Returns the name, LayerID, and values of this layer.
            /// </summary>
            public override string ToString()
            {
                return $"{Name} ID: {LayerID} ({Unk08}, {Unk0C}, {Unk10}, {Unk14}, {Unk18}, {Unk1C})";
            }
        }
    }
}
