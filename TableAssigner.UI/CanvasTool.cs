using System;
using System.Numerics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TableAssigner.Locations;
using TableAssigner.UI;

public class CanvasTool
{
    private readonly Canvas _canvas;
    private readonly double _unitSize;

    public CanvasTool(Canvas canvas, double unitSize = 20)
    {
        _canvas = canvas;
        _unitSize = unitSize;
    }

    public void Clear() => _canvas.Children.Clear();

    public void DrawTables(IEnumerable<Table> tables)
    {
        foreach (var table in tables)
            DrawTable(table);
    }

    public void DrawTable(Table table)
    {
        DrawTableBody(table);

        foreach (var seat in table.Seats)
            DrawSeat(table, seat);
    }

    public void DrawLocation(Location location)
    {
        if (location.Tables.Count == 0)
            return;

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        // Compute bounding in seat units
        foreach (var table in location.Tables)
        {
            float left = table.Position.X;
            float top = table.Position.Y;
            float right = table.Position.X + table.Width;
            float bottom = table.Position.Y + table.Length;

            if (left < minX) minX = left;
            if (top < minY) minY = top;
            if (right > maxX) maxX = right;
            if (bottom > maxY) maxY = bottom;
        }

        // Padding
        float pad = 0.5f;
        minX -= pad;
        minY -= pad;
        maxX += pad;
        maxY += pad;

        // Convert to pixel space
        double x = minX * _unitSize;
        double y = minY * _unitSize;
        double width = (maxX - minX) * _unitSize;
        double height = (maxY - minY) * _unitSize;

        // Draw bounding box
        var rect = new Rectangle
        {
            Width = width,
            Height = height,
            Stroke = Brushes.Black,
            StrokeThickness = 3,
            Fill = null,
            Tag = CanvasTags.Location
        };

        Canvas.SetLeft(rect, x);
        Canvas.SetTop(rect, y);
        _canvas.Children.Add(rect);

        //// ----------------------------------------
        //// ADD LOCATION NAME LABEL (Upper-right)
        //// ----------------------------------------
        //var label = new TextBlock
        //{
        //    Text = location.Name,
        //    FontSize = 16,
        //    FontWeight = FontWeights.Bold,
        //    Foreground = Brushes.Black,
        //    Tag = CanvasTags.Location
        //};

        //// margin above the border
        //double labelOffsetY = -22;
        //double labelOffsetX = -5;  // slight inward margin from the right edge

        //Canvas.SetLeft(label, x + width + labelOffsetX - label.ActualWidth);
        //Canvas.SetTop(label, y + labelOffsetY);

        //// WPF quirk: we must force measurement so ActualWidth works
        //label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

        //// Now reposition the label correctly with actual width
        //Canvas.SetLeft(label, x + width - label.ActualWidth + labelOffsetX);

        //_canvas.Children.Add(label);

        // Draw tables inside this location
        foreach (var table in location.Tables)
            DrawTable(table);
    }



    // ----------------------------------------------------------
    //  Draw the table rectangle with rotation
    // ----------------------------------------------------------
    private void DrawTableBody(Table table)
    {
        // Convert table size from seat-units → pixels
        double widthPx = table.Width * _unitSize;
        double lengthPx = table.Length * _unitSize;

        // Convert table position from seat-units → pixels
        double posX = table.Position.X * _unitSize;
        double posY = table.Position.Y * _unitSize;

        // Rotation from normalized direction vector
        double angleDeg = Math.Atan2(table.Rotation.Y, table.Rotation.X) * (180.0 / Math.PI);

        var rect = new Rectangle
        {
            Width = widthPx,
            Height = lengthPx,
            Fill = Brushes.LightGray,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Tag = CanvasTags.Table,
            RenderTransform = new RotateTransform(
                angleDeg,
                widthPx / 2,
                lengthPx / 2
            )
        };

        Canvas.SetLeft(rect, posX);
        Canvas.SetTop(rect, posY);

        _canvas.Children.Add(rect);
    }


    // ----------------------------------------------------------
    //  Draw seat with full rotation support
    // ----------------------------------------------------------
    private void DrawSeat(Table table, Seat seat)
    {
        Vector2 local = seat.TableCoordinate;

        // Base position in seat-units
        double seatX = table.Position.X + local.X;
        double seatY = table.Position.Y + local.Y;

        // Determine seat centering based on direction
        Vector2 dir = seat.Direction;

        // Horizontal centering for vertical seats (Top/Bottom)
        if (dir.X == 0)
            seatX += 0.5;

        //// Vertical centering for horizontal seats (Left/Right)
        if (dir.Y == 0)
            seatY += 0.5;

        // Convert to pixels
        double unrotatedX = seatX * _unitSize;
        double unrotatedY = seatY * _unitSize;
        var unrotated = new Point(unrotatedX, unrotatedY);

        // Apply table rotation around table center
        Point rotated = RotatePointAroundTableCenter(table, unrotated);

        var dot = new Ellipse
        {
            Width = 8,
            Height = 8,
            Fill = Brushes.Blue,
            Tag = CanvasTags.Seat
        };

        Canvas.SetLeft(dot, rotated.X - 4);
        Canvas.SetTop(dot, rotated.Y - 4);

        _canvas.Children.Add(dot);
    }


    public void ClearByTag(string tag)
    {
        // Collect first to avoid modifying the collection while iterating
        var toRemove = _canvas.Children
            .OfType<FrameworkElement>()
            .Where(e => e.Tag is string s && s == tag)
            .ToList();

        foreach (var element in toRemove)
            _canvas.Children.Remove(element);
    }


    public void ClearTables()
    {
        ClearByTag(CanvasTags.Table);
        ClearByTag(CanvasTags.Seat);
    }

    // ----------------------------------------------------------
    // Compute rotation angle from normalized direction vector
    // ----------------------------------------------------------
    private double GetAngleFromVector(Vector2 dir)
    {
        // dir.X = cos(angle), dir.Y = sin(angle)
        double angleRad = Math.Atan2(dir.Y, dir.X);
        return angleRad * (180.0 / Math.PI);
    }

    // ----------------------------------------------------------
    // Rotate a point around table center based on table.Rotation
    // ----------------------------------------------------------
    private Point RotatePointAroundTableCenter(Table table, Point p)
    {
        double widthPx = table.Width * _unitSize;
        double heightPx = table.Length * _unitSize;

        double angleDeg = GetAngleFromVector(table.Rotation);
        double angleRad = angleDeg * (Math.PI / 180.0);

        // table center
        double cx = table.Position.X + widthPx / 2;
        double cy = table.Position.Y + heightPx / 2;

        // translate to origin
        double dx = p.X - cx;
        double dy = p.Y - cy;

        // rotate
        double rx = dx * Math.Cos(angleRad) - dy * Math.Sin(angleRad);
        double ry = dx * Math.Sin(angleRad) + dy * Math.Cos(angleRad);

        // translate back
        return new Point(cx + rx, cy + ry);
    }
}
