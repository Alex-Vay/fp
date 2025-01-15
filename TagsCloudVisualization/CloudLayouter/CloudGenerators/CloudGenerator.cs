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
            text = text.Then(processor.ProcessText);

        var frequencyDict = text
            .Then(w => w
            .GroupBy(word => word)
            .OrderByDescending(group => group.Count())
            .ToDictionary(group => group.Key, group => group.Count()));

        frequencyDict
            .Then(dict =>
            {
                var maxFrequency = dict.Values.Max();
                return dict.Select(pair => ToWordSize(pair, maxFrequency)).ToList();
            })
            .Then(imageCreator.CreateBitmap)
            .Then(saver.SaveImage)
            .Then(path => Console.WriteLine("File saved in " + path))
            .OnFail(err => Console.WriteLine("Generating finished with error: " + err));
    }

    private WordSize ToWordSize(KeyValuePair<string, int> pair, int maxFreq)
    {
        var fontSize = (int)(MIN_FONTSIZE + (float)pair.Value / maxFreq * (MAX_FONTSIZE - MIN_FONTSIZE));
        return new(pair.Key, fontSize);
    }

}