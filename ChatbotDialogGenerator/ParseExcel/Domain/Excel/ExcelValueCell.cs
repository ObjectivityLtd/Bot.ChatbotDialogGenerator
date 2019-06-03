namespace ParseExcel.Domain.Excel
{
    using System;

    public class ExcelValueCell
    {
        private string cellValue;

        public string Value
        {
            get => cellValue;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                this.cellValue = value.Trim();
            }
        }

        public int StartLine { get; set; }

        public int EndLine { get; set; }

        public string CellName { get; set; }

        public int Column { get; set; }
    }
}