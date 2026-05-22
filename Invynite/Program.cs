using Invynite.Infrastructure.Data;
using Invynite.Middlewares;
using Invynite.Services.BOM;
using Invynite.Services.Inventories;
using Invynite.Services.Procurement;
using Invynite.Services.Productions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddOpenApi();

// exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// business logic services
builder.Services.AddScoped<IBillOfMaterialService, BillOfMaterialService>();
builder.Services.AddScoped<IProductionOrderService, ProductionOrderService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();

// build app
var app = builder.Build();

using var scope = app.Services.CreateScope();
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // DANGER: Only use this in Development!
    // This wipes the DB and applies all migrations from scratch.
    await context.Database.EnsureDeletedAsync();
    //await context.Database.MigrateAsync();

    //await Seeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Invynite V1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
