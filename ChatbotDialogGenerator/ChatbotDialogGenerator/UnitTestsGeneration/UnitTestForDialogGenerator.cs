namespace ChatbotDialogGenerator.UnitTestsGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.StepTreeHelpers;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.CompilationUnits;
    using RoslynWrapper.SyntaxBuilders.Namespaces;

    public class UnitTestForDialogGenerator : IUnitTestForDialogGenerator
    {
        private readonly IUnitTestMethodGenerator testMethodGenerator;
        private readonly CommonConfiguration commonConfiguration;
        private readonly TestsConfiguration testsConfiguration;
        private readonly StepsTreeToListConverter stepsTreeToListConverter = new StepsTreeToListConverter();
        private readonly DuplicateStepsFilter duplicateStepsFilter = new DuplicateStepsFilter();

        public UnitTestForDialogGenerator(IUnitTestMethodGenerator testMethodGenerator, CommonConfiguration commonConfiguration, TestsConfiguration testsConfiguration)
        {
            this.testMethodGenerator = testMethodGenerator;
            this.commonConfiguration = commonConfiguration;
            this.testsConfiguration = testsConfiguration;
        }

        public CompilationUnitSyntax GenerateUnitTestsForDialog(Step step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            var stepsList = this.stepsTreeToListConverter.ConvertToList(step);
            var stepsQueued = this.duplicateStepsFilter.RemoveDuplicatesAndOrderByStepId(stepsList);
            var ends = stepsQueued.OfType<EndStep>();

            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            foreach (var end in ends)
            {
                methods.Add(this.testMethodGenerator.GenerateMethod(end));
            }

            var classDeclaration = new ClassBuilder(className: this.testsConfiguration.UnitTestClassName(step.Name))
                .WithAccessModifier(AccessModifier.Public)
                .WithBaseTypes($"{this.testsConfiguration.UnitTestBaseClass}<{this.commonConfiguration.DialogClassName(step.Name)}>")
                .AddMethods(methods.ToArray())
                .Build();

            var namespaceDeclaration = new NamespaceBuilder(namespaceName: this.testsConfiguration.UnitTestNamespace)
                .WithUsings(this.testsConfiguration.UnitTestsUsings)
                .WithClasses(classDeclaration)
                .Build();

            return new CompilationUnitBuilder()
                    .WithMembers(namespaceDeclaration)
                    .Build();
        }
    }
}