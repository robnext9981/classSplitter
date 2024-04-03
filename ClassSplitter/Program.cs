using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClassSplitter
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Add target folder from where the files will be split (full path)
            const string sourcePath = @"E:\Work\Xtel\Xtel\Core\Product\Xtel.SM1.SalesPromotion\"; 
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
            var root = tree.GetRoot();

            var namespaceDeclaration = (NamespaceDeclarationSyntax)root.ChildNodes().FirstOrDefault(n => n is NamespaceDeclarationSyntax);
            if (namespaceDeclaration == null)
            {
                Console.WriteLine("No namespace found in the given file.");
                return;
            }
            
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

            foreach (var classDeclaration in classes)
            {
                var newTree = SyntaxFactory.SyntaxTree(
                    SyntaxFactory.CompilationUnit()
                        .AddMembers(
                            namespaceDeclaration.WithMembers(
                                new SyntaxList<MemberDeclarationSyntax>(classDeclaration))));

                var targetDirectory = Path.Combine(targetPath, $"Split{nameOfFileToBeSplit}");
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                File.WriteAllText($"{targetDirectory}\\{classDeclaration.Identifier}TODO.cs", newTree.ToString());
            }

            Console.WriteLine($"Successfully split {nameOfFileToBeSplit} into {classes.Count} classes into separate files.");
        }
    }
}