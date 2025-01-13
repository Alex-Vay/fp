using FluentAssertions;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests;

public class CsvFileReaderTests
{
    [Test]
    public void ReadLines_ShouldCorrect_WhenReadWordsFromFile()
    {
        var fileReaderSettings = new CsvFileReaderSettings("./../../../TestData/text.csv");
        var reader = new CsvFileReader(fileReaderSettings);

        var result = reader.ReadLines();

        result.Should().BeEquivalentTo("Всем", "Привет", "Этот", "файл", "должен", "обрабатываться", "корректно");
    }

    [Test]
    public void ReadLines_ShouldThrow_WhenFileDoesNotExists()
    {
        var fileReaderSettings = new CsvFileReaderSettings("text ttt ty");
        var reader = new CsvFileReader(fileReaderSettings);

        Action act = () => reader.ReadLines();

        act.Should().Throw<FileNotFoundException>();
    }
}