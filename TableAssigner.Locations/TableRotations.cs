using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TableAssigner.Locations
{
    public class TableRotations
    {
        public static readonly Vector2 Rot_0 = Vector2.UnitX;
        public static readonly Vector2 Rot_90 = Rot_0.Rotate(90);
        public static readonly Vector2 Rot_180 = Rot_0.Rotate(180);
        public static readonly Vector2 Rot_270 = Rot_0.Rotate(270);
    }
}
