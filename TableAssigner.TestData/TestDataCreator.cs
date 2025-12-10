using TableAssigner.Locations;

namespace TableAssigner.TestData
{
    public class TestDataCreator
    {
        public static Venue CreateVenue()
        {
            var venue = new Venue("Christmas Party House");
            var rand = new Random(1234); 
            for (int i = 0; i < 2; i++)
            {
                var location = new Location($"Floor {i + 1}", venue);
                venue.Locations.Add(location);

                var tableCount = rand.Next(5, 10);
                for (int x = 1; x < tableCount; x++)
                {
                    var table = new Table($"Table {x + 1}", location)
                    {
                        Width = 2,
                        Length = 10,
                        Position = new System.Numerics.Vector2(x*3, 1)
                    };
                    location.Tables.Add(table);
                }
            }
            return venue;
        }

    }
}
