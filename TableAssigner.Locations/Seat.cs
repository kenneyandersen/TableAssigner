using System.Numerics;

namespace TableAssigner.Locations
{
    public class Seat
    {
        public string Id { get; }
        public string Side { get; }

        /// <summary>
        /// Local table coordinate (relative X/Y along the table surface).
        /// </summary>
        public Vector2 TableCoordinate { get; set; }

        /// <summary>
        /// Facing direction of the seat in table-local space.
        /// </summary>
        public Vector2 Direction { get; set; }

        public Seat(string id, string side, Vector2 tableCoordinate, Vector2 direction)
        {
            Id = id;
            Side = side;
            TableCoordinate = tableCoordinate;
            Direction = direction;
        }
    }


}
