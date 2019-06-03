namespace ChatbotDialogGenerator.Configuration
{
    using System;
    using Utils;

    public class CommonConfiguration
    {
        // dialog
        public string DialogNamespaceName => CommonSettings.Default.DialogNamespaceName;

        public string[] DialogUsings => ConfigurationUtils.GetUsingsFromSetting(CommonSettings.Default.DialogUsings);

        public string DialogProjectName => CommonSettings.Default.DialogProjectName;

        public string DialogFolderPath => CommonSettings.Default.DialogFolderPath;

        public bool UsePostAsSeparateBubble { get; set; } = CommonSettings.Default.PostAsSeparateBubble;

        public Func<string, string> DialogClassName => step => $"{step}Dialog";

        public string TicketDialog => CommonSettings.Default.TicketDialog;

        // interface
        public string InterfaceNamespaceName => CommonSettings.Default.InterfaceNamespaceName;

        public string BaseDialogInterface => CommonSettings.Default.BaseDialogInterface;

        public string[] InterfaceUsings => ConfigurationUtils.GetUsingsFromSetting(CommonSettings.Default.InterfaceUsings);

        public string InterfaceProjectName => CommonSettings.Default.InterfaceProjectName;

        public string InterfaceFolderPath => CommonSettings.Default.InterfaceFolderPath;

        // resources
        public string[] ResourceAccessorUsings => ConfigurationUtils.GetUsingsFromSetting(CommonSettings.Default.ResourceAccessorUsings);

        public Func<string, string> ResourcesNamespaceName => step => $"{step}Resources";

        // comm
        public string SolutionPath => CommonSettings.Default.SolutionPath;

        public string SolutionName => CommonSettings.Default.SolutionName;

        public string ExcelFilePath => CommonSettings.Default.ExcelFilePath;

        public bool IsComplete()
        {
            return !SolutionPath.IsNullOrWhitespace() &&
                   !ExcelFilePath.IsNullOrWhitespace() &&
                   !DialogProjectName.IsNullOrWhitespace() &&
                   !InterfaceProjectName.IsNullOrWhitespace() &&
                   !TicketDialog.IsNullOrWhitespace() &&
                   !DialogFolderPath.IsNullOrWhitespace() &&
                   !InterfaceFolderPath.IsNullOrWhitespace() &&
                   !DialogNamespaceName.IsNullOrWhitespace() &&
                   !InterfaceNamespaceName.IsNullOrWhitespace();
        }
    }
}