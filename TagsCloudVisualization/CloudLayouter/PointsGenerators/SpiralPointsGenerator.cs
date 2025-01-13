using System.Drawing;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.CloudLayouter.PointsGenerators;

public class SpiralPointsGenerator : IPointsGenerator
{
    private readonly double step;
    private readonly double angleOffset;
    private readonly Point center;
    private double currentAngle = 0;

    public SpiralPointsGenerator(SpiralPointsGeneratorSettings settings)
    {
        if (settings.Radius == 0 || settings.AngleOffset == 0)
            throw new ArgumentException($"Step and angleOffset must not be zero");
        center = settings.Center;
        step = settings.Radius;
        angleOffset = settings.AngleOffset;
    }

    public Point GetNextPointPosition()
    {
        var radius = step * currentAngle;
        var x = (int)(center.X + radius * Math.Cos(currentAngle));
        var y = (int)(center.Y + radius * Math.Sin(currentAngle));
        currentAngle += angleOffset;
        return new(x, y);
    }
}