namespace TagsCloudVisualization.FileReaders.Processors;

public class LowercaseTransformer : ITextProcessor
{
    public Result<List<string>> ProcessText(List<string> text) =>
        text.Select(word => word.ToLower()).ToList();
}
