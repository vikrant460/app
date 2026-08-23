using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis;

namespace sample1.AdventOfCode.QUIZ
{
    [MemoryDiagnoser]
    public class CrackPassword
    {
        private readonly int _mod;
        private readonly int _initialPosition = 0;
        public CrackPassword(int max, int initialPosition)
        {
            _mod = max + 1;
            _initialPosition = initialPosition;
        }
        public async Task<List<Move>> GetMove(string filePath)
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
        public async Task<int> CrackPasswordV1(List<Move> moves)
        {
           int currentPosition = _initialPosition;
           int atZeroCount = 0;
           foreach (var move in moves)
           { 
                var fullmoves = move.Steps / _mod;
                var remainingMOves = move.Steps % _mod;
                if (move.Direction == Direction.Left)
                {
                    int initialPosition = currentPosition;
                    currentPosition = MoveLeft(new Move(move.Direction, remainingMOves), currentPosition);
                    if(currentPosition == initialPosition)
                    {
                        if (initialPosition == 0)
                        {
                            atZeroCount += fullmoves;

                        }
                        else
                        {
                            atZeroCount += fullmoves + 1;
                        }
                    }
                    else
                    {
                        if(currentPosition == 0)
                        {
                            atZeroCount += fullmoves + 1;
                        }
                        else if (currentPosition > initialPosition)
                        {
                            if (initialPosition == 0)
                            {
                                atZeroCount += fullmoves;
                            }
                            else
                            {
                                atZeroCount += fullmoves + 1;
                            }
                        }
                        else
                        {
                            atZeroCount += fullmoves;
                        }
                    }

                }
                else
                {
                    int initialPosition = currentPosition;
                    currentPosition = MoveRight(new Move(move.Direction, remainingMOves), currentPosition);
                    if (currentPosition == initialPosition)
                    {
                        if (initialPosition == 0)
                        {
                            atZeroCount += fullmoves;
                        }
                        else
                        {
                            atZeroCount += fullmoves + 1;
                        }
                    }
                    else
                    {
                        if (currentPosition == 0)
                        {
                            atZeroCount += fullmoves + 1;
                        }
                        else if (currentPosition < initialPosition)
                        {
                            if (initialPosition == 0)
                            {
                                atZeroCount += fullmoves;
                            }
                            else
                            {
                                atZeroCount += fullmoves + 1;
                            }
                        }
                        else
                        {
                            atZeroCount += fullmoves;
                        }
                    }
                }
            }
            return atZeroCount;
        }
        
        public int MoveRight(Move move, int currentPosition)
        {
            int currPosition = (currentPosition + move.Steps)  % _mod;
            return currPosition;
        }

        public int MoveLeft(Move move, int currentPosition) 
        {
            var newPosition = ((currentPosition - move.Steps) + _mod)  % _mod;
            return newPosition;
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
