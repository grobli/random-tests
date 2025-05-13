using System.Text.Json.Serialization;

namespace ConsoleApp1.Models;

[JsonDerivedType(typeof(Canine), nameof(Canine))]
[JsonDerivedType(typeof(Golden), nameof(Golden))]
public record Animal
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}

public record Canine : Animal
{
    public string Race { get; }
    public Canine? Partner { get; set; }
    public IList<Canine> Pups { get; set; } = [];

    public Canine(string race)
    {
        Race = race;
    }
}

public record Golden() : Canine("Golden Retriever")
{
    public int HappinessLevel { get; set; }
}