using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using sodoff.Configuration;
using sodoff.Middleware;
using sodoff.Model;
using sodoff.Services;
using sodoff.Utils;
using System.Xml;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AssetServerConfig>(builder.Configuration.GetSection("AssetServer"));
builder.Services.Configure<ApiServerConfig>(builder.Configuration.GetSection("ApiServer"));
builder.Services.AddControllers(options => {
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter(new XmlWriterSettings() { OmitXmlDeclaration = false }));
    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    options.Filters.Add<LogRequestOnError>();
});
builder.Services.AddDbContext<DBContext>();

builder.Services.AddSingleton<ModdingService>();
builder.Services.AddSingleton<MissionStoreSingleton>();
builder.Services.AddSingleton<AchievementStoreSingleton>();
builder.Services.AddSingleton<ItemService>();
builder.Services.AddSingleton<StoreService>();
builder.Services.AddSingleton<DisplayNamesService>();

builder.Services.AddScoped<KeyValueService>();
builder.Services.AddScoped<MissionService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<GameDataService>();
builder.Services.AddScoped<ProfileService>();

bool assetServer = builder.Configuration.GetSection("AssetServer").GetValue<bool>("Enabled");
string assetIP = builder.Configuration.GetSection("AssetServer").GetValue<string>("ListenIP");
int assetPort = builder.Configuration.GetSection("AssetServer").GetValue<int>("Port");
if (assetServer)
    builder.Services.Configure<KestrelServerOptions>(options => {
        if (String.IsNullOrEmpty(assetIP) || assetIP == "*")
            options.ListenAnyIP(assetPort);
        else
            options.Listen(IPAddress.Parse(assetIP), assetPort);
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<DBContext>().Database.EnsureCreated();

// create Modding Service singleton ... do this before start http server to avoid serve invalid assets list

new ModdingService();

// Configure the HTTP request pipeline.

if (assetServer)
    app.UseMiddleware<AssetMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
