using System;

namespace StructureGo
{
    public class NameSpace : IEquatable<NameSpace>
    {
        public NameSpace Father { get; set; }

        public string Name { get; set; }

        public NameSpace(string @namespace)
        {
            if (@namespace != "")
            {
                var lastPointIndex = @namespace.LastIndexOf('.');

                Name = @namespace.Substring(lastPointIndex + 1);
                if (lastPointIndex >= 0)
                {
                    Father = new NameSpace(@namespace.Substring(0, lastPointIndex));
                }
            }
        }

        public bool Equals(NameSpace other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Father, other.Father) && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NameSpace)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Father, Name);
        }

        public string FullName {
            get {
                if (Father != null)
                {
                    return $"{Father.FullName}.{Name}";
                }
                else
                {
                    return Name;
                }
            }
        }

        public override string ToString()
        {
            if (Father != null)
            {
                return $"\"{FullName}\" -> \"{Father.FullName}\"";
            }
            else
            {
                return $"\"{FullName}\"";
            }
        }
    }
}