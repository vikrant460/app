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

}
