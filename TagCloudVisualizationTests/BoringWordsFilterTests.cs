using FluentAssertions;
using System.Diagnostics;
using TagsCloudVisualization;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class BoringWordsFilterTests
{
    private string path = "./../../../TestData/text.txt";
    private TxtFileReader reader;
    private BoringWordsFilter filter;

    [SetUp]
    public void Init()
    {
        filter = new BoringWordsFilter();
        var fileReaderSettings = new TxtFileReaderSettings(path);
        reader = new TxtFileReader(fileReaderSettings);
    }

    [Test]
    public void BoringWordFilter_FilterText_ShouldExcludeAllBoringWords()
    {
        var text = reader.ReadLines();
        var filtered = text.Then(filter.ProcessText);

        filtered.Then(r => r.Should().BeEquivalentTo("Привет", "файл", "должен", "обрабатываться", "корректно"));
    }

    [Test]
    public void BoringWordFilter_AddBoringPartOfSpeech_ShouldExcludeAllBoringWords()
    {
        var text = reader.ReadLines();
        filter.AddBoringPartOfSpeech("S");
        var filtered = text.Then(filter.ProcessText);

        filtered.Then(r => r.Should().BeEquivalentTo("должен", "обрабатываться", "корректно"));
    }

    [Test]
    public void BoringWordFilter_AddBoringPartOfSpeech_ShouldExcludeBoringWord()
    {
        var text = reader.ReadLines();
        filter.AddBoringPartOfSpeech("должен");
        var filtered = text.Then(filter.ProcessText);

        filtered.Then(r => r.Should().BeEquivalentTo("Привет", "файл", "обрабатываться", "корректно"));
    }
}