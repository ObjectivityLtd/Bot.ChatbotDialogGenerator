namespace RoslynWrapper.Tests.SyntaxBuilders.Statements.MethodCalls
{
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;
    using Xunit;

    public class MethodCallStatementTest
    {
        [Fact]
        public void Can_Create_MethodCallStatement()
        {
            // arrange
            const string methodName = "TestMethod";
            var arguments = new[]
            {
                "context",
                "Resources.ManagedSteps",
                "this.AfterManagedStepsChoice"
            };

            // act
            var methodCallStatement = MethodCallStatement.BuildCall(methodName, arguments);

            //assert
            var expectedCall = $"{methodName}({string.Join(", ", arguments)});";
            methodCallStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedCall);
        }

        [Fact]
        public void Can_Create_Awaited_MethodCallStatement()
        {
            // arrange
            const string methodName = "TestMethod";
            var arguments = new[]
            {
                "context",
                "Resources.ManagedSteps",
                "this.AfterManagedStepsChoice"
            };

            // act
            var methodCallStatement = MethodCallStatement.BuildAwaitedCall(methodName, arguments);

            //assert
            var expectedCall = $"await {methodName}({string.Join(", ", arguments)});";
            methodCallStatement.NormalizeWhitespace().ToFullString().Should().Be(expectedCall);
        }
    }
}