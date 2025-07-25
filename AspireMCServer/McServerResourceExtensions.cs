using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using AspireMCServer.Configuration;

namespace AspireMCServer;

public static class McServerResourceExtensions
{
    public static IResourceBuilder<McServerResource> AddMcServer(this IDistributedApplicationBuilder builder, string name)
    {
        var containerBuilder = builder.AddResource(new McServerResource(name))
            .WithImage("itzg/minecraft-server")
            .WithImageRegistry("docker.io")
            .WithEndpoint(25565, 25565, name: "http")
            .WithImageVersion("latest")
            .WithMcVersion("latest")
            .WithDifficulty(Difficulty.Normal)
            .WithMotd("MC Server running with Aspire!!");

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

    public static IResourceBuilder<McServerResource> WithImageVersion(this IResourceBuilder<McServerResource> builder, string version)
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