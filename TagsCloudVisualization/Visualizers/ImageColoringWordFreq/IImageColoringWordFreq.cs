using System.Drawing;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualization.Visualizers.ImageColoringWordFreq;

public interface IImageColoringWordFreq
{
    Color GetNextColor(List<WordSize> text);
}
