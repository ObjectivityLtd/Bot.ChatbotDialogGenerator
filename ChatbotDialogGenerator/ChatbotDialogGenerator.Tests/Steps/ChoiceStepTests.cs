namespace ChatbotDialogGenerator.Tests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class ChoiceStepResourcesTests
    {
        private StepBuilder stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void While_Generating_Invoke_Statement_Resources_Should_Be_Created_And_Used()
        {
            // arrange
            const string choiceDescription1 = "Do nothing man";
            var step1 = this.stepBuilder.DoneStep("Nothing");

            const string choiceDescription2 = "Well dunno man";
            var step2 = this.stepBuilder.DoneStep("Dunno");

            var choices = new Dictionary<ChoiceDescription, Step>
            {
                {new ChoiceDescription(choiceDescription1), step1},
                {new ChoiceDescription(choiceDescription2), step2}
            };

            const string question = "What do you want to do?";
            var step = this.stepBuilder.ChoiceStep(question, choices);
            step.Name = "TestDialog";
            step.Id = 10;

            // act
            var statement = step.GenerateInvokeStepStatement()
                .NormalizeWhitespace().ToFullString();

            //assert
            var resources = step.Resources;

            var expectedQuestionResource = new StringResource("Step10TestDialogQuestion", question);
            var expectedChoicesResource = new StringResource("Step10TestDialogChoices", $"{choiceDescription1}, {choiceDescription2}");

            resources.Should().HaveCount(2);
            resources.Should().ContainEquivalentOf(expectedQuestionResource);
            resources.Should().ContainEquivalentOf(expectedChoicesResource);

            var expectedStatement =
                $"this.promptInvoker.Choice(context, ResourcesAccessor.{expectedQuestionResource.Name}, ResourcesAccessor.{expectedChoicesResource.Name}, this.Step10TestDialog);";

            statement.Should().Be(expectedStatement);
        }
    }
}