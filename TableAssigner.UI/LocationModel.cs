using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using TableAssigner.Locations;

namespace TableAssigner.UI
{
    public partial class LocationModel : ObservableObject
    {
        private readonly CanvasTool _canvasTool;
        private int tableCounter = 0;
        private Dictionary<Table, TableModel> tableModels = [];

        public LocationModel(Location location)
        {
            _canvasTool = new CanvasTool(Canvas, unitSize: 20);
            _canvasTool.TableSelected += _canvasTool_TableSelected;

            //if (location.Tables.Count == 0)
            //{
            //    location.Tables.Add(new Table("Table 1", location));
            //}
            foreach (var table in location.Tables)
            {
                var model = new TableModel(table);
                model.TableChanged += (s,e) => RedrawLocation();
                Tables.Add(model);
                tableModels[table] = model;
            }
            tableCounter = location.Tables.Count;
            SelectedTable = Tables.FirstOrDefault();
            Location = location;
        }

        private void RedrawLocation()
        {
            _canvasTool.Clear();
            _canvasTool.DrawLocation(Location);
        }

        private void _canvasTool_TableSelected(object? sender, Table e)
        {
            if (tableModels.TryGetValue(e, out var model))
            {
                SelectedTable = model;
            }
        }

        private Location Location
        {
            get => field;
            set {
                field = value;
                RedrawLocation();
                Name = field.Name;
                SelectedTable = field.Tables.Select(t => tableModels.TryGetValue(t, out var model) ? model : null).FirstOrDefault();
            }
        }

        [ObservableProperty]
        private TableModel? _selectedTable;

        [ObservableProperty]
        private string _name = string.Empty;
        public ObservableCollection<TableModel> Tables { get; } = [];

        [ObservableProperty]
        private Canvas _canvas = new() { Width = 1200, Height = 1200, Background = Brushes.Pink };

        [RelayCommand]
        private void AddTable() => CreateTableModel();

        [RelayCommand]
        public void CopyTable(TableModel model)
        {            
            var newModel = CreateTableModel();
            newModel.X = model.X +1;
            newModel.Y = model.Y +1;
            newModel.Width = model.Width;
            newModel.Length = model.Length;
        }

        private TableModel CreateTableModel()
        {
            var table = new Table($"Table {++tableCounter}", Location);
            var model = new TableModel(table);
            Tables.Add(model);
            tableModels[table] = model;
            Location.Tables.Add(table);
            RedrawLocation();
            SelectedTable = model;
            model.TableChanged += (s, e) => RedrawLocation();
            return model;
        }

        [RelayCommand]
        private void DeleteTable(TableModel tableModel)
        {
            Tables.Remove(tableModel);
            Location.Tables.Remove(tableModel.Table);
            if (SelectedTable != tableModel)
            {
                SelectedTable = null;
            }
            RedrawLocation();
        }
    }
}
