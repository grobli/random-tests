// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Mappers.Flat;
using ConsoleApp1.Models;


var summary = BenchmarkRunner.Run<Benchmarks>();
return;


[MemoryDiagnoser]
public class Benchmarks
{
	private readonly ICollection<Canine> _testData = Enumerable.Repeat(new Canine("Grey Wolf")
	{
		Name = "Wolf", Age = 2, Pups = [new Golden { Name = "James", Age = 42, HappinessLevel = 69 }],
		Partner = new Golden { Name = "Isabel", Age = 18, HappinessLevel = 999 }
	}, 100).ToList();

	private readonly CanineFlatDto.PupsCollection _pupsCollectionTestData;

	public Benchmarks()
	{
		_pupsCollectionTestData = ToPupsCollectionDto(_testData);
	}

	//[Benchmark]
	//public CanineFlatDto.PupsCollection MapperWithSwitch() => ToPupsCollectionDto(_testData);

	//[Benchmark]
	//public CanineFlatDto.PupsCollection MapperWithLinq() => ToPupsCollectionDto(_testData);

	[Benchmark]
	public List<Canine> MapperWithListOptimizations() => FromPupsCollectionDto(_pupsCollectionTestData);
	
	[Benchmark]
	public List<Canine> MapperWithLinq() => FromPupsCollectionDtoLinq(_pupsCollectionTestData);

	public static List<Canine> FromPupsCollectionDto(CanineFlatDto.PupsCollection pupsCollection)
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

	public static List<Canine> FromPupsCollectionDtoLinq(CanineFlatDto.PupsCollection pupsCollection)
	{
		return Enumerable.Empty<Canine>()
			.Concat(pupsCollection.Canine?.Select(x => x.ToModel()) ?? [])
			.Concat(pupsCollection.Golden?.Select(x => x.ToModel()) ?? [])
			.ToList();
	}


	public static CanineFlatDto.PupsCollection ToPupsCollectionDto(ICollection<Canine> pups)
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

	public static CanineFlatDto.PupsCollection ToPupsCollectionDtoLinq(ICollection<Canine> pups)
	{
		if (pups.Count == 0)
		{
			return new CanineFlatDto.PupsCollection();
		}

		return new CanineFlatDto.PupsCollection
		{
			Canine = pups.Where(x => x.GetType().Name == nameof(Canine))
				.Select(x => x.ToFlatDto())
				.ToArray(),
			Golden = pups.Where(x => x.GetType().Name == nameof(Golden))
				.Cast<Golden>()
				.Select(x => x.ToFlatDto())
				.ToArray()
		};
	}
}