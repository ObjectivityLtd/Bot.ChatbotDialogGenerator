namespace RoslynWrapper.SyntaxBuilders.Classes.Constructors
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ConstructorDeclarationSyntaxExtensions
    {
        public static ConstructorDeclarationSyntax AddAttribute(this ConstructorDeclarationSyntax constructor, AttributeSyntax attribute)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            return constructor.AddAttributeLists(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList(attribute)));
        }
    }
}