using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClassSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add file to be split (full path)
            const string sourcePath = @"E:\Work\Xtel\Xtel\Core\Product\Xtel.SM1.SalesPlan\SalesPlanEngineExtensionServer.cs";
            // Add target folder where resulted files will be added (full path)
            const string targetPath = @"E:\Work\Xtel\Xtel\Core\Product\Xtel.SM1.SalesPromotion\"; 
            
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

                File.WriteAllText($"{targetPath}{classDeclaration.Identifier}.cs", newTree.ToString());
            }

            Console.WriteLine($"Successfully split {classes.Count} classes into separate files.");
        }
    }
}