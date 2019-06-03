namespace ParseExcel.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;
    using ParseExcel.Domain.Excel;
    using ParseExcel.Luis;
    using Xunit;

    public class ExcelParserTestsBasedOnInput
    {
        [Fact]
        public void Given_WrongInput_When_ParseExcel_Throws()
        {
            // arrange
            var utteranceParser = new Mock<IUtteranceFinder>();
            var nodeTypeChecker = new Mock<INodeTypeChecker>();
            var unit = new DialogExcelParser(utteranceParser.Object, nodeTypeChecker.Object);
            utteranceParser
                .Setup(s => s.FindUtterancesForDialog(It.IsAny<ExcelDialogCell>(), It.IsAny<ExcelWorksheet>()))
                .Returns(() => new List<string> { "test" });

            // act
            Action act = () => unit.ParseInputFromFile(@"test.xslx");

            // assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Given_SampleInput_When_ParseExcel_FindsAllDialogs()
        {
            // arrange
            var utteranceParser = new Mock<IUtteranceFinder>();
            var nodeTypeChecker = new NodeTypeCheckerBasedOnNextCell();
            var unit = new DialogExcelParser(utteranceParser.Object, nodeTypeChecker);
            utteranceParser
                .Setup(s => s.FindUtterancesForDialog(It.IsAny<ExcelDialogCell>(), It.IsAny<ExcelWorksheet>()))
                .Returns(() => new List<string> {"test"});

            // act
            var actual = unit.ParseInputFromFile(@"sample.xlsx");

            // assert
            actual.Count.Should().Be(4);
        }

        [Fact]
        public void Given_SampleInput_When_ParseExcel_ParsesDialogCorrectly()
        {
            // arrange
            var utteranceParser = new Mock<IUtteranceFinder>();
            var nodeTypeChecker = new NodeTypeCheckerBasedOnNextCell();
            var unit = new DialogExcelParser(utteranceParser.Object, nodeTypeChecker);
            utteranceParser
                .Setup(s => s.FindUtterancesForDialog(It.IsAny<ExcelDialogCell>(), It.IsAny<ExcelWorksheet>()))
                .Returns(() => new List<string> { "test" });

            // act
            var actual = unit.ParseInputFromFile(@"sample.xlsx");
            var testedDialog = actual.Single(s => s.Cell.Value.Trim().Equals("Test 2"));

            // assert
            testedDialog.Question.Should().Be("Could you please confirm if you are a:");
            testedDialog.Choices.Count.Should().Be(2);
            var yesnoChild = testedDialog.Children.Single(s => s.Cell.Value == "2");
            yesnoChild.Question.Should().Be("Did it help you?");

            yesnoChild.Children.First().NodeType.Should().Be(ExcelStepType.End);
            yesnoChild.Children.First().Messages.Count.Should().Be(1);
            yesnoChild.Children.First().Question.Should().Be("Can I help you with anything else?");

            yesnoChild.Children.Last().NodeType.Should().Be(ExcelStepType.End);
            yesnoChild.Children.Last().Messages.Count.Should().Be(0);
            yesnoChild.Children.Last().Question.Should().Be("[ticket creation]");
        }

        [Fact]
        public void Given_SampleInput_When_ParseExcelWithContinueDialogs_IdsAreSame()
        {
            // arrange
            var utteranceParser = new Mock<IUtteranceFinder>();
            var nodeTypeChecker = new NodeTypeCheckerBasedOnNextCell();
            var unit = new DialogExcelParser(utteranceParser.Object, nodeTypeChecker);
            utteranceParser
                .Setup(s => s.FindUtterancesForDialog(It.IsAny<ExcelDialogCell>(), It.IsAny<ExcelWorksheet>()))
                .Returns(() => new List<string> { "test" });

            // act
            var actual = unit.ParseInputFromFile(@"sample.xlsx");

            // assert
            var reusedCellId = actual.First().Children.First().Id; // Test 1 D1
            var testedPathDialog =
                actual.First().Children.Last().Children.First().Children.First().Children.First().Children.First().Id; // Test 1 K5

            reusedCellId.Should().Be(testedPathDialog);
        }
    }
}