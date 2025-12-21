using Microsoft.Xna.Framework.Input;

namespace Tetris.App
{
    public class InputHandler
    {
        private KeyboardState _previousKeyState;
        private float _moveTimer;
        private const float MoveDelay = 0.1f;

        public void Update(float deltaTime, GameState gameState)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

            if (gameState.IsGameOver)
            {
                if (currentKeyState.IsKeyDown(Keys.R) && _previousKeyState.IsKeyUp(Keys.R))
                {
                    gameState.Reset();
                }
                _previousKeyState = currentKeyState;
                return;
            }

            _moveTimer += deltaTime;
            if (_moveTimer >= MoveDelay)
            {
                if (currentKeyState.IsKeyDown(Keys.Left))
                {
                    TryMovePiece(gameState, -1, 0);
                }
                else if (currentKeyState.IsKeyDown(Keys.Right))
                {
                    TryMovePiece(gameState, 1, 0);
                }
            }

            if (currentKeyState.IsKeyDown(Keys.Up) && _previousKeyState.IsKeyUp(Keys.Up))
            {
                TryRotatePiece(gameState);
            }

            if (currentKeyState.IsKeyDown(Keys.Space) && _previousKeyState.IsKeyUp(Keys.Space))
            {
                HardDrop(gameState);
            }

            _previousKeyState = currentKeyState;
        }

        private void TryMovePiece(GameState gameState, int deltaX, int deltaY)
        {
            var testPiece = gameState.CurrentPiece.Clone();
            testPiece.X += deltaX;
            testPiece.Y += deltaY;
            
            if (gameState.Board.CanPlacePiece(testPiece))
            {
                gameState.CurrentPiece.X = testPiece.X;
                gameState.CurrentPiece.Y = testPiece.Y;
                _moveTimer = 0;
            }
        }

        private void TryRotatePiece(GameState gameState)
        {
            var rotatedPiece = gameState.CurrentPiece.Clone();
            rotatedPiece.Rotate();
            
            if (gameState.Board.CanPlacePiece(rotatedPiece))
            {
                gameState.CurrentPiece = rotatedPiece;
            }
        }

        private void HardDrop(GameState gameState)
        {
            while (CanMovePieceDown(gameState))
            {
                gameState.CurrentPiece.Y++;
            }
        }

        private bool CanMovePieceDown(GameState gameState)
        {
            var testPiece = gameState.CurrentPiece.Clone();
            testPiece.Y++;
            return gameState.Board.CanPlacePiece(testPiece);
        }

        public bool IsFastFalling()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Down);
        }
    }
}