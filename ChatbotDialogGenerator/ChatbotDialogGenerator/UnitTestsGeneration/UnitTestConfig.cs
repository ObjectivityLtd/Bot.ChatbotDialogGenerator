namespace ChatbotDialogGenerator.UnitTestsGeneration
{
    using System;

    public class UnitTestConfig
    {
        public string[] Usings { get; set; }

        public string Namespace { get; set; }

        public Func<string, string> UnitTestClassName => s => $"{s}Tests";
    }
}