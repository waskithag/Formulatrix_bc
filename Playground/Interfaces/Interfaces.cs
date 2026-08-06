using System;

namespace Interfaces;

public interface ILandCreature
{
    void Run();
}

public interface IWaterCreature
{
    void Swim();
}

public class Hewan
{
    public string Name {get; set;} = "";
    public virtual void Suara()
    {
        Console.WriteLine("...");
    }
}

public class Sapi : Hewan, ILandCreature
{
    public override void Suara()
    {
        base.Suara();
    }

    public void Run()
    {
        Console.WriteLine("gedebug gedebug");
    }
}