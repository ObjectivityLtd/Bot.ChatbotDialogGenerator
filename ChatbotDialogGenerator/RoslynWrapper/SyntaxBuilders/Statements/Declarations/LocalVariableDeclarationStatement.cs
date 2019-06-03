namespace RoslynWrapper.SyntaxBuilders.Statements.Declarations
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common.VariableDeclaration;

    public static class LocalVariableDeclarationStatement
    {
        public static LocalDeclarationStatementSyntax Build(string variableType, string variableName)
        {
            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(variableType))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.VariableDeclarator(variableName))));
        }

        public static LocalDeclarationStatementSyntax BuildWithAwaitedDeclaration(string variableType, string variableName, string rightValue)
        {
            var variableDeclarator = SyntaxFactory.VariableDeclarator(variableName)
                .WithAwaitedInitializer(rightValue);

            return SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                        SyntaxFactory.IdentifierName(variableType))
                    .WithVariables(
                        SyntaxFactory.SingletonSeparatedList(variableDeclarator)));
        }
    }
}