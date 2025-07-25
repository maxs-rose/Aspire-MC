using Aspire.Hosting.ApplicationModel;

namespace AspireMCServer;

public sealed class MinecraftServerResource : ContainerResource
{
    internal MinecraftServerResource(string name) : base(name)
    {
    }
}