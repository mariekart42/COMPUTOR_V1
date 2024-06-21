using COMPUTOR_V1;

try
{
    if (args.Length != 1)
        throw new Exception("wrong amount of arguments");

    Dictionary<string, string> termsDic = PolynomialHandler.FormatEquation(args[0]);
    Utils.PlotGraph(termsDic);
    PolynomialHandler.PrintReducedForm(termsDic);
    PolynomialHandler.PrintDegree(termsDic);
    PolynomialHandler.SolveEquation(termsDic);
}
catch (Exception e)
{
    Console.WriteLine("\nError in program:");
    Utils.PrintInColor("   "+e.Message+ "\n", ConsoleColor.Red);
    Console.WriteLine("\nStack Trace:");
    Utils.PrintInColor(e.StackTrace+"\n\n", ConsoleColor.Red);
}


