namespace  sample1;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using static sample1.Helper;
[MemoryDiagnoser]
public class LinqVsForeachBenchmark
{
    private List<double> numbers;
    private const long Max = 100_000_00_00;
    [GlobalSetup]
    public void Setup()
    {
        numbers = LongRange(1, Max).ToList();
    }

    [Benchmark]
    public double LinqVersion()
    {
        return numbers.Where(x => (long)(x % 2) == 0).Sum();
    }

    [Benchmark]
    public double ForeachVersion()
    {
        double sum = 0;
        foreach (var x in numbers)
        {
            if ((long)(x % 2) == 0)
                sum += x;
        }
        return sum;
    }

    public static void Run() => BenchmarkRunner.Run<LinqVsForeachBenchmark>();
}
