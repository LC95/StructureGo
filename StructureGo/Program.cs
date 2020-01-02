using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StructureGo
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Begin To Process");
             
                if (!args.Any())
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), $"{nameof(StructureGo)}.dll");
                    var dotSource = AnalyzeNamespaceToDot(path, nameof(StructureGo));
                    Graphviz.RenderImageToFile(dotSource, "svg", $"{nameof(StructureGo)}.svg");
                }
                else
                {
                    var dotSource = AnalyzeNamespaceToDot(args[0], args[1]);
                    Graphviz.RenderImageToFile(dotSource, "svg", $"{args[1]}.svg");
                }

                Console.WriteLine("Finish!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }

        public static string AnalyzeNamespaceToDot(string assemblyPath, string nameSpace)
        {
            using var assRes = GetAssemblyResolver(assemblyPath);
            var ass = assRes.Assembly;
            var types = ass.GetTypes().Where(t => t.Namespace != null && t.Namespace.Contains(nameSpace)).ToArray();
            var structureMap = new StructureMap();
            structureMap.AddTypes(types);
            return structureMap.ToString(nameSpace);
        }


        public static void AnalyzeAssembly(string assemblyPath)
        {
            using (var a = GetAssemblyResolver(assemblyPath))
            {
                var ass = a.Assembly;
                AnalyzeTypesByInherits(ass.GetTypes());
            }
        }

        public static AssemblyResolver GetAssemblyResolver(string assemblyPath)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), assemblyPath);
            var dynamicContext = new AssemblyResolver(path);
            return dynamicContext;

        }

        private static void AnalyzeTypesByInherits(IEnumerable<Type> types)
        {
            var hashSet = new HashSet<string>();
            hashSet.UnionWith(types.Select(t => t.InheritDescriptions()));
            var final = string.Concat(hashSet.Select(t => t + "\n"));
            Console.WriteLine(final);
        }


    }
}