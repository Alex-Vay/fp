using System.Drawing;
using TagsCloudVisualization.Settings;
namespace TagsCloudVisualization.Visualizers;

public class ImageSaver(ImageSaveSettings settings)
{
    private readonly List<string> supportedFormats = ["png", "jpg", "jpeg", "bmp"];

    public string SaveImage(Bitmap image)
    {
        if (!supportedFormats.Contains(settings.ImageFormat))
            throw new ArgumentException($"Unsupported image format: {settings.ImageFormat}");

        var fullImageName = $"{settings.ImageName}.{settings.ImageFormat}";
        if (settings.OutputPath == null)
        {
            image.Save(fullImageName);
            return Path.Combine(Directory.GetCurrentDirectory(), fullImageName);
        }
        image.Save(Path.Combine(settings.OutputPath, fullImageName));
        return Path.Combine(settings.OutputPath, fullImageName);
    }
}
