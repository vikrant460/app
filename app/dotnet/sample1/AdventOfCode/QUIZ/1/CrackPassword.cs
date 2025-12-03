using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;

namespace sample1.AdventOfCode.QUIZ
{
    [MemoryDiagnoser]
    public class CrackPassword
    {
        private readonly int _mod;

        public CrackPassword(int max)
        {
            _mod = max + 1;
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
        [Benchmark]
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

        [Benchmark]
        public async Task<int> CrackPasswordAsync(string filePath, int initialPosition)
        {
            int pos = initialPosition;
            int atZero = 0;

            using var reader = new StreamReader(filePath);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                char dir = line[0];
                int steps = int.Parse(line.AsSpan(1));

                if (dir == 'L')
                    pos = (pos - steps + _mod) % _mod;
                else
                    pos = (pos + steps) % _mod;

                if (pos == 0)
                    atZero++;
            }

            return atZero;

        }

        public int MoveRight(Move move, int currentPosition)
        {
            int currPosition = (currentPosition + move.Steps)  % _mod;
            return currPosition;
        }

        public int MoveLeft(Move move, int currentPosition) 
        {
            currentPosition = ((currentPosition - move.Steps) + _mod)  % _mod;
            return currentPosition;
        }
        public static void Run() => BenchmarkRunner.Run<CrackPassword>();
    }
    public record Move(Direction Direction, int Steps);
   
    public enum Direction
    {
        Left,
        Right,
    }
}
