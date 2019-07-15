using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Move(MoveVector vector)
        {
            switch (vector)
            {
                case MoveVector.BOTTOM:
                    y++;
                    break;
                case MoveVector.TOP:
                    y--;
                    break;
                case MoveVector.LEFT:
                    x--;
                    break;
                case MoveVector.RIGHT:
                    x++;
                    break;
            }
        }

        public static bool operator ==(Point left, Point right) => (left.x == right.x) && (left.y == right.y);
        public static bool operator !=(Point left, Point right) => !(right == left); //logic vne the magic))
    }
}
