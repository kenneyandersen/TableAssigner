using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TableAssigner.Locations
{
    public static class Vector2Extensions
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float rad = degrees * (MathF.PI / 180f);

            float cos = MathF.Cos(rad);
            float sin = MathF.Sin(rad);

            float x = v.X * cos - v.Y * sin;
            float y = v.X * sin + v.Y * cos;

            // ensures normalization even if floating-point drift occurs
            return Vector2.Normalize(new Vector2(x, y));
        }
    }
}
