namespace sample1;

public static class Helper
{
    public static IEnumerable<double> LongRange(double start, double count)
    {
        var end = start + count;
        for (var i = start; i < end; i++)
        {
            yield return i;
        }
    }
    public static Func<int, int> Fact => (n) => n switch
    {
        1 => 1,
        > 1 => n * Fact(n - 1),
        _ => throw new NotSupportedException($"{n}")
    };

    public static Func<int, int> Fib => (n) => n switch
    {
        0 => 0,
        1 => 1,
        >= 2 => Fib(n - 1) + Fib(n - 2),
        _ => throw new NotSupportedException($"{n}")
    };
    public static Func<int, int, int, int> AddThree => (a, b, c) => a + b + c;
    public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) => (arg1) => (arg2) => (arg3) => func(arg1, arg2, arg3);
    public static Func<TInput, TResult> Memoize<TInput, TResult>(this Func<TInput, TResult> func)
    {
        // create cache ("memo")
        var memo = new Dictionary<TInput, TResult>();

        // wrap provided function with cache handling
        return input =>
        {
            // check if result for set input was already cached
            if (memo.TryGetValue(input, out var fromMemo))
                // if yes, return value
                return fromMemo;

            // if no, call function
            var result = func(input);

            // cache the result
            memo.Add(input, result);

            // return result
            return result;
        };
    }

}
