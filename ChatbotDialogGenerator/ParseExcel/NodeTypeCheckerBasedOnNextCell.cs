namespace ParseExcel
{
    using System;
    using System.Text.RegularExpressions;
    using OfficeOpenXml;

    public class NodeTypeCheckerBasedOnNextCell : INodeTypeChecker
    {
        public ExcelStepType ReturnTypeForCell(ExcelWorksheet worksheet, ExcelRange cell)
        {
            if (worksheet == null)
            {
                throw new ArgumentNullException(nameof(worksheet));
            }

            if (cell == null)
            {
                throw new ArgumentNullException(nameof(cell));
            }

            var nextColumnCell = worksheet.Cells[cell.Start.Row, cell.Start.Column + 1].Value;
            if (nextColumnCell == null || Regex.IsMatch(nextColumnCell.ToString(), "\\[.*\\]|{.*}"))
            {
                return ExcelStepType.End;
            }
            var nextColumnValue = nextColumnCell.ToString().Trim();
            if (nextColumnValue.Equals("Yes", StringComparison.OrdinalIgnoreCase) ||
                nextColumnValue.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                return ExcelStepType.YesNo;
            }

            return ExcelStepType.Choice;
        }
    }
}