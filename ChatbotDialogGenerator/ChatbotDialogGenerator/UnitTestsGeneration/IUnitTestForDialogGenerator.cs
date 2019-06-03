namespace ChatbotDialogGenerator.UnitTestsGeneration
{
    using ChatbotDialogGenerator.Steps;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public interface IUnitTestForDialogGenerator
    {
        CompilationUnitSyntax GenerateUnitTestsForDialog(Step step);
    }
}