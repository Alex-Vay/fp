using System.Drawing;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter(IPointsGenerator pointGenerator) : ICircularCloudLayouter
{
    public List<Rectangle> GeneratedRectangles { get; } = new ();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException($"{nameof(rectangleSize)} height and width must be greater than zero");
        Rectangle rectangle;
        do
        {
            rectangle = GetNextRectangle(rectangleSize);
        } while (GeneratedRectangles.Any(rectangle.IntersectsWith));
        GeneratedRectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle GetNextRectangle(Size rectangleSize)
    {
        var rectanglePosition = pointGenerator.GetNextPointPosition();
        return CreateRectangle(rectanglePosition, rectangleSize);
    }

    private static Rectangle CreateRectangle(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }
}