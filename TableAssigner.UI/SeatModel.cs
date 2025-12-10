using CommunityToolkit.Mvvm.ComponentModel;
using System.Numerics;
using TableAssigner.Locations;

namespace TableAssigner.UI
{
    public partial class SeatViewModel : ObservableObject
    {
        public SeatViewModel(Seat model)
        {
            Id = model.Id;
            Side = model.Side;
            TableCoordinate = model.TableCoordinate;
            Direction = model.Direction;
        }

        public string Id { get; }
        public string Side { get; }
        public Vector2 TableCoordinate { get; }
        private Vector2 Direction { get; }
    }
}
