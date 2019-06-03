namespace ChatbotDialogGenerator.Tests.Steps
{
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class YesNoStepTests
    {
        private StepBuilder stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void While_Generating_Invoke_Statement_Resources_Should_Be_Created_And_Used()
        {
            // arrange
            const string question = "Do you need any help?";

            var step = this.stepBuilder.YesNoStep(question, this.stepBuilder.DoneStep("test"), this.stepBuilder.DoneStep("test"));
            step.Name = "TestDialog";
            step.Id = 10;

            // act
            var statement = step.GenerateInvokeStepStatement()
                .NormalizeWhitespace().ToFullString();

            //assert
            var resources = step.Resources;

            var expectedQuestionResource = new StringResource("Step10TestDialogQuestion", question);

            resources.Should().HaveCount(1);
            resources.Should().ContainEquivalentOf(expectedQuestionResource);

            var expectedStatement =
                $"this.promptInvoker.Confirm(context, ResourcesAccessor.{expectedQuestionResource.Name}, this.Step10TestDialog);";

            statement.Should().Be(expectedStatement);
        }
    }
}