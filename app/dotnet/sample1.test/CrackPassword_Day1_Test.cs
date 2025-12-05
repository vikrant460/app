using sample1.AdventOfCode.QUIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample1.test
{
    public class CrackPassword_Day1_Test
    {
        [Theory]
        [MemberData(nameof(_testLeftMove))]
        public async Task CrackAsyncTest(List<Move> moves, int maxLockNumber, int initialLockArrowPosition, int expectedPassword)
        {
            var cracker = new CrackPassword(maxLockNumber, initialLockArrowPosition);
            var result = await cracker.CrackPasswordV1(moves);
            Assert.Equal(expectedPassword, result);
        }
        public static IEnumerable<object[]> _testLeftMove = new List<object[]>
        {
            new object[] { new List<Move> { new Move(Direction.Left, 1) }, 2, 0, 0 },
            new object[] { new List<Move> { new Move(Direction.Left, 2) }, 2, 0, 0 },
            new object[] { new List<Move> { new Move(Direction.Left, 3) }, 2, 0, 1 },
            new object[] { new List<Move> { new Move(Direction.Left, 7) }, 2, 1, 3 },
            new object[] { new List<Move> { new Move(Direction.Left, 8) }, 2, 1, 2 },
        };

        [Fact]
        public void RunBenchmark()
        {
            MyBenchmark.Run();
        }
    }
}
