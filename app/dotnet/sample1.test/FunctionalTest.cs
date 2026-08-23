namespace sample1.test;
using static sample1.Helper;
using static sample1.MyBenchmark;
public class UnitTest1
{
    // [Theory]
    // [InlineData(5, 5)]
    // [InlineData(6, 8)]
    // public void MemoizedFibTest(int input, int expected)
    // {
    //     var memoizedFib = Fib.Memoize();
    //     Assert.Equal(expected, memoizedFib(input));
    // }
    // [Theory]
    // [InlineData(5, 5)]
    // [InlineData(6, 8)]
    // public void FibTest(int input, int expected)
    // {
    //     Assert.Equal(expected, Fib(input));
    // }

    //[Fact]
    //public void FibBenchmarkTest()
    //{
    //    FibBenchmark.Run();
    //}
    // [Theory]
    // [InlineData(10_000_000_000_000)]
    // public async Task When_ResponseNotReceivedWithinThreeSeconds_TimeoutOccurs(long counter)
    // {
    //     // Given
    //     using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
    //     var progress = new Progress<int>(percent => Console.WriteLine($"Progress: {percent}%"));
    //     await Assert.ThrowsAsync<OperationCanceledException>(async () =>
    //     {
    //         await AsyncDemo.DoWorkAsync(counter, progress, cts.Token);
    //     });
    //     Assert.True(cts.IsCancellationRequested, "Cancellation token should be triggered after timeout.");
    // }

    //[Fact]
    //public async Task Should_ReportProgressAndCompleteSuccessfully()
    //{
    //    // Given
    //    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
    //    int percent = 0;
    //    var progress = new Progress<int>(p => percent = p);
    //    await AsyncDemo.DoWorkAsync(1000, progress, cts.Token);
    //    Assert.Equal(100, percent);
    //}
    // [Fact]
    // public void StringInterningTest()
    // {
    //     string x = "hello";
    //     string y = "he" + "llo";
    //     string z = string.Concat("he", "llo");// created at runtime, not interned
    //     string w = string.IsInterned(z); // 
    //     Assert.True(ReferenceEquals(y, x));
    //     Assert.False(ReferenceEquals(z, y));
    //     Assert.False(ReferenceEquals(z, x));
    //     Assert.True(ReferenceEquals(w, y));
    //     Assert.Equal(z, y);
        
    // }
    // [Fact]
    // public void TestStatic()
    // {
    //     var s = new StaticFun();
    //     Assert.Equal(0, StaticFun.x);
    //     Assert.Equal(6, StaticFun.y);
    // }
   [Fact]
   public void TestGroup()
    {
        var data = new List<Record>
        {
           new Record { Id = 1, IsActive = 1, Name = "A", Foo = 11, Bar = 123 },
           new Record { Id = 2, IsActive = 1, Name = "A", Foo = 11, Bar = 123 },
           new Record { Id = 3, IsActive = 1, Name = "A", Foo = 11, Bar = 456 },
           new Record { Id = 4, IsActive = 1, Name = "B", Foo = 22, Bar = 321 }
        };

        // var fooDistinct = data
        // .Where(x=> x.Foo != null)
        // .Select(x=>x.Foo)
        // .Distinct().Count();
        
        // var barDistinct = data
        // .Where(x=>x.Bar != null)
        // .Select(x=>x.Bar)
        // .Distinct()
        // .Count();
        
        // Assert.Equal(1, fooDistinct);
        // Assert.Equal(2, barDistinct);
        
        var group = data.Where(x => x.Foo != null && x.Bar != null)
        .GroupBy(d => d.Name)
        .Select(g => new 
        {
             fooDistinct = g.Select(x=>x.Foo).Distinct().Count(),
             barDistinct = g.Select(x=>x.Bar).Distinct().Count()
        }).FirstOrDefault();


        Assert.Equal(1, group?.fooDistinct);
        Assert.Equal(2, group?.barDistinct);
    }

}
public class Record
{
    public int Id { get; set; }
    public int IsActive { get; set; }
    public string Name { get; set; }
    public int? Foo { get; set; }
    public int? Bar { get; set; }
}
public class StaticFun
{
    public static int y;
    public static int x = y;
    
    static StaticFun()
    {
        y = 5;    
    }
    public StaticFun()
    {
        y = 6;  
    }
}
