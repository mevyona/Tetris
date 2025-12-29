using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Tetris.Logic
{
    public class Block
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Block(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
    }
}
