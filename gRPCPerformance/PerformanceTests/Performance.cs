using CalculatorLib;
using Grpc.Net.Client;
using GrpcCalculator;
using System.Diagnostics;
using static GrpcCalculator.Calculator;

namespace PerformanceTests;

[TestClass]
public class Performance
{
    private Process? serverProcess;
    private GrpcChannel? channel;
    private CalculatorClient? calculatorClient;

    [TestInitialize]
    public void Initialize()
    {
        serverProcess = new();
        serverProcess.StartInfo.FileName = "Server.exe";
        serverProcess.StartInfo.UseShellExecute = false;
        serverProcess.Start();
        channel = GrpcChannel.ForAddress("https://localhost:5000");
        calculatorClient = new(channel);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (!serverProcess!.HasExited)
        {
            serverProcess.Kill();
        }
        serverProcess.Dispose();
        //calculatorClient?.Dispose();
        channel?.Dispose();
    }

    [TestMethod]
    public async Task MeasureSingleCallToGrpc()
    {
        await calculatorClient!.GetTimeMessageAsync(new MessageRequest { Time = "21/01/2024 17:55:14" }, null);
    }

    [TestMethod]
    public void Measure100SyncCallToGrpc()
    {
        for (int i = 0; i < 100; i++)
        {
            calculatorClient!.Sum(new SumRequest { A = 4566, B = i }, null);
        }
    }

    [TestMethod]
    public async Task Measure100AsyncCallToGrpc()
    {
        for (int i = 0; i < 100; i++)
        {
            await calculatorClient!.SumAsync(new SumRequest { A = 4566, B = i }, null);
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