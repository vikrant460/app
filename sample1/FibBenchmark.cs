namespace sample1;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using static sample1.Helper;
[MemoryDiagnoser]
public class FibBenchmark
{
    [Params(6)]
     public int Input;
    [Benchmark]
    public int NonMemoized_Fib()
    {
         return Fib(Input);
    }

    // [Benchmark]
    // public int Memoized_Fib()
    // {
    //     var fib = Fib.Memoize();
    //     return fib(num);
    // }

    public static void Run() => BenchmarkRunner.Run<FibBenchmark>();
}