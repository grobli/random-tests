using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Flat;

public static class AnimalMappingExtensions
{
	public static AnimalFlatDto ToFlatDto(this Animal animal)
	{
		return new AnimalFlatDto
		{
			Name = animal.Name,
			Age = animal.Age
		};
	}

	public static Animal ToModel(this AnimalFlatDto animalFlatDto)
	{
		return new Animal
		{
			Name = animalFlatDto.Name,
			Age = animalFlatDto.Age
		};
	}
}