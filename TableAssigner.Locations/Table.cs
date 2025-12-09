using System;
using System.Collections.Generic;
using System.Text;

namespace TableAssigner.Locations
{
    public class Table
    {
        public Table(string tableName, Location location)
        {
            this.TableName = tableName;
            this.Location = location;
            Position = new(0, 0);
        }

        public string TableName { get; set; }
        public Position Position { get; set; }
        public Location Location { get; set; }
    }
}
