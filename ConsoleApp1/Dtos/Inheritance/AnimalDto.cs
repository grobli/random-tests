namespace ConsoleApp1.Dtos.Inheritance;

public record AnimalDto
{
	public required string Name { get; set; }
	public required int Age { get; set; }
}

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