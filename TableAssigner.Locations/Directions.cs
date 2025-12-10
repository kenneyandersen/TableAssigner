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
        public static readonly Vector2 Left = Right.Rotate(180);
        public static readonly Vector2 Up = Down.Rotate(180);
    }
}
