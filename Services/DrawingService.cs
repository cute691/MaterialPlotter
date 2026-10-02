using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MaterialPlotter.Services
{
    public class DrawingService
    {
        private Canvas _canvas;

        public DrawingService(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void DrawLine(Point start, Point end, Color color, double thickness)
        {
            Line line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            _canvas.Children.Add(line);
        }

        public void DrawRectangle(Point topLeft, Point bottomRight, Color strokeColor, Color fillColor, double thickness)
        {
            Rectangle rect = new Rectangle
            {
                Width = Math.Abs(bottomRight.X - topLeft.X),
                Height = Math.Abs(bottomRight.Y - topLeft.Y),
                Fill = new SolidColorBrush(fillColor),
                Stroke = new SolidColorBrush(strokeColor),
                StrokeThickness = thickness
            };
            Canvas.SetLeft(rect, Math.Min(topLeft.X, bottomRight.X));
            Canvas.SetTop(rect, Math.Min(topLeft.Y, bottomRight.Y));
            _canvas.Children.Add(rect);
        }

        public void DrawEllipse(Point topLeft, Point bottomRight, Color strokeColor, Color fillColor, double thickness)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = Math.Abs(bottomRight.X - topLeft.X),
                Height = Math.Abs(bottomRight.Y - topLeft.Y),
                Fill = new SolidColorBrush(fillColor),
                Stroke = new SolidColorBrush(strokeColor),
                StrokeThickness = thickness
            };
            Canvas.SetLeft(ellipse, Math.Min(topLeft.X, bottomRight.X));
            Canvas.SetTop(ellipse, Math.Min(topLeft.Y, bottomRight.Y));
            _canvas.Children.Add(ellipse);
        }

        public void ExportToPNG(string filePath)
        {
            try
            {
                RenderTargetBitmap rtb = new RenderTargetBitmap(
                    (int)_canvas.ActualWidth,
                    (int)_canvas.ActualHeight,
                    96, 96, PixelFormats.Pbgra32);
                rtb.Render(_canvas);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using (var stream = System.IO.File.Create(filePath))
                {
                    encoder.Save(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
