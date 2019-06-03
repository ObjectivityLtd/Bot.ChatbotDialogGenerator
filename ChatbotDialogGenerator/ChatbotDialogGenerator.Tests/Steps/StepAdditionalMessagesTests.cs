namespace ChatbotDialogGenerator.Tests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ChatbotDialogGenerator.Tests.Utils;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class StepAdditionalMessagesTests
    {
        public static IEnumerable<object[]> TestCaseSource()
        {
            return typeof(Step).GetSubclasses()
                .Select(type => new object[] { type });
        }

        [Theory]
        [MemberData(nameof(TestCaseSource))]
        public void While_Generating_Squashed_AdditionalMessages_Statements_Resources_Should_Be_Created_And_Used(Type type)
        {
            // arrange
            const string message1 = "First message";
            const string message2 = "Second message";

            var step = CreateStep(type);
            step.AdditionalMessages = new List<string>
            {
                message1, message2
            };
            step.Name = "TestDialog";
            step.Id = 10;

            // act
            var statements = step.GeneratePostAdditionalMessagesStatements()
                .Select(s => s.NormalizeWhitespace().ToFullString())
                .ToArray();

            //assert
            var resources = step.Resources;

            var squashedMessageResource = new StringResource("Step10TestDialogAdditionalMessages", "First messageSecond message"); // TODO: [js] to be updated when separating implemented

            resources.Should().HaveCount(1);
            resources.Should().ContainEquivalentOf(squashedMessageResource);

            var expectedStatement =
                $"await context.PostAsSeperateBubblesAsync(ResourcesAccessor.{squashedMessageResource.Name});";

            statements.Should().HaveCount(1);
            statements[0].Should().Be(expectedStatement);
        }

        [Theory]
        [MemberData(nameof(TestCaseSource))]
        public void While_Generating_Separated_AdditionalMessages_Statements_Resources_Should_Be_Created_And_Used(Type type)
        {
            // arrange
            var commonConfiguration = new CommonConfiguration
            {
                UsePostAsSeparateBubble = false
            };

            const string message1 = "First message";
            const string message2 = "Second message";

            var step = this.CreateStep(commonConfiguration, type);
            step.AdditionalMessages = new List<string>
            {
                message1, message2
            };
            step.Name = "TestDialog";
            step.Id = 10;

            // act
            var statements = step.GeneratePostAdditionalMessagesStatements()
                .Select(s => s.NormalizeWhitespace().ToFullString())
                .ToArray();

            //assert
            var resources = step.Resources;

            var squashedMessageResource1 = new StringResource("Step10TestDialogAdditionalMessage0", "First message");
            var squashedMessageResource2 = new StringResource("Step10TestDialogAdditionalMessage1", "Second message");

            resources.Should().HaveCount(2);
            resources.Should().ContainEquivalentOf(squashedMessageResource1);
            resources.Should().ContainEquivalentOf(squashedMessageResource2);

            var expectedStatement1 = $@"await context.PostAsync(ResourcesAccessor.{squashedMessageResource1.Name});";
            var expectedStatement2 = $@"await context.PostAsync(ResourcesAccessor.{squashedMessageResource2.Name});";

            statements.Should().HaveCount(2);
            statements[0].Should().Be(expectedStatement1);
            statements[1].Should().Be(expectedStatement2);
        }

        private Step CreateStep(Type type)
        {
            var stepBuilder = new StepBuilder(new CommonConfiguration(), new StaticResources());

            if (type.IsAssignableFrom(typeof(Step)))
            {
                throw new InvalidOperationException($"Cannot create step of type {type.Name}");
            }

            if (type == typeof(ChoiceStep))
            {
                return stepBuilder.ChoiceStep("TestQuestion", new Dictionary<ChoiceDescription, Step>());
            }

            if (type == typeof(YesNoStep))
            {
                return stepBuilder.YesNoStep("TestQuestion", stepBuilder.DoneStep("TestAction"), stepBuilder.DoneStep("TestAction"));
            }

            if (type == typeof(RedirectStep))
            {
                return stepBuilder.RedirectStep("TestTarget");
            }

            if (type == typeof(DoneStep))
            {
                return stepBuilder.DoneStep("TestAction");
            }

            throw new InvalidOperationException("Unsupported step type");
        }

        private Step CreateStep(CommonConfiguration commonConfiguration, Type type)
        {
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            if (type.IsAssignableFrom(typeof(Step)))
            {
                throw new InvalidOperationException($"Cannot create step of type {type.Name}");
            }

            if (type == typeof(ChoiceStep))
            {
                return stepBuilder.ChoiceStep("TestQuestion", new Dictionary<ChoiceDescription, Step>());
            }

            if (type == typeof(YesNoStep))
            {
                return stepBuilder.YesNoStep("TestQuestion", stepBuilder.DoneStep("TestAction"), stepBuilder.DoneStep("TestAction"));
            }

            if (type == typeof(RedirectStep))
            {
                return stepBuilder.RedirectStep("TestTarget");
            }

            if (type == typeof(DoneStep))
            {
                return stepBuilder.DoneStep("TestAction");
            }

            throw new InvalidOperationException("Unsupported step type");
        }
    }
}