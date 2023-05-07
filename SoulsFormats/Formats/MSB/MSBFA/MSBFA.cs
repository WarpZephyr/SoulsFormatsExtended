using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    /// <summary>
    /// A map layout file used in Armored Core: For Answer. Extension: .msb
    /// </summary>
    public partial class MSBFA : SoulsFile<MSBFA>, IMsb
    {
        /// <summary>
        /// Model files that are available for parts to use.
        /// </summary>
        public ModelParam Models { get; set; }
        IMsbParam<IMsbModel> IMsb.Models => Models;

        /// <summary>
        /// Dynamic or interactive systems.
        /// </summary>
        public EventParam Events { get; set; }
        IMsbParam<IMsbEvent> IMsb.Events => Events;

        /// <summary>
        /// Routes in this MSB.
        /// </summary>
        public List<Route> Routes { get; set; }

        /// <summary>
        /// Layers in this MSB.
        /// </summary>
        public List<Layer> Layers { get; set; }

        /// <summary>
        /// Points or areas of space that trigger some sort of behavior.
        /// </summary>
        public PointParam Regions { get; set; }
        IMsbParam<IMsbRegion> IMsb.Regions => Regions;

        /// <summary>
        /// Instances of actual things in the map.
        /// </summary>
        public PartsParam Parts { get; set; }
        IMsbParam<IMsbPart> IMsb.Parts => Parts;

        /// <summary>
        /// Unknown; Relates to rendering somehow.
        /// </summary>
        public List<Tree> Trees { get; set; }

        internal struct Entries
        {
            public List<Model> Models;
            public List<Event> Events;
            public List<Region> Regions;
            public List<Part> Parts;
        }

        /// <summary>
        /// Deserializes file data from a stream.
        /// </summary>
        protected override void Read(BinaryReaderEx br)
        {
            br.BigEndian = true;

            Entries entries;
            Models = new ModelParam();
            entries.Models = Models.Read(br);
            Events = new EventParam();
            entries.Events = Events.Read(br);
            Routes = new RouteParam().Read(br);
            Layers = new LayerParam().Read(br);
            Regions = new PointParam();
            entries.Regions = Regions.Read(br);
            Parts = new PartsParam();
            entries.Parts = Parts.Read(br);
            var tree = new MapStudioTree();
            Trees = tree.Read(br);
        }

        /// <summary>
        /// Serializes file data to a stream.
        /// </summary>
        protected override void Write(BinaryWriterEx bw)
        {

        }

        /// <summary>
        /// A generic MSB section containing a list of entries.
        /// </summary>
        public abstract class Param<T> where T : Entry
        {
            /// <summary>
            /// This param's version, indicating whether or not a Layer param is needed. System MSB do not have a Layer param.
            /// </summary>
            internal abstract int Version { get; }

            /// <summary>
            /// This param's name string indicating its type.
            /// </summary>
            internal abstract string Name { get; }

            /// <summary>
            /// Read a param entry.
            /// </summary>
            internal abstract T ReadEntry(BinaryReaderEx br);

            /// <summary>
            /// Returns every entry in this section in the order they will be written.
            /// </summary>
            public abstract List<T> GetEntries();

            /// <summary>
            /// Read a param from a BinaryReaderEx.
            /// </summary>
            internal List<T> Read(BinaryReaderEx br)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Write a param to a BinaryWriterEx.
            /// </summary>
            internal void Write(BinaryWriterEx bw, List<T> entries)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns a string representation of the param.
            /// </summary>
            public override string ToString()
            {
                return $"{Name}:[{GetEntries().Count}]";
            }
        }

        /// <summary>
        /// A generic entry in an MSB param.
        /// </summary>
        public abstract class Entry
        {
            internal abstract void Write(BinaryWriterEx bw, int id);
        }

        /// <summary>
        /// A generic entry in an MSB param that has a name.
        /// </summary>
        public abstract class NamedEntry : Entry, IMsbEntry
        {
            /// <summary>
            /// The name of this entry; should generally be unique.
            /// </summary>
            public abstract string Name { get; set; }
        }
    }
}
