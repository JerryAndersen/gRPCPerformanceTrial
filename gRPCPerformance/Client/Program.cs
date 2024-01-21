using CalculatorLib;
using Grpc.Net.Client;
using GrpcCalculator;
using System.Diagnostics;

Console.WriteLine("Press any key to begin masure gRPC performance vs class library");
Console.ReadKey();

using var channel = GrpcChannel.ForAddress("https://localhost:5000");

var gRPCClient = new GrpcCalculator.Calculator.CalculatorClient(channel);
var calulatorLib = new CalculatorLibClass();

Stopwatch stopwatch = new();

stopwatch.Start();
var reply = await gRPCClient.GetTimeMessageAsync(new MessageRequest { Time = "Client" }, null);
stopwatch.Stop();
Console.WriteLine("");
Console.WriteLine($"Time to open gRPC channel: {stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine("");

stopwatch.Reset();
stopwatch.Start();
for (int i = 0; i < 100; i++)
{
    await gRPCClient.SumAsync(new SumRequest { A = 4566, B = i }, null);
}
stopwatch.Stop();
Console.WriteLine($"Sum was calculated by gRPC (100) in: {stopwatch.ElapsedMilliseconds}ms");

stopwatch.Reset();
stopwatch.Start();
for (int i = 0; i < 1000; i++)
{
    await gRPCClient.SumAsync(new SumRequest { A = 4566, B = i }, null);
}
stopwatch.Stop();
Console.WriteLine($"Sum was calculated by gRPC (1000) in: {stopwatch.ElapsedMilliseconds}ms");

stopwatch.Reset();
stopwatch.Start();
for (int i = 0; i < 1000; i++)
{
    await calulatorLib.SummarizeAsync(4566, i);
}
stopwatch.Stop();
Console.WriteLine($"Sum was calculated by class library (1000)(async) in: {stopwatch.ElapsedMilliseconds}ms");


stopwatch.Reset();
stopwatch.Start();
for (int i = 0; i < 1000; i++)
{
    calulatorLib.Summarize(4566, i);
}
stopwatch.Stop();
Console.WriteLine($"Sum was calculated by class library (1000)(sync) in: {stopwatch.ElapsedMilliseconds}ms");



Console.WriteLine("Press any key to exit...");
Console.ReadKey();