namespace sample1.test;
using static sample1.Helper;
using static sample1.FibBenchmark;
public class UnitTest1
{
    [Theory]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    public void MemoizedFibTest(int input, int expected)
    {
        var memoizedFib = Fib.Memoize();
        Assert.Equal(expected, memoizedFib(input));
    }
    [Theory]
    [InlineData(5, 5)]
    [InlineData(6, 8)]
    public void FibTest(int input, int expected)
    {
        Assert.Equal(expected, Fib(input));
    }

    [Fact]
    public void FibBenchmarkTest()
    {
        FibBenchmark.Run();
    }
    [Theory]
    [InlineData(10_000_000_000_000)]
    public async Task When_ResponseNotReceivedWithinThreeSeconds_TimeoutOccurs(long counter)
    {
        // Given
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        var progress = new Progress<int>(percent => Console.WriteLine($"Progress: {percent}%"));
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await AsyncDemo.DoWorkAsync(counter, progress, cts.Token);
        });
        Assert.True(cts.IsCancellationRequested, "Cancellation token should be triggered after timeout.");
    }

    [Fact]
    public async Task Should_ReportProgressAndCompleteSuccessfully()
    {
        // Given
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        int percent = 0;
        var progress = new Progress<int>(p => percent = p);
        await AsyncDemo.DoWorkAsync(1000, progress, cts.Token);
        Assert.Equal(100, percent);
    }
}
