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

    public static IResourceBuilder<MinecraftServerResource> WithCurseforgeModpack(this IResourceBuilder<MinecraftServerResource> builder, string apiKey, string packUrl)
    {
        if (builder.Resource.Modpack is not (null or Modpack.Curseforge))
            throw new InvalidOperationException($"The resource already has a modpack set ({builder.Resource.Modpack}.");

        builder.Resource.Modpack = Modpack.Curseforge;
        builder.WithEnvironment("MODPACK_PLATFORM", "AUTO_CURSEFORGE");
        builder.WithEnvironment("CF_API_KEY", apiKey);
        builder.WithEnvironment("CF_PAGE_URL", packUrl);

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithFtbModpack(
        this IResourceBuilder<MinecraftServerResource> builder,
        string modpackId,
        string? version = null,
        bool forceReinstall = false)
    {
        if (builder.Resource.Modpack is not (null or Modpack.Ftb))
            throw new InvalidOperationException($"The resource already has a modpack set ({builder.Resource.Modpack}.");

        builder.Resource.Modpack = Modpack.Ftb;
        builder.WithEnvironment("MODPACK_PLATFORM", "FTBA");
        builder.WithEnvironment("FTB_MODPACK_ID", modpackId);

        if (version is not null)
            builder.WithEnvironment("FTB_MODPACK_VERSION_ID", version);

        if (forceReinstall)
            builder.WithEnvironment("FTB_FORCE_REINSTALL", "true");

        return builder;
    }

    public static IResourceBuilder<MinecraftServerResource> WithModrinthModpack(
        this IResourceBuilder<MinecraftServerResource> builder,
        string modpack,
        ModrinthVersionTypes allowedVersionTypes = ModrinthVersionTypes.Release,
        ModrinthDownladDpendendies downloadDependencies = ModrinthDownladDpendendies.None,
        ModrinthLoader? loader = null)
    {
        if (builder.Resource.Modpack is not (null or Modpack.Modrinth))
            throw new InvalidOperationException($"The resource already has a modpack set ({builder.Resource.Modpack}.");

        builder.Resource.Modpack = Modpack.Modrinth;
        builder.WithEnvironment("MODPACK_PLATFORM", "MODRINTH");
        builder.WithEnvironment("MODRINTH_MODPACK", modpack);
        builder.WithEnvironment(
            "MODRINTH_ALLOWED_VERSION_TYPES",
            allowedVersionTypes switch
            {
                ModrinthVersionTypes.Release => "release",
                ModrinthVersionTypes.Alpha => "alpha",
                ModrinthVersionTypes.Beta => "beta",
                _ => throw new ArgumentOutOfRangeException(nameof(allowedVersionTypes), allowedVersionTypes, null)
            });
        builder.WithEnvironment(
            "MODRINTH_DOWNLOAD_DEPENDENCIES",
            downloadDependencies switch
            {
                ModrinthDownladDpendendies.None => "none",
                ModrinthDownladDpendendies.Required => "required",
                ModrinthDownladDpendendies.Optional => "optional",
                _ => throw new ArgumentOutOfRangeException(nameof(downloadDependencies), downloadDependencies, null)
            });

        if (loader is not null)
            builder.WithEnvironment(
                "MODRINTH_LOADER",
                loader switch
                {
                    ModrinthLoader.Forge => "forge",
                    ModrinthLoader.Fabric => "fabric",
                    ModrinthLoader.Quilt => "quilt",
                    _ => throw new ArgumentOutOfRangeException(nameof(loader), loader, null)
                });

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