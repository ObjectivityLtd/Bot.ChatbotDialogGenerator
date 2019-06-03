namespace ChatbotDialogGenerator.Generation.ResourcesGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using ChatbotDialogGenerator.Steps;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.CompilationUnits;
    using RoslynWrapper.SyntaxBuilders.Namespaces;
    using RoslynWrapper.SyntaxBuilders.Statements.Declarations;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;

    public class ResourceGenerator
    {
        public void GenerateResxFile(List<StringResource> resources, string filePath)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }

            using (var resx = new ResXResourceWriter(filePath))
            {
                foreach (var resource in resources)
                    resx.AddResource(resource.Name, resource.Value?.Replace("\\\"", "\""));
            }
        }

        public CompilationUnitSyntax GenerateResourcesAccessor(List<StringResource> resources, ResourcesDetails resourcesDetails)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (resourcesDetails == null)
            {
                throw new ArgumentNullException(nameof(resourcesDetails));
            }

            const string resourceManagerClass = "ResourceManager";
            const string resourceManagerInstance = "ResourceManager";
            const string culture = "CultureInfo.InvariantCulture";

            var classDeclaration = new ClassBuilder(resourcesDetails.ClassName)
                .WithAccessModifier(AccessModifier.Public)
                .SetStatic()
                .WithFields(
                    FieldDeclarationSyntaxExtensions.Build(fieldType: resourceManagerClass,
                            fieldName: resourceManagerInstance,
                            rightValue: MethodCallStatement.BuildCallWithoutLastTrivia($"new {resourceManagerClass}",
                                    $"\"{resourcesDetails.ResourcesBaseName}\"",
                                    $"typeof({resourcesDetails.ClassName}).Assembly")
                                .NormalizeWhitespace()
                                .ToFullString())
                        .WithAccessModifier(AccessModifier.Private)
                        .SetStatic()
                        .SetReadonly()
                );

            foreach (var resource in resources)
            {
                classDeclaration.AddFields(
                    FieldDeclarationSyntaxExtensions.Build(fieldType: "string",
                            fieldName: $"{resource.Name}",
                            rightValue: MethodCallStatement.BuildCallWithoutLastTrivia($"{resourceManagerInstance}.GetString",
                                    $"\"{resource.Name}\"", culture)
                                .NormalizeWhitespace()
                                .ToFullString())
                        .WithAccessModifier(AccessModifier.Public)
                        .SetStatic()
                        .SetReadonly()
                );
            }

            var namespaceDeclaration = new NamespaceBuilder(resourcesDetails.NamespaceName)
                .WithUsings(resourcesDetails.Usings)
                .WithClasses(classDeclaration.Build())
                .Build();

            var compilationUnit = new CompilationUnitBuilder()
                .WithMembers(namespaceDeclaration)
                .Build();

            return compilationUnit;
        }
    }
}