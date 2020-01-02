using System;

namespace StructureGo
{
    public class Structure
    {
        public NameSpace NameSpace { get; set; }

        public string Name { get; set; }

        public Structure(Type type)
        {
            Name = type.Name;
            NameSpace = new NameSpace(type.Namespace);
        }

        public override string ToString()
        {
            return $"\"{Name}\" -> \"{NameSpace.FullName}\"";
        }
    }
}
