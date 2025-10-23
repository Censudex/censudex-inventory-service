using DotNetEnv;
using InventoryService.Src.Data;
using InventoryService.Src.Grpc;
using InventoryService.Src.Interface;
using InventoryService.Src.Repositories;
using InventoryService.Src.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService.Src.Service.InventoryService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, o =>
    {
        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var connectionString = $"Host={Environment.GetEnvironmentVariable("SUPABASE_HOST")};" +
                      $"Port={Environment.GetEnvironmentVariable("SUPABASE_PORT")};" +
                      $"Database={Environment.GetEnvironmentVariable("SUPABASE_DATABASE")};" +
                      $"Username={Environment.GetEnvironmentVariable("SUPABASE_USERNAME")};" +
                      $"Password={Environment.GetEnvironmentVariable("SUPABASE_PASSWORD")};" +
                      "Pooling=true;SSL Mode=Require;Trust Server Certificate=true;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.Send<StockAlertMessage>(s =>
        {
            s.UseRoutingKeyFormatter(context => "stock.low");
        });          

        cfg.ConfigureEndpoints(context);       
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    var seeder = new DataSeeder(dbContext);
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGrpcService<InventoryGrpcService>();
app.Run();