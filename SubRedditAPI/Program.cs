using SubRedditAPI.Configurations;

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

// Register the RedditService
builder.Services.AddHttpClient("RedditClient" , client =>
{
    client.BaseAddress = new Uri("https://www.reddit.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "SubRedditAPI");
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
