using Microsoft.EntityFrameworkCore;
using Backend_dotNet.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Enable OpenAPI documentation (Microsoft standard)
builder.Services.AddOpenApi();

// Enable Swashbuckle Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestdbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// MapOpenApi provides the /openapi/v1.json endpoint
app.MapOpenApi();

// Enable Swagger UI at /swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.RoutePrefix = "swagger";
});

app.UseCors();

// Root health check
app.MapGet("/", () => "API is running! Visit /swagger for documentation.");

app.MapControllers();

app.Run();

app.MapGet("/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});
