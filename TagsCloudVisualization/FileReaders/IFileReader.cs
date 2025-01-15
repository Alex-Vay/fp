namespace TagsCloudVisualization.FileReaders;

public interface IFileReader
{
    public Result<List<string>> ReadLines();
}
