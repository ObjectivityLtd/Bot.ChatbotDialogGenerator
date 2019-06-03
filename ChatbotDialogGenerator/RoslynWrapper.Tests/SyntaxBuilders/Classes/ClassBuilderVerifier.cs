namespace RoslynWrapper.Tests.SyntaxBuilders.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Classes;
    using RoslynWrapper.SyntaxBuilders.Common;

    public class ClassBuilderVerifier
    {
        public void VerifyClassName(SyntaxToken classIdentifier, string expectedClassName)
        {
            classIdentifier.Value.Should().Be(expectedClassName);
        }

        public void VerifyClassAttribute(AttributeListSyntax classAttribute, string expectedAttributeName, string expectedAttributeArgument)
        {
            if (classAttribute == null)
            {
                throw new ArgumentNullException(nameof(classAttribute));
            }

            classAttribute.Attributes.Should().HaveCount(1);

            var attribute = classAttribute.Attributes.Single();
            attribute.Name.ToFullString().Should().Be(expectedAttributeName);

            attribute.ArgumentList.Arguments.Should().HaveCount(1);
            var argument = attribute.ArgumentList.Arguments.Single().ToFullString();
            argument.Should().Contain(expectedAttributeArgument);
        }

        public void VerifyAccessModifier(SyntaxTokenList classModifiers, AccessModifier expectedAccessModifier)
        {
            classModifiers.Should().HaveCount(1);

            var modifier = classModifiers.Single();
            modifier.ValueText.ToLower().Should().Be(expectedAccessModifier.ToString().ToLower());
        }

        public void VerifyBaseTypes(BaseListSyntax classBaseTypes, string[] expectedBaseTypes)
        {
            if (classBaseTypes == null)
            {
                throw new ArgumentNullException(nameof(classBaseTypes));
            }
            if (expectedBaseTypes == null)
            {
                throw new ArgumentNullException(nameof(expectedBaseTypes));
            }

            classBaseTypes.Types.Should().HaveCount(expectedBaseTypes.Length);

            var stringBaseTypes = classBaseTypes.Types.ToList().Select(baseType => baseType.Type.ToFullString()).ToArray();
            stringBaseTypes.Should().BeEquivalentTo(expectedBaseTypes);
        }

        public void VerifyPrivateFields(List<FieldDeclarationSyntax> privateFields, string[] expectedFields)
        {
            if (privateFields == null)
            {
                throw new ArgumentNullException(nameof(privateFields));
            }
            if (expectedFields == null)
            {
                throw new ArgumentNullException(nameof(expectedFields));
            }

            privateFields.Count.Should().Be(expectedFields.Length);

            var privateFieldsToString = privateFields.Select(field => field.Declaration.Variables.First().Identifier.ValueText).ToArray();
            privateFieldsToString.Should().BeEquivalentTo(expectedFields);
        }

        public void VerifyDependencyInjection(List<ConstructorDeclarationSyntax> constructors, ClassDependency[] expectedDependencies)
        {
            if (constructors == null)
            {
                throw new ArgumentNullException(nameof(constructors));
            }

            constructors.Count.Should().Be(1);

            var constructor = constructors.First();
            VerifyThatDependenciesWereInjected(constructor.ParameterList.Parameters, expectedDependencies);

            var statementsToString = constructor.Body.Statements.Select(statement => statement.NormalizeWhitespace().ToFullString()).ToArray();
            VerifyThatDependenciesWereInitialized(statementsToString, expectedDependencies);
        }

        private void VerifyThatDependenciesWereInjected(SeparatedSyntaxList<ParameterSyntax> parameters, ClassDependency[] expectedDependencies)
        {
            parameters.Should().HaveCount(expectedDependencies.Length);

            var dependencies = parameters
                .Select(param => new ClassDependency(param.Type.ToFullString(), param.Identifier.ValueText))
                .ToArray();

            dependencies.Should().BeEquivalentTo(expectedDependencies);
        }

        private void VerifyThatDependenciesWereInitialized(string[] assignmentStatements, ClassDependency[] dependencies)
        {
            foreach (var dependency in dependencies)
            {
                assignmentStatements.Should().Contain($"this.{dependency.FieldName} = {dependency.FieldName};"); 
            }
        }
    }
}