using CalculatorLib;
using Grpc.Net.Client;
using GrpcCalculator;
using System.Diagnostics;

namespace PerformanceTests;

[TestClass]
public class Performance
{
    private Process myProcess;

    [TestInitialize]
    public void Initialize()
    {
        myProcess = new Process();
        myProcess.StartInfo.FileName = "Server.exe";
        myProcess.StartInfo.UseShellExecute = false;
        myProcess.Start();
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (!myProcess.HasExited)
        {
            myProcess.Kill();
        }
        myProcess.Dispose();
    }

    [TestMethod]
    public async Task MeasureSingleCallToGrpc()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5000");

        var gRPCClient = new Calculator.CalculatorClient(channel);
       
        await gRPCClient.GetTimeMessageAsync(new MessageRequest { Time = "21/01/2024 17:55:14" }, null);
    }

    [TestMethod]
    public void Measure100SyncCallToGrpc()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5000");

        var gRPCClient = new Calculator.CalculatorClient(channel);

        for (int i = 0; i < 100; i++)
        {
            gRPCClient.Sum(new SumRequest { A = 4566, B = i }, null);
        }
    }

    [TestMethod]
    public async Task Measure100AsyncCallToGrpc()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:5000");

        var gRPCClient = new Calculator.CalculatorClient(channel);

        for (int i = 0; i < 100; i++)
        {
            await gRPCClient.SumAsync(new SumRequest { A = 4566, B = i }, null);
        }
    }

    [TestMethod]
    public async Task Measure100CallsToClassLibrary()
    {
        var calulatorLib = new CalculatorLibClass();
        for (int i = 0; i < 100; i++)
        {
            await calulatorLib.SummarizeAsync(4566, i);
        }
    }
}