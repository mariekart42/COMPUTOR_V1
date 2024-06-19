// See https://aka.ms/new-console-template for more information

using COMPUTOR_V1;
using ScottPlot.WinForms;
using ScottPlot;
Console.WriteLine("Helljo, World!");

try
{
    if (args.Length != 1)
        throw new Exception("wrong amount of arguments");

    PolynomialHandler.SolveEquation(args[0]);

    Console.WriteLine("me calculati :)");




    // Define the polynomial function: y = ax^2 + bx + c
    Func<double, double> polynomialFunction = x => 1 * Math.Pow(x, 3) + 1 * Math.Pow(x, 2) + 1 * x;

    // Generate data points for the polynomial function
    int pointCount = 100;
    double[] xs = new double[pointCount];
    double[] ys = new double[pointCount];

    int scale = 10;

    double xStart = -scale;
    double xEnd = scale;
    double step = (xEnd - xStart) / (pointCount - 1);

    for (int i = 0; i < pointCount; i++)
    {
        xs[i] = xStart + i * step;
        ys[i] = polynomialFunction(xs[i]);
    }

    // Create a new plot
    var plt = new ScottPlot.Plot();

    plt.Axes.SetLimits(-scale, scale, -scale, scale);
    // make the data area cover the full figure
    plt.Layout.Frameless();

    // set the data area background so we can observe its size
    // plt.DataBackground.Color = Colors.WhiteSmoke;

    plt.Add.Line(-scale, 0, scale, 0);
    plt.Add.Line(0, -scale, 0, scale);

    // Plot the polynomial function
    plt.Add.Scatter(xs, ys);

    // Add labels and title
    plt.Title("Polynomial Function Plot");

    // Save the plot as an image
    plt.SavePng("polynomial_plot.png", 600, 400);

}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
