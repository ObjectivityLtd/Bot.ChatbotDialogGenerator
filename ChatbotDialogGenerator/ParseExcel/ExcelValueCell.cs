namespace ParseExcel
{
    public class ExcelValueCell
    {
        public string Value { get; set; }
        
        public int StartLine { get; set; }
        public int EndLine { get; set; }

        public string CellName { get; set; }

        public int Column { get; set; }
    }
}