using System.Drawing;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public interface IImageColoring
{
    Color GetNextColor();
}
