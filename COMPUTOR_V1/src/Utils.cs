namespace COMPUTOR_V1;

public static class Utils
{
    public static void PrintInColor(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(msg);
        Console.ForegroundColor = ConsoleColor.Black;
    }

    public static void PlotGraph(Dictionary<string, string> termsDic)
    {
        Func<double, double> polynomialFunction = GetPolynomialFunction(termsDic);

        int pointCount = 1000;
        double[] xs = new double[pointCount];
        double[] ys = new double[pointCount];

        int scale = 10;

        double xStart = -scale;
        double xEnd = scale;
        double step = (xEnd - xStart) / (pointCount - 1);

        for (int i = 0; i < pointCount; i++)
        {
            xs[i] = -scale + i * step;
            ys[i] = polynomialFunction(xs[i]);
        }

        var plt = new ScottPlot.Plot();
        plt.Axes.SetLimits(-scale, scale, -scale, scale);
        plt.Layout.Frameless();
        plt.Add.Line(-scale, 0, scale, 0);
        plt.Add.Line(0, -scale, 0, scale);
        plt.Add.Scatter(xs, ys);
        plt.Title("Polynomial Function Plot");
        plt.SavePng("polynomial_plot.png", 600, 400);
    }

    private static Func<double, double> GetPolynomialFunction(Dictionary<string, string> termsDic)
    {
        double x = 1.0;
        double part1 = termsDic.ContainsKey("xˆ2") ? double.Parse(termsDic["xˆ2"]) : 0;
        double part2 = termsDic.ContainsKey("x") ? double.Parse(termsDic["x"]) : 0;
        double part3 = termsDic.ContainsKey("constant") ? double.Parse(termsDic["constant"]) : 0;

        if (termsDic.ContainsKey("xˆ2"))
            if (termsDic.ContainsKey("x"))
                if (termsDic.ContainsKey("constant"))
                    return x => part1 * Math.Pow(x, 2) + part2 * x + part3;
                else
                    return x => part1 * Math.Pow(x, 2) + part2 * x;
            else
            if (termsDic.ContainsKey("constant"))
                return x => part1 * Math.Pow(x, 2) + part3;
            else
                return x => part1 * Math.Pow(x, 2);
        else
        if (termsDic.ContainsKey("x"))
            if (termsDic.ContainsKey("constant"))
                return x => part2 * x + part3;
            else
                return x => part2 * x;
        else
        if (termsDic.ContainsKey("constant"))
            return x => part3;
        else
            return x => 0;
    }
}