using DotNetEnv;
using InventoryService.Src.Data;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = $"Host={Environment.GetEnvironmentVariable("SUPABASE_HOST")};" +
                      $"Port={Environment.GetEnvironmentVariable("SUPABASE_PORT")};" +
                      $"Database={Environment.GetEnvironmentVariable("SUPABASE_DATABASE")};" +
                      $"Username={Environment.GetEnvironmentVariable("SUPABASE_USERNAME")};" +
                      $"Password={Environment.GetEnvironmentVariable("SUPABASE_PASSWORD")};" +
                      "Pooling=true;SSL Mode=Require;Trust Server Certificate=true;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

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
app.Run();