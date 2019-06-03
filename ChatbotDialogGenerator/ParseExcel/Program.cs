namespace ParseExcel
{
    class Program
    {
        private static IUtteranceFinder utteranceFinder = new UtteranceFinder();
        private static INodeTypeChecker nodeTypeChecker = new NodeTypeCheckerBasedOnNextCell();
        static void Main(string[] args)
        {
            var path = @"c:\Users\jgratka\sample.xlsx";
            var dialogColumnNumber = 1;
            var utterancesDialogNumber = 2;

            IInputParser parser = new DialogExcelParser(new UtteranceFinder(), new NodeTypeCheckerBasedOnNextCell());
            parser.ParseInputFromFile(path);
        }

    }
}
