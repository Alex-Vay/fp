﻿using FluentAssertions;
using System.Text;
using TagsCloudVisualization;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.Settings;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class LowercaseProcessorTests
{
    private readonly LowercaseTransformer processor = new();

    [Test]
    public void LowercaseProcessor_ProcessText_ShouldLowercaseAllWords()
    {
        var fileReaderSettings = new TxtFileReaderSettings("TestData/text.txt");
        var reader = new TxtFileReader(fileReaderSettings);

        var text = reader.ReadLines();
        var processed = text.Then(processor.ProcessText);

        processed.IsSuccess.Should().BeTrue();
        processed.GetValueOrThrow().Should().BeEquivalentTo("всем", "привет", "этот", "файл", "должен", "обрабатываться", "корректно");
    }
}