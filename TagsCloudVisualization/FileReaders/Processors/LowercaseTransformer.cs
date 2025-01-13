namespace TagsCloudVisualization.FileReaders.Processors;

public class LowercaseTransformer : ITextProcessor
{
    public List<string> ProcessText(List<string> text) =>
        text.Select(word => word.ToLower()).ToList();
}
