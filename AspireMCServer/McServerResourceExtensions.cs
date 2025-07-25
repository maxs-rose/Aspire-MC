using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using AspireMCServer.Configuration;

namespace AspireMCServer;

public static class McServerResourceExtensions
{
    public static IResourceBuilder<McServerResource> AddMinecraftServer(this IDistributedApplicationBuilder builder, string name)
    {
        var containerBuilder = builder.AddResource(new McServerResource(name))
            .WithImage(MinecraftContainerConfiguration.Image)
            .WithImageRegistry(MinecraftContainerConfiguration.Registry)
            .WithEndpoint(25565, 25565, name: "http")
            .WithJavaVersion(MinecraftContainerConfiguration.JavaVersion)
            .WithMcVersion(MinecraftContainerConfiguration.McVersion)
            .WithDifficulty(MinecraftContainerConfiguration.DefaultDifficulty)
            .WithMotd(MinecraftContainerConfiguration.DefaultMotd);

        return containerBuilder;
    }

    public static IResourceBuilder<McServerResource> AcceptEula(this IResourceBuilder<McServerResource> builder)
    {
        builder.WithEnvironment("EULA", "TRUE");

        return builder;
    }

    public static IResourceBuilder<McServerResource> WithPort(this IResourceBuilder<McServerResource> builder, ushort port)
    {
        builder.WithEndpoint("http", x => x.Port = port);

        return builder;
    }

    public static IResourceBuilder<McServerResource> WithJavaVersion(this IResourceBuilder<McServerResource> builder, string version)
    {
        builder.WithImageTag(version);

        return builder;
    }

    public static IResourceBuilder<McServerResource> WithMcVersion(this IResourceBuilder<McServerResource> builder, string version)
    {
        builder.WithEnvironment("VERSION", version);

        return builder;
    }

    public static IResourceBuilder<McServerResource> WithDifficulty(this IResourceBuilder<McServerResource> builder, Difficulty difficulty)
    {
        builder.WithEnvironment("DIFFICULTY", DifficultyName(difficulty));

        return builder;
    }

    public static IResourceBuilder<McServerResource> WithMotd(this IResourceBuilder<McServerResource> builder, string motd)
    {
        builder.WithEnvironment("MOTD", motd);
        return builder;
    }

    private static string DifficultyName(Difficulty difficulty)
    {
        return difficulty switch
        {
            Difficulty.Peaceful => "peaceful",
            Difficulty.Easy => "easy",
            Difficulty.Normal => "normal",
            Difficulty.Hard => "hard",
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
        };
    }
}