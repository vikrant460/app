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
        [Fact]
        public void CrackPasswordTest()
        {
            var cracker = new CrackPassword(0, 99);  
            var result = cracker.Crack("Input1.txt", 50);
            Assert.Equal(3, result);
        }
        [Fact]
        public void CrackPasswordTest2()
        {
            var cracker = new CrackPassword(0, 99);
            var result = cracker.Crack("Input2.txt", 50);
            Assert.Equal(1086, result);
        }

        [Theory]
        [InlineData(68, 50, 82)]
        public void MoveLeftTest(int steps, int currentposition, int newPosition)
        {
            var cracker = new CrackPassword(0, 99);  
            var currentPosition = cracker.MoveLeft(new Move(Direction.Left, steps), currentposition);
            Assert.Equal(newPosition, currentPosition);
        }
        [Theory]
        [InlineData(1, 99, 0)]
        public void MoveRightTest(int steps, int currentposition, int newPosition)
        {
            var cracker = new CrackPassword(0, 99);  
            var currentPosition = cracker.MoveRight(new Move(Direction.Right, steps), currentposition);
            Assert.Equal(newPosition, currentPosition);
        }
    }
}
