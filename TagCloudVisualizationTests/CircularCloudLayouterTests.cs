using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Visualizers.ImageColoring;
using TagsCloudVisualization.CloudLayouter.CloudGenerators;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests;

[TestFixture, NonParallelizable]
public class CircularCloudLayouterTests
{
    private CircularCloudLayouter cloudLayouter;
    private const int imageWidth = 2000;
    private const int imageHeight = 2000;
    private Point center;
    private CloudGenerator cloudGenerator;

    [SetUp]
    public void Init()
    {
        center = new Point(imageWidth / 2, imageHeight / 2);
        var pointGeneratorSettings = new SpiralPointsGeneratorSettings(center, 0.1, 0.1);
        var pointGenerator = new SpiralPointsGenerator(pointGeneratorSettings);
        cloudLayouter = new CircularCloudLayouter(pointGenerator);

        var fileReaderSettings = new TxtFileReaderSettings("TestData/text.txt");
        var fileReader = new TxtFileReader(fileReaderSettings);

        var imageSaverSettings = new ImageSaveSettings("test", "png", "default");
        var imageSaver = new ImageSaver(imageSaverSettings);

        var imageSettings = new ImageSettings(
            new Size(imageWidth, imageHeight), new FontFamily("Calibri"),
            new BlackColoring(), new RandomColoring());
        var imageGenerator = new BitmapCreator(imageSettings, cloudLayouter);
        List<ITextProcessor> processors = [new LowercaseTransformer(), new BoringWordsFilter()];
        cloudGenerator = new CloudGenerator(imageSaver, fileReader, imageGenerator, processors);
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBeInCenter()
    {
        var rectangleSize = new Size(10, 10);
        var expectedRectangle = new Rectangle(
            center.X - rectangleSize.Width / 2,
            center.Y - rectangleSize.Height / 2,
            rectangleSize.Width,
            rectangleSize.Height
        );

        var actualRectangle = cloudLayouter.PutNextRectangle(rectangleSize);

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test, Parallelizable(ParallelScope.Self)]
    [Repeat(10)]
    public void PutNextRectangle_Rectangles_ShouldNotHaveIntersects() =>
        AreRectanglesHaveIntersects(cloudLayouter.GeneratedRectangles).Should().BeFalse();

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_CloudCenterMust_ShouldBeInLayoterCenter()
    {
        var expectedDiscrepancy = 10;

        cloudGenerator.GenerateTagCloud();

        var actualCenter = GetCenterOfAllRectangles(cloudLayouter.GeneratedRectangles);
        actualCenter.X.Should().BeInRange(center.X - expectedDiscrepancy, center.X + expectedDiscrepancy);
        actualCenter.Y.Should().BeInRange(center.Y - expectedDiscrepancy, center.Y + expectedDiscrepancy);
    }

    [Test]
    public void PutNextRectangle_RectanglesDensity_ShouldBeMax()
    {
        var expectedDensity = 0.3;
        cloudGenerator.GenerateTagCloud();
        var rectangles = cloudLayouter.GeneratedRectangles;

        var rectanglesArea = rectangles.Sum(rect => rect.Width * rect.Height);

        var radius = GetMaxDistanceBetweenRectangleAndCenter(rectangles);
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;
        density.Should().BeGreaterThanOrEqualTo(expectedDensity);
    }

    private Point GetCenterOfAllRectangles(List<Rectangle> rectangles)
    {
        var top = rectangles.Max(r => r.Top);
        var right = rectangles.Max(r => r.Right);
        var bottom = rectangles.Min(r => r.Bottom);
        var left = rectangles.Min(r => r.Left);
        var x = left + (right - left) / 2;
        var y = bottom + (top - bottom) / 2;
        return new(x, y);
    }

    private double GetMaxDistanceBetweenRectangleAndCenter(List<Rectangle> rectangles)
    {
        var center = GetCenterOfAllRectangles(rectangles);
        double maxDistance = -1;
        foreach (var rectangle in rectangles)
        {
            var corners = new Point[4]
            {
                new(rectangle.Top, rectangle.Left),
                new(rectangle.Bottom, rectangle.Left),
                new(rectangle.Top, rectangle.Right),
                new(rectangle.Bottom, rectangle.Right)
            };
            var distance = corners.Max(p => GetDistanceBetweenPoints(p, center));
            maxDistance = Math.Max(maxDistance, distance);
        }
        return maxDistance;
    }

    private static bool AreRectanglesHaveIntersects(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
        return false;
    }

    private static double GetDistanceBetweenPoints(Point point1, Point point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}