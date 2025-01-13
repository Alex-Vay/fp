using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization.Visualizers.ImageColoring;

public interface IImageColoring
{
    Color GetNextColor(List<WordSize> text)
    {
        throw new NotImplementedException();
    }

    Color GetNextColor()
    {
        throw new NotImplementedException();
    }
}
