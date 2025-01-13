using TagsCloudVisualization.Settings;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;

namespace TagsCloudVisualization;

public static class SettingsFactory
{
    public static TxtFileReaderSettings BuildFileReaderSettings(Options options)
        => new(options.FilePath);

    public static ImageSettings BuildBitmapSettings(Options options)
        => new(options.Size, options.Font, options.BackgroundColor, options.ForegroundColor);

    public static SpiralPointsGeneratorSettings BuildPolarSpiralSettings(Options options)
        => new(options.Center, options.Step, options.AngleOffset);

    public static CircularCloudLayouterSettings BuildPointLayouterSettings(IPointsGenerator generator)
        => new(generator);

    public static WordFileReaderSettings BuildWordReaderSettings(Options options)
        => new(options.FilePath);

    public static CsvFileReaderSettings BuildCsvReaderSettings(Options options)
        => new(options.FilePath);

    public static ImageSaveSettings BuildFileSaveSettings(Options options)
        => new(options.ImageName, options.ImageFormat, options.OutputPath);
}