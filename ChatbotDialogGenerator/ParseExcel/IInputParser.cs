namespace ParseExcel
{
    using System.Collections.Generic;
    using ParseExcel.Domain.Excel;

    public interface IInputParser
    {
        List<ExcelDialogCell> ParseInputFromFile(string path);
    }
}