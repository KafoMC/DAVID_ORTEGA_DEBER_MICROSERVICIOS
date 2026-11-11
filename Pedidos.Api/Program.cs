using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Pedidos.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pedidos.Api",
        Version = "v1",
        Description = "API para clientes, pedidos y sus detalles."
    });
});
builder.Services.AddDbContext<PedidosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosConnection")));
builder.Services.AddCors(options => options.AddPolicy("LocalSwagger", policy => policy
    .WithOrigins(
        "http://localhost:5149",
        "http://localhost:5069",
        "http://localhost:5079",
        "https://localhost:44316",
        "https://localhost:7228",
        "https://localhost:7244",
        "https://localhost:7290")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();
app.UseCors("LocalSwagger");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("https://localhost:44316/swagger/v1/swagger.json", "Inventario.Api v1");
        options.SwaggerEndpoint("https://localhost:7244/swagger/v1/swagger.json", "Catalogo.Api v1");
        options.SwaggerEndpoint("https://localhost:7290/swagger/v1/swagger.json", "Pedidos.Api v1");
    });
}

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { servicio = "Pedidos.Api", estado = "activo" }));
app.UseAuthorization();
app.MapControllers();
app.Run();
