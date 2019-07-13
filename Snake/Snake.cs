using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class Snake
    {
        /// <summary>
        /// Body of snake (list of points)
        /// </summary>
        public List<Point> body;
        /// <summary>
        /// Vectors of every body element
        /// </summary>
        public List<MoveVector> vectors;

        public Snake()
        {
            body = new List<Point>();
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

        /// <summary>
        /// Check on snake has this point
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns></returns>
        public bool has(Point point)
        {
            foreach (Point elem in body)
            {
                if (elem == point) return true;
            }
            return false;
        }

        public void Add(Point point, MoveVector vector)
        {
            body.Add(point); //add point to snake
            vectors.Add(vector); //add vector of added point
        }

        /// <summary>
        /// Move snake
        /// </summary>
        /// <param name="vector">Vector of move</param>
        public void Move(MoveVector vector)
        {
            try
            {
                for (int i = vectors.Count - 1; i > 0; i--)
                {
                    body[i].Move(vectors[i - 1]);
                    vectors[i] = vectors[i - 1];
                }
                body[0].Move(vector);
                vectors[0] = vector;
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// Check over snake from field with this coordinates
        /// </summary>
        /// <param name="w">Width of field</param>
        /// <param name="h">Height of field</param>
        /// <returns></returns>
        public bool Over(int w, int h)
        {
            return false;
        }
    }
}
