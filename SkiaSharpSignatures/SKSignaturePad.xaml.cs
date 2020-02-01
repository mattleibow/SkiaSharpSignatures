using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.ComponentModel;
using Xamarin.Forms;

namespace SkiaSharpSignatures
{
    [DesignTimeVisible(true)]
    public partial class SKSignaturePad : Grid
    {
        private SKPath currentPath;
        private SKPaint paint = new SKPaint
        {
            IsAntialias = true,
            StrokeWidth = 20,
            Color = SKColors.DarkBlue,
            Style = SKPaintStyle.Stroke
        };

        public SKSignaturePad()
        {
            InitializeComponent();
        }

        public SKPath CurrentPath => currentPath;

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    currentPath?.Dispose();
                    currentPath = new SKPath();
                    currentPath.MoveTo(e.Location);
                    break;
                case SKTouchAction.Moved:
                    if (e.InContact)
                        currentPath?.LineTo(e.Location);
                    break;
                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    break;
            }

            e.Handled = true;

            signatureView.InvalidateSurface();
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear();

            if (currentPath != null)
            {
                canvas.DrawPath(currentPath, paint);
            }
        }
    }
}
