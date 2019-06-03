namespace ParseExcel.Luis
{
    using System.Collections.Generic;
    using OfficeOpenXml;
    using ParseExcel.Domain.Excel;

    public interface IUtteranceFinder
    {
        IList<string> FindUtterancesForDialog(ExcelDialogCell dialogCell, ExcelWorksheet worksheet);
    }
}