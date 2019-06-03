namespace RoslynWrapper.SyntaxBuilders.Statements.MethodCalls
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class MethodCallStatement
    {
        public static StatementSyntax BuildCall(string invokeTarget, params string[] arguments)
        {
            var argumentSyntaxes = arguments.ToList()
                .Select(arg => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(arg))).ToArray();

            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(invokeTarget))
                    .AddArgumentListArguments(argumentSyntaxes));
        }

        public static StatementSyntax BuildCallWithoutLastTrivia(string invokeTarget, params string[] arguments)
        {
            var argumentSyntaxes = arguments.ToList()
                .Select(arg => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(arg))).ToArray();

            return SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(invokeTarget))
                        .AddArgumentListArguments(argumentSyntaxes))
                .WithSemicolonToken(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken));
        }

        public static StatementSyntax BuildAwaitedCall(string invokeTarget, params string[] arguments)
        {
            var argumentSyntaxes = arguments.ToList()
                .Select(arg => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(arg))).ToArray();

            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AwaitExpression(
                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(invokeTarget))
                        .AddArgumentListArguments(argumentSyntaxes)));
        }
    }
}