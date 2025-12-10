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

    // ----------------------------------------------------------
    //  Draw the table rectangle with rotation
    // ----------------------------------------------------------
    private void DrawTableBody(Table table)
    {
        double widthPx = table.Width * _unitSize;
        double lengthPx = table.Length * _unitSize;

        // compute rotation angle from normalized vector
        double angleDeg = GetAngleFromVector(table.Rotation);

        var rect = new Rectangle
        {
            Width = widthPx,
            Height = lengthPx,
            Fill = Brushes.LightGray,
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            RenderTransform = new RotateTransform(angleDeg, widthPx / 2, lengthPx / 2),
            Tag = CanvasTags.Table
        };

        Canvas.SetLeft(rect, table.Position.X);
        Canvas.SetTop(rect, table.Position.Y);

        _canvas.Children.Add(rect);
    }

    // ----------------------------------------------------------
    //  Draw seat with full rotation support
    // ----------------------------------------------------------
    private void DrawSeat(Table table, Seat seat)
    {
        // local seat coordinate
        Vector2 local = seat.TableCoordinate;

        // convert to pixel space BEFORE rotation
        Point unrotated = new Point(
            table.Position.X + local.X * _unitSize,
            table.Position.Y + local.Y * _unitSize
        );

        // compute rotated point
        Point p = RotatePointAroundTableCenter(table, unrotated);

        var dot = new Ellipse
        {
            Width = 8,
            Height = 8,
            Fill = Brushes.Blue,
            Tag = CanvasTags.Seat
        };

        Canvas.SetLeft(dot, p.X - 4);
        Canvas.SetTop(dot, p.Y - 4);

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
