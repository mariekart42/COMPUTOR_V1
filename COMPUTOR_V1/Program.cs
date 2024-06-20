// See https://aka.ms/new-console-template for more information

using COMPUTOR_V1;
using ScottPlot.WinForms;
using ScottPlot;
Console.WriteLine("Helljo, World!");

try
{
    if (args.Length != 1)
        throw new Exception("wrong amount of arguments");

    // PolynomialHandler.SolveEquation(args[0]);
    Dictionary<string, string> termsDic = PolynomialHandler.FormatEquation(args[0]);
    PolynomialHandler.PlotGraph(termsDic);

    Console.WriteLine("\nEquation after extracting terms: ");
    foreach (var lol in termsDic)
    {
        if (lol.Key == "constant")
            Console.Write($"{lol.Value} + ");
        else
            Console.Write($"{lol.Value}{lol.Key} + ");
    }
    Console.WriteLine("= 0\n-------------------");



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
