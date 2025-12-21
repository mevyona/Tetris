using Microsoft.Xna.Framework;

namespace Tetris.App
{
    public class Tetromino
    {
        public TetrominoType Type { get; set; }
        public int[,] Shape { get; set; }
        public Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tetromino(TetrominoType type)
        {
            Type = type;
            X = 3;
            Y = 0;
            
            switch (type)
            {
                case TetrominoType.I:
                    Shape = new int[,] { { 1, 1, 1, 1 } };
                    Color = Color.Cyan;
                    break;
                case TetrominoType.O:
                    Shape = new int[,] { { 1, 1 }, { 1, 1 } };
                    Color = Color.Yellow;
                    break;
                case TetrominoType.T:
                    Shape = new int[,] { { 0, 1, 0 }, { 1, 1, 1 } };
                    Color = Color.Purple;
                    break;
                case TetrominoType.S:
                    Shape = new int[,] { { 0, 1, 1 }, { 1, 1, 0 } };
                    Color = Color.Green;
                    break;
                case TetrominoType.Z:
                    Shape = new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
                    Color = Color.Red;
                    break;
                case TetrominoType.J:
                    Shape = new int[,] { { 1, 0, 0 }, { 1, 1, 1 } };
                    Color = Color.Blue;
                    break;
                case TetrominoType.L:
                    Shape = new int[,] { { 0, 0, 1 }, { 1, 1, 1 } };
                    Color = Color.Orange;
                    break;
            }
        }

        public void Rotate()
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);
            int[,] rotated = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[j, rows - 1 - i] = Shape[i, j];
                }
            }

            Shape = rotated;
        }

        public Tetromino Clone()
        {
            var clone = new Tetromino(Type)
            {
                X = this.X,
                Y = this.Y,
                Shape = (int[,])this.Shape.Clone(),
                Color = this.Color
            };
            return clone;
        }
    }
}