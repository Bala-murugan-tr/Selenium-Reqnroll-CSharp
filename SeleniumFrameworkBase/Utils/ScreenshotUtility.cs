using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Input;
using System.Buffers.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SeleniumFrameworkBase.Utils;
internal static class ScreenshotUtility {

    internal static Media getscreenshot(IWebDriver driver) {
        var scaleFactor = 1.0;
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        using MemoryStream ms = new MemoryStream(screenshot.AsByteArray);
        Bitmap original = new Bitmap(ms);
        int width = (int)(original.Width * scaleFactor);
        int height = (int)(original.Height * scaleFactor);

        using (Bitmap resized = new Bitmap(width, height)) {
            using (Graphics g = Graphics.FromImage(resized)) {
                // Apply bilinear interpolation for smoother scaling
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(original, 0, 0, width, height);
            }

            using (MemoryStream outputStream = new MemoryStream()) {
                resized.Save(outputStream, ImageFormat.Jpeg); // Use "jpg" for smaller size
                string base64 = Convert.ToBase64String(outputStream.ToArray());

                return MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build();
            }
        }
    }
    internal static Media Capture(IWebDriver driver, double scaleFactor = 1.0, long jpegQuality = 75L) {
        try {
            using var originalStream = new MemoryStream(((ITakesScreenshot)driver).GetScreenshot().AsByteArray);
            using var originalImage = new Bitmap(originalStream);

            using var resizedImage = ResizeImage(originalImage, scaleFactor);
            using var outputStream = new MemoryStream();

            var jpegEncoder = GetJpegEncoder();

            var encoderParams = new EncoderParameters(1) {
                Param = { [0] = new EncoderParameter(Encoder.Quality, jpegQuality) }
            };

            resizedImage.Save(outputStream, jpegEncoder!, encoderParams);
            string base64 = Convert.ToBase64String(outputStream.ToArray());

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build();
        } catch {
            return EmptyMedia();
        }
    }
    // resize image with Bilinear interpolation smoothen
    private static Bitmap ResizeImage(Bitmap original, double scaleFactor) {
        int width = (int)(original.Width * scaleFactor);
        int height = (int)(original.Height * scaleFactor);

        var resized = new Bitmap(width, height);
        using var g = Graphics.FromImage(resized);
        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.DrawImage(original, 0, 0, width, height);

        return resized;
    }
    // image encoder
    private static ImageCodecInfo GetJpegEncoder() {
        return ImageCodecInfo.GetImageEncoders()
            .FirstOrDefault(enc => enc.FormatID == ImageFormat.Jpeg.Guid)!;
    }

    private static Media EmptyMedia() =>
        MediaEntityBuilder.CreateScreenCaptureFromBase64String("").Build();
}

