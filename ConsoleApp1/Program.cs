// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleApp1.Dtos.Flat;
using ConsoleApp1.Mappers.Flat;
using ConsoleApp1.Mappers.Inheritance;
using ConsoleApp1.Models;

var jsonSerializerOptions = new JsonSerializerOptions
	{ WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

var animals = new AnimalsResponse();
var dtos = animals.Animals.Select(a => a.ToDto()).ToArray();
Console.WriteLine(JsonSerializer.Serialize(dtos, jsonSerializerOptions));


// var models = dtos.Select(d => d.ToModel()).ToArray();
// JsonSerializer.Serialize(models, jsonSerializerOptions);

var responseDto = animals.ToDto();

var json = JsonSerializer.Serialize(responseDto, jsonSerializerOptions);
Console.WriteLine(json);

var response = responseDto.ToModel();
var responseJson = JsonSerializer.Serialize(response, jsonSerializerOptions);
Console.WriteLine(responseJson);

public sealed record AnimalsResponse
{
	public List<Animal> Animals { get; set; } =
	[
		new() { Name = "Bird", Age = 1 },
		new Canine("Grey Wolf")
		{
			Name = "Wolf", Age = 2, Pups = [new Golden { Name = "James", Age = 42, HappinessLevel = 69 }],
			Partner = new Golden { Name = "Isabel", Age = 18, HappinessLevel = 999 }
		},
		new Golden
		{
			Name = "Bobby", Age = 4, HappinessLevel = 1,
			Partner = new Canine("Bulldog") { Name = "Lucas", Age = 22 }
		}
	];
}

public sealed record AnimalsResponseDto
{
	public required IReadOnlyCollection<AnimalOneOf> Animals { get; set; }
}

public sealed record AnimalOneOf
{
	public AnimalFlatDto? Animal { get; set; }
	public CanineFlatDto? Canine { get; set; }
	public GoldenFlatDto? Golden { get; set; }
}

public static class AnimalsResponseExtensions
{
	public static AnimalOneOf ToAnimalOneOf(this Animal animal)
	{
		if (animal.GetType() == typeof(Animal))
		{
			return new AnimalOneOf { Animal = animal.ToFlatDto() };
		}

		if (animal.GetType() == typeof(Canine))
		{
			return new AnimalOneOf { Canine = ((Canine)animal).ToFlatDto() };
		}

		if (animal.GetType() == typeof(Golden))
		{
			return new AnimalOneOf { Golden = ((Golden)animal).ToFlatDto() };
		}

		throw new Exception("Unknown animal type");
	}

	public static Animal FromAnimalOneOf(this AnimalOneOf animal)
	{
		if (animal.Animal is not null)
		{
			return animal.Animal.ToModel();
		}

		if (animal.Canine is not null)
		{
			return animal.Canine.ToModel();
		}

		if (animal.Golden is not null)
		{
			return animal.Golden.ToModel();
		}

		throw new Exception("OneOf has no value set!");
	}

	public static AnimalOneOf[] ToAnimalOneOfArray(this IEnumerable<Animal> animals)
	{
		return animals.Select(a => a.ToAnimalOneOf()).ToArray();
	}

	public static List<Animal> FromAnimalOneOfArray(this IEnumerable<AnimalOneOf> animals)
	{
		return animals.Select(x => x.FromAnimalOneOf()).ToList();
	}


	public static AnimalsResponseDto ToDto(this AnimalsResponse response)
	{
		return new AnimalsResponseDto
		{
			Animals = response.Animals.ToAnimalOneOfArray()
		};
	}

	public static AnimalsResponse ToModel(this AnimalsResponseDto dto)
	{
		return new AnimalsResponse
		{
			Animals = dto.Animals.FromAnimalOneOfArray()
		};
	}
}