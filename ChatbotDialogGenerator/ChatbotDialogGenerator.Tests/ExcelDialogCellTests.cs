namespace ChatbotDialogGenerator.Tests
{
    using System;
    using System.Collections.Generic;
    using ChatbotDialogGenerator.Steps;
    using FluentAssertions;
    using ParseExcel;
    using ParseExcel.Domain.Excel;
    using Xunit;
    using ChatbotDialogGenerator.Configuration;

    public class ExcelDialogCellTests
    {
        [Fact]
        public void Given_YesNoDialogWithWrongChildren_When_ConvertsToSteps_Then_Throws()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            ExcelDialogCell cell = new ExcelDialogCell()
            {
                Question = "",
                NodeType = ExcelStepType.YesNo,
                Cell = new ExcelValueCell()
            };

            // act
            Action act = () => new ConverterExcelCellToStep(commonConfiguration, stepBuilder).ConvertToStep(cell);

            // assert
            act.Should().Throw<InvalidOperationException>()
                .WithInnerException<InvalidOperationException>();
        }

        [Fact]
        public void Given_CorrectYesNoDialog_When_ConvertsToSteps_Then_Converts()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            var cell = YesNoExcelDialogCell();

            // act
            var actual = new ConverterExcelCellToStep(commonConfiguration, stepBuilder).ConvertToStep(cell);

            // assert
            actual.Should().BeOfType<YesNoStep>();
        }

        [Fact]
        public void Given_CorrectChoiceDialog_When_ConvertsToSteps_Then_Converts()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            var cell = ChoiceExcelDialogCell();

            // act
            var actual = new ConverterExcelCellToStep(commonConfiguration, stepBuilder).ConvertToStep(cell);

            // assert
            actual.Should().BeOfType<ChoiceStep>();
        }

        [Fact]
        public void Given_DialogContinuesFromOtherMethod_When_ConvertsToSteps_Then_RedirectNodeIsOmitted()
        {
            // arrange
            var commonConfiguration = new CommonConfiguration { UsePostAsSeparateBubble = false };
            var stepBuilder = new StepBuilder(commonConfiguration, new StaticResources());

            var dialogWithRedirection = YesNoExcelDialogCell();
            dialogWithRedirection.NodeType = ExcelStepType.Redirect;

            // act
            var actual = new ConverterExcelCellToStep(commonConfiguration, stepBuilder).ConvertToStep(dialogWithRedirection);

            // assert
            actual.Should().BeOfType<DoneStep>();
        }

        private static ExcelDialogCell YesNoExcelDialogCell()
        {
            ExcelDialogCell unit = new ExcelDialogCell()
            {
                Id = new Random().Next(),
                Question = "yes or no?",
                NodeType = ExcelStepType.YesNo,
                Cell = new ExcelValueCell()
            };

            unit.Children.Add(new ExcelDialogCell()
            {
                NodeType = ExcelStepType.End,
                Cell = new ExcelValueCell { Value = "Yes" }
            });
            unit.Children.Add(new ExcelDialogCell()
            {
                NodeType = ExcelStepType.End,
                Cell = new ExcelValueCell { Value = "No" }
            });
            return unit;
        }

        private static ExcelDialogCell ChoiceExcelDialogCell()
        {
            ExcelDialogCell unit = new ExcelDialogCell()
            {
                Id = new Random().Next(),
                Question = "how are you?",
                NodeType = ExcelStepType.Choice,
                Cell = new ExcelValueCell(),
            };
            unit.Choices.Add(1, "1");
            unit.Choices.Add(2, "2");
            unit.Children.Add(new ExcelDialogCell()
            {
                NodeType = ExcelStepType.End,
                Cell = new ExcelValueCell { Value = "1" }
            });
            unit.Children.Add(new ExcelDialogCell()
            {
                NodeType = ExcelStepType.End,
                Cell = new ExcelValueCell { Value = "2" }
            });
            return unit;
        }
    }
}