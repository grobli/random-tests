namespace ConsoleApp1.Dtos.Flat;

public sealed record CanineFlatDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Race { get; set; }
    public PartnerOneOf? Partner { get; set; }
    public required PupsCollection Pups { get; set; }

    public sealed record PupsCollection
    {
        public required IReadOnlyCollection<CanineFlatDto> Canine { get; set; }
        public required IReadOnlyCollection<GoldenFlatDto> Golden { get; set; }
    }

    public sealed record PartnerOneOf
    {
        public CanineFlatDto? Canine { get; set; }
        public GoldenFlatDto? Golden { get; set; }
    }
}