using System.Text;
using FluentAssertions;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests
{
    public class TxtFileReaderTests
    {
        [Test]
        public void ReadLines_ReturnCorrect_WhenReadWordsFromFile()
        {
            var fileReaderSettings = new TxtFileReaderSettings("./../../../TestData/text.txt");
            var reader = new TxtFileReader(fileReaderSettings);

            var result = reader.ReadLines();

            result.Should().BeEquivalentTo("Всем", "Привет", "Этот", "файл", "должен", "обрабатываться", "корректно");
        }

        [Test]
        public void ReadLines_ShouldThrow_WhenFileDoesNotExists()
        {
            var fileReaderSettings = new TxtFileReaderSettings("text ttt ty");
            var reader = new TxtFileReader(fileReaderSettings);

            Action act = () => reader.ReadLines();

            act.Should().Throw<FileNotFoundException>();
        }
    }
}