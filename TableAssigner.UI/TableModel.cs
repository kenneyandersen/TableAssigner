using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using TableAssigner.Locations;

namespace TableAssigner.UI
{
    public partial class TableModel : ObservableObject
    {
        public Table Table { get; }

        public event EventHandler? TableChanged;

        public TableModel(Table table)
        {
            Table = table;
            Width = table.Width;
            Length = table.Length;
            X = (int)table.Position.X;
            Y = (int)table.Position.Y;
            TableName = table.TableName;
        }

        [ObservableProperty]
        private string tableName;

        partial void OnTableNameChanged(string value)
        {
            Table.TableName = value;
            OnPropertyChanged(nameof(TableName));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int width;

        partial void OnWidthChanged(int value)
        {
            Table.Width = value;      // Regenerates seats in your Table model
            OnPropertyChanged(nameof(Seats)); // Notify UI if needed
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int length;

        partial void OnLengthChanged(int value)
        {
            Table.Length = value;
            OnPropertyChanged(nameof(Seats));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int x;
        partial void OnXChanged(int value)
        {
            Table.Position = Table.Position with { X = value };
            OnPropertyChanged(nameof(X));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        private int y;
        partial void OnYChanged(int value)
        {
            Table.Position = Table.Position with { Y = value };
            OnPropertyChanged(nameof(Y));
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        public IReadOnlyList<Seat> Seats => Table.Seats;

        [RelayCommand]
        private void ModifyX(int shift) => X += shift;
        [RelayCommand]
        private void ModifyY(int shift) => Y += shift;

        [RelayCommand]
        private void ModifyLength(int shift) => Length += shift;
        [RelayCommand]
        private void ModifyWidth(int width) => Width += width;

        [RelayCommand]
        private void SetVertical()
        {
            Table.Rotation = TableRotations.Rot_0;
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void SetHorizontal()
        {
            Table.Rotation = TableRotations.Rot_90;
            TableChanged?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void SwitchDimensions()
        {
            var width = Width;
            var length = Length;
            Length = width;
            Width = length;
        }
    }
}
