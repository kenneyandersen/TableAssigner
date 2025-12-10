using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TableAssigner.Visualizer.Console
{
    class AutoAsciiMap
    {
        private readonly char[,] grid;
        private readonly float minX, minY;
        private readonly float scale;

        public int Width => grid.GetLength(1);
        public int Height => grid.GetLength(0);

        public AutoAsciiMap(List<Vector2[]> tableCorners, float scale = 4, int padding = 2)
        {
            this.scale = scale;

            // Find global bounds
            minX = tableCorners.SelectMany(t => t).Min(v => v.X) - padding / scale;
            float maxX = tableCorners.SelectMany(t => t).Max(v => v.X) + padding / scale;
            minY = tableCorners.SelectMany(t => t).Min(v => v.Y) - padding / scale;
            float maxY = tableCorners.SelectMany(t => t).Max(v => v.Y) + padding / scale;

            int width = (int)((maxX - minX) * scale) + 1;
            int height = (int)((maxY - minY) * scale) + 1;

            grid = new char[height, width];

            // Fill empty spaces
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    grid[y, x] = ' ';
        }

        // Map world coordinates to grid indices
        private int MapX(float x) => (int)((x - minX) * scale);
        private int MapY(float y) => (int)((y - minY) * scale);

        public void DrawTable(Vector2[] corners, char ch = '#')
        {
            for (int i = 0; i < corners.Length; i++)
            {
                var start = corners[i];
                var end = corners[(i + 1) % corners.Length];
                DrawLine(MapX(start.X), MapY(start.Y), MapX(end.X), MapY(end.Y), ch);
            }
        }

        private void DrawLine(int x0, int y0, int x1, int y1, char ch)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                if (y0 >= 0 && y0 < grid.GetLength(0) && x0 >= 0 && x0 < grid.GetLength(1))
                    grid[y0, x0] = ch;

                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        public void Print()
        {
            for (int y = grid.GetLength(0) - 1; y >= 0; y--)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                    System.Console.Write(grid[y, x]);
                System.Console.WriteLine();
            }
        }
    }
}
