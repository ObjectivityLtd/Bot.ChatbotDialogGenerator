namespace RoslynWrapper.SyntaxBuilders.Statements.Returns
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ReturnVariableStatement
    {
        public static StatementSyntax Build(string variableName)
        {
            return SyntaxFactory.ReturnStatement(
                SyntaxFactory.IdentifierName(variableName));
        }
    }
}