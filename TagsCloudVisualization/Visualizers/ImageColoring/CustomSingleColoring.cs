using System.Drawing;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public class CustomSingleColoring(Color color) : IImageColoring
{
    public Color GetNextColor() => color;
}
