namespace COMPUTOR_V1;
using System.Collections.Generic;

public static class PolynomialHandler
{
    public static Dictionary<string, string> FormatEquation(string equation)
    {
        equation = equation.Replace(" ", "");
        CheckEquationSyntax(equation);
        return GetTerms(equation);
    }

    private static void CheckEquationSyntax(string equation)
    {
        if (string.IsNullOrWhiteSpace(equation))
            throw new Exception("Equation is invalid. Can't be empty.");

        for (int i = 0; i < equation.Length; i++)
        {
            if (InvalidSymbol(equation[i]))
                throw new Exception($"Equation contains invalid symbol: \'{equation[i]}\'.");
            if (equation[i] == 'ˆ' && i > 0 && equation[i-1] == 'x')
                i = CheckExponent(equation, i+1);
        }
    }

    private static Dictionary<string, string> GetTerms(string equation)
    {
        Dictionary<string, string> termsDic = new Dictionary<string, string>();
        List<string> terms = new List<string>();
        int equalSign = GetEqualSignPosition(equation);

        ExtractTerms(equation, 0, equalSign, terms);
        terms.Add("=");
        ExtractTerms(equation, equalSign + 1, equation.Length, terms);

        OrderTermsToTheLeft(terms);
        SimplifyEquation(terms, termsDic);
        return termsDic;
    }

    private static void SimplifyEquation(List<string> terms, Dictionary<string, string> termsDic)
    {
        foreach (var term in terms)
        {
            string key;
            string replace;
            if (!term.Contains("x") || term.Contains("xˆ0"))
            {
                key = "constant";
                replace = term.Contains("xˆ0") ? "xˆ0" : "";
            }
            else if (term.Contains("xˆ2"))
                key = replace = "xˆ2";
            else if (term.Contains("xˆ1") || term.Contains("x"))
            {
                key = "x";
                replace = term.Contains("xˆ1") ? "xˆ1" : "x";
            }
            else
                throw new Exception("AHHH. This should not happen. Delete later."); // TODO: delete later

            termsDic[key] = CalculateTerms(termsDic, key, term, replace);
            if (termsDic[key] == "0" || termsDic[key] == "-0")
                termsDic.Remove(key);
        }
    }

    private static string CalculateTerms(Dictionary<string, string> termsDic, string key, string term2, string replace)
    {
        if (!termsDic.ContainsKey(key))
        {
            if (replace == "")
                return term2;
            else
            {
                term2 = term2.Replace(replace, "");
                if (term2 == "")
                    return "1";
                return term2;
            }
        }

        string term1 = termsDic[key];
        if (replace != "")
        {
            term1 = term1.Replace(replace, "");
            term2 = term2.Replace(replace, "");
        }

        if (string.IsNullOrEmpty(term1))
            return term2;

        if (string.IsNullOrEmpty(term2))
            term2 = "1";
        try
        {
            double tmp1 = double.Parse(term1);
            double tmp2 = double.Parse(term2);
            double result = tmp1 + tmp2;
            return result + "";
        }
        catch (Exception e)
        {
            throw new Exception($"Equation is invalid. One of the following are not valid numbers: \'{term1}\', \'{term2}\'.");
        }
    }

    private static int GetEqualSignPosition(string equation)
    {
        if (equation.Count(c => c == '=') > 1)
            throw new Exception("Equation is invalid. There can only be 1 \'=\' sign.");
        int equalSign = equation.IndexOf('=');
        if (equalSign == -1)
            throw new Exception("Equation is invalid. There is no \'=\' sign.");
        if (equalSign == 0)
            throw new Exception("Equation is invalid. Left side of equation cant be empty.");
        return equalSign;
    }

    private static void OrderTermsToTheLeft(List<string> terms)
    {
        int equalSignIndex = terms.IndexOf("=");
        for (int k = equalSignIndex + 1; k < terms.Count; k++)
            terms[k] = NegateTerm(terms[k]);
        terms.RemoveAt(equalSignIndex); // Remove the '='
    }

    private static string NegateTerm(string term)
    {
        if (term.StartsWith("-"))
            return term.Substring(1);  // Remove the '-' sign
        else if (term.StartsWith("+"))
            return "-" + term.Substring(1); // Change '+' to '-'
        else
            return "-" + term; // Prepend '-' if no sign
    }

