namespace RoslynWrapper.Tests.Utils
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public static class DeclarationTypesExtensions
    {
        public static T ExtractDeclarationSyntaxFromString<T>(this string member)
        {
            var tree = CSharpSyntaxTree.ParseText(member);
            if (!tree.IsTreeValid())
            {
                throw new InvalidSyntaxTreeException();
            }

            var descendantNodes = tree.GetRoot().DescendantNodes();

            return descendantNodes.OfType<T>()
                .FirstOrDefault();
        }

        private static bool IsTreeValid(this SyntaxTree tree)
        {
            return tree.GetDiagnostics().All(n => n.Severity != DiagnosticSeverity.Error);
        }
    }
}