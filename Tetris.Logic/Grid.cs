    
using System;
using System.Drawing;

namespace Tetris.Logic
{
    public class Grid
    {
        public const int Width = 10;
        public const int Height = 20;

        private int[,] _board;
        private Color[,] _boardColors;

        public Grid()
        {
            _board = new int[Height, Width];
            _boardColors = new Color[Height, Width];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _board[i, j] = 0;
                    _boardColors[i, j] = Color.White;
                }
            }
        }

        public bool IsOccupied(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return true;
            return _board[y, x] == 1;
        }

        public Color GetColorAt(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return Color.White;
            return _boardColors[y, x];
        }

        public bool CanPlacePiece(Piece piece)
        {
            int rows = piece.Shape.GetLength(0);
            int cols = piece.Shape.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int boardX = piece.X + j;
                        int boardY = piece.Y + i;

                        if (boardX < 0 || boardX >= Width || boardY >= Height)
                            return false;

                        if (boardY >= 0 && _board[boardY, boardX] == 1)
                            return false;
                    }
                }
            }

            return true;
        }

        public void PlacePiece(Piece piece)
        {
            int rows = piece.Shape.GetLength(0);
            int cols = piece.Shape.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (piece.Shape[i, j] == 1)
                    {
                        int boardX = piece.X + j;
                        int boardY = piece.Y + i;

                        if (boardY >= 0 && boardY < Height && boardX >= 0 && boardX < Width)
                        {
                            _board[boardY, boardX] = 1;
                            _boardColors[boardY, boardX] = piece.Color;
                        }
                    }
                }
            }
        }

        public int ClearFullLines()
        {
            int linesCleared = 0;

            for (int i = Height - 1; i >= 0; i--)
            {
                bool lineFull = true;
                for (int j = 0; j < Width; j++)
                {
                    if (_board[i, j] == 0)
                    {
                        lineFull = false;
                        break;
                    }
                }

                if (lineFull)
                {
                    linesCleared++;
                    
                    for (int k = i; k > 0; k--)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            _board[k, j] = _board[k - 1, j];
                            _boardColors[k, j] = _boardColors[k - 1, j];
                        }
                    }

                    for (int j = 0; j < Width; j++)
                    {
                        _board[0, j] = 0;
                        _boardColors[0, j] = Color.White;
                    }

                    i++;
                }
            }

            return linesCleared;
        }

        public void Draw(int offsetX, int offsetY, int blockSize, Action<int, int, int, Color> drawBlock)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Color color = _board[i, j] == 1 ? _boardColors[i, j] : Color.DarkGray;
                    drawBlock(offsetX + j * blockSize, offsetY + i * blockSize, blockSize - 2, color);
                }
            }
        }

        public bool Update(Piece currentPiece)
        {
            var testPiece = currentPiece.Clone();
            testPiece.Y++;

            if (CanPlacePiece(testPiece))
            {
                currentPiece.Y++;
                return false; 
            }
            else
            {
                PlacePiece(currentPiece);
                ClearFullLines();
                return true; 
            }
        }


    }
}