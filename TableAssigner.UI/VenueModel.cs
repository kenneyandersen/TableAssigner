using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TableAssigner.Locations;
using TableAssigner.TestData;

namespace TableAssigner.UI
{
    public partial class VenueModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = "My Venue";

        [ObservableProperty]
        private ObservableCollection<LocationModel> _locations = [];

        public VenueModel()
        {
            var venue = TestDataCreator.CreateVenue();
            Name = venue.Name;
            foreach (var location in venue.Locations)
            {
                Locations.Add(new LocationModel(location));
            }
        }
    }
}
