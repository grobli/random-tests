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
			Partner = canine.Partner.ToPartnerOneOf()
		};
	}

	public static Canine ToModel(this CanineFlatDto canineFlatDto)
	{
		return new Canine(canineFlatDto.Race)
		{
			Name = canineFlatDto.Name,
			Age = canineFlatDto.Age,
			Pups = canineFlatDto.Pups.FromPupsCollectionDto(),
			Partner = canineFlatDto.Partner.FromPartnerOneOf()
		};
	}

	internal static CanineFlatDto.PupsCollection ToPupsCollectionDto(this ICollection<Canine> pups)
	{
		if (pups.Count == 0)
		{
			return new CanineFlatDto.PupsCollection();
		}

		var canineEnumerable = Enumerable.Empty<CanineFlatDto>();
		var goldenEnumerable = Enumerable.Empty<GoldenFlatDto>();

		foreach (var item in pups)
		{
			switch (item.GetType().Name)
			{
				case nameof(Canine):
					canineEnumerable = canineEnumerable.Append(item.ToFlatDto());
					break;
				case nameof(Golden):
					goldenEnumerable = goldenEnumerable.Append(((Golden)item).ToFlatDto());
					break;
			}
		}

		return new CanineFlatDto.PupsCollection
		{
			Canine = canineEnumerable.ToArray(),
			Golden = goldenEnumerable.ToArray()
		};
	}


	internal static List<Canine> FromPupsCollectionDto(this CanineFlatDto.PupsCollection pupsCollection)
	{
		var enumerable = Enumerable.Empty<Canine>();
		var count = 0;

		if (pupsCollection.Canine != null)
		{
			count += pupsCollection.Canine.Count;
			enumerable = enumerable.Concat(pupsCollection.Canine.Select(x => x.ToModel()));
		}

		if (pupsCollection.Golden != null)
		{
			count += pupsCollection.Golden.Count;
			enumerable = enumerable.Concat(pupsCollection.Golden.Select(x => x.ToModel()));
		}

		var list = new List<Canine>(count);
		list.AddRange(enumerable);

		return list;
	}

	internal static CanineFlatDto.PartnerOneOf ToPartnerOneOf(this Canine? canine)
	{
		if (canine == null)
		{
			return new CanineFlatDto.PartnerOneOf();
		}

		if (canine.GetType() == typeof(Golden))
		{
			return new CanineFlatDto.PartnerOneOf { Golden = ((Golden)canine).ToFlatDto() };
		}

		return new CanineFlatDto.PartnerOneOf { Canine = canine.ToFlatDto() };
	}

	internal static Canine? FromPartnerOneOf(this CanineFlatDto.PartnerOneOf partnerOneOf)
	{
		if (partnerOneOf.Canine != null)
		{
			return partnerOneOf.Canine.ToModel();
		}

		return partnerOneOf.Golden?.ToModel();
	}
}