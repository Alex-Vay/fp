using System.Drawing;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter(IPointsGenerator pointGenerator) : ICircularCloudLayouter
{
    public List<Rectangle> GeneratedRectangles { get; } = new ();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
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