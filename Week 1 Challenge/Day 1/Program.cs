// See https://aka.ms/new-console-template for more information
void Generate(int x)
{
    if (x % 3 == 0)
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
};

int angka = Convert.ToInt32(Console.ReadLine());
for (int i = 1; i <= angka; i++)
{
    Generate(i);
}
