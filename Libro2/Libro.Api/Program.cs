using Libro.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<LibroDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibrosConnection")));

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Libro.Api", Version = "v1" });
});
builder.Services.AddCors(options => options.AddPolicy("LocalSwagger", policy => policy
    .WithOrigins(
        "http://localhost:5149", "https://localhost:44316", "https://localhost:7228", "https://localhost:7244")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();
app.UseCors("LocalSwagger");
app.MapGet("/health", () => Results.Ok(new { servicio = "Libro.Api", estado = "activo" }));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Libro.Api v1");
        options.SwaggerEndpoint("https://localhost:7228/swagger/v1/swagger.json", "Inventario.Api v1");
    });
}

if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
