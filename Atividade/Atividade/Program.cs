using Atividade.Services;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<StreamingRepository>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Streaming",
        Version = "v1",
        Description = "API simples para consultar catalogo, conteudos em alta e planos de um servico de streaming."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "API de Streaming";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Streaming v1");
        options.DisplayRequestDuration();
        options.DefaultModelsExpandDepth(2);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    projeto = "API de Streaming",
    documentacao = "/swagger",
    endpoints = new[]
    {
        "/api/dashboard/resumo",
        "/api/catalogo",
        "/api/catalogo/em-alta",
        "/api/catalogo/{id}",
        "/api/planos",
        "/api/planos/{id}",
        "/api/assinantes",
        "/api/assinantes/{id}",
        "/api/assinantes/{id}/perfis"
    }
}));

app.Run();
