using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsWPF;
using static System.Console;
namespace Extensions
{
    /// <summary>
    /// Higher order functions are ones which take another function as argument
    /// </summary>
    public static class HighterOrderFunctionExtensions
    {
        public static Func<TArg2, TArg1, TResult> SwapArgs<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> func) => (arg2, arg1) => func(arg1, arg2);
        public static Func<T, bool> Not<T>(this Func<T, bool> predicate) => (p) => !predicate(p);
    }
    public static class EnumerableExtensions
    {
        public static IEnumerable<int> Odds(this IEnumerable<int> input)
        {
            var odds = input.Where(x => x.IsOdd());
            return odds;
        }

        public static IEnumerable<int> Triple(this IEnumerable<int> input)
        {
            var result = input.Select(i => i * 3);
            return result;
        }
    }
    public static class NumberExtensions
    {
        public static bool IsOdd(this int number) => number % 2 == 1;
    }
    public static class PrintExtensions
    {
        public static void Print<T>(this IEnumerable<T> values, string info = "")
        {
            WriteLine($"{info}: {string.Join(" ", values.Select(x => x) ?? Enumerable.Empty<T>())}");
        }
        public static void Print<T>(this T? value, string info = "")
        {
            WriteLine($"{info}: {value ?? default}");
        }
    }
}
