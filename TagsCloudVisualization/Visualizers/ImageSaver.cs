using System.Drawing;
using TagsCloudVisualization.Settings;
namespace TagsCloudVisualization.Visualizers;

public class ImageSaver(ImageSaveSettings settings)
{
    private readonly List<string> supportedFormats = ["png", "jpg", "jpeg", "bmp"];

    public Result<string> SaveImage(Bitmap image)
    {
        if (!supportedFormats.Contains(settings.ImageFormat))
            return Result.Fail<string>($"Unsupported image format: {settings.ImageFormat}");

        var fullImageName = $"{settings.ImageName}.{settings.ImageFormat}";
        if (settings.OutputPath == "default")
        {
            image.Save(fullImageName);
            return Result.Ok(Path.Combine(Directory.GetCurrentDirectory(), fullImageName));
        }
        image.Save(Path.Combine(settings.OutputPath, fullImageName));
        return Result.Ok(Path.Combine(settings.OutputPath, fullImageName));
    }
}
