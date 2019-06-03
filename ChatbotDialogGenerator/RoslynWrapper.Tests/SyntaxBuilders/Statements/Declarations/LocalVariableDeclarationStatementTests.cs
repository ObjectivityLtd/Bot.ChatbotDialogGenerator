namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.Declarations
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using RoslynWrapper.SyntaxBuilders.Statements.Declarations;
    using Xunit;

    public class LocalVariableDeclarationStatementTests
    {
        [Fact]
        public void Can_Create_Local_Variable_Declaration_Statement()
        {
            // arrange
            const string declaredVariableType = "testType";
            const string declaredVariableName = "testVariable";

            // act
            var declarationStatement = LocalVariableDeclarationStatement.Build(declaredVariableType, declaredVariableName);

            //assert
            var expectedStatement = $"{declaredVariableType} {declaredVariableName};";
            declarationStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedStatement);
        }

        [Fact]
        public void Can_Create_Local_Variable_Declaration_With_Awaited_Initial_Value_Statement()
        {
            // arrange
            const string declaredVariableType = "testType";
            const string declaredVariableName = "testVariable";
            const string initialValue = "testValue";

            // act
            var declarationStatement = LocalVariableDeclarationStatement.BuildWithAwaitedDeclaration(declaredVariableType, declaredVariableName, initialValue);

            //assert
            var expectedStatement = $"{declaredVariableType} {declaredVariableName} = await {initialValue};";
            declarationStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedStatement);
        }
    }
}