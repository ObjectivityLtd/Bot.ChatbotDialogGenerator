namespace ChatbotDialogGenerator.UnitTestsGeneration
{
    using ChatbotDialogGenerator.Steps;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public interface IUnitTestMethodGenerator
    {
        MethodDeclarationSyntax GenerateMethod(EndStep step);
    }
}