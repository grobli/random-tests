namespace ConsoleApp1.Dtos.Flat;

public sealed record CanineFlatDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Race { get; set; }
    public CanineOneOf? Partner { get; set; }
    public required IReadOnlyCollection<CanineOneOf> Pups { get; set; }
}

public sealed record CanineOneOf
{
    public CanineFlatDto? Canine { get; set; }
    public GoldenFlatDto? Golden { get; set; }
}