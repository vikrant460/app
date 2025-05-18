
using Extensions;
using static System.Linq.Enumerable;

IEnumerable<int> input = Range(1, 5);
input.Print("Input");
input.Triple().Print("Tripled");
input.Odds().Print("Odds");

Func<int, int, int> divideBy = (a, b) => a / b;
var result = divideBy.SwapArgs();
result(2, 10).Print("Divide 10 by 2");


var divisor = 3;
Func<int, bool> IsMod(int n) => (i) => i % n == 0;
Range(1, 20).Where(IsMod(3)).Print($"Divisible by {divisor}");
Range(1, 20).Where(IsMod(3).Not()).Print($"Not Divisible by {divisor}");







