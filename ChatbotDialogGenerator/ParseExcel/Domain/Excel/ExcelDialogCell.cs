namespace ParseExcel.Domain.Excel
{
    using System.Collections.Generic;

    public class ExcelDialogCell
    {
        private string question;

        public int Id { get; set; }

        public Dictionary<int, string> Choices { get; } = new Dictionary<int, string>();

        public string Title { get; set; }

        public List<string> Utterances { get; } = new List<string>();

        public ExcelStepType NodeType { get; set; }

        public string Question
        {
            get => question;
            set => question = value?.Trim().Replace("\"", "\\\"");
        }

        public List<string> Messages { get; } = new List<string>();

        public List<ExcelDialogCell> Children { get; } = new List<ExcelDialogCell>();


        public ExcelValueCell Cell { get; set; }

        public List<string> TextLines { get; } = new List<string>();
    }
}