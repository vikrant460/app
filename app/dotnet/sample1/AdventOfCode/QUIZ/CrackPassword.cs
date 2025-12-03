using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample1.AdventOfCode.QUIZ
{
    public class CrackPassword
    {
        private readonly int MinPosition = 0;
        private readonly int MaxPosition = 99;
        const int initialPosition = 50;
        public CrackPassword(int min, int max)
        {
            MinPosition = min; 
            MaxPosition = max;
        }
        private List<Move> GetMove(string filePath)
        {
            var getLinesFromFile = System.IO.File.ReadAllLines(filePath);
            var moves = getLinesFromFile.Select(line =>
            {
                var direction = line[0] == 'L' ? Direction.Left : Direction.Right;
                var moveCount = int.Parse(line.Substring(1));
                return new Move(direction, moveCount);
            }).ToList();
            return moves;
        }
        public int Crack(string filePath, int initialPosition)
        {
           int currentPosition = initialPosition;
           int atZeroCount = 0;
            var moves = GetMove(filePath);
           foreach (var move in moves)
           {
                if (move.Direction == Direction.Left)
                {
                    currentPosition = MoveLeft(move, currentPosition);
                    atZeroCount = currentPosition == 0 ? atZeroCount + 1 : atZeroCount;
                }
                else
                {
                    currentPosition = MoveRight(move, currentPosition);
                    atZeroCount = currentPosition == 0 ? atZeroCount + 1 : atZeroCount;
                }
            }
            return atZeroCount;
        }

        public int MoveRight(Move move, int currentPosition)
        {
            int currPosition = (currentPosition + move.Steps)  %  (MaxPosition + 1);
            return currPosition;
        }

        public int MoveLeft(Move move, int currentPosition) 
        {
            currentPosition = ((currentPosition - move.Steps) + (MaxPosition + 1)) % (MaxPosition + 1);
            return currentPosition;
        }
        
    }
    public record Move(Direction Direction, int Steps);
   
    public enum Direction
    {
        Left,
        Right,
    }
}
