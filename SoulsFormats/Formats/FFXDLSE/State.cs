using System.Collections.Generic;

namespace SoulsFormats
{
    public partial class FFXDLSE
    {
        /// <summary>
        /// A state which contains actions to be executed and triggers to evaluate which state is next.
        /// </summary>
        public class State : FXSerializable
        {
            /// <summary>
            /// Actions to execute.
            /// </summary>
            public List<Action> Actions { get; set; }

            /// <summary>
            /// Triggers to evaluate which state is next.
            /// </summary>
            public List<Trigger> Triggers { get; set; }

            /// <summary>
            /// Create a new <see cref="State"/>.
            /// </summary>
            public State()
            {
                Actions = new List<Action>();
                Triggers = new List<Trigger>();
            }

            #region FXSerializable

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override string ClassName => "FXSerializableState";

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override int Version => 1;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal State(BinaryReaderEx br, List<string> classNames) : base(br, classNames) { }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Deserialize(BinaryReaderEx br, List<string> classNames)
            {
                int actionCount = br.ReadInt32();
                int triggerCount = br.ReadInt32();
                Actions = new List<Action>(actionCount);
                for (int i = 0; i < actionCount; i++)
                    Actions.Add(new Action(br, classNames));
                Triggers = new List<Trigger>(triggerCount);
                for (int i = 0; i < triggerCount; i++)
                    Triggers.Add(new Trigger(br, classNames));
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            internal override void AddClassNames(List<string> classNames)
            {
                base.AddClassNames(classNames);
                foreach (Action action in Actions)
                    action.AddClassNames(classNames);
                foreach (Trigger trigger in Triggers)
                    trigger.AddClassNames(classNames);
            }

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            protected internal override void Serialize(BinaryWriterEx bw, List<string> classNames)
            {
                bw.WriteInt32(Actions.Count);
                bw.WriteInt32(Triggers.Count);
                foreach (Action action in Actions)
                {
                    action.Write(bw, classNames);
                }

                foreach (Trigger trigger in Triggers)
                {
                    trigger.Write(bw, classNames);
                }
            }

            #endregion
        }
    }
}
