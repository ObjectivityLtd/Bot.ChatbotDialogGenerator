namespace RoslynWrapper.SyntaxBuilders.Statements.Conditions
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class IfStatementPayload
    {
        public string Condition { get; set; }

        public BlockSyntax Body { get; set; }
    }
}