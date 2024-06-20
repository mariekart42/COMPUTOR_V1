// See https://aka.ms/new-console-template for more information

using COMPUTOR_V1;
using ScottPlot.WinForms;
using ScottPlot;
Console.WriteLine("Helljo, World!");

try
{
    if (args.Length != 1)
        throw new Exception("wrong amount of arguments");

    Dictionary<string, string> termsDic = PolynomialHandler.FormatEquation(args[0]);
    PolynomialHandler.PlotGraph(termsDic);
    PolynomialHandler.PrintReducedForm(termsDic);
    PolynomialHandler.PrintDegree(termsDic);
    PolynomialHandler.SolveEquation(termsDic);

    Console.WriteLine("me calculati :)");

}
catch (Exception e)
{
    ConsoleColor originalColor = ConsoleColor.Black;
    Console.ForegroundColor = originalColor;
    Console.WriteLine("\nError in program:");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("   "+e.Message);
    Console.ForegroundColor = originalColor;
    Console.WriteLine("\nStack Trace:");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.StackTrace+"\n");
}
