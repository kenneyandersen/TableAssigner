using TableAssigner.Locations;

namespace TableAssigner.TestData
{
    public class TestDataCreator
    {
        public static Venue CreateVenue()
        {
            var venue = new Venue("Christmas Party House");
            var rand = new Random(1234); 
            for (int i = 0; i < 1; i++)
            {
                var location = new Location($"Floor {i + 1}", venue);
                venue.Locations.Add(location);

                for (int x = 0; x < rand.Next(2, 2); x++)
                {
                    var table = new Table($"Table {x + 1}", location)
                    {
                        Width = 2,
                        Length = 10,
                        Position = new System.Numerics.Vector2(x*4, 0)
                    };
                    location.Tables.Add(table);
                }
            }
            return venue;
        }

    }
}
