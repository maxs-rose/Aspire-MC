using AspireMCServer;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddMcServer("the-craft")
    .WithPort(23455)
    .AcceptEula();

builder.Build().Run();