using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace ParseExcel
{
    public class UtteranceFinder : IUtteranceFinder
    {
        private int utterancesDialogNumber = 2;

        public IList<string> FindUtterancesForDialog(ExcelDialogCell excelDialogCell, ExcelWorksheet ws) => ws
            .Cells[excelDialogCell.Cell.StartLine, this.utterancesDialogNumber].Value
            .ToString()
            .Split(Environment.NewLine.ToCharArray()).Where(s => !string.IsNullOrEmpty(s)).ToList();
    }

    public interface IUtteranceFinder
    {
        IList<string> FindUtterancesForDialog(ExcelDialogCell dialogCell, ExcelWorksheet worksheet);
    }
}