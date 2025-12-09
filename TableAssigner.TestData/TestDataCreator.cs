using TableAssigner.Locations;

namespace TableAssigner.TestData
{
    public class TestDataCreator
    {
        public static Venue CreateVenue()
        {
            var venue = new Venue("Christmas Party House");
            var rand = new Random(1234); 
            for (int i = 0; i < 5; i++)
            {
                var location = new Location($"Floor {i + 1}", venue);
                venue.Locations.Add(location);

                for (int x = 0; x < rand.Next(10, 20); x++)
                {
                    var table = new Table($"Table {i + 1}", location);
                    location.Tables.Add(table);
                }
            }
            return venue;
        }

    }
}
