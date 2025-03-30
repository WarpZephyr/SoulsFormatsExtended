using System;
using System.Collections.Generic;

namespace SoulsFormats
{
    internal class Ap2Parser
    {
        private List<Ap2Node> Nodes { get; set; }

        void Parse(string text)
        {
            int nodeStartIndex = text.IndexOf('#');
            int valueStartIndex = text.IndexOf(':', nodeStartIndex);
            int childrenStartIndex = text.IndexOf('{', nodeStartIndex);

            bool valueNode;
            int dataIndex;
            int dataEndIndex;
            if (valueStartIndex < childrenStartIndex)
            {
                valueNode = true;
                dataIndex = valueStartIndex;
                dataEndIndex = text.IndexOf('\n', dataIndex);
            }
            else
            {
                valueNode = false;
                dataIndex = childrenStartIndex;
                dataEndIndex = text.IndexOf('}', dataIndex);
            }
        }

        internal class Ap2Node
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Ap2Node(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public Ap2Node(string name)
            {
                Name = name;
                Value = string.Empty;
            }
        }

        internal class Ap2ParseException : Exception
        {
            public Ap2ParseException() : base()
            {

            }

            public Ap2ParseException(string message) : base(message)
            {

            }
        }
    }
}
