using ConsoleApp1.Dtos;
using ConsoleApp1.Dtos.Inheritance;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Inheritance;

public static class AnimalMappingExtensions
{
    public static AnimalDto ToDto(this Animal animal)
    {
        if (animal is Canine canine)
        {
            return canine.ToDto();
        }

        return new AnimalDto
        {
            Name = animal.Name,
            Age = animal.Age
        };
    }

    public static Animal ToModel(this AnimalDto animalDto)
    {
        if (animalDto is CanineDto canineDto)
        {
            return canineDto.ToModel();
        }

        return new Animal
        {
            Name = animalDto.Name,
            Age = animalDto.Age
        };
    }
}