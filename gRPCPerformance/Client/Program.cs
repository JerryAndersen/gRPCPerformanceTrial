using Grpc.Net.Client;
using GrpcCalculator;

Console.WriteLine("Press any key to continue");
Console.ReadKey();

using var channel = GrpcChannel.ForAddress("https://localhost:5000");

var client = new GrpcCalculator.Calculator.CalculatorClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Client" }, null);

Console.WriteLine($"Message from service: {reply.Message}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();