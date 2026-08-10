// See https://aka.ms/new-console-template for more information
void Generate(int x)
{
    if (x % 7 == 0)
    {
        if (x % 3 == 0)
        {
            if (x % 5 == 0)
            {
                Console.Write("foobarjazz ");
            }
            else
            {
                Console.Write("foojazz ");
            }
        }
        else if (x % 5 == 0)
        {
            Console.Write("barjazz ");
        }
        else
        {
            Console.Write("jazz ");
        }
    }
    else if (x % 3 == 0)
    {
        if (x % 5 == 0)
        {
            Console.Write("foobar ");
        }
        else
        {
            Console.Write("foo ");
        }
    }
    else if (x % 5 == 0)
    {
        Console.Write("bar ");
    }
    else
    {
        Console.Write($"{x} ");
    }
}

// Generate(21);
// Console.WriteLine();
// Generate(35);
// Console.WriteLine();
// Generate(105);
int angka = Convert.ToInt32(Console.ReadLine());
for (int i = 1; i <= angka; i++)
{
    Generate(i);
}
