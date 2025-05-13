using System.Text.Json.Serialization;

namespace ConsoleApp1.Dtos.Inheritance;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "@discriminator")]
[JsonDerivedType(typeof(AnimalDto), nameof(AnimalDto))]
[JsonDerivedType(typeof(CanineDto), nameof(CanineDto))]
[JsonDerivedType(typeof(GoldenDto), nameof(GoldenDto))]
public record AnimalDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "@discriminator")]
[JsonDerivedType(typeof(CanineDto), nameof(CanineDto))]
[JsonDerivedType(typeof(GoldenDto), nameof(GoldenDto))]
public record CanineDto : AnimalDto
{
    public required string Race { get; set; }
    public IReadOnlyCollection<CanineDto> Pups { get; set; } = [];
    public CanineDto? Partner { get; set; }
}

public record GoldenDto : CanineDto
{
    public required int HappinessLevel { get; set; }
}