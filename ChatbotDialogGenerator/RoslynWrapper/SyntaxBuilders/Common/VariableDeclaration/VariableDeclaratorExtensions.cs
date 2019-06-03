namespace RoslynWrapper.SyntaxBuilders.Common.VariableDeclaration
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class VariableDeclaratorExtensions
    {
        public static VariableDeclaratorSyntax WithInitializer(this VariableDeclaratorSyntax declarator, string initializer)
        {
            if (declarator == null)
            {
                throw new ArgumentNullException(nameof(declarator));
            }

            return declarator.WithInitializer(
                SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.IdentifierName(initializer)));
        }

        public static VariableDeclaratorSyntax WithAwaitedInitializer(this VariableDeclaratorSyntax declarator, string initializer)
        {
            if (declarator == null)
            {
                throw new ArgumentNullException(nameof(declarator));
            }

            return declarator.WithInitializer(
                SyntaxFactory.EqualsValueClause(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.IdentifierName(initializer))));
        }
    }
}