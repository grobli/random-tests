namespace ConsoleApp1.Models;

public record Animal
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}

public record Canine : Animal
{
    public Canine? Partner { get; set; }
    public string Race { get; }
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