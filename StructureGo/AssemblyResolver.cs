using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace StructureGo
{
    internal sealed class AssemblyResolver : IDisposable
    {
        private readonly ICompilationAssemblyResolver _assemblyResolver;
        private readonly DependencyContext _dependencyContext;
        private readonly AssemblyLoadContext _loadContext;
        public Assembly Assembly { get; }

        public AssemblyResolver(string path)
        {
            Assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

            _dependencyContext = DependencyContext.Load(Assembly);
            var nugetPackagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                @".nuget\packages");
            var dotnetPackagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                @"dotnet\shared");
            _assemblyResolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
            {
                new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
                new PackageCompilationAssemblyResolver(nugetPackagePath),
                new PackageCompilationAssemblyResolver(dotnetPackagePath),
            });

            _loadContext = AssemblyLoadContext.GetLoadContext(Assembly);
            _loadContext.Resolving += OnResolving;
        }


        public void Dispose()
        {
            _loadContext.Resolving -= OnResolving;
        }

        private Assembly OnResolving(AssemblyLoadContext context, AssemblyName name)
        {
            bool NamesMatch(CompilationLibrary cl)
            {
                var assPath = cl.Assemblies.FirstOrDefault();
                if (assPath != null)
                {
                    var assName = Path.GetFileNameWithoutExtension(assPath);
                    return assName.Contains(name.Name, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }

            var library =
                _dependencyContext.CompileLibraries.FirstOrDefault(NamesMatch);
            if (name.Name.StartsWith("Util"))
            {
                var a = ";";
            }

            if (library != null)
            {
                var paths = library.ResolveReferencePaths(_assemblyResolver).ToList();
                return _loadContext.LoadFromAssemblyPath(paths[0]);
            }

            return null;
        }
    }
}