using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class Snake
    {
        public List<Point> body;
        public List<MoveVector> vectors;

        public Snake()
        {
            body = new List<Point>(3);
        }

        public Snake(List<Point> points, MoveVector vector)
        {
            body = points;
            vectors = new List<MoveVector>();
            for (int i = 0; i < 3; i++)
            {
                vectors.Add(vector);
            }
        }

        public bool has(Point point)
        {
            foreach (Point elem in body)
            {
                if (elem == point) return true;
            }
            return false;
        }

        public void Add(Point point)
        {
            body.Add(point);
            vectors.Add(vectors[body.Count - 1]);
        }

        public void Move(MoveVector vector)
        {
            for (int i = vectors.Count; i > 0; i--)
            {
                body[i].Move(vectors[i]);
                vectors[i] = vectors[i - 1];
            }
            vectors[0] = vector;
        }
    }
}
