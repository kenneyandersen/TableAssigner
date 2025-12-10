using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TableAssigner.Locations
{
    public static class Directions
    {
        public static readonly Vector2 Right = Vector2.UnitX;
        public static readonly Vector2 Down = Vector2.UnitY;
        public static readonly Vector2 Left = new(-1, 0);
        public static readonly Vector2 Up = new(0, -1);
    }
}
