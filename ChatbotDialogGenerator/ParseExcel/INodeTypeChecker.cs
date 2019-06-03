namespace ParseExcel
{
    using OfficeOpenXml;
    using ParseExcel.Domain.Excel;

    public interface INodeTypeChecker
    {
        ExcelStepType ReturnTypeForCell(ExcelWorksheet worksheet, ExcelRange cell);
    }
}