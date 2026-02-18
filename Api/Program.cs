using Api.Middlewares;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SystemTask.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        builder.Services.AddScoped<ITaskItemService, TaskItemService>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("Default")));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            // Aqui estamos configurando o Swagger (via Swashbuckle) para gerar a documentação da API.
            // O método AddSwaggerGen recebe um delegate onde você pode customizar o comportamento.

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // Essa linha monta o nome do arquivo XML de documentação.
            // O Assembly.GetExecutingAssembly() pega o assembly atual (o projeto da API).
            // .GetName().Name retorna o nome do assembly (ex: "Api").
            // No final, concatena ".xml" → "Api.xml".
            // Esse arquivo é gerado automaticamente pelo compilador quando você habilita <GenerateDocumentationFile> no .csproj.

            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // Aqui ele monta o caminho completo até o arquivo XML.
            // AppContext.BaseDirectory aponta para a pasta onde a aplicação está rodando.
            // Assim, xmlPath fica algo como ".../bin/Release/net8.0/Api.xml".

            c.IncludeXmlComments(xmlPath);
            // Essa linha diz ao Swagger para incluir os comentários XML na documentação.
            // Ou seja, todos os "/// <summary>" e "/// <param>" que você escreveu nos controllers
            // vão aparecer na interface do Swagger UI como descrições dos endpoints.
        });



        var app = builder.Build();

        // Middleware de tratamento global de exceções
        app.UseMiddleware<ExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}