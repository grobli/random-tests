using ConsoleApp1.Dtos.Inheritance;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Inheritance;

public static class GoldenMappingExtensions
{
    public static GoldenDto ToDto(this Golden golden)
    {
        return new GoldenDto
        {
            Name = golden.Name,
            Age = golden.Age,
            Race = golden.Race,
            Pups = golden.Pups.Select(x => x.ToDto()).ToList(),
            Partner = golden.Partner?.ToDto(),
            HappinessLevel = golden.HappinessLevel
        };
    }


    public static Golden ToModel(this GoldenDto goldenDto)
    {
        return new Golden
        {
            Name = goldenDto.Name,
            Age = goldenDto.Age,
            HappinessLevel = goldenDto.HappinessLevel,
            Pups = goldenDto.Pups.Select(x => x.ToModel()).ToList(),
            Partner = goldenDto.Partner?.ToModel()
        };
    }
}