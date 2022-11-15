using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;


namespace Splatoon.SplatoonScripting
{
    internal class Compiler
    {
        public static byte[] Compile(string sourceCode)
        {
            using (var peStream = new MemoryStream())
            {
                var result = GenerateCode(sourceCode).Emit(peStream);

                if (!result.Success)
                {
                    PluginLog.Warning("Compilation done with error.");

                    var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        PluginLog.Warning($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                    }

                    return null;
                }

                PluginLog.Information("Compilation done without any error.");

                peStream.Seek(0, SeekOrigin.Begin);

                return peStream.ToArray();
            }
        }

        private static CSharpCompilation GenerateCode(string sourceCode)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp11);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var references = new List<MetadataReference>();
            foreach (var f in Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location), "*", SearchOption.AllDirectories))
            {
                if (IsValidAssembly(f)) references.Add(MetadataReference.CreateFromFile(f));
            }
            foreach (var f in Directory.GetFiles(Svc.PluginInterface.AssemblyLocation.DirectoryName, "*", SearchOption.AllDirectories))
            {
                if (IsValidAssembly(f)) references.Add(MetadataReference.CreateFromFile(f));
            }
            foreach (var f in Directory.GetFiles(Path.GetDirectoryName(Svc.PluginInterface.GetType().Assembly.Location), "*", SearchOption.AllDirectories))
            {
                if (IsValidAssembly(f)) references.Add(MetadataReference.CreateFromFile(f));
            }

            PluginLog.Information($"References: {references.Select(x => x.Display).Join(", ")}");


            return CSharpCompilation.Create("Hello.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }
        static bool IsValidAssembly(string path)
        {
            try
            {
                // Attempt to resolve the assembly
                var assembly = AssemblyName.GetAssemblyName(path);
                // Nothing blew up, so it's an assembly
                return true;
            }
            catch (Exception ex)
            {
                // Something went wrong, it is not an assembly (specifically a 
                // BadImageFormatException will be thrown if it could be found
                // but it was NOT a valid assembly
                return false;
            }
        }
    }
}
