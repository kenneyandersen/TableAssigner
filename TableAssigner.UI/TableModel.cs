using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TableAssigner.Locations;

namespace TableAssigner.UI
{
    public partial class TableModel : ObservableObject
    {
        public Table Model { get; }

        public event EventHandler? TableChanged;

        public TableModel(Table table)
        {
            Model = table;
            Width = table.Width;
            Length = table.Length;
            //X = (int)table.Position.X;
            //Y = (int)table.Position.Y;
            TableName = table.TableName;
        }

        [ObservableProperty]
        private string tableName;

        partial void OnTableNameChanged(string value)
        {
            Model.TableName = value;
            OnPropertyChanged(nameof(TableName));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int width;

        partial void OnWidthChanged(int value)
        {
            Model.Width = value;      // Regenerates seats in your Table model
            OnPropertyChanged(nameof(Seats)); // Notify UI if needed
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int length;

        partial void OnLengthChanged(int value)
        {
            Model.Length = value;
            OnPropertyChanged(nameof(Seats));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int x;
        partial void OnXChanged(int value)
        {
            Model.Position = Model.Position with { X = value };
            OnPropertyChanged(nameof(X));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int y;
        partial void OnYChanged(int value)
        {
            Model.Position = Model.Position with { X = value };
        }

        public IReadOnlyList<Seat> Seats => Model.Seats;

        [RelayCommand]
        private void ModifyX(int shift) => X += shift;

        [RelayCommand]
        private void ModifyY(int shift) => Y += shift;
    }
}
