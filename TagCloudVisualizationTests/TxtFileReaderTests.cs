using System.Text;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests
{
    public class TxtFileReaderTests
    {
        [Test]
        public void ReadLines_ReturnCorrect_WhenReadWordsFromFile()
        {
            var fileReaderSettings = new TxtFileReaderSettings("TestData/text.txt");
            var reader = new TxtFileReader(fileReaderSettings);

            var result = reader.ReadLines();

            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo("Всем", "Привет", "Этот", "файл", "должен", "обрабатываться", "корректно");
        }

        [Test]
        public void ReadLines_ShouldThrow_WhenFileDoesNotExists()
        {
            var fileReaderSettings = new TxtFileReaderSettings("text ttt ty");
            var reader = new TxtFileReader(fileReaderSettings);

            var result = reader.ReadLines();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("File not found");
        }
    }
}