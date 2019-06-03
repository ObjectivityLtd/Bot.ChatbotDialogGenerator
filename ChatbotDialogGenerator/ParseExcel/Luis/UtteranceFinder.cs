namespace ParseExcel.Luis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OfficeOpenXml;
    using ParseExcel.Domain.Excel;

    public class UtteranceFinder : IUtteranceFinder
    {
        private int utterancesDialogNumber = 2;

        public IList<string> FindUtterancesForDialog(ExcelDialogCell dialogCell, ExcelWorksheet worksheet)
        {
            if (dialogCell == null)
                throw new ArgumentNullException(nameof(dialogCell));

            if (worksheet == null)
                throw new ArgumentNullException(nameof(worksheet));

            return worksheet
                .Cells[dialogCell.Cell.StartLine, this.utterancesDialogNumber].Value
                .ToString()
                .Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }
    }
}