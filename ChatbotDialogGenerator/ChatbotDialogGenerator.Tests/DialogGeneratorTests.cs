namespace ChatbotDialogGenerator.Tests
{
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Generation.DialogGeneration;
    using ChatbotDialogGenerator.Steps;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Xunit;
    using ChatbotDialogGenerator.Configuration;

    public class DialogGeneratorTests
    {
        [Fact]
        public void Generated_Dialog_Should_Be_As_Expected()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            var step5 = stepBuilder.RedirectStep("IDonNeedHelpEnd");
            step5.Id = 5;
            step5.Name = "DontNeedHelp";

            var step4 = stepBuilder.DoneStep("IFemaleEnd");
            step4.Id = 4;
            step4.Name = "Female";
            step4.AdditionalMessages = new List<string>
            {
                "Female additional message"
            };

            var step3 = stepBuilder.DoneStep("IMaleEnd");
            step3.Id = 3;
            step3.Name = "Male";
            step3.AdditionalMessages = new List<string>
            {
                "MaleAdditionalMessage"
            };

            var step2 = stepBuilder.ChoiceStep(
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

            var step1 = stepBuilder.YesNoStep(
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

            var steps = new List<Step>
            {
                step1, step2, step3, step4, step5
            };

            var dialogDetails = new DialogDetails
            {
                DialogName = "TestDialog",
                NamespaceName = "TestNamespace",
                Usings = new[]
                {
                    "Core.Dialogs",
                    "Core.Extensions",
                    "Core.Invokers"
                }
            };

            // act
            var generatedDialog = new DialogGenerator()
                .GenerateDialog(steps, dialogDetails)
                .GeneratedCode
                .NormalizeWhitespace()
                .ToFullString();

            //assert
            var expectedDialog = Resources.ExpectedDialogWithDependencies;

            generatedDialog.Should().Be(expectedDialog);
        }

        [Fact]
        public void Should_Not_Inject_Dependencies_When_There_Is_Only_End_Step_To_Generate()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            var step = stepBuilder.DoneStep("TestStep");
            step.Id = 3;
            step.Name = "Test";
            step.AdditionalMessages = new List<string>
            {
                "Some sample message"
            };

            var steps = new List<Step> { step };

            var dialogDetails = new DialogDetails
            {
                DialogName = "TestDialog",
                NamespaceName = "TestNamespace",
                Usings = new[]
                {
                    "Core.Dialogs",
                    "Core.Extensions",
                    "Core.Invokers"
                }
            };

            // act
            var generatedDialog = new DialogGenerator()
                .GenerateDialog(steps, dialogDetails)
                .GeneratedCode
                .NormalizeWhitespace()
                .ToFullString();

            //assert
            var expectedDialog = Resources.ExpectedDialogWithoutDependencies;

            generatedDialog.Should().Be(expectedDialog);
        }
    }
}