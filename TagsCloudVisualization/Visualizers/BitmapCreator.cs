using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Settings;
using TagsCloudVisualization.Visualizers.ImageColoring;

namespace TagsCloudVisualization.Visualizers;

public class BitmapCreator
    
{
    private Size size;
    private FontFamily family;
    private IImageColoring background;
    private IImageColoring coloring;
    private ICircularCloudLayouter layouter;

    public BitmapCreator(ImageSettings settings, ICircularCloudLayouter layouter)
    {
        size = settings.Size;
        family = settings.Font;
        background = settings.BackgroundColor;
        coloring = settings.Coloring;
        this.layouter = layouter;
    }

    public Bitmap CreateBitmap(
        List<WordSize> text
        )
    {
        var bitmap = new Bitmap(size.Width, size.Height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(background.GetNextColor());
        foreach (var word in text)
        {
            var wordColor = new SolidBrush(coloring.GetNextColor());
            var font = new Font(family, word.FontSize);
            var wordSize = CeilSize(graphics.MeasureString(word.Word, font));
            var rectPosition = layouter.PutNextRectangle(wordSize);
            graphics.DrawRectangle(new Pen(Color.White), rectPosition);
            graphics.DrawString(word.Word, font, wordColor, rectPosition);
        }
        return bitmap;
    }

    private static Size CeilSize(SizeF size)
        => new((int)size.Width + 1, (int)size.Height + 1);
}