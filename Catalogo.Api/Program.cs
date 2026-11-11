using Catalogo.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogo.Api", Version = "v1" }));
builder.Services.AddDbContext<CatalogoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogoConnection")));
builder.Services.AddCors(options => options.AddPolicy("LocalSwagger", policy => policy
    .WithOrigins("https://localhost:7228", "https://localhost:44316", "https://localhost:7244", "https://localhost:7290")
    .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("https://localhost:44316/swagger/v1/swagger.json", "Inventario.Api v1");
        options.SwaggerEndpoint("https://localhost:7244/swagger/v1/swagger.json", "Catalogo.Api v1");
        options.SwaggerEndpoint("https://localhost:7290/swagger/v1/swagger.json", "Pedidos.Api v1");
    });
}
app.UseCors("LocalSwagger");
app.UseHttpsRedirection();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { servicio = "Catalogo.Api", estado = "activo" }));
app.MapControllers();
app.Run();
