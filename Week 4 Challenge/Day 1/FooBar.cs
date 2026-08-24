using System;
using System.Text;
namespace FooBar;

class FooBarBuzz
{
    Dictionary<int, string> Rules { get; } = new();
    public void AddRule(int divisor, string output)
    {
        Rules[divisor] = output;
    }

    public string Evaluate(int number)
    {
        StringBuilder output = new();
        foreach (var rule in Rules)
        {
            if (number % rule.Key == 0)
            {
                output.Append(rule.Value);
            }
        }

        if (output.ToString() == "")
        {
            return number.ToString();
        }

        return output.ToString();
    }

    public string GenerateSequence(int start, int end)
    {
        StringBuilder output = new();

        for (int i = start; i < end; i++)
        {
            output.Append($"{Evaluate(i)}");
            output.Append(", ");
        }
        output.Append($"{Evaluate(end)}");

        return output.ToString();
    }
}