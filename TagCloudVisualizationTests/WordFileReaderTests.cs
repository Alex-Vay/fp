using System.Text;
using FluentAssertions;
using TagsCloudVisualization;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests
{
    public class WordFileReaderTests
    {
        [Test]
        public void ReadLines_ReturnCorrect_WhenReadWordsFromFile()
        {
            var fileReaderSettings = new WordFileReaderSettings("TestData/text.docx");
            var reader = new WordFileReader(fileReaderSettings);

            var result = reader.ReadLines();

            result.IsSuccess.Should().BeTrue();
            result.GetValueOrThrow().Should().BeEquivalentTo("Всем", "Привет", "Этот", "файл", "должен", "обрабатываться", "корректно");
        }

        [Test]
        public void ReadLines_ShouldThrow_WhenFileDoesNotExists()
        {
            var fileReaderSettings = new WordFileReaderSettings("text ttt ty");
            var reader = new WordFileReader(fileReaderSettings);

            var result = reader.ReadLines();

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().BeEquivalentTo("File not found");
        }
    }
}