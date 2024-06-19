namespace COMPUTOR_V1;

public static class PolynomialHandler
{
    private static string FormatEquation(string equation)
    {
        equation = equation.Replace(" ", "");
        if (string.IsNullOrWhiteSpace(equation))
            throw new Exception("Equation is empty.");
        Console.WriteLine($"Equation: {equation}");
        foreach (var t in equation)
        {
            if (InvalidSymbol(t))
                throw new Exception($"Equation contains invalid symbol: \'{t}\'.");

        }


        return equation;
    }

    private static bool InvalidSymbol(char c)
    {
        if (char.IsDigit(c) || c == 'π')
            return false;
        if (c == '-' || c == '+' || c == '/' || c == '*')
            return false;
        if (c == '=' || c == 'ˆ' || c == '(' || c == ')')
            return false;
        if (c == 'x')
            return false;
        return true;
    }

    public static void SolveEquation(string equation)
    {
        equation = FormatEquation(equation);
    }
}