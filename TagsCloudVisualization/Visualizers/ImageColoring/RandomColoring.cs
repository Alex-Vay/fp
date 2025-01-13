using System.Drawing;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public class RandomColoring : IImageColoring
{
    private Random random = new Random();

    public Color GetNextColor() => 
        Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
}
