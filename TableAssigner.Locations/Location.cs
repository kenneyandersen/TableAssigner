namespace TableAssigner.Locations
{
    public record Location
    {
        public Location(string name, Venue venue) 
        {
            this.Name = name;
            this.Venue = venue;
            Tables = [];
        }

        public List<Table> Tables { get; }
        public string Name { get; }
        public Venue Venue { get; }
    };
}
