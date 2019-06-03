namespace ParseExcel.Configuration
{
    public static class ExcelConfiguration
    {
        public static string RedirectCellPattern => ExcelSettings.Default.RedirectCellPattern;

        public static int ColumnWithDialogs => ExcelSettings.Default.ColumnWithDialogs;

        public static int FlowStartColumn => ExcelSettings.Default.FlowStartColumn;

        public static string DefaultEndAction => ExcelSettings.Default.DefaultEndAction;

        public static string DefaultEndText => ExcelSettings.Default.DefaultEndText;

        public static string TicketCreationText => ExcelSettings.Default.TicketCreationText;

        public static char ChoiceDelimiter => ExcelSettings.Default.ChoiceDelimiter;

        public static bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(RedirectCellPattern) &&
                   !string.IsNullOrWhiteSpace(DefaultEndAction) &&
                   !string.IsNullOrWhiteSpace(DefaultEndText) &&
                   !string.IsNullOrWhiteSpace(TicketCreationText) &&
                   ColumnWithDialogs != 0 &&
                   FlowStartColumn != 0;
        }
    }
}