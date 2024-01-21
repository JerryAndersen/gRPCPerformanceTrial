using Grpc.Net.Client;
using GrpcCalculator;
using System.Windows;
using System.Windows.Threading;

namespace WpfClient;

public partial class MainWindow : Window
{
    private readonly GrpcCalculator.Calculator.CalculatorClient? gRPCClient = null;
    Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

    public MainWindow()
    {
        InitializeComponent();
        var channel = GrpcChannel.ForAddress("https://localhost:5000");
        gRPCClient = new GrpcCalculator.Calculator.CalculatorClient(channel);
    }

    private void OnSyncCallClicked(object sender, EventArgs e)
    {
        syncTb.Text = gRPCClient!.GetTimeMessage(new MessageRequest { Time = DateTime.Now.ToString() }, null).Message;
    }

    private void OnAsyncCallClicked(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            var response = await gRPCClient!.GetTimeMessageAsync(new MessageRequest { Time = DateTime.Now.ToString() }, null);
            dispatcher.Invoke(() => asyncTb.Text = response.Message);
        });
    }
}