namespace AspireMCServer.Configuration;

internal static class MinecraftContainerConfiguration
{
    public const string Image = "itzg/minecraft-server";
    public const string Registry = "docker.io";
    public const string JavaVersion = "latest";
    public const string McVersion = "latest";
    public const Difficulty DefaultDifficulty = Difficulty.Normal;
    public const string DefaultMotd = "Mincecraft running with Aspire!!";
}