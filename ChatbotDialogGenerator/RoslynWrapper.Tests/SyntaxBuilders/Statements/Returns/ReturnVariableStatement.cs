namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.Returns
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using RoslynWrapper.SyntaxBuilders.Statements.Returns;
    using Xunit;

    public class ReturnVariableStatementTests
    {
        [Fact]
        public void Can_Create_Return_Statement()
        {
            // arrange
            const string variableName = "TestVariable";

            // act
            var returnStatement = ReturnVariableStatement.Build(variableName);

            //assert
            var expectedVariable = $"return {variableName};";
            returnStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedVariable);
        }
    }
}