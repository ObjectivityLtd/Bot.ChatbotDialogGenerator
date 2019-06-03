namespace ChatbotDialogGenerator.Generation.DialogGeneration
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Steps;

    public class GeneratedDialog
    {
        public CompilationUnitSyntax GeneratedCode { get; set; }
        public List<StringResource> Resources { get; set; }
    }
}