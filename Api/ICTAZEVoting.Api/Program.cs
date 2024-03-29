using ICTAZEVoting.Api;
using ICTAZEVoting.Api.Hubs;
using ICTAZEVoting.Core.Extensions;
using ICTAZEVoting.Core.Middleware;
using ICTAZEVoting.Core.Models;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDatabase(connectionString);
builder.Services.ConfigureAppSettings(builder.Configuration);
builder.Services.AddServerServices();
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(o => {
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddSignalR();
builder.Services.RegisterSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication()
   .UseAuthorization(); 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/files"
});
app.UseMiddleware<SignInMiddleware<User>>();
app.UseHttpsRedirection();
app.MapEndpointRoutes();
app.MapHub<BlockChainHub>("/blockchain");
app.Initialize();
await app.CleanUneccessaryFiles();
app.Run();