    private static void ExtractTerms(string equation, int start, int end, List<string> terms)
    {
        int i = start;
        while (i < end)
        {
            int nextSign = equation.IndexOfAny(new char[] { '+', '-' }, i + 1, end - i - 1);
            int termLength = (nextSign == -1 ? end : nextSign) - i;
            string term = "";

            if (equation[i] == '+')
                term = equation.Substring(i+1, termLength-1);
            else
                term = equation.Substring(i, termLength);

            if (!string.IsNullOrEmpty(term))
            {
                term = term.Replace("*", "");
                if (term.Contains('ˆ'))
                {
                    if (!(term.Contains("xˆ") && equation.Count(c => c == '=') == 1))
                        term = ResolveExponentExpressions(term);
                }
                terms.Add(term);
            }
            if (nextSign == -1)
                break;
            i = nextSign;
        }
    }

    private static string ResolveExponentExpressions(string term)
    {
        string resolved_term = term;
        for (int i = 0; i < term.Length; i++)
        {
            int found = term.IndexOf('ˆ', i);
            if (found == -1)
                break;
            if (found == 0)
                throw new Exception("Equation is invalid. Contains wrong syntax around this symbol: \'ˆ\'.");
            if (term[found-1] == 'x')
                continue;
            // 4x + 11.11ˆ2
            char exponent = '1';
            if (term.Length > found + 1)
            {
                exponent = term[found + 1];
            }

            string number_before = "";
            int number_start = 0;
            for (int k = found - 1; k >= 0; k--)
            {
                if (char.IsDigit(term[k]) || term[k] == ',' || term[k] == '-')
                    continue;
            }
            string number = term.Substring(number_start, found - number_start);
            double result = Math.Pow(double.Parse(number), (int)char.GetNumericValue(exponent));
            resolved_term = term.Replace(number + "ˆ" + exponent, result.ToString());

        }

        return resolved_term;
    }

    private static int CheckExponent(string equation, int i)
    {
        char exponent = equation[i];

        if (exponent == '0' || exponent == '1' || exponent == '2')
            return i;
        if (exponent == '(')
        {
            int bracketPosition = equation.IndexOf(')', i + 2);
            if (bracketPosition == -1)
                throw new Exception($"Equation is invalid. Equation contains invalid exponent: {equation[i - 2]}ˆ{exponent}");
            int k = 0;
            if (equation[i + 1] == '+')
                k++;
            while (equation[i+k + 1] == '0')
                k++;
            string number = null;
            for (k += i+1; k < bracketPosition; k++)
            {
                if (equation[k] == 'x')
                    throw new Exception("Equation is invalid. It is illegal to write x as an exponent.");
                if (char.IsDigit(equation[k]))
                {
                    number += equation[k];
                    continue;
                }
                throw new Exception(
                    $"Equation is invalid. Equation contains invalid exponent: {equation[i - 2]}ˆ{equation.Substring(i, bracketPosition-i+1)}");
            }
            if (number == null || number == "0" || number == "1" || number == "2")
                return i;
            throw new Exception(
                    $"Equation is invalid. Equation contains invalid exponent: {equation[i - 2]}ˆ{equation.Substring(i, bracketPosition-i+1)}");
        }

        if (exponent == '0' || exponent == '1' || exponent == '2')
            return i;
        if (char.GetNumericValue(exponent) > 2)
            throw new Exception(
                $"Equation is invalid. The polynomial degree is {exponent}, the program doesnt support degrees greater then 2.");
        if (exponent == 'x')
            throw new Exception("Equation is invalid. It is illegal to write x as an exponent.");
        throw new Exception($"Equation contains invalid exponent: {equation[i - 2]}ˆ{equation[i]}");
    }

    private static bool InvalidSymbol(char c)
    {
        if (char.IsDigit(c) || c == 'π' || c == ',')
            return false;
        if (c == '-' || c == '+' || c == '/' || c == '*')
            return false;
        if (c == '=' || c == 'ˆ' || c == '(' || c == ')')
            return false;
        if (c == 'x')
            return false;
        return true;
    }

