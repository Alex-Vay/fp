using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization.CloudLayouter.CloudGenerators;

public class CloudGenerator(
    ImageSaver saver,
    IFileReader reader,
    BitmapCreator imageCreator,
    IEnumerable<ITextProcessor> processors) : ITagCloudGenerator
{
    private const int MinFontSize = 20;
    private const int MaxFontSize = 100;

    public void GenerateTagCloud()
    {
        var text = ProcessText().GetValueOrThrow();
        var frequencyDict = GetFrequencyDictionary(text);
        var wordSizes = GetWordSizes(frequencyDict);
        var bitmap = imageCreator.CreateBitmap(wordSizes).GetValueOrThrow();
        var savedPath = saver.SaveImage(bitmap).GetValueOrThrow();
        Console.WriteLine("File saved in " + savedPath);
    }

    private Result<List<string>> ProcessText()
    {
        var text = reader.ReadLines();
        foreach (var processor in processors)
            text = text.Then(processor.ProcessText);
        if (text.Value == null || !text.Value.Any())
            return Result.Fail<List<string>>("Processed text is empty.");
        return text;
    }

    private List<KeyValuePair<string, int>> GetFrequencyDictionary(List<string> text)
    {
        var sortedWords = text
            .GroupBy(word => word)
            .OrderByDescending(group => group.Count())
            .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))
            .ToList();
        return sortedWords;
    }

    private List<WordSize> GetWordSizes(List<KeyValuePair<string, int>> frequencyDict)
    {
        var maxFrequency = frequencyDict[0].Value;

        return frequencyDict
            .Select(entry => ToWordSize(entry.Key, entry.Value, maxFrequency))
            .ToList();
    }

    private WordSize ToWordSize(string word, int count, int maxFrequency)
    {
        var fontSize = MinFontSize + (float)count / maxFrequency * (MaxFontSize - MinFontSize);
        return new WordSize(word, (int)fontSize);
    }
}