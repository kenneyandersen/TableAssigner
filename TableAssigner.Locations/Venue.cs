namespace TableAssigner.Locations
{
    public record Venue
    {
        public Venue(string Name)
        {
            this.Name = Name;
            Locations = [];
        }

        public List<Location> Locations { get; }
        public string Name { get; }
    }
}
