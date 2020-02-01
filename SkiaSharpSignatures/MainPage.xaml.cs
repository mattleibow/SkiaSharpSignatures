using SkiaSharp;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SkiaSharpSignatures
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            var path = signaturePad.CurrentPath;
            if (path == null)
                return;

            var bounds = path.TightBounds;

            if (path.PointCount < 3 || bounds.Width < 20 || bounds.Height < 20)
            {
                var isYes = await DisplayAlert(
                    "ERROR",
                    "Not a real signature! Do you want to continue?",
                    "YES", "NO");
                if (!isYes)
                    return;
            }

            bounds.Inflate(10, 10);
            var info = new SKImageInfo((int)bounds.Width, (int)bounds.Height);
            using var surface = SKSurface.Create(info);

            var paint = new SKPaint
            {
                IsAntialias = true,
                StrokeWidth = 15,
                Color = SKColors.Navy,
                Style = SKPaintStyle.Stroke
            };

            var canvas = surface.Canvas;
            canvas.Clear();
            canvas.Translate(-bounds.Left, -bounds.Top);
            canvas.DrawPath(path, paint);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var filePath = Path.Combine(FileSystem.CacheDirectory, "signature.png");
            using (var file = File.Create(filePath))
            {
                data.SaveTo(file);
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                File = new ShareFile(filePath),
                Title = "Signature"
            });
        }
    }
}
