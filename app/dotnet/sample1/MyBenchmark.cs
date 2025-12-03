namespace sample1;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using sample1.AdventOfCode.QUIZ;
using static sample1.Helper;
[MemoryDiagnoser]
[InProcess]
public class MyBenchmark
{
    //[Params(6)]
    // public int Input;
    //[Benchmark]
    //public int NonMemoized_Fib()
    //{
    //     return Fib(Input);
    //}

    [Benchmark]
    public async Task<int> CrackPassword()
    {
        return await new CrackPassword(99).CrackAsync("Input2.txt", 50);
    }
    // [Benchmark]
    // public int Memoized_Fib()
    // {
    //     var fib = Fib.Memoize();
    //     return fib(num);
    // }

    public static void Run() => BenchmarkRunner.Run<MyBenchmark>();
}