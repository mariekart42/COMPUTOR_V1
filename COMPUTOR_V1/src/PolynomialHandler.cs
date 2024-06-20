namespace COMPUTOR_V1;
using System.Collections.Generic;

public static class PolynomialHandler
{
    private static string FormatEquation(string equation)
    {
        equation = equation.Replace(" ", "");
        if (string.IsNullOrWhiteSpace(equation))
            throw new Exception("Equation is invalid. Can't be empty.");
        Console.WriteLine($"Equation: {equation}");

        Dictionary<string, string> terms = GetTerms(equation);



        for (int i = 0; i < equation.Length; i++)
        {
            if (InvalidSymbol(equation[i]))
                throw new Exception($"Equation contains invalid symbol: \'{equation[i]}\'.");
            if (equation[i] == 'ˆ')
                i = CheckExponent(equation, i+1);

        }

        return equation;
    }

    private static Dictionary<string, string> GetTerms(string equation)
    {
        Dictionary<string, string> something = new Dictionary<string, string>();
        int equalSign = GetEqualSignPosition(equation);

        List<string> terms = new List<string>();

        ExtractTerms(equation, 0, equalSign, terms);
        terms.Add("=");
        ExtractTerms(equation, equalSign + 1, equation.Length, terms);

        OrderTermsToTheLeft(terms);
        Console.Write("Term: ");
        foreach (var term in terms)
        {
            Console.Write($"{term} ");
        }
        Console.WriteLine("\n-----------");
        SimplifyEquation(terms, something);

        // foreach (var some in something)
        // {
        //     Console.WriteLine($"Key: {some.Key}|Value: {some.Value}");
        // }

        return something;
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

        Console.WriteLine("\nHEEEREEE:");
        foreach (var lol in termsDic)
        {
            if (lol.Key == "constant")
                Console.Write($"{lol.Value} + ");
            else
                Console.Write($"{lol.Value}{lol.Key} + ");
        }
        Console.WriteLine(" = 0\n-------------------");
    }

    private static string CalculateTerms(Dictionary<string, string> termsDic, string key, string term2, string replace)
    {
        //termsDic, key, term, replace
        if (!termsDic.ContainsKey(key))
        {
            // Console.WriteLine($"new entry[{key}]: {term2}");
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
        Console.WriteLine($"Term1: {term1}");
        Console.WriteLine($"Term1: {term2}");
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
            double term1_double = double.Parse(term1);
            double term2_double = double.Parse(term2);
            double result = term1_double + term2_double;
            // Console.WriteLine($"calculating type[{key}] {term1_double} + {term2_double} = {result}");
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
        return equalSign;
    }

    private static void OrderTermsToTheLeft(List<string> terms)
    {
        int equalSignIndex = terms.IndexOf("=");
        for (int k = equalSignIndex + 1; k < terms.Count; k++)
        {
            terms[k] = NegateTerm(terms[k]);
            // Console.WriteLine($"right side: {terms[k]}");
        }

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

    private static List<string> ExtractTerms(string equation, int start, int end, List<string> terms)
    {
        int i = start;
        string term = null;
        int termIndex = 1;
        while (i < end)
        {
            int nextSign = equation.IndexOfAny(new char[] { '+', '-' }, i + 1, end - i - 1);

            int termLength = (nextSign == -1 ? end : nextSign) - i;
            if (equation[i] == '+')
                term = equation.Substring(i+1, termLength-1);
            else
                term = equation.Substring(i, termLength);

            if (!string.IsNullOrEmpty(term))
            {
                terms.Add(term);
                termIndex++;
            }
            if (nextSign == -1)
                break;
            i = nextSign;
        }
        return terms;
    }

    private static int CheckExponent(string equation, int i)
    {
        char exponent = equation[i];

        if (exponent == '0' || exponent == '1' || exponent == '2')
            return i;
        if (exponent == '(')
        {
            int bracketPosition = equation.IndexOf(')', i + 2);
            Console.WriteLine($"indexOf ): {equation[bracketPosition]}");
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
                    $"1 Equation is invalid. Equation contains invalid exponent: {equation[i - 2]}ˆ{equation.Substring(i, bracketPosition-i+1)}");
            }
            if (number == null || number == "0" || number == "1" || number == "2")
                return i;
            throw new Exception(
                    $"2 Equation is invalid. Equation contains invalid exponent: {equation[i - 2]}ˆ{equation.Substring(i, bracketPosition-i+1)}");
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