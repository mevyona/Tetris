using System;
using Tetris.Logic;
namespace Tetris.App
{
    public class GameState
    {
        private Random _random;

        public Grid Board { get; private set; }
        public Piece CurrentPiece { get; set; }
        public Piece NextPiece { get; set; }
        public int Score { get; set; }
        public int LinesCleared { get; set; }
        public bool IsGameOver { get; set; }
        public float CurrentFallSpeed { get; set; }

        public const float InitialFallSpeed = 1f;
        public const float FastFallSpeed = 0.05f;

        public GameState()
        {
            _random = new Random();
            Board = new Grid();
            Reset();
        }

        public void Reset()
        {
            Board.Clear();
            Score = 0;
            LinesCleared = 0;
            IsGameOver = false;
            CurrentFallSpeed = InitialFallSpeed;
            CurrentPiece = CreateRandomPiece();
            NextPiece = CreateRandomPiece();
        }

        public Piece CreateRandomPiece()
        {
            var types = Enum.GetValues<TetrominoType>();
            return new Piece(types[_random.Next(types.Length)]);
        }

        public void ProcessClearedLines(int linesCleared)
        {
            if (linesCleared > 0)
            {
                LinesCleared += linesCleared;
                int[] scoreValues = { 0, 100, 300, 500, 800 };
                Score += scoreValues[Math.Min(linesCleared, 4)];
                
                CurrentFallSpeed = Math.Max(0.1f, InitialFallSpeed - (LinesCleared / 10) * 0.1f);
            }
        }

        public void SpawnNextPiece()
        {
            CurrentPiece = NextPiece;
            NextPiece = CreateRandomPiece();

            if (!Board.CanPlacePiece(CurrentPiece))
            {
                IsGameOver = true;
            }
        }
    }
}