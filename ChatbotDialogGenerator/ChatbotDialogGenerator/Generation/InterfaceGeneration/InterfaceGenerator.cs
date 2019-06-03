namespace ChatbotDialogGenerator.Generation.InterfaceGeneration
{
    using System;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.CompilationUnits;
    using RoslynWrapper.SyntaxBuilders.Interfaces;
    using RoslynWrapper.SyntaxBuilders.Namespaces;

    public class InterfaceGenerator
    {
        public CompilationUnitSyntax GenerateInterface(DialogInterfaceDetails interfaceDetails)
        {
            if (interfaceDetails == null)
            {
                throw new ArgumentNullException(nameof(interfaceDetails));
            }

            var interfaceDeclaration = new InterfaceBuilder($"{interfaceDetails.InterfaceName}")
                .WithAccessModifier(AccessModifier.Public)
                .WithBaseTypes(interfaceDetails.BaseInterface)
                .Build();

            var namespaceDeclaration = new NamespaceBuilder($"{interfaceDetails.NamespaceName}")
                .WithUsings(interfaceDetails.Usings)
                .WithInterfaces(interfaceDeclaration)
                .Build();

            var compilationUnit = new CompilationUnitBuilder()
                .WithMembers(namespaceDeclaration)
                .Build();

            return compilationUnit;
        }
    }
}