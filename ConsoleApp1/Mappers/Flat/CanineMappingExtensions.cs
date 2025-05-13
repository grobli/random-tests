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
			Pups = canine.Pups.ToCanineOneOfCollection(),
			Partner = canine.Partner?.ToCanineOneOf()
		};
	}

	public static Canine ToModel(this CanineFlatDto canineFlatDto)
	{
		return new Canine(canineFlatDto.Race)
		{
			Name = canineFlatDto.Name,
			Age = canineFlatDto.Age,
			Pups = canineFlatDto.Pups.FromCanineOneOfCollection(),
			Partner = canineFlatDto.Partner?.FromCanineOneOf()
		};
	}
	
	internal static CanineOneOf ToCanineOneOf(this Canine canine)
	{
		if (canine.GetType() == typeof(Canine))
		{
			return new CanineOneOf { Canine = canine.ToFlatDto() };
		}

		if (canine.GetType() == typeof(Golden))
		{
			return new CanineOneOf { Golden = ((Golden)canine).ToFlatDto() };
		}

		throw new InvalidOperationException("Missing conversion for type: " + canine.GetType());
	}
	
	internal static CanineOneOf[] ToCanineOneOfCollection(this IEnumerable<Canine> canine)
	{
		return canine.Select(x => x.ToCanineOneOf()).ToArray();
	}

	internal static Canine FromCanineOneOf(this CanineOneOf canineOneOf)
	{
		if (canineOneOf.Canine is not null)
		{
			return canineOneOf.Canine.ToModel();
		}

		if (canineOneOf.Golden is not null)
		{
			return canineOneOf.Golden.ToModel();
		}

		throw new InvalidOperationException("No value");
	}

	internal static List<Canine> FromCanineOneOfCollection(this IEnumerable<CanineOneOf> canineOneOf)
	{
		return canineOneOf.Select(x => x.FromCanineOneOf()).ToList();
	}
}