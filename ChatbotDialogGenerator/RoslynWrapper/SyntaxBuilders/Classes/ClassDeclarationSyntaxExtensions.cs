namespace RoslynWrapper.SyntaxBuilders.Classes
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ClassDeclarationSyntaxExtensions
    {
        public static ClassDeclarationSyntax WithAttribute(this ClassDeclarationSyntax classDeclarationSyntax,
            AttributeSyntax attributeSyntax)
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