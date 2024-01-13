namespace CalculatorLib;

public class CalculatorLibClass
{
    public Task<int> SummarizeAsync(int a, int b)
    {
        return Task.FromResult(a + b);
    }

    public int Summarize(int a, int b)
    {
        return a + b;
    }
}
