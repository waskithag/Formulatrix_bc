namespace Classes;

public class Hewan
{
    public string Name {get; set;} = "";
    public virtual void suara()
    {
        Console.WriteLine("...");
    }
}

public class Anjing : Hewan
{
    public override void suara()
    {
        Console.WriteLine("woof");
    }
}

public class Kucing : Hewan
{
    public override void suara()
    {
        Console.WriteLine("meow");
    }
}