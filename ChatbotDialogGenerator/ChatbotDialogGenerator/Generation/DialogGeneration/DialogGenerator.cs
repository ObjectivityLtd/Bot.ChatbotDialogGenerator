namespace ChatbotDialogGenerator.Generation.DialogGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.CompilationUnits;
    using RoslynWrapper.SyntaxBuilders.Namespaces;
    using Steps;

    public class DialogGenerator
    {
        private List<MethodDeclarationSyntax> _methods;

        public DialogGenerator()
        {
            this.StaticResources = new StaticResources();
        }

        public StaticResources StaticResources { get; set; }

        public GeneratedDialog GenerateDialog(List<Step> steps, DialogDetails dialogDetails)
        {
            if (steps == null || steps.Count == 0)
            {
                throw new InvalidOperationException("Steps list cannot be empty.");
            }

            this._methods = new List<MethodDeclarationSyntax>();
            var resources = new List<StringResource>();

            this._methods.Add(steps.First().GenerateStartMethod());

            foreach (var step in steps)
            {
                this._methods.Add(step.GenerateMethod());
                resources.AddRange(step.Resources);
            }

            var classDeclaration = steps.All(s => s is DoneStep) ?
                this.GenerateClassWithoutDependencies(dialogDetails) :
                this.GenerateClassWithDependencies(dialogDetails);

            var namespaceDeclaration = this.GenerateNamespace(dialogDetails, classDeclaration);
            var compilationUnit = this.GenerateCompilationUnit(namespaceDeclaration);

            return new GeneratedDialog
            {
                GeneratedCode = compilationUnit,
                Resources = resources
            };
        }

        private ClassDeclarationSyntax GenerateClassWithDependencies(DialogDetails dialogDetails)
        {
            return new ClassBuilder(className: dialogDetails.DialogName)
                .WithAccessModifier(AccessModifier.Public)
                .WithAttribute(attributeName: this.StaticResources.Export, argumentType: $"I{dialogDetails.DialogName}")
                .WithBaseTypes($"I{dialogDetails.DialogName}")
                .WithDependencies(
                    new ClassDependency(typeName: this.StaticResources.dialogInvoker.Type, fieldName: this.StaticResources.dialogInvoker.Name),
                    new ClassDependency(typeName: this.StaticResources.promptInvoker.Type, fieldName: this.StaticResources.promptInvoker.Name))
                .AddMethods(this._methods.ToArray())
                .Build();
        }

        private ClassDeclarationSyntax GenerateClassWithoutDependencies(DialogDetails dialogDetails)
        {
            return new ClassBuilder(className: dialogDetails.DialogName)
                .WithAccessModifier(AccessModifier.Public)
                .WithAttribute(attributeName: this.StaticResources.Export, argumentType: $"I{dialogDetails.DialogName}")
                .WithBaseTypes($"I{dialogDetails.DialogName}")
                .AddMethods(this._methods.ToArray())
                .Build();
        }

        private NamespaceDeclarationSyntax GenerateNamespace(DialogDetails dialogDetails, ClassDeclarationSyntax classDeclaration)
        {
            return new NamespaceBuilder($"{dialogDetails.NamespaceName}")
                .WithUsings(dialogDetails.Usings)
                .WithClasses(classDeclaration)
                .Build();
        }

        private CompilationUnitSyntax GenerateCompilationUnit(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            return new CompilationUnitBuilder()
                .WithMembers(namespaceDeclaration)
                .Build();
        }
    }
}