using System;
using System.Collections.Generic;

// Base class
public abstract class Monster
{
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Speed { get; set; }
    public string Weapon { get; set; } = string.Empty;
}

// Subclasses created solely to hardcode presets:
public class FastScoutMonster : Monster
{
    public FastScoutMonster()
    {
        Health = 50;
        AttackPower = 10;
        Speed = 35;
        Weapon = "Dagger";
    }
}

public class ArmoredTankMonster : Monster
{
    public ArmoredTankMonster()
    {
        Health = 300;
        AttackPower = 25;
        Speed = 8;
        Weapon = "Warhammer";
    }
}

public class FireBossMonster : Monster
{
    public FireBossMonster()
    {
        Health = 1000;
        AttackPower = 80;
        Speed = 15;
        Weapon = "Flame Sword";
    }
}

#region Prototype
// 1. Single configurable class implementing a clone interface
public interface IPrototype<T>
{
    T Clone();
}

public class Monster : IPrototype<Monster>
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Speed { get; set; }
    public string Weapon { get; set; }

    public Monster(string name, int health, int attackPower, int speed, string weapon)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
        Speed = speed;
        Weapon = weapon;
    }

    // Shallow copy is sufficient here since fields are primitive/immutable types
    public Monster Clone()
    {
        return (Monster)this.MemberwiseClone();
    }

    public override string ToString() =>
        $"{Name} [HP: {Health}, ATK: {AttackPower}, SPD: {Speed}, Weapon: {Weapon}]";
}

// 2. Prototype Registry storing preset templates
public class MonsterSpawner
{
    private readonly Dictionary<string, Monster> _prototypes = new();

    public void Register(string key, Monster prototype) => _prototypes[key] = prototype;

    public Monster Spawn(string key)
    {
        if (_prototypes.TryGetValue(key, out var prototype))
        {
            return prototype.Clone();
        }
        throw new KeyNotFoundException($"No monster prototype registered for key: '{key}'");
    }
}

class Program
{
    static void Main()
    {
        var spawner = new MonsterSpawner();

        // Register archetypes dynamically (can also be loaded from JSON/Database)
        spawner.Register("scout", new Monster("Goblin Scout", 50, 10, 35, "Dagger"));
        spawner.Register("tank",  new Monster("Orc Tank", 300, 25, 8, "Warhammer"));
        spawner.Register("boss",  new Monster("Dragon Boss", 1000, 80, 15, "Flame Sword"));

        // Spawn instances on demand by cloning
        Monster scout1 = spawner.Spawn("scout");
        Monster scout2 = spawner.Spawn("scout");

        // Tweak an individual instance after cloning without affecting other instances
        scout2.Health = 70; // Elite scout variant

        Console.WriteLine(scout1); // Goblin Scout [HP: 50, ATK: 10, SPD: 35, Weapon: Dagger]
        Console.WriteLine(scout2); // Goblin Scout [HP: 70, ATK: 10, SPD: 35, Weapon: Dagger]
    }
}
#endregion

// Skill class to represent complex reference data
public class Skill
{
    public string Name { get; set; }
    public int ManaCost { get; set; }

    public Skill(string name, int manaCost)
    {
        Name = name;
        ManaCost = manaCost;
    }

    // Clone method to safely copy individual skills
    public Skill Clone() => new Skill(Name, ManaCost);

    public override string ToString() => $"{Name} ({ManaCost} MP)";
}

public interface IPrototype<T>
{
    T Clone();
}

public class Monster : IPrototype<Monster>
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public string Weapon { get; set; }
    
    // Reference type list requiring deep copy
    public List<Skill> Skills { get; set; }

    public Monster(string name, int health, int attackPower, string weapon, List<Skill> skills)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
        Weapon = weapon;
        Skills = skills;
    }

    public Monster Clone()
    {
        // 1. Shallow copy primitive and immutable fields (strings, ints)
        Monster copy = (Monster)this.MemberwiseClone();

        // 2. Deep copy the collection and each nested reference element
        copy.Skills = new List<Skill>(this.Skills);

        return copy;
    }

    public override string ToString()
    {
        var skillList = string.Join(", ", Skills);
        return $"{Name} [HP: {Health}, Weapon: {Weapon}] | Skills: [{skillList}]";
    }
}

#region gamemap
public class GameMap
{
    public string EnvironmentType { get; set; }
    public byte[] ElevationGrid { get; set; }       // Heavy in-memory array (e.g., 20 MB)
    public Dictionary<string, string> ConfigMeta { get; set; }

    public GameMap(string environmentType, string configFilePath)
    {
        EnvironmentType = environmentType;

        // Simulate expensive I/O and heavy parsing
        Console.WriteLine($"[Slow] Reading file '{configFilePath}' and parsing elevation grid...");
        System.Threading.Thread.Sleep(2000); // 2-second bottleneck per instance!

        ElevationGrid = new byte[1024 * 1024 * 20]; // 20 MB grid allocation
        ConfigMeta = new Dictionary<string, string>
        {
            { "Difficulty", "Hard" },
            { "Weather", "Rain" }
        };
    }
}

public class GameMap : ICloneable
{
    public string EnvironmentType { get; set; }
    public byte[] ElevationGrid { get; private set; }
    public Dictionary<string, string> SessionSettings { get; set; }

    // 1. Expensive constructor executed ONCE for the base template
    public GameMap(string environmentType, string configFilePath)
    {
        EnvironmentType = environmentType;
        Console.WriteLine($"[Startup] Parsing config '{configFilePath}' (expensive initialization)...");
        System.Threading.Thread.Sleep(1500); // Simulating heavy I/O / DB fetch

        ElevationGrid = new byte[1024 * 1024 * 10]; // Allocate 10 MB buffer
        SessionSettings = new Dictionary<string, string>
        {
            { "SpawnRate", "Normal" }
        };
    }

    // Private lightweight constructor used exclusively for cloning
    private GameMap() { }

    // 2. Clone: fast memory copy instead of re-reading disk/DB
    public object Clone()
    {
        var copy = new GameMap
        {
            EnvironmentType = this.EnvironmentType,
            
            // Fast in-memory array copy (Buffer.BlockCopy)
            ElevationGrid = new byte[this.ElevationGrid.Length]
        };
        Buffer.BlockCopy(this.ElevationGrid, 0, copy.ElevationGrid, 0, this.ElevationGrid.Length);

        // Deep copy mutable session settings
        copy.SessionSettings = new Dictionary<string, string>(this.SessionSettings);

        return copy;
    }
}
#endregion