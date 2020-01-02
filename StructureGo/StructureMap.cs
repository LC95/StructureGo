using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace StructureGo
{
    public class StructureMap
    {
        private readonly Dictionary<NameSpace, List<Structure>> _nameSpaces = new Dictionary<NameSpace, List<Structure>>();

        public void AddTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                if (type.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) != null)
                {
                    continue;
                }

                var interfaces = type.GetInterfaces();
                if (interfaces.Contains(typeof(IAsyncStateMachine)))
                {
                    continue;
                }
                var structure = new Structure(type);
                if (!_nameSpaces.ContainsKey(structure.NameSpace))
                {
                    _nameSpaces[structure.NameSpace] = new List<Structure>()
                    {
                        structure
                    };
                }
                else
                {
                    _nameSpaces[structure.NameSpace].Add(structure);
                }
            }
        }

        public string ToString(string center)
        {
            var sb = new StringBuilder(20000);

            void AppendNameSpaceNode()
            {
                sb.Append("//namespace:\n");
                sb.Append("{\n node [shape=ellipse]");
                foreach (var (nameSpace, _) in _nameSpaces)
                {
                    sb.Append($"\"{nameSpace.FullName}\"[Label=\"{nameSpace.Name}\"]");
                    sb.Append(Environment.NewLine);
                }

                foreach (var (nameSpace, _) in _nameSpaces)
                {
                    sb.Append(nameSpace);
                    sb.Append(Environment.NewLine);
                }
                sb.Append("}\n");
            }

            void AppendNodeDirections()
            {
                sb.Append("//directions:\n");
                sb.Append("{\n node [shape=none]");
                foreach (var (_, structures) in _nameSpaces)
                {
                    foreach (var structure in structures)
                    {
                        sb.Append(structure + Environment.NewLine);
                    }
                }
                sb.Append("}\n");
            }

            sb.Append($"digraph G{{ concentrate = true \n rankdir = LR \n ranksep = 10 \n overlap = true \n root =  \"{center}\" \n");
            AppendNameSpaceNode();
            AppendNodeDirections();
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}