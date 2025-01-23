using System.Text.Json.Serialization;
using Lotus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Lotus.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados
builder.Services.AddDbContext<MLotusContext>(options =>
    options.UseNpgsql("Host=localhost;Database=lotus;Username=postgres;Password=1a2badmin#$#"));

// Configuração de serialização JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

// Adicionar controladores
builder.Services.AddControllers();

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer(); // Necessário para endpoints
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Lotus",
        Version = "v1",
        Description = "Documentação da API Lotus para gerenciar clientes, funcionários e agendamentos.",
        Contact = new OpenApiContact
        {
            Name = "Edna Cardoso Gonçalves",
            Email = "edna.goncalves.peixoto@gmail.com",
            Url = new Uri("https://github.com/ednacardoso")
        }
    });
});

var app = builder.Build();

// Habilitar Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Lotus v1");
        options.RoutePrefix = string.Empty; // Deixa o Swagger disponível na raiz (ex: http://localhost:5000)
    });
}
app.UseDefaultFiles(); // Procura por arquivos padrão como index.html
app.UseStaticFiles();  // Permite servir arquivos estáticos, como HTML, CSS, JS

// Redireciona para index.html se acessar a raiz "/"
app.MapGet("/", context =>
{
    context.Response.Redirect("/index.html");
    return Task.CompletedTask;
});

// Mapear controladores
app.MapControllers();

app.Run();

[JsonSerializable(typeof(Lotus.Models.Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
