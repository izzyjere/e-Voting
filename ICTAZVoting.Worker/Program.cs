using ICTAZEVoting.Api.Hubs;

using ICTAZVoting.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
//builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapHub<BlockChainHub>("/blockchain");

app.Run();
