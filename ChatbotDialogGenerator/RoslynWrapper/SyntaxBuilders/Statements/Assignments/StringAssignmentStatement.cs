namespace RoslynWrapper.SyntaxBuilders.Statements.Assignments
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class StringAssignmentStatement
    {
        public static ExpressionStatementSyntax Build(string variableName, string rightValue)
        {
           return  SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(variableName),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(rightValue))));
        }
    }
}