using Grpc.Core;
using GrpcCalculator;
using Microsoft.Extensions.Logging;
using static GrpcCalculator.Calculator;

namespace Server.Math;

public class CalculatorService : CalculatorBase
{
    private readonly ILogger<CalculatorService> _logger;
    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger;
    }

    public override Task<MessageReply> GetTimeMessage(MessageRequest request, ServerCallContext context)
    {
        return Task.FromResult(new MessageReply
        {
            Message = "The clock is: " + request.Time
        });
    }

    public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SumResponse
        {
            Result = request.A + request.B
        });
    }
}
