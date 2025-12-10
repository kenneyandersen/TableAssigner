using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableAssigner.Locations;
using TableAssigner.TestData;

namespace TableAssigner.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CanvasTool _canvasTool;
        public MainWindow()
        {
            InitializeComponent();

            _canvasTool = new CanvasTool(LayoutCanvas, unitSize: 20);

            DrawDemo();
        }

        private void DrawDemo()
        {
            // Example Location (you probably already have your own)

            var venue = TestDataCreator.CreateVenue();
            var table = venue.Locations.First().Tables.First();

            // Seats are generated automatically by your Table class
            _canvasTool.DrawTables(venue.Locations.First().Tables);
        }

    }
}