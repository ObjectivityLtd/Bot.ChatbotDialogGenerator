namespace ChatbotDialogGenerator.Tests
{
    using System;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.UnitTestsGeneration;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class UnitTestMethodGeneratorTests
    {
        private StepBuilder stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void When_EndMethodWithParentIsPassed_Then_GenerateMethodForTests()
        {
            // arrange
            var step = this.stepBuilder.YesNoStep("test question?", this.stepBuilder.DoneStep("action:done"), this.stepBuilder.RedirectStep("INewDialog"));
            step.YesPath.Parent = step;
            step.NoPath.Parent = step;
            step.YesPath.Name = "Yes";
            step.Name = "System1";
            var unit = new UnitTestMethodGenerator(new TestsConfiguration());
            // act

            var actual = unit.GenerateMethod((EndStep) step.YesPath).NormalizeWhitespace().ToFullString();

            // assert
            var expected = @"[Fact]
public async Task System1_Yes()
{
    var storyBuilder = new StoryRecorder();
    var bot = storyBuilder.Bot;
    var user = storyBuilder.User;
    bot.Says(""test question?"");
    user.Says(""Yes"");
    var story = storyBuilder.DialogDoneWithResult<DialogResultObject>(result => result.DialogResultType == DialogResultType.AskForAnotherQuestion);
    await this.Play(story);
}";

            string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        }
    }
}