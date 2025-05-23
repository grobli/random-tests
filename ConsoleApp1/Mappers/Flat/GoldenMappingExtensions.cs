using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Flat;

public static class GoldenMappingExtensions
{
    public static GoldenFlatDto ToFlatDto(this Golden golden)
    {
        return new GoldenFlatDto
        {
            Name = golden.Name,
            Age = golden.Age,
            Race = golden.Race,
            Pups = golden.Pups.ToCanineOneOfCollection(),
            Partner = golden.Partner?.ToCanineOneOf(),
            HappinessLevel = golden.HappinessLevel
        };
    }


    public static Golden ToModel(this GoldenFlatDto goldenFlatDto)
    {
        return new Golden
        {
            Name = goldenFlatDto.Name,
            Age = goldenFlatDto.Age,
            HappinessLevel = goldenFlatDto.HappinessLevel,
            Pups = goldenFlatDto.Pups.FromCanineOneOfCollection(),
            Partner = goldenFlatDto.Partner?.FromCanineOneOf()
        };
    }
}