    public static void SolveEquation(Dictionary<string, string> termsDic)
    {
        if (termsDic.ContainsKey("xˆ2"))
            SolveQuadraticEquation(termsDic);
        else if (termsDic.ContainsKey("x"))
            SolveLinearEquation(termsDic);
        else
            if (termsDic.ContainsKey("constant"))
                Utils.PrintInColor("The polynomial has no solution.", ConsoleColor.Red);
            else
                Utils.PrintInColor("The polynomial is an identity, true for any value of x.", ConsoleColor.Green);
    }

    private static void SolveQuadraticEquation(Dictionary<string, string> termsDic)
    {
        double a = double.Parse(termsDic["xˆ2"]);
        double b = termsDic.ContainsKey("x") ? double.Parse(termsDic["x"]) : 0;
        double c = termsDic.ContainsKey("constant") ? double.Parse(termsDic["constant"]) : 0;

        // Console.WriteLine($"calc: {b}ˆ2 - 4 * {a} * {c}");

        double discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant > 0)
        { // can determine x1 and x2
            Utils.PrintInColor($"Since the discriminant Δ = {discriminant} > 0; there are two distinct real roots, means the graph intersects the x-axis at two distinct points:\n", ConsoleColor.DarkRed);

            // x1 = (-b+√∆)/2a
            double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            // Console.WriteLine($"calc: (-{b} + {discriminant}ˆ2) / (2 * {a})");
            // x2 = (-b-√∆)/2a
            double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            // Console.WriteLine($"calc: (-{b} - {discriminant}ˆ2) / (2 * {a})");
            Utils.PrintInColor($"x1 = {x1}\n", ConsoleColor.Green);
            Utils.PrintInColor($"x2 = {x2}\n", ConsoleColor.Green);

        }
        else if (discriminant == 0)
        { // can determine x
            Utils.PrintInColor($"Since the discriminant Δ = 0; there is one real root (a repeated root), means the graph touches the x-axis at exactly one point:\n", ConsoleColor.DarkRed);
            // (-b)/2a
            double x = (-b) / (2 * a);
            Utils.PrintInColor($"x = {x}\n", ConsoleColor.Green);
        }
        else
        {
            Utils.PrintInColor($"Since the discriminant Δ = {discriminant} < 0; there are two complex roots, means the graph does not intersect the x-axis.\n", ConsoleColor.DarkRed);
            Utils.PrintInColor("I am not able to calculate complex functions. Me dumb, bye.", ConsoleColor.Red);
        }
    }

    private static void SolveLinearEquation(Dictionary<string, string> termsDic)
    {
        double rightSite = 0;

        if (termsDic.ContainsKey("constant"))
        {
            if (termsDic["constant"].StartsWith('-'))
                rightSite = double.Parse(termsDic["constant"].Substring(1));
            else if (termsDic["constant"].StartsWith('+'))
                rightSite = double.Parse("-" + termsDic["constant"].Substring(1));
            else
                rightSite = double.Parse("-" + termsDic["constant"]);
        }
        double x = rightSite / double.Parse(termsDic["x"]);
        Utils.PrintInColor($"The solution is: {x}", ConsoleColor.DarkGreen);
    }

    public static void PrintReducedForm(Dictionary<string, string> termsDic)
    {
        string print = "";
        var keys = termsDic.Keys.ToList();

        Console.Write("Reduced Form: ");
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            string value = termsDic[key];

            if (value.StartsWith('-'))
                print += $"- {value.Substring(1)} ";
            else
            {
                if (i != 0) // Avoid leading "+"
                    print += "+ ";
                print += value + " ";
            }
            if (key != "constant")
                print += $"* {key} ";
        }
        if (string.IsNullOrEmpty(print))
            print = "0 = 0";
        else
            print += "= 0";
        Console.WriteLine(print);
    }

    public static void PrintDegree(Dictionary<string, string> termsDic)
    {
        int degree;
        if (termsDic.ContainsKey("xˆ2"))
            degree = 2;
        else if (termsDic.ContainsKey("x"))
            degree = 1;
        else
            degree = 0;
        Utils.PrintInColor($"Polynomial degree: {degree}\n", ConsoleColor.DarkBlue);
    }
}