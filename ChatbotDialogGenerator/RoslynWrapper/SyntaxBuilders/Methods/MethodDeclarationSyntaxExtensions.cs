namespace RoslynWrapper.SyntaxBuilders.Methods
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class MethodDeclarationSyntaxExtensions
    {
        public static MethodDeclarationSyntax WithAttribute(this MethodDeclarationSyntax classDeclarationSyntax, AttributeSyntax attributeSyntax)
        {
            if (classDeclarationSyntax == null)
            {
                throw new ArgumentNullException(nameof(classDeclarationSyntax));
            }

            return classDeclarationSyntax.AddAttributeLists(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(attributeSyntax)));
        }
    }
}