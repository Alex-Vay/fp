using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter.PointsGenerators;

public interface IPointsGenerator
{
    Point GetNextPointPosition();
}
