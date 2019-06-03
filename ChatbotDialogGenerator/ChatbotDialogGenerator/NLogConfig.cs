namespace ChatbotDialogGenerator
{
    using System;
    using NLog;

    public static class NLogConfig
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Configure()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_CurrentDomain_UnhandledException;

        }

        private static void AppDomain_CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error(e.ExceptionObject as Exception, "Unhandled exception was thrown.");
        }
    }
}