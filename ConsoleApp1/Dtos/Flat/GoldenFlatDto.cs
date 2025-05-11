namespace ConsoleApp1.Dtos.Flat;

public sealed record GoldenFlatDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Race { get; set; }
    public CanineFlatDto.PartnerOneOf? Partner { get; set; }
    public required CanineFlatDto.PupsCollection Pups { get; set; }
    public required int HappinessLevel { get; set; }
}