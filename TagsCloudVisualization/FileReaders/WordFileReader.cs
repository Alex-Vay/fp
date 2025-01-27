using DocumentFormat.OpenXml.Packaging;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.FileReaders;

public class WordFileReader(WordFileReaderSettings settings) : IFileReader
{
    public Result<List<string>> ReadLines()
    {
        if (!File.Exists(settings.FilePath))
            return Result.Fail<List<string>>("File not found");
        using var document = WordprocessingDocument.Open(settings.FilePath, false);
        var paragraphs = document.MainDocumentPart?.Document.Body;
        if (paragraphs == null)
            return Result.Fail<List<string>>("The document body is null");
        return paragraphs
            .Select(word => word.InnerText)
            .Where(word => word.Length > 0)
            .ToList();
    }
}