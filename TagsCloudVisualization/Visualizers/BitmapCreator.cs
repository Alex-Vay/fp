using DocumentFormat.OpenXml.InkML;
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

    public Result<Bitmap> CreateBitmap(List<WordSize> text)
    {
        var width = size.Width;
        var height = size.Height;
        var bitmap = (width < 0 || height < 0)
            ? Result.Fail<Bitmap>("Width and height should be positive")
            : new Bitmap(width, height).AsResult();

        return bitmap.Then(b =>
        {
            using var graphics = Graphics.FromImage(b);
            graphics.Clear(background.GetNextColor());
            var result = ProcessTags(text, graphics)
                .FirstOrDefault(r => !r.IsSuccess, Result.Ok());
            return result.IsSuccess ? bitmap : Result.Fail<Bitmap>(result.Error!);
        });
    }

    private Result<Font> BuildFont(int fontSize)
        => fontSize > 0
            ? new Font(family, fontSize).AsResult()
            : Result.Fail<Font>("Cannot generate font with negative size");

    private IEnumerable<Result<None>> ProcessTags(List<WordSize> tags, Graphics graphics)
        => tags.Select(t => BuildFont(t.FontSize).Then(f => DrawTag(f, t, graphics)));

    private Result<None> DrawTag(Font font, WordSize wordSize, Graphics graphics)
        => font.AsResult()
            .Then(font => CeilSize(graphics.MeasureString(wordSize.Word, font)))
            .Then(layouter.PutNextRectangle).Then(FitsInRange)
            .Then(rect =>
            {
                var wordColor = new SolidBrush(coloring.GetNextColor());
                graphics.DrawRectangle(new Pen(Color.White), rect);
                graphics.DrawString(wordSize.Word, font, wordColor, rect);
            });

    private Result<Rectangle> FitsInRange(Rectangle rect)
        => new Rectangle(Point.Empty, size).Contains(rect)
            ? Result.Ok(rect)
            : Result.Fail<Rectangle>("Cannot fit in the given size");

    private static Size CeilSize(SizeF size)
        => new((int)size.Width + 1, (int)size.Height + 1);
}