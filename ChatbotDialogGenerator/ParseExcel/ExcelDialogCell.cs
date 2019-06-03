using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseExcel
{
    public class ExcelDialogCell
    {
        public int Id { get; set; }

        public Dictionary<int, string> Choices { get; set; }

        public string Title { get; set; }

        public IList<string> Utterances { get; set; }

        public ExcelStepType NodeType { get; set; }

        public string Question { get; set; }

        public List<string> Messages { get; set; }

        public List<ExcelDialogCell> Children
        {
            get => this.childeren ?? (this.childeren = new List<ExcelDialogCell>());
            set => this.childeren = value;
        }

        private List<ExcelDialogCell> childeren;

        public ExcelValueCell Cell { get; set; }

        public static Step ConvertToStep(ExcelDialogCell dialogCell)
        {
            Step step;

            switch (dialogCell.NodeType)
            {
                case ExcelStepType.YesNo:
                    var yesNoStep = new YesNoStep
                    {
                        Question = dialogCell.Question
                    };
                    step = yesNoStep;
                    try
                    {
                        yesNoStep.YesPath = ConvertToStep(dialogCell.Children.First(c =>
                                        c.Cell.Value.Trim().Equals(ExcelParserConfig.Yes, StringComparison.InvariantCultureIgnoreCase)));
                        yesNoStep.NoPath = ConvertToStep(dialogCell.Children.First(c =>
                            c.Cell.Value.Trim().Equals(ExcelParserConfig.No, StringComparison.InvariantCultureIgnoreCase)));
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new InvalidOperationException(
                            $"Invalid Children types for node {dialogCell.Id} Cell: {dialogCell.Cell.CellName}", ex);
                    }
                    break;
                case ExcelStepType.Choice:
                    var choice = new ChoiceStep
                    {
                        ChoiceSteps = new Dictionary<ChoiceDescription, Step>(),
                        Question = dialogCell.Question
                    };
                    for (int i = 1; i <= dialogCell.Choices.Count; i++)
                    {
                        var number = dialogCell.Choices.Keys.ElementAt(i - 1);
                        choice.ChoiceSteps.Add(new ChoiceDescription()
                        {
                            Description = dialogCell.Choices[number],
                            Number = number
                        },
                            ConvertToStep(dialogCell.Children.First(s =>
                                s.Cell.Value.Split(ExcelParserConfig.ChoiceSeperator).ToList()
                                    .Contains(number.ToString()))));
                    }
                    step = choice;
                    break;
                case ExcelStepType.Redicret:
                    step = ConvertToStep(dialogCell.Children.First());
                    break;
                default:
                    step = new EndStep();
                    break;
            }

            step.Id = dialogCell.Id;
            step.AdditionalMessages = dialogCell.Messages;
            return step;
        }
    }
}