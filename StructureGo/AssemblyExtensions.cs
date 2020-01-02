using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StructureGo
{
    static class AssemblyExtensions
    {
        public static string InheritDescriptions(this Type t)
        {
            if (t.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) != null)
            {
                return "";
            }

            var interfaces = t.GetInterfaces();
            if (interfaces.Contains(typeof(IAsyncStateMachine)))
            {
                return "";
            }
            if (t.IsInterface)
            {
                return $"\"{t.FullName}\" -> \"Interface\" -> \"Object\"";
            }
            
            var baseType = t.BaseType;
            var fullName = t.Name;

            if (baseType == null)
            {
                return $"\"{fullName}\"";
            }
            else
            {
                if (t.IsGenericType)
                {
                    var nameOfGeneric = $"\"{t.Namespace}.{t.Name}\"";
                    return $"{nameOfGeneric} -> {InheritDescriptions(baseType)}";
                }
                return $"\"{fullName}\" -> {InheritDescriptions(baseType)}";
            }
        }

        public static string NamespaceDescription(this Type t)
        {
            try
            {

                if (t.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) != null)
                {
                    return "";
                }

                var interfaces = t.GetInterfaces();
                if (interfaces.Contains(typeof(IAsyncStateMachine)))
                {
                    return "";
                }

                var ns = t.Namespace;

                var names = ns.Split('.');
                var nearName = names[^1];
                return $"\"{t.Name}\"-> \"{nearName}\"";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

        }
    }
}