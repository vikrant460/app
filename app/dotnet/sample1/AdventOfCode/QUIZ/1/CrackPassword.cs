using Microsoft.CodeAnalysis;

namespace sample1.AdventOfCode.QUIZ
{
    public class CrackPassword
    {
        private readonly int MaxPosition = 99;

        public CrackPassword(int min, int max)
        {
            MaxPosition = max;
        }
        private async Task<List<Move>> GetMove(string filePath)
        {
            var getLinesFromFile = await File.ReadAllLinesAsync(filePath);
            var moves = getLinesFromFile.Select(line =>
            {
                var direction = line[0] == 'L' ? Direction.Left : Direction.Right;
                var moveCount = int.Parse(line.Substring(1));
                return new Move(direction, moveCount);
            }).ToList();
            return moves;
        }
        public async Task<int> CrackAsync(string filePath, int initialPosition)
        {
           int currentPosition = initialPosition;
           int atZeroCount = 0;
            var moves = await GetMove(filePath);
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
