namespace TagsCloudVisualization.FileReaders.Processors;

public interface ITextProcessor
{
    public Result<List<string>> ProcessText(List<string> text);
}
