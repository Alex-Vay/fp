using System.Drawing;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public class BlackColoring : IImageColoring
{
    public Color GetNextColor() => Color.Black;
}
