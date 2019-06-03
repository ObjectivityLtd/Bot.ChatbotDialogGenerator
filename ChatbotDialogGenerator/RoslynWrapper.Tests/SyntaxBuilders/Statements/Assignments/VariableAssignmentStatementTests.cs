namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.Assignments
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using RoslynWrapper.SyntaxBuilders.Statements.Assignments;
    using Xunit;

    public class VariableAssignmentStatementTests
    {
        [Fact]
        public void Can_Create_Variable_Assignment_Statement()
        {
            // arrange
            const string variableName = "testVariable";
            const string assignmentVariableName = "valueSource";

            // act
            var assignmentStatement = VariableAssignmentStatement.Build(variableName, assignmentVariableName);

            //assert
            var expectedStatement = $"{variableName} = {assignmentVariableName};";
            assignmentStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedStatement);
        }
    }
}