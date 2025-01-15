using System.Diagnostics;

namespace TagsCloudVisualization.FileReaders.Processors;

public class BoringWordsFilter : ITextProcessor
{
    private HashSet<string> boringPartsOfSpeeches = new() { "PR", "PART", "CONJ" };

    public Result<List<string>> ProcessText(List<string> text)
    {
        var result = new List<string>();
        var stem = MyStem();
        foreach (var word in text)
        {
            stem.StandardInput.Write($"{word}\n");
            var wordInfo = stem.StandardOutput.ReadLine();
            var infoStart = wordInfo.IndexOf("{");
            if (wordInfo is not null && !IsBoring(wordInfo))
                result.Add(wordInfo.Substring(0, infoStart));
        }
        stem.Close();
        stem.Dispose();
        return result;
    }

    private bool IsBoring(string line) =>
        boringPartsOfSpeeches.Any(part => line.Contains(part));

    private Process MyStem()
    {
        var stem = new Process
        {
            StartInfo =
            {
                FileName = "./../../../../mystem.exe",
                Arguments = "-i -n -e cp866",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
            }
        };
        stem.Start();
        return stem;
    }

    public void AddBoringPartOfSpeech(string partOfSpeech) =>
        boringPartsOfSpeeches.Add(partOfSpeech);
}
