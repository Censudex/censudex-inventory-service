using DotNetEnv;
using InventoryService.Src.Consumers;
using InventoryService.Src.Data;
using InventoryService.Src.Grpc;
using InventoryService.Src.Interface;
using InventoryService.Src.Messages;
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
    options.ListenLocalhost(5110, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

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
    // Registrar el consumer para escuchar order.created
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // Configurar host de RabbitMQ
        cfg.Host(
            Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
            "/",
            h =>
            {
                h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest");
                h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest");
            });

        // === MENSAJES QUE PUBLICAS (Producer) ===
        
        // Configurar StockAlertMessage (stock.low)
        cfg.Message<StockAlertMessage>(e =>
        {
            e.SetEntityName("inventory_events");
        });

        cfg.Publish<StockAlertMessage>(e =>
        {
            e.ExchangeType = "topic";
        });

        cfg.Send<StockAlertMessage>(s =>
        {
            s.UseRoutingKeyFormatter(context => "stock.low");
        });
        

        // Configurar OrderFailedStockMessage (order.failed.stock)
        cfg.Message<OrderFailedStockMessage>(e =>
        {
            e.SetEntityName("order_events");
        });

        cfg.Publish<OrderFailedStockMessage>(e =>
        {
            e.ExchangeType = "topic";
        });

        cfg.Send<OrderFailedStockMessage>(s =>
        {
            s.UseRoutingKeyFormatter(context => "order.failed.stock");
        });

        // === MENSAJES QUE CONSUMES (Consumer) ===

        cfg.ReceiveEndpoint("stock-low-temp-queue", e =>
        {
            e.Bind("inventory_events", x =>
            {
                x.RoutingKey = "stock.low";
                x.ExchangeType = "topic";
            });
        });

        cfg.ReceiveEndpoint("order-failed-temp-queue", e =>
        {
            e.Bind("order_events", x =>
            {
                x.RoutingKey = "order.failed.stock";
                x.ExchangeType = "topic";
            });
        });

        // Configurar cola para escuchar order.created
        cfg.ReceiveEndpoint("inventory-order-queue", e =>
        {
            // Configurar el consumer
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
            
            // Bind a la cola con el routing key order.created
            e.Bind("order_events", x =>
            {
                x.RoutingKey = "order.created";
                x.ExchangeType = "topic";
            });
        });

        // Configurar endpoints automáticamente
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