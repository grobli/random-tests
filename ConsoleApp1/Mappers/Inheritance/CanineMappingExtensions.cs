using ConsoleApp1.Dtos.Inheritance;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Inheritance;

public static class CanineMappingExtensions
{
    public static CanineDto ToDto(this Canine canine)
    {
        if (canine is Golden golden)
        {
            return golden.ToDto();
        }

        return new CanineDto
        {
            Name = canine.Name,
            Age = canine.Age,
            Race = canine.Race,
            Pups = canine.Pups.Select(p => p.ToDto()).ToArray(),
            Partner = canine.Partner?.ToDto()
        };
    }


    public static Canine ToModel(this CanineDto canineDto)
    {
        if (canineDto is GoldenDto goldenDto)
        {
            return goldenDto.ToModel();
        }

        return new Canine(canineDto.Race)
        {
            Name = canineDto.Name,
            Age = canineDto.Age,
            Pups = canineDto.Pups.Select(x => x.ToModel()).ToList(),
            Partner = canineDto.Partner?.ToModel()
        };
    }
}