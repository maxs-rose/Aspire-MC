using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using AspireMCServer.Configuration;

namespace AspireMCServer;

public static class MinecraftServerResourceExtensions
{
    public static IResourceBuilder<MinecraftServerResource> AddMinecraftServer(this IDistributedApplicationBuilder builder, string name)
    {
        var containerBuilder = builder.AddResource(new MinecraftServerResource(name))
            .WithImage(MinecraftContainerConfiguration.Image)
            .WithImageRegistry(MinecraftContainerConfiguration.Registry)
            .WithEndpoint(25565, 25565, name: "http")
            .WithJavaVersion(MinecraftContainerConfiguration.JavaVersion)
            .WithMcVersion(MinecraftContainerConfiguration.McVersion)
            .WithDifficulty(MinecraftContainerConfiguration.DefaultDifficulty)
            .WithMotd(MinecraftContainerConfiguration.DefaultMotd);

        return containerBuilder;
    }

    public static IResourceBuilder<MinecraftServerResource> AcceptEula(this IResourceBuilder<MinecraftServerResource> builder)
    {
        builder.WithEnvironment("EULA", "TRUE");

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithPort(this IResourceBuilder<MinecraftServerResource> builder, ushort port)
    {
        builder.WithEndpoint("http", x => x.Port = port);

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithJavaVersion(this IResourceBuilder<MinecraftServerResource> builder, string version)
    {
        builder.WithImageTag(version);

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithMcVersion(this IResourceBuilder<MinecraftServerResource> builder, string version)
    {
        builder.WithEnvironment("VERSION", version);

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithDifficulty(this IResourceBuilder<MinecraftServerResource> builder, Difficulty difficulty)
    {
        builder.WithEnvironment("DIFFICULTY", DifficultyName(difficulty));

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithMotd(this IResourceBuilder<MinecraftServerResource> builder, string motd)
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