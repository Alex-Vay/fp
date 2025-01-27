using System.Text;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.FileReaders;

public class TxtFileReader(TxtFileReaderSettings settings) : IFileReader
{
    public Result<List<string>> ReadLines()
    {
        if (!File.Exists(settings.FilePath))
            return Result.Fail<List<string>>("File not found");

        return File.ReadAllLines(settings.FilePath, Encoding.UTF8)
        .Select(line => line.Split())
        .SelectMany(mas => mas)
        .Where(line => line.Length > 0)
        .ToList();
    }     
}
