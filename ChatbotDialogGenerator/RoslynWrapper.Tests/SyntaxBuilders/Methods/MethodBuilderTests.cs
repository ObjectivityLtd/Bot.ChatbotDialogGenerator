namespace RoslynWrapper.Tests.SyntaxBuilders.Methods
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Common.Attributes;
    using RoslynWrapper.SyntaxBuilders.Common.Parameters;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.Tests.Utils;
    using Xunit;

    public class MethodBuilderTests
    {
        [Fact]
        public void Can_Create_Method_Declaration()
        {
            // arrange
            const string returnType = "int";
            const string methodName = "TestMethod";

            // act
            var methodDeclaration = new MethodBuilder(returnType, methodName)
                .Build();

            //assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyMethodName(methodDeclaration.Identifier, methodName);
        }

        [Fact]
        public void Can_Create_Method_With_Parameters()
        {
            // arrange
            var parameters = new[]
            {
                new Parameter("int", "param1"),
                new Parameter("TestType", "param2"),
            };

            // act
            var methodDeclaration = new MethodBuilder("int", "TestMethod")
                .WithParameters(parameters)
                .Build();

            //assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyMethodParameters(methodDeclaration.ParameterList, parameters);
        }

        [Theory]
        [InlineData(AccessModifier.Private)]
        [InlineData(AccessModifier.Public)]
        [InlineData(AccessModifier.Protected)]
        [InlineData(AccessModifier.Internal)]
        public void Can_Create_Method_With_Access_Modifier(AccessModifier accessModifier)
        {
            // arrange, act
            var methodDeclaration = new MethodBuilder("int", "TestMethod")
                .WithAccessModifier(accessModifier)
                .Build();

            //assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyMethodAccessModifier(methodDeclaration.Modifiers, accessModifier);
        }

        [Fact]
        public void Can_Create_Async_Method()
        {
            // arrange, act
            var methodDeclaration = new MethodBuilder("int", "TestMethod")
                .WithAccessModifier(AccessModifier.Public)
                .MakeAsync()
                .Build();

            //assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyThatMethodIsAsync(methodDeclaration);
        }

        [Fact]
        public void Can_Create_Method_With_Statements()
        {
            // arrange
            const string variableDeclarationStatement = "var x = 1;";
            var variableDeclarationStatementSyntax = variableDeclarationStatement.ConvertToLocalDeclarationStatementSyntax();

            // act
            var methodDeclaration = new MethodBuilder("int", "TestMethod")
                .AddStatements(variableDeclarationStatementSyntax)
                .Build();

            //assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyMethodStatements(methodDeclaration.Body.Statements, new[] { variableDeclarationStatement });
        }

        [Fact]
        public void Can_Create_Method_With_Attributes()
        {
            // arrange
            var attributeName = "Fact";
            var attribute = new AttributeBuilder().WithName(attributeName).Build();


            // act
            var methodDeclaration = new MethodBuilder("void", "TestMethod")
                .WithAttribute(attributeName).Build();

            // assert
            var validator = new MethodBuilderVerifier();
            validator.VerifyMethodAttributes(methodDeclaration.AttributeLists.SingleOrDefault(), attribute);
        }
    }

    public class MethodBuilderVerifier
    {
        public void VerifyMethodName(SyntaxToken identifier, string expectedMethodName)
        {
            identifier.ValueText.Should().Be(expectedMethodName);
        }

        public void VerifyMethodParameters(ParameterListSyntax parameters, Parameter[] expectedParameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (expectedParameters == null)
            {
                throw new ArgumentNullException(nameof(expectedParameters));
            }

            parameters.Parameters.Should().HaveCount(expectedParameters.Length);

            var parametersNormalized = parameters.Parameters.Select(param => param.NormalizeWhitespace().ToFullString());
            var expectedParametersNormalized = expectedParameters.Select(param => $"{param.TypeName} {param.ParameterName}");

            parametersNormalized.Should().BeEquivalentTo(expectedParametersNormalized);
        }

        public void VerifyMethodAccessModifier(SyntaxTokenList modifiers, AccessModifier expectedAccessModifier)
        {
            var modifiersNormalized = modifiers.Select(modifier => modifier.NormalizeWhitespace().ToFullString().ToLower());
            modifiersNormalized.Should().Contain(expectedAccessModifier.ToString().ToLower());
        }

        public void VerifyThatMethodIsAsync(MethodDeclarationSyntax method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var modifiersNormalized = method.Modifiers.Select(modifier => modifier.NormalizeWhitespace().ToFullString().ToLower());
            modifiersNormalized.Should().Contain("async");
        }

        public void VerifyMethodStatements(SyntaxList<StatementSyntax> statements, string[] expectedStatements)
        {
            if (expectedStatements == null)
            {
                throw new ArgumentNullException(nameof(expectedStatements));
            }

            statements.Should().HaveCount(expectedStatements.Length);

            var normalizedStatements = statements.Select(statement => statement.NormalizeWhitespace().ToFullString());
            normalizedStatements.Should().BeEquivalentTo(expectedStatements);
        }

        public void VerifyMethodAttributes(AttributeListSyntax methodDeclarationAttribute, AttributeSyntax attributeSyntax)
        {
            if (methodDeclarationAttribute == null)
            {
                throw new ArgumentNullException(nameof(methodDeclarationAttribute));
            }

            if (attributeSyntax == null)
            {
                throw new ArgumentNullException(nameof(attributeSyntax));
            }

            methodDeclarationAttribute.Attributes.Count.Should().Be(1);

            attributeSyntax.Name.ToFullString().Should()
                .Be(methodDeclarationAttribute.Attributes.First().Name.ToFullString());
        }
    }
}