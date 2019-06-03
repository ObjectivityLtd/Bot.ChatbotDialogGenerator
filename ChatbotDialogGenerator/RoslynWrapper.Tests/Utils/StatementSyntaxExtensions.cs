namespace RoslynWrapper.Tests.Utils
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class StatementSyntaxExtensions
    {
        public static LocalDeclarationStatementSyntax ConvertToLocalDeclarationStatementSyntax(this string statement)
        {
            return SyntaxFactory.LocalDeclarationStatement(statement
                .ExtractDeclarationSyntaxFromString<VariableDeclarationSyntax>());
        }
    }
}