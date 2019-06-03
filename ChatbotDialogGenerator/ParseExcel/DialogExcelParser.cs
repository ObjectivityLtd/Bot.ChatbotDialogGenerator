namespace ParseExcel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Configuration;
    using Domain.Excel;
    using Luis;
    using OfficeOpenXml;
    using Utils;

    public class DialogExcelParser : IInputParser
    {
        private readonly IUtteranceFinder utteranceFinder;
        private readonly INodeTypeChecker nodeTypeChecker;

        private int counter;

        public DialogExcelParser(IUtteranceFinder utteranceFinder, INodeTypeChecker nodeTypeChecker)
        {
            this.utteranceFinder = utteranceFinder;
            this.nodeTypeChecker = nodeTypeChecker;
        }

        public List<ExcelDialogCell> ParseInputFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("Wrong path to excel file");
            }

            List<ExcelDialogCell> steps = new List<ExcelDialogCell>();
            var allDialogs = new List<ExcelDialogCell>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                this.counter = 1;
                foreach (var workbookWorksheet in package.Workbook.Worksheets)
                {
                    var excelDialogs = FindDialogs(workbookWorksheet, ExcelConfiguration.ColumnWithDialogs);
                    foreach (var excelDialog in excelDialogs)
                    {
                        allDialogs.Add(excelDialog);
                        excelDialog.Utterances.AddRange(
                            this.utteranceFinder.FindUtterancesForDialog(excelDialog, workbookWorksheet));
                        this.ParseChildNodes(excelDialog, ExcelConfiguration.FlowStartColumn, workbookWorksheet, allDialogs);
                        steps.Add(excelDialog);
                    }
                }
            }

            return steps;
        }

        private static List<ExcelDialogCell> FindDialogs(ExcelWorksheet worksheet, int dialogColumnNumber)
        {
            List<ExcelDialogCell> excelDialogs = new List<ExcelDialogCell>();

            var dimensionStart = worksheet.Dimension.Start;
            var dimensionEnd = worksheet.Dimension.End;
            for (int row = dimensionStart.Row; row <= dimensionEnd.Row; row++)
            {
                var cell = worksheet.Cells[row, dialogColumnNumber];
                var cellRange = ExcelUtils.GetCellRange(cell, worksheet);
                excelDialogs.Add(new ExcelDialogCell()
                {
                    Cell = cellRange,
                    Title = cellRange.Value,
                });
                row = cellRange.EndLine;
            }

            return excelDialogs;
        }

        private void ParseChildNodes(ExcelDialogCell node, int currentColumn, ExcelWorksheet worksheet, List<ExcelDialogCell> allDialogs)
        {
            var nextColumn = currentColumn + 1;
            var cell = worksheet.Cells[node.Cell.StartLine, currentColumn];

            node.NodeType = this.nodeTypeChecker.ReturnTypeForCell(worksheet, cell);
            node.TextLines.AddRange(cell.Value.ToString().Split(Environment.NewLine.ToCharArray())
                .Where(s => !string.IsNullOrEmpty(s)).Reverse());

            FillQuestionsAndTexts(node);

            var childDialogs = new List<ExcelDialogCell>();

            if (node.NodeType != ExcelStepType.End)
            {
                ParseNonEndNode(node, worksheet, nextColumn, childDialogs);
            }
            else
            {
                var shouldRedirect = ParseEndNode(node, allDialogs);
                if (shouldRedirect)
                {
                    return;
                }
            }

            node.Children.AddRange(childDialogs);
            node.Id = this.counter;
            this.counter++;
            foreach (var parentChild in node.Children)
            {
                this.ParseChildNodes(parentChild, nextColumn + 1, worksheet, allDialogs);
            }
        }

        private static void ParseNonEndNode(ExcelDialogCell node, ExcelWorksheet worksheet, int nextColumn, List<ExcelDialogCell> childDialogs)
        {
            for (int i = node.Cell.StartLine; i <= node.Cell.EndLine; i++)
            {
                var nextCell = worksheet.Cells[i, nextColumn];
                var range = ExcelUtils.GetCellRange(nextCell, worksheet);
                childDialogs.Add(new ExcelDialogCell
                {
                    Cell = range
                });
                i = range.EndLine;
            }
        }

        private static bool ParseEndNode(ExcelDialogCell node, List<ExcelDialogCell> allDialogs)
        {
            if (!node.Question.MatchesPattern(ExcelConfiguration.RedirectCellPattern))
            {
                return false;
            }

            var nextDialog = allDialogs.Flatten(dialogCell => dialogCell.Children)
                .FirstOrDefault(s => s.Cell.CellName == node.Question.RemoveNonAlphanumeric());

            if (nextDialog != null)
            {
                node.Children.Add(nextDialog);
            }

            node.NodeType = ExcelStepType.Redirect;
            return true;
        }

        private static void FillQuestionsAndTexts(ExcelDialogCell node)
        {
            if (node.NodeType == ExcelStepType.Choice)
            {
                node.Question = node.TextLines.First(s => !char.IsDigit(s.First()));
                node.Messages.AddRange(node.TextLines.Where(s => !node.Choices.ContainsValue(s)));

                foreach (var choice in node.TextLines.Where(s => char.IsDigit(s.First())).Reverse())
                {
                    Regex regex = new Regex("[0-9].");
                    node.Choices.Add(int.Parse(choice.First().ToString()), regex.Replace(choice, string.Empty, 1).Trim());
                    node.Messages.Remove(choice);
                }
            }
            else
            {
                node.Question = node.TextLines.FirstOrDefault();
                node.Messages.AddRange(node.TextLines.Skip(1).Where(s => !s.IsSurroundedByAnyBraces()).Reverse().ToList());
            }

            if (node.NodeType != ExcelStepType.End)
            {
                node.Messages.Remove(node.Question);
            }
        }
    }
}