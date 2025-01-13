using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualization;

public class FileReadersFactory(
    CsvFileReaderSettings csvSettings,
    TxtFileReaderSettings txtSettings,
    WordFileReaderSettings wordSettings,
    Options settings)
{
    public IFileReader CreateReader()
    {
        var path = settings.FilePath;
        var extension = Path.GetExtension(path).ToLower();

        return extension switch
        {
            ".txt" => new TxtFileReader(txtSettings),
            ".csv" => new CsvFileReader(csvSettings),
            ".docx" => new WordFileReader(wordSettings),
            _ => throw new InvalidOperationException($"Unsupported file extension: {extension}")
        };
    }
}
