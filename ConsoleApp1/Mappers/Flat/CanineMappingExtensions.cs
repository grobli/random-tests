using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Models;

namespace ConsoleApp1.Mappers.Flat;

public static class CanineMappingExtensions
{
    public static CanineFlatDto ToFlatDto(this Canine canine)
    {
        return new CanineFlatDto
        {
            Name = canine.Name,
            Age = canine.Age,
            Race = canine.Race,
            Pups = canine.Pups.ToPupsCollectionDto(),
            Partner = canine.Partner?.ToPartnerOneOf()
        };
    }

    public static Canine ToModel(this CanineFlatDto canineFlatDto)
    {
        return new Canine(canineFlatDto.Race)
        {
            Name = canineFlatDto.Name,
            Age = canineFlatDto.Age,
            Pups = canineFlatDto.Pups.FromPupsCollectionDto(),
            Partner = canineFlatDto.Partner?.FromPartnerOneOf()
        };
    }

    internal static CanineFlatDto.PupsCollection ToPupsCollectionDto(this ICollection<Canine> pups)
    {
        return new CanineFlatDto.PupsCollection
        {
            Canine = pups.Where(x => x.GetType() == typeof(Canine))
                .Select(x => x.ToFlatDto())
                .ToList(),
            Golden = pups.Where(x => x.GetType() == typeof(Golden))
                .Cast<Golden>()
                .Select(x => x.ToFlatDto())
                .ToList()
        };
    }


    internal static List<Canine> FromPupsCollectionDto(this CanineFlatDto.PupsCollection pups)
    {
        return pups.Canine.Select(x => x.ToModel())
            .Concat(pups.Golden.Select(x => x.ToModel()))
            .ToList();
    }

    internal static CanineFlatDto.PartnerOneOf ToPartnerOneOf(this Canine canine)
    {
        if (canine.GetType() == typeof(Golden))
        {
            return new CanineFlatDto.PartnerOneOf { Golden = ((Golden)canine).ToFlatDto() };
        }

        return new CanineFlatDto.PartnerOneOf { Canine = canine.ToFlatDto() };
    }

    internal static Canine FromPartnerOneOf(this CanineFlatDto.PartnerOneOf partnerOneOf)
    {
        if (partnerOneOf.Canine is not null)
        {
            return partnerOneOf.Canine.ToModel();
        }

        if (partnerOneOf.Golden is not null)
        {
            return partnerOneOf.Golden.ToModel();
        }

        throw new InvalidOperationException("No value");
    }
}