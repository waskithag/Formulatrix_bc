using System.Text;

static string Generate(int x)
{
    StringBuilder output = new("");

    if (x % 3 == 0)
    {
        output.Append("foo");
    }

    if (x % 4 == 0)
    {
        output.Append("baz");
    }

    if (x % 5 == 0)
    {
        output.Append("bar");
    }

    if (x % 7 == 0)
    {
        output.Append("jazz");
    }

    if (x % 9 == 0)
    {
        output.Append("huzz");
    }

    if (output.ToString() == "")
    {
        return x.ToString();
    }
    
    return output.ToString();
}

int angka = Convert.ToInt32(Console.ReadLine());
for (int i = 1; i <= angka; i++)
{
    Console.Write($"{Generate(i)} ");
}