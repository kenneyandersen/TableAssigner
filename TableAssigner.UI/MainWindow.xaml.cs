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
        private Point _panStart;
        private bool _isPanning = false;
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
            //_canvasTool.DrawTables(venue.Locations.First().Tables);
            _canvasTool.DrawLocation(venue.Locations.First());
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;

            // Current mouse position relative to canvas
            var mousePos = e.GetPosition(LayoutCanvas);

            // Apply zoom
            ZoomTransform.ScaleX *= zoomFactor;
            ZoomTransform.ScaleY *= zoomFactor;

            // Adjust pan so zoom stays centered on mouse cursor
            PanTransform.X = mousePos.X - zoomFactor * (mousePos.X - PanTransform.X);
            PanTransform.Y = mousePos.Y - zoomFactor * (mousePos.Y - PanTransform.Y);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isPanning = true;
            _panStart = e.GetPosition(this);
            LayoutCanvas.CaptureMouse();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isPanning = false;
            LayoutCanvas.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var currentPos = e.GetPosition(this);
                var delta = currentPos - _panStart;

                PanTransform.X += delta.X;
                PanTransform.Y += delta.Y;

                _panStart = currentPos;
            }
        }


    }
}