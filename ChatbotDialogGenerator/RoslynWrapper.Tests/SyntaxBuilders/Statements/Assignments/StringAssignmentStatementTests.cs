namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.Assignments
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using RoslynWrapper.SyntaxBuilders.Statements.Assignments;
    using Xunit;

    public class StringAssignmentStatementTests
    {
        [Theory]
        [InlineData("testValue")]
        [InlineData("test value")]
        [InlineData("123")]
        public void Can_Create_String_Assignment_Statement(string value)
        {
            // arrange
            const string variableName = "testVariable";

            // act
            var assignmentStatement = StringAssignmentStatement.Build(variableName, value);

            //assert
            var expectedStatement = $"{variableName} = \"{value}\";";
            assignmentStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedStatement);
        }
    }
}