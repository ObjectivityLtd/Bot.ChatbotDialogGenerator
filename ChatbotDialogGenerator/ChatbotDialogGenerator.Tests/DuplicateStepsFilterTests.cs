namespace ChatbotDialogGenerator.Tests
{
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.StepTreeHelpers;
    using FluentAssertions;
    using Xunit;

    public class DuplicateStepsFilterTests
    {
        private StepBuilder stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void Should_Filter_Out_Duplicated_Steps_Ids_And_Order_By_Step_Id()
        {
            // arrange
            var step1 = this.stepBuilder.DoneStep("Test");
            step1.Id = 1;

            var step2 = this.stepBuilder.DoneStep("Test");
            step2.Id = 1;

            var step3 = this.stepBuilder.DoneStep("Test");
            step3.Id = 2;

            var step4 = this.stepBuilder.DoneStep("Test");
            step4.Id = 3;

            var step5 = this.stepBuilder.DoneStep("Test");
            step5.Id = 4;

            var step6 = this.stepBuilder.DoneStep("Test");
            step6.Id = 4;

            var steps = new List<Step>
            {
                step1,
                step2,
                step3,
                step4,
                step5,
                step6
            };

            // act
            var listOfDistinctSteps = new DuplicateStepsFilter().RemoveDuplicatesAndOrderByStepId(steps);

            // assert
            var expectedList = new List<Step>
            {
                step1,
                step3,
                step4,
                step5
            };

            listOfDistinctSteps.Should().BeEquivalentTo(expectedList, opt => opt.WithStrictOrdering());
        }
    }
}