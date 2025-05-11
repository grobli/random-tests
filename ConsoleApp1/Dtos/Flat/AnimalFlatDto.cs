namespace ConsoleApp1.Dtos.Flat;

public sealed record AnimalFlatDto
{
	public required string Name { get; set; }
	public required int Age { get; set; }
}