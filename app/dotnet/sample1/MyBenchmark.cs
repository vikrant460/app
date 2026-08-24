namespace sample1;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
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

    //[Benchmark]
    //public async Task<int> CrackPasswordWithLinq()
    //{
    //    return await new CrackPassword(99).CrackPasswordV1("Input2.txt", 50);
    //}
    //[Benchmark]
    //public int CrackPasswordWithSpan()
    //{
    //    return new CrackPassword(99).CrackPasswordV2("Input2.txt", 50);
    //}

    public static void Run() => BenchmarkRunner.Run<MyBenchmark>();
}