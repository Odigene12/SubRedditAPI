using Microsoft.Net.Http.Headers;
using SubRedditAPI.Configurations;
using SubRedditAPI.Interfaces;
using SubRedditAPI.Repositories;
using SubRedditAPI.Services;
using Serilog;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();




// Access user secrets to use Reddit API
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<RedditAPIConfig>();
}

// Otherwise, I would use the environment variables to access the Reddit API credentials in production.

var redditApiCredentials = new RedditAPIConfig();
builder.Configuration.Bind("RedditApiCredentials", redditApiCredentials);
builder.Services.AddSingleton(redditApiCredentials);

builder.Services.AddScoped<RedditRepository>();
builder.Services.AddScoped<IRedditRepository, RedditCacheRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IRedditOAuthService, RedditOAuthService>();
builder.Services.AddScoped<IRedditService, RedditService>();
builder.Services.AddScoped<IRateLimitService, RateLimitService>();

builder.Host.UseSerilog((context, logConfig) => logConfig
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(context.Configuration));

Log.Information("Starting up");

// Register the RedditService
builder.Services.AddHttpClient("RedditClient" , client =>
{
    client.BaseAddress = new Uri("https://www.reddit.com/api/v1/");
    client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "trafficAPI/1.0.0 (by /u/Zealousideal-Arm9079)");
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
