using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization.CloudLayouter.CloudGenerators;

public class CloudGenerator(
    ImageSaver saver,
    IFileReader reader,
    BitmapCreator imageCreator,
    IEnumerable<ITextProcessor> processors) : ITagCloudGenerator
{
    private int MIN_FONTSIZE = 20;
    private int MAX_FONTSIZE = 100;

    public void GenerateTagCloud()
    {
        var text = reader.ReadLines();
        foreach (var processor in processors)
            text = processor.ProcessText(text);

        var frequencyDict = text
            .GroupBy(word => word)
            .OrderByDescending(group => group.Count())
            .ToDictionary(group => group.Key, group => group.Count());

        var maxFrequency = frequencyDict.Values.Max();
        var tagsList = frequencyDict.Select(pair => ToWordSize(pair, maxFrequency)).ToList();
        var imageSavePath = saver.SaveImage(imageCreator.CreateBitmap(tagsList));

        Console.WriteLine("File saved in " + imageSavePath);
    }

    private WordSize ToWordSize(KeyValuePair<string, int> pair, int maxFreq)
    {
        var fontSize = (int)(MIN_FONTSIZE + (float)pair.Value / maxFreq * (MAX_FONTSIZE - MIN_FONTSIZE));
        return new(pair.Key, fontSize);
    }

}