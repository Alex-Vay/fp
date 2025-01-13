using System.Drawing;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public class CustumSingleColoring(Color color) : IImageColoring
{
    public Color GetNextColor() => color;
}
