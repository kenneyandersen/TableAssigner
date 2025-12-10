using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TableAssigner.Locations
{
    public class Table
    {
        public Table(string tableName, Location location)
        {
            this.TableName = tableName;
            this.Location = location;
            Rotation = Rotations.Rot_0;
            Position = new(0, 0);
            Seats = [];
            GenerateSeats();
        }

        public string TableName { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Rotation { get; set;  }
        public Location Location { get; }
        public List<Seat> Seats { get; }
        public int Width
        {
            get => field;
            set
            {
                field = value;
                GenerateSeats();
            }
        }

        public int Length {
            get => field;
            set
            {
                field = value;
                GenerateSeats();
            }
        }

        private void GenerateSeats()
        {
            Seats.Clear();

            // Top edge
            for (int i = 0; i < Width; i++)
            {
                Seats.Add(new Seat($"{TableName}-Top-{i + 1}", "Top", new Vector2(i, 0), Directions.Up));
            }

            // Bottom edge
            for (int i = 0; i < Width; i++)
            {
                Seats.Add(new Seat($"{TableName}-Bottom-{i + 1}", "Bottom", new Vector2(i, Length), Directions.Down));
            }

            // Left edge
            for (int i = 0; i < Length; i++)
            {
                Seats.Add(new Seat($"{TableName}-Left-{i + 1}", "Left", new Vector2(0,i), Directions.Left));
            }

            for (int i = 0; i < Length; i++)
            {
                Seats.Add(new Seat($"{TableName}-Right-{i + 1}", "Right", new Vector2(Width, i), Directions.Left));
            }
        }
    }
}
