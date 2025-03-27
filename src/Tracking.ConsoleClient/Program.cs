
using Bogus;
using Grpc.Net.Client;
using System.Diagnostics;
using Tracking.Api;

Console.WriteLine("Hello, gRPC Client!");

const string url = "https://localhost:7266";

var channel = GrpcChannel.ForAddress(url);
var client = new Tracking.Api.TrackingService.TrackingServiceClient(channel);

// dotnet add package Bogus
var faker = new Faker<AddLocationRequest>()
    .RuleFor(p => p.PlateNumber, f => f.Vehicle.Vin())
    .RuleFor(p => p.Latitude, f => f.Random.Float(50, 51))
    .RuleFor(p => p.Longitude, f => f.Random.Float(27, 28))
    .RuleFor(p=>p.Speed, f=>f.Random.Int(0, 140));

var locations = faker.GenerateForever();

Stopwatch stopwatch = new Stopwatch();

foreach(var location in locations)
{
    Console.WriteLine($"Send request {location.PlateNumber} ({location.Latitude},{location.Longitude}) {location.Speed} km/h");

    stopwatch.Restart();

    await client.AddLocationAsync(location);

    stopwatch.Stop();

    Console.WriteLine($"Sent in {stopwatch.ElapsedMilliseconds}ms");

    await Task.Delay(1000);

}

Console.WriteLine("Press any key to exit.");

Console.ReadKey();


