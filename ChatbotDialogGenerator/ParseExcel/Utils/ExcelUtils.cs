namespace ParseExcel.Utils
{
    using System;
    using System.Linq;
    using Domain.Excel;
    using OfficeOpenXml;

    public static class ExcelUtils
    {
        public static ExcelValueCell GetCellRange(ExcelRange cell, ExcelWorksheet worksheet)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            if (worksheet == null)
                throw new ArgumentNullException(nameof(worksheet));

            var cellValue = cell.Value.ToString();
            if (cell.Merge)
            {
                var id = worksheet.GetMergeCellId(cell.Start.Row, cell.Start.Column);
                var cellFromArray = worksheet.MergedCells[id - 1];
                var mergedCellArray = cellFromArray.Split(':');
                try
                {
                    var startLine = int.Parse(string.Join(string.Empty, mergedCellArray[0].Where(char.IsDigit).ToList()));
                    var endLine = int.Parse(string.Join(string.Empty, mergedCellArray[1].Where(char.IsDigit).ToList()));
                    return new ExcelValueCell()
                    {
                        Value = cellValue,
                        StartLine = startLine,
                        EndLine = endLine,
                        CellName = mergedCellArray[0],
                        Column = cell.Start.Column
                    };
                }
                catch (Exception)
                {
                    throw new InvalidOperationException("Dialogs cells are merged with other columns.");
                }
            }

            return new ExcelValueCell()
            {
                Value = cellValue,
                StartLine = cell.Start.Row,
                EndLine = cell.Start.Row,
                CellName = cell.Address,
                Column = cell.Start.Column
            };
        }
    }
}