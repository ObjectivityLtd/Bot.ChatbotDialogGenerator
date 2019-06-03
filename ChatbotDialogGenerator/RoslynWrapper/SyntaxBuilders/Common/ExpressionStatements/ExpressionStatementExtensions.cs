namespace RoslynWrapper.SyntaxBuilders.Common.ExpressionStatements
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ExpressionStatementExtensions
    {
        public static ExpressionStatementSyntax AssignValueToField(string fieldName, string rightValue)
        {
            var left = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ThisExpression(),
                SyntaxFactory.IdentifierName(fieldName));

            var right = SyntaxFactory.IdentifierName(rightValue);

            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    left, right));
        }
    }
}