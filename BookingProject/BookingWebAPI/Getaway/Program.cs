using AspNetCoreRateLimit;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromMemory(
    [
        new RouteConfig
        {
            RouteId = "route_to_monolith",
            ClusterId = "monolith_cluster",
            Match = new RouteMatch { Path = "/api/apartments/{**catch-all}" }
        },
        new RouteConfig
        {
            RouteId = "route_to_audit",
            ClusterId = "audit_cluster",
            Match = new RouteMatch { Path = "/api/Audits/{**catch-all}" }
        }
    ],
    [
        new ClusterConfig
        {
            ClusterId = "monolith_cluster",
            LoadBalancingPolicy = "RoundRobin",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "monolith_api", new DestinationConfig { Address = "http://localhost:5001" } }
            }
        },
        new ClusterConfig
        {
            ClusterId = "audit_cluster",
            LoadBalancingPolicy = "RoundRobin",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "audit_api", new DestinationConfig { Address = "http://localhost:5002" } },
                { "audit_api2", new DestinationConfig { Address = "http://localhost:5003" } }
            }
        }
    ]);


builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*", 
            Period = "1m",   
            Limit = 100     
        }
    };
});
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseIpRateLimiting();


app.MapReverseProxy();


app.MapControllers();


app.Run();
