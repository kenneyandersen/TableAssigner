using System;
using System.Collections.Generic;
using System.Text;

namespace TableAssigner.Visualizer.Console
{
    using System;
    using System.Numerics;

    class TableRenderer
    {
        public static void DrawTable(Vector2[] corners, int scale = 2)
        {
            // Determine bounds
            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            foreach (var c in corners)
            {
                if (c.X < minX) minX = c.X;
                if (c.X > maxX) maxX = c.X;
                if (c.Y < minY) minY = c.Y;
                if (c.Y > maxY) maxY = c.Y;
            }

            int width = (int)((maxX - minX) * scale) + 1;
            int height = (int)((maxY - minY) * scale) + 1;

            char[,] grid = new char[height, width];

            // Fill with spaces
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    grid[y, x] = ' ';

            // Map coordinates → grid indices
            int MapX(float x) => (int)((x - minX) * scale);
            int MapY(float y) => (int)((y - minY) * scale);

            // Draw edges
            for (int i = 0; i < corners.Length; i++)
            {
                var start = corners[i];
                var end = corners[(i + 1) % corners.Length];
                DrawLine(grid, MapX(start.X), MapY(start.Y), MapX(end.X), MapY(end.Y), '#');
            }

            // Print grid
            for (int y = height - 1; y >= 0; y--) // top-to-bottom
            {
                for (int x = 0; x < width; x++)
                    Console.Write(grid[y, x]);
                Console.WriteLine();
            }
        }

        // Bresenham line algorithm
        private static void DrawLine(char[,] grid, int x0, int y0, int x1, int y1, char ch)
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
    }

}
