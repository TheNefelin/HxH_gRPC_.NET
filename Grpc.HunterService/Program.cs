using ClassLibrary.HxH_Services.Features.Hunters.GetAll;
using ClassLibrary.HxH_Services.Infrastructure;
using Grpc.HunterService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<GetAllHuntersHandler>();

builder.Services.AddSingleton<OracleDbContext>(options =>
{
    var logger = options.GetRequiredService<ILogger<OracleDbContext>>();
    var congectionString = builder.Configuration.GetConnectionString("OracleDB");
    return new OracleDbContext(congectionString, logger);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GrpcHunterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
