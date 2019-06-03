namespace RoslynWrapper.SyntaxBuilders.Common.Attributes
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class AttributeSyntaxExtensions
    {
        public static AttributeSyntax WithTypeOfArgument(this AttributeSyntax attributeSyntax, string argument)
        {
            if (attributeSyntax == null)
            {
                throw new ArgumentNullException(nameof(attributeSyntax));
            }

            if (argument == null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            return attributeSyntax.AddArgumentListArguments(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.TypeOfExpression(
                                SyntaxFactory.IdentifierName(argument))));
        }
    }
}