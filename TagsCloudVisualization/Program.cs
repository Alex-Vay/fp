using Autofac;
using CommandLine;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.CloudLayouter.CloudGenerators;
using TagsCloudVisualization.CloudLayouter.PointsGenerators;
using TagsCloudVisualization.FileReaders;
using TagsCloudVisualization.FileReaders.Processors;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization;

internal class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(settings =>
            {
                var container = BuildContainer(settings);
                var generator = container.Resolve<CloudGenerator>();
                generator.GenerateTagCloud();
            });
    }

    private static IContainer BuildContainer(Options settings)
    {
        var builder = new ContainerBuilder();

        RegisterSettings(builder, settings);
        RegisterLayouters(builder, settings);
        RegisterWordsReaders(builder, settings);
        RegisterWordsProcessors(builder, settings);

        builder.RegisterType<CloudGenerator>().AsSelf();
        builder.RegisterType<BitmapCreator>().AsSelf();
        builder.RegisterType<ImageSaver>().AsSelf();

        return builder.Build();
    }

    private static void RegisterSettings(ContainerBuilder builder, Options settings)
    {
        builder.RegisterInstance(SettingsFactory.BuildBitmapSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileSaveSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildCsvReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildWordReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildFileReaderSettings(settings)).AsSelf();
        builder.RegisterInstance(SettingsFactory.BuildPolarSpiralSettings(settings)).AsSelf();
        builder.Register(context => SettingsFactory.BuildPointLayouterSettings(
            context.Resolve<IPointsGenerator>())).AsSelf();
    }

    private static void RegisterWordsReaders(ContainerBuilder builder, Options settings)
    {
        builder.RegisterType<FileReadersFactory>()
        .WithParameter("settings", settings)
        .AsSelf()
        .SingleInstance();

        builder.Register<IFileReader>(context =>
        {
            var factory = context.Resolve<FileReadersFactory>();
            return factory.CreateReader().GetValueOrThrow();
        })
        .SingleInstance();
    }

    private static void RegisterWordsProcessors(ContainerBuilder builder, Options settings)
    {
        builder.RegisterType<LowercaseTransformer>().As<ITextProcessor>();
        builder.RegisterType<BoringWordsFilter>().As<ITextProcessor>();
    }

    private static void RegisterLayouters(ContainerBuilder builder, Options settings)
    {
        builder
            .RegisterType<SpiralPointsGenerator>().As<IPointsGenerator>();
        builder.RegisterType<CircularCloudLayouter>().As<ICircularCloudLayouter>();
    }
}