// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Mappers.Flat;
using ConsoleApp1.Mappers.Inheritance;
using ConsoleApp1.Models;

var animals = new AnimalsResponse();
var dtos = animals.Animals.Select(a => a.ToDto()).ToArray();
foreach (var animalDto in dtos)
{
	Console.WriteLine(animalDto);
}

var models = dtos.Select(d => d.ToModel()).ToArray();
foreach (var animal in models)
{
	Console.WriteLine(animal);
}

var responseDto = animals.ToDto();
var options = new JsonSerializerOptions
	{ WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
var json = JsonSerializer.Serialize(responseDto, options);
Console.WriteLine(json);

var response = responseDto.ToModel();
var responseJson = JsonSerializer.Serialize(response, options);
Console.WriteLine(responseJson);

public sealed record AnimalsResponse
{
	public List<Animal> Animals { get; set; } =
	[
		new() { Name = "Bird", Age = 1 },
		
	];
}

public sealed record AnimalsResponseDto
{
	public required AnimalsCollection Animals { get; set; }

	public sealed record AnimalsCollection
	{
		public required IReadOnlyCollection<AnimalFlatDto> Animal { get; set; }
		public required IReadOnlyCollection<CanineFlatDto> Canine { get; set; }
		public required IReadOnlyCollection<GoldenFlatDto> Golden { get; set; }
	}
}

public static class AnimalsResponseExtensions
{
	public static AnimalsResponseDto ToDto(this AnimalsResponse response)
	{
		return new AnimalsResponseDto
		{
			Animals = new AnimalsResponseDto.AnimalsCollection
			{
				Animal = response.Animals.Where(x => x.GetType() == typeof(Animal))
					.Select(a => a.ToFlatDto())
					.ToList(),
				Canine = response.Animals.Where(x => x.GetType() == typeof(Canine))
					.Cast<Canine>()
					.Select(x => x.ToFlatDto())
					.ToList(),
				Golden = response.Animals.Where(x => x.GetType() == typeof(Golden))
					.Cast<Golden>()
					.Select(x => x.ToFlatDto())
					.ToList()
			}
		};
	}

	public static AnimalsResponse ToModel(this AnimalsResponseDto dto)
	{
		return new AnimalsResponse
		{
			Animals = dto.Animals.Animal.Select(x => x.ToModel())
				.Concat(dto.Animals.Canine.Select(x => x.ToModel()))
				.Concat(dto.Animals.Golden.Select(x => x.ToModel()))
				.ToList()
		};
	}
}