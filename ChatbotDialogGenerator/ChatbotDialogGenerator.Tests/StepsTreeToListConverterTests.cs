namespace ChatbotDialogGenerator.Tests
{
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using FluentAssertions;
    using Xunit;

    public class StepsTreeToListConverterTests
    {
        private StepBuilder StepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

        [Fact]
        public void Can_Convert_Tree_To_List()
        {
            // arrange
            var step5 = this.StepBuilder.RedirectStep("IDonNeedHelpEnd");
            step5.Id = 5;
            step5.Name = "DontNeedHelp";

            var step4 = this.StepBuilder.DoneStep("IFemaleEnd");
            step4.Id = 4;
            step4.Name = "Female";
            step4.AdditionalMessages = new List<string>
            {
                "Female additional message"
            };

            var step3 = this.StepBuilder.DoneStep("IMaleEnd");
            step3.Id = 3;
            step3.Name = "Male";
            step3.AdditionalMessages = new List<string>
            {
                "MaleAdditionalMessage"
            };

            var step2 = this.StepBuilder.ChoiceStep(
                question: "Are you male?",
                choiceSteps: new Dictionary<ChoiceDescription, Step>
                {
                    {
                        new ChoiceDescription("Male"), step3
                    },
                    {
                        new ChoiceDescription("Female"), step4
                    }
                });
            step2.Id = 2;
            step2.Name = "AreYouMale";

            var step1 = this.StepBuilder.YesNoStep(
                question: "Do you need help?",
                yesPath: step2,
                noPath: step5);

            step1.Id = 1;
            step1.Name = "DoYouNeedHelp";
            step1.AdditionalMessages = new List<string>
            {
                "First test message",
                "Second test message"
            };

            // act
            var flattenedTree = new StepTreeHelpers.StepsTreeToListConverter().ConvertToList(step1);

            //assert
            var expectedList = new List<Step>
            {
                step1,
                step2,
                step3,
                step4,
                step5
            };

            flattenedTree.Should().BeEquivalentTo(expectedList, opt => opt.WithStrictOrdering());
        }
    }
}