namespace ChatbotDialogGenerator.Tests
{
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.UnitTestsGeneration;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Moq;
    using RoslynWrapper.SyntaxBuilders.Common;
    using RoslynWrapper.SyntaxBuilders.Methods;
    using RoslynWrapper.SyntaxBuilders.Statements.MethodCalls;
    using Xunit;

    public class UnitTestDialogGeneratorTests
    {
        private StepBuilder stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void When_GenerateUnitTests_Then_FileLooksAsExpected()
        {
            // arrange 
            var methodGenerator = new Mock<IUnitTestMethodGenerator>();
            var testGenerator = new UnitTestForDialogGenerator(methodGenerator.Object, new CommonConfiguration(), new TestsConfiguration());
            var step = this.stepBuilder.DoneStep("action");

            methodGenerator.Setup(generator => generator.GenerateMethod(It.IsAny<EndStep>())).Returns(() =>
                new MethodBuilder("Task", "Test")
                    .WithAccessModifier(AccessModifier.Public)
                    .MakeAsync()
                    .AddStatements(MethodCallStatement.BuildAwaitedCall("this.Play", "story"))
                    .WithAttribute("Fact").Build());

            // act
            var actual = testGenerator.GenerateUnitTestsForDialog(step).NormalizeWhitespace()
                .ToFullString();

            // assert
            var expected = Resources.ExpectedDialogTest;
            actual.Should().Be(expected);
        }
    }
}