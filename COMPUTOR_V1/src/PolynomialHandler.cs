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


        // x+2=0
        // xˆ2-1=0
        Console.WriteLine($"After Formatting: {equation}");
        return equation;
    }

    private static Dictionary<string, string> GetTerms(string equation)
    {
        Dictionary<string, string> terms = new Dictionary<string, string>();
        int equalSign = equation.IndexOf('=');

        // Extract terms from the left side of the equation
        ExtractTerms(equation, 0, equalSign, terms);

        // Extract terms from the right side of the equation
        ExtractTerms(equation, equalSign + 1, equation.Length, terms);

        // Display extracted terms for debugging
        foreach (var term in terms)
        {
            Console.WriteLine($"Key: {term.Key}, Value: {term.Value}");
        }



        // int start = 0;
        // List<string> hehe = new List<string>();
        // int equalSign = equation.IndexOf('=');
        // for (int i = 0; i < equalSign; i++)
        // {
        //     if (equation[i] == '+' || equation[i] == '-')
        //     {
        //         if (equation[start] == '+')
        //             hehe.Add(equation.Substring(start+1, i - start -1));
        //         else
        //             hehe.Add(equation.Substring(start, i - start));
        //         start = i;
        //     }
        // }
        // if (equation[start] == '+')
        //     hehe.Add(equation.Substring(start+1, equalSign - start -1));
        // else
        //     hehe.Add(equation.Substring(start, equalSign - start));
        // start = equalSign + 1;
        // for (int i = equalSign + 1; i < equation.Length; i++)
        // {
        //     if (equation[i] == '+' || equation[i] == '-')
        //     {
        //         if (equation[start] == '+')
        //             hehe.Add(equation.Substring(start+1, i - start -1));
        //         else
        //             hehe.Add(equation.Substring(start, i - start));
        //         start = i;
        //     }
        // }
        // if (equation[start] == '+')
        //     hehe.Add(equation.Substring(start+1, equation.Length - start -1));
        // else
        //     hehe.Add(equation.Substring(start, equation.Length - start));
        return terms;
    }

    private static void ExtractTerms(string equation, int start, int end, Dictionary<string, string> terms)
    {
        List<string> hehe = new List<string>();
        int i = start;
        string term = null;
        while (i < end)
        {
            int nextSign = equation.IndexOfAny(new char[] { '+', '-' }, i, end - i);
            if (nextSign == -1)
            {
                term = equation.Substring(i, end - i);
                i = end + 1;
            }
            else
            {
                term = equation.Substring(i, nextSign - i);
                i = nextSign + 1;
            }
            hehe.Add(term);
        }

        foreach (var ll in hehe)
        {
            Console.WriteLine($"term: {ll}");
        }

        // Store the extracted terms in the dictionary
        // for (int j = 0; j < hehe.Count; j++)
        // {
        //     terms.Add($"Term{j + 1}", hehe[j]);
        // }
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