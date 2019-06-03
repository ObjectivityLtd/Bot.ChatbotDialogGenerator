namespace RoslynWrapper.SyntaxBuilders.Common.Parameters
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ParametersExtensions
    {
        public static ParameterSyntax Parameter(Parameter newParameter)
        {
            if (newParameter == null)
            {
                throw new ArgumentNullException(nameof(newParameter));
            }

            return SyntaxFactory.Parameter(SyntaxFactory.Identifier(newParameter.ParameterName))
                .WithType(SyntaxFactory.IdentifierName(newParameter.TypeName));
        }
    }
}