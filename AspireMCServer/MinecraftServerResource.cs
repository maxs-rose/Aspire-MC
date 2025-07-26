using Aspire.Hosting.ApplicationModel;
using AspireMCServer.Configuration;

namespace AspireMCServer;

public sealed class MinecraftServerResource : ContainerResource
{
    internal MinecraftServerResource(string name) : base(name)
    {
    }

    public Modpack? Modpack { get; internal set; }
}