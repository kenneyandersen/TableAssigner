using System.Diagnostics.Metrics;
using System.Numerics;
using TableAssigner.Locations;
using Con = System.Console;

namespace TableAssigner.Visualizer.Console
{
    public class ConsoleVisualizer
    {
        public static void Draw(Venue venue)
        {
            foreach(var location in venue.Locations)
            {
                Draw(location);
            }
        }

        public static void Draw(Location location)
        {
            var tables = location.Tables.Select(GetCorners).ToList();
            
            var map = new AutoAsciiMap(tables, scale: 2, padding: 1);

            // Draw all tables
            foreach(var table in tables)
            {
                map.DrawTable(table, '#');
            }
            map.Print();
        }

        //public static void Draw(Table table)
        //{           
        //    var corners = GetCorners(table);
        //    Con.WriteLine($"Table: {table.TableName}");

        //    TableRenderer.DrawTable(corners);
        //}

        public static Vector2[] GetCorners(Table table)
        {
            float w = table.Width;
            float h = table.Length;

            
            // Local corners relative to the center
            Vector2[] localCorners =
            [
                new Vector2(0, 0),
                new Vector2(w, 0),
                new Vector2(w, h),
                new Vector2(0, h)
            ];

            // Rotate and translate to world space
            float rad = table.RotationDegrees * (float)Math.PI / 180f;
            var rotation = Matrix3x2.CreateRotation(rad);

            return localCorners.Select(c => Vector2.Transform(c, rotation) + table.Position).ToArray();
        }
    }
}
