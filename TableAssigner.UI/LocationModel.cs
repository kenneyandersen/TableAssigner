using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using TableAssigner.Locations;

namespace TableAssigner.UI
{
    public partial class LocationModel : ObservableObject
    {
        public LocationModel(Location location)
        {
            var canvasTool = new CanvasTool(Canvas, unitSize: 20);
            canvasTool.DrawLocation(location);
            Name = location.Name;
        }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private Canvas _canvas = new() { Width = 1200, Height = 1200, Background = Brushes.Pink };
    }
}
