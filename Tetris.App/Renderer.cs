using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DrawingColor = System.Drawing.Color;

using Tetris.Logic;


namespace Tetris.App
{
    public class Renderer
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _blockTexture;
        private SpriteFont _font;
        private const int BlockSize = 30;

        public void Initialize(SpriteBatch spriteBatch, Texture2D blockTexture, SpriteFont font)
        {
            _spriteBatch = spriteBatch;
            _blockTexture = blockTexture;
            _font = font;
        }

        public void Draw(GameState gameState)
        {
            int offsetX = 50;
            int offsetY = 50;

            gameState.Board.Draw(offsetX, offsetY, BlockSize, DrawBlock);

            if (!gameState.IsGameOver)
            {
                DrawTetromino(gameState.CurrentPiece, offsetX, offsetY);
            }

            DrawNextPiece(gameState.NextPiece, offsetX + Grid.Width * BlockSize + 40, offsetY);
            DrawGameInfo(gameState, offsetX + Grid.Width * BlockSize + 40, offsetY + 150);

            if (gameState.IsGameOver)
            {
                DrawGameOverMessage(gameState, offsetX, offsetY);
            }
        }

        private void DrawBlock(int x, int y, int size, DrawingColor color)
        {
            Rectangle rect = new Rectangle(x, y, size, size);
            var mgColor = new Color(color.R, color.G, color.B, color.A);
            _spriteBatch.Draw(_blockTexture, rect, mgColor);
        }

        private void DrawTetromino(Piece piece, int offsetX, int offsetY)
        {
            int rows = piece.Shape.GetLength(0);
            int cols = piece.Shape.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int screenX = offsetX + (piece.X + j) * BlockSize;
                        int screenY = offsetY + (piece.Y + i) * BlockSize;
                        DrawBlock(screenX, screenY, BlockSize - 2, piece.Color);
                    }
                }
            }
        }

        private void DrawNextPiece(Piece piece, int x, int y)
        {
            DrawText("SUIVANT :", x, y - 30, Color.White);

            int rows = piece.Shape.GetLength(0);
            int cols = piece.Shape.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        DrawBlock(x + j * BlockSize, y + i * BlockSize, BlockSize - 2, piece.Color);
                    }
                }
            }
        }

        private void DrawGameInfo(GameState gameState, int x, int y)
        {
            DrawText($"SCORE :", x, y, Color.White);
            DrawText($"{gameState.Score}", x, y + 25, Color.Yellow);
            DrawText($"LIGNES :", x, y + 60, Color.White);
            DrawText($"{gameState.LinesCleared}", x, y + 85, Color.Yellow);
            DrawText($"CONTROLES :", x, y + 130, Color.White);
            DrawText($"Fleches : Bouger", x, y + 155, Color.Gray);
            DrawText($"Haut : Rotation", x, y + 175, Color.Gray);
            DrawText($"Bas : Rapide", x, y + 195, Color.Gray);
            DrawText($"Espace : Chute", x, y + 215, Color.Gray);
            DrawText($"Echap : Quitter", x, y + 240, Color.Gray);
        }

        private void DrawGameOverMessage(GameState gameState, int offsetX, int offsetY)
        {
            Rectangle overlay = new Rectangle(
                offsetX,
                offsetY + Grid.Height / 2 * BlockSize - 60,
                Grid.Width * BlockSize,
                120
            );
            _spriteBatch.Draw(_blockTexture, overlay, Color.Black * 0.8f);

            DrawText("JEU TERMINE", offsetX + 50, offsetY + Grid.Height / 2 * BlockSize - 40, Color.Red);
            DrawText($"Score : {gameState.Score}", offsetX + 60, offsetY + Grid.Height / 2 * BlockSize - 10, Color.White);
            DrawText("Appuyer sur R pour relancer", offsetX + 70, offsetY + Grid.Height / 2 * BlockSize + 20, Color.Yellow);
        }

        private void DrawText(string text, int x, int y, Color color)
        {
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, text, new Vector2(x, y), color);
            }
        }
    }
}