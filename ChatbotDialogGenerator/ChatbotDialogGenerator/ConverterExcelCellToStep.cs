namespace ChatbotDialogGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ChatbotDialogGenerator.Configuration;
    using ChatbotDialogGenerator.Steps;
    using ParseExcel;
    using ParseExcel.Configuration;
    using ParseExcel.Domain.Excel;

    public class ConverterExcelCellToStep
    {
        private const string Yes = nameof(Yes);
        private const string No = nameof(No);

        private readonly CommonConfiguration commonConfiguration;
        private readonly StepBuilder stepBuilder;
        private readonly Dictionary<string, Func<Step>> questionEndActionMap;

        public ConverterExcelCellToStep(CommonConfiguration commonConfiguration, StepBuilder stepBuilder)
        {
            this.commonConfiguration = commonConfiguration;
            this.stepBuilder = stepBuilder;

            this.questionEndActionMap = new Dictionary<string, Func<Step>>
            {
                {ExcelConfiguration.DefaultEndText, () => this.stepBuilder.DoneStep(ExcelConfiguration.DefaultEndAction) },
                {ExcelConfiguration.TicketCreationText, () => this.stepBuilder.RedirectStep(this.commonConfiguration.TicketDialog) },
                {string.Empty, () => this.stepBuilder.DoneStep(ExcelConfiguration.DefaultEndAction) }
            };
        }

        public Step ConvertToStep(ExcelDialogCell dialogCell)
        {
            if (dialogCell == null)
            {
                throw new ArgumentNullException(nameof(dialogCell));
            }

            Step step;

            switch (dialogCell.NodeType)
            {
                case ExcelStepType.YesNo:
                    step = this.TryParseYesNo(dialogCell);
                    break;
                case ExcelStepType.Choice:
                    step = this.TryParseChoice(dialogCell);
                    break;
                case ExcelStepType.Redirect:
                    var redirectedDialog = dialogCell.Children.First();
                    step = this.ConvertToStep(redirectedDialog);
                    break;
                case ExcelStepType.End:
                    step = this.TryParseEndStep(dialogCell);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported Excel step Type : {dialogCell.NodeType}");
            }

            bool isNotRedirected = step.Id == 0;
            step.Name = dialogCell.Cell.Value;
            if (isNotRedirected)
            {
                step.Id = dialogCell.Id;
            }

            if (this.commonConfiguration.UsePostAsSeparateBubble)
            {
                step.AdditionalMessages = new List<string>();
                for (int i = 0; i < dialogCell.Messages.Count; i++)
                {
                    var message = dialogCell.Messages[i].Replace("\"", "\\\"");
                    if (i != dialogCell.Messages.Count - 1)
                    {
                        message = message + Environment.NewLine + Environment.NewLine;
                    }

                    step.AdditionalMessages.Add(message);

                }
            }
            else
            {
                step.AdditionalMessages = dialogCell.Messages?.Select(s => s.Replace("\"", "\\\"")).ToList();
            }

            return step;
        }

        private Step TryParseEndStep(ExcelDialogCell dialogCell)
        {
            Step step;
            var trimmedKey = dialogCell.Question ?? string.Empty;
            if (this.questionEndActionMap.ContainsKey(trimmedKey))
            {
                step = this.questionEndActionMap[trimmedKey]();
            }
            else if (Regex.IsMatch(trimmedKey, @"\{\w+\}"))
            {
                var target = Regex.Match(trimmedKey, @"\w+").Value;
                step = this.stepBuilder.RedirectStep($"{target}.I{this.commonConfiguration.DialogClassName(target)}");
            }
            else
            {
                step = this.stepBuilder.DoneStep(ExcelConfiguration.DefaultEndAction);
            }

            return step;
        }

        private Step TryParseChoice(ExcelDialogCell dialogCell)
        {
            var choice = this.stepBuilder.ChoiceStep(dialogCell.Question, new Dictionary<ChoiceDescription, Step>());

            for (int i = 0; i < dialogCell.Choices.Count; i++)
            {
                var number = dialogCell.Choices.Keys.ElementAt(i);
                var choiceChild = this.ConvertToStep(dialogCell.Children.First(s =>
                    s.Cell.Value.Split(ExcelConfiguration.ChoiceDelimiter).ToList()
                        .Contains(number.ToString())));
                choiceChild.Parent = choice;
                choice.ChoiceSteps.Add(new ChoiceDescription(dialogCell.Choices[number]), choiceChild);
            }

            return choice;
        }

        private Step TryParseYesNo(ExcelDialogCell dialogCell)
        {
            try
            {
                var yesNoStep = this.stepBuilder.YesNoStep(
                    question: dialogCell.Question,
                    yesPath: this.ConvertToStep(
                        dialogCell.Children.Single(c => c.Cell.Value.Equals(Yes, StringComparison.OrdinalIgnoreCase))),
                    noPath: this.ConvertToStep(
                        dialogCell.Children.Single(c => c.Cell.Value.Equals(No, StringComparison.OrdinalIgnoreCase))));

                yesNoStep.YesPath.Parent = yesNoStep;
                yesNoStep.NoPath.Parent = yesNoStep;

                return yesNoStep;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    $"Invalid Children types for node {dialogCell.Id} Cell: {dialogCell.Cell.CellName}", ex);
            }
        }
    }
}
