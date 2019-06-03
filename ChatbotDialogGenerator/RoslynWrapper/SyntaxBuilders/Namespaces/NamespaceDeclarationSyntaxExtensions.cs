namespace RoslynWrapper.SyntaxBuilders.Namespaces
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class NamespaceDeclarationSyntaxExtensions
    {
        public static NamespaceDeclarationSyntax NamespaceDeclaration(string namespaceName)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(namespaceName));
        }
    }
}