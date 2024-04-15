using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClassSplitter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Add target folder from where the files will be split (full path)
            var sourcePath = args[0]; 
            ProcessFilesInDirectory(sourcePath);
        }

        private static void ProcessFilesInDirectory(string directoryPath)
        {
            // Get all file paths from the directory
            var fileEntries = Directory.GetFiles(directoryPath);

            foreach (var fileName in fileEntries)
            {
                // Process .cs files
                if (Path.GetExtension(fileName) != ".cs") continue;
                // Call your SplitFileIntoClasses method here
                Console.WriteLine(fileName);
                SplitFileIntoClasses(fileName);
            }

            // Get all subdirectory paths from the directory
            var subDirectoryEntries = Directory.GetDirectories(directoryPath);

            foreach (var subDirectoryPath in subDirectoryEntries)
            {
                // Recurse into subdirectories
                ProcessFilesInDirectory(subDirectoryPath);
            }
        }

        private static void SplitFileIntoClasses(string sourcePath)
        {
            var nameOfFileToBeSplit = sourcePath.Split(@"\").Last().Replace(".cs", string.Empty);
            var targetPath = sourcePath.Replace(sourcePath.Split(@"\").Last(), "");

            var code = File.ReadAllText(sourcePath);

            var tree = CSharpSyntaxTree.ParseText(code);
            var root =  tree.GetCompilationUnitRoot();
            if (root == null || root.Members.Count == 0)
            {
                Console.WriteLine($"No root member found on {nameOfFileToBeSplit} file.");
                return;
            }

            var namespaceDeclaration = (NamespaceDeclarationSyntax)root.Members[0];
            if(namespaceDeclaration == null)
            {
                Console.WriteLine($"No namespace as first elemnt found on {nameOfFileToBeSplit} file.");
                return;
            }

            List<BaseTypeDeclarationSyntax> types = root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>().ToList();

            if (types.Count == 1)
            {
                Console.WriteLine($"File {nameOfFileToBeSplit} contains a single type and it was NOT split");
                return;
            }

            SplitForType(nameOfFileToBeSplit, targetPath, namespaceDeclaration, types);

            Console.WriteLine($"Successfully split {nameOfFileToBeSplit} into {types.Count} classes into separate files.");
        }
        private static void SplitForType(string nameOfFileToBeSplit, string targetPath, NamespaceDeclarationSyntax namespaceDeclaration, IList<BaseTypeDeclarationSyntax> types)
        {
            foreach (var type in types)
            {
                var newTree = SyntaxFactory.SyntaxTree(
                    SyntaxFactory.CompilationUnit()
                        .AddMembers(
                            namespaceDeclaration.WithMembers(
                                new SyntaxList<MemberDeclarationSyntax>(type))));

                var targetDirectory = Path.Combine(targetPath, $"Split{nameOfFileToBeSplit}");
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                File.WriteAllText($"{targetDirectory}\\{type.Identifier}_{type.GetType()}_Splitted.cs", newTree.ToString());
            }
        }
    }
}