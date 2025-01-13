using DocumentFormat.OpenXml.Packaging;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization.FileReaders;

public class WordFileReader(WordFileReaderSettings settings) : IFileReader
{
    public List<string> ReadLines()
    {
        using var document = WordprocessingDocument.Open(settings.FilePath, false);
        var paragraphs = document.MainDocumentPart.Document.Body;
        return paragraphs
            .Select(word => word.InnerText)
            .Where(word => word.Length > 0)
            .ToList();
    }
}