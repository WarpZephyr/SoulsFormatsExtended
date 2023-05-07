using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsFormats
{
    public partial class MSBFA
    {
        internal enum ModelType : uint
        {
            ModelTypeMapPiece = 0,
            ModelTypeObject = 1,
            ModelTypeEnemy = 2,
            ModelTypeDummy = 4
        }

        /// <summary>
        /// Model files that are available for parts to use.
        /// </summary>
        public class ModelParam : Param<Model>, IMsbParam<IMsbModel>
        {
            internal override int Version => throw new NotImplementedException();
            internal override string Name => "MODEL_PARAM_ST";

            /// <summary>
            /// Creates an empty ModelParam.
            /// </summary>
            public ModelParam() : base()
            {
                throw new NotImplementedException();
            }

            internal override Model ReadEntry(BinaryReaderEx br)
            {
                ModelType type = br.GetEnum32<ModelType>(br.Position + 4);
                switch (type)
                {

                    default:
                        throw new NotImplementedException($"Unimplemented model type: {type}");
                }
            }

            /// <summary>
            /// Adds a model to the appropriate list for its type; returns the model.
            /// </summary>
            public Model Add(Model model)
            {
                switch (model)
                {

                    default:
                        throw new ArgumentException($"Unrecognized type {model.GetType()}.", nameof(model));
                }
                return model;
            }
            IMsbModel IMsbParam<IMsbModel>.Add(IMsbModel item) => Add((Model)item);

            /// <summary>
            /// Returns every Model in the order they will be written.
            /// </summary>
            public override List<Model> GetEntries()
            {
                return SFUtil.ConcatAll<Model>( );
            }
            IReadOnlyList<IMsbModel> IMsbParam<IMsbModel>.GetEntries() => GetEntries();
        }

        /// <summary>
        /// A model file available for parts to reference.
        /// </summary>
        public abstract class Model : Entry, IMsbModel
        {
            private protected abstract ModelType Type { get; }

            /// <summary>
            /// The name of the model.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// A path to a .sib file, presumed to be some kind of editor placeholder.
            /// </summary>
            public string SibPath { get; set; }

            private int InstanceCount;

            private protected Model(string name)
            {
                Name = name;
                SibPath = "";
            }

            /// <summary>
            /// Creates a deep copy of the model.
            /// </summary>
            public Model DeepCopy()
            {
                return (Model)MemberwiseClone();
            }
            IMsbModel IMsbModel.DeepCopy() => DeepCopy();

            private protected Model(BinaryReaderEx br)
            {
                throw new NotImplementedException();
            }

            internal override void Write(BinaryWriterEx bw, int id)
            {
                throw new NotImplementedException();
            }

            internal void CountInstances(List<Part> parts)
            {
                InstanceCount = parts.Count(p => p.ModelName == Name);
            }

            /// <summary>
            /// Returns a string representation of the model.
            /// </summary>
            public override string ToString()
            {
                return $"{Type} {Name}";
            }
        }
    }
}
