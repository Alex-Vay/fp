using System.Drawing;
using CommandLine;
using TagsCloudVisualization.Visualizers.ImageColoring;

public class Options
{
    [Value(0, Required = true, HelpText = "Source file path (default: project dir)")]
    public string FilePath { get; set; }

    [Option('o', "output-path",
    Required = false,
    HelpText = "Image output path")]
    public string OutputPath { get; set; } = "default";

    [Option('w', "width",
    Required = false,
    HelpText = "Image width (positive integer)")]
    public int Width { get; set; } = 2000;

    [Option('h', "height",
        Required = false,
        HelpText = "Image height (positive integer)")]
    public int Height { get; set; } = 2000;

    public Size Size
    {
        get
        {
            return new Size(Width, Height);
        }
    }

    [Option('f', "font",
        Required = false,
        HelpText = "Words font")]
    public string FontFamilyName { get; set; } = "Calibri";

    public FontFamily Font
    {
        get
        {
            try
            {
                return new FontFamily(FontFamilyName);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Invalid font name: '{FontFamilyName}'. Make sure the font is installed on your system.");
            }
        }
    }

    [Option('b', "background-color",
        Required = false,
        HelpText = "Background color (format: 'black' or 'random' or '0xRRGGBB')")]
    public string BackgroundColorType { get; set; } = "black";

    public IImageColoring BackgroundColor
    {
        get => GetColoringAlg(BackgroundColorType);
    }

    [Option('c', "word-color",
        Required = false,
        HelpText = "Words color (format: 'black' or 'random' or '0xRRGGBB')")]
    public string WordColorType { get; set; } = "random";

    public IImageColoring ForegroundColor
    {
        get => GetColoringAlg(WordColorType);
    }

    private IImageColoring GetColoringAlg(string colotStr) =>
        colotStr.ToLower() switch
        {
            "black" => new BlackColoring(),
            "random" => new RandomColoring(),
            _ when char.IsDigit(colotStr[0]) =>
                new CustomSingleColoring(
                    Color.FromArgb(255,
                    Color.FromArgb(Convert.ToInt32(colotStr, 16)))),
            _ => throw new ArgumentException($"Invalid color type: '{colotStr}'. Use 'black' or 'random'.")
        };

    [Option("image-name",
        Required = false,
        HelpText = "Image name (format: 'name')")]
    public string ImageName { get; set; } = "result";

    [Option("image-format",
        Required = false,
        HelpText = "Image format (format: format)")]
    public string ImageFormat { get; set; } = "png";

    [Option("angle-offset",
        Required = false,
        HelpText = "Angle offset of the angle for the spiral")]
    public double AngleOffset { get; set; } = 0.1;

    [Option("step",
        Required = false,
        HelpText = "Step between the turns of the spiral")]
    public double Step { get; set; } = 0.1;

    [Option("center",
        Required = false,
        HelpText = "The center of the cloud in the image (format: 'x,y')")]
    public string CenterInput { get; set; }
    public Point Center
    {
        get
        {
            var defaultCenter = new Point(Width / 2, Height / 2);
            if (!string.IsNullOrEmpty(CenterInput))
            {
                var parts = CenterInput.Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out var x) &&
                    int.TryParse(parts[1], out var y))
                {
                    return new Point(x, y);
                }
                throw new ArgumentException($"Invalid center format: '{CenterInput}'. Use 'x,y', e.g., '1000,1000'.");
            }
            return defaultCenter;
        }
    }
}