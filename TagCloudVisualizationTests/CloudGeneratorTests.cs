using System.Drawing;
using FluentAssertions;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;
using TagsCloudVisualization.Visualizers.ImageColoring;
using TagsCloudVisualization.CloudLayouter.CloudGenerators;
using TagsCloudVisualization.Settings;


namespace TagsCloudVisualizationTests;

[TestFixture]
public class CloudGeneratorTest
{
    [Test]
    public void CloudGenerator_GenerateTagCloud_ShouldGenerateFile()
    {
        var generator = InitGenerator();

        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "test.png");

        File.Exists(savePath).Should().BeTrue();
    }

    private static CloudGenerator InitGenerator()
    {
        var fileReaderSettings = new TxtFileReaderSettings("./../../../TestData/text.txt");
        var fileReader = new TxtFileReader(fileReaderSettings);
        var imageSaverSettings = new ImageSaveSettings("test", "png", null);
        var imageSaver = new ImageSaver(imageSaverSettings);
        var pointGeneratorSettings = new SpiralPointsGeneratorSettings(new Point(1000, 1000), 0.1, 0.1);
        var pointGenerator = new SpiralPointsGenerator(pointGeneratorSettings);
        var imageSettings = new ImageSettings(
            new Size(2000, 2000), new FontFamily("Calibri"),
            new BlackColoring(), new RandomColoring());

        var layouter = new CircularCloudLayouter(pointGenerator);
        var imageCreator = new BitmapCreator(imageSettings, layouter);
        List<ITextProcessor> processors = [new LowercaseTransformer(), new BoringWordsFilter()];

        return new CloudGenerator(imageSaver, fileReader, imageCreator, processors);
    }
}