using AdminBFF._Core;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Corre��o do erro de digita��o
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1"
    });
    // Outras configura��es do Swagger
});


//Database + Context
builder.Services.AddContext(builder.Configuration);

//Services
builder.Services.AddApplicationServices();

// var userName = builder.Configuration.GetSection("ElasticsearchOption")["Username"];
// var password = builder.Configuration.GetSection("ElasticsearchOption")["Password"];
// var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration.GetSection("ElasticsearchOption")["Url"]!)).Authentication( new BasicAuthentication(userName!,password!));
//
// var client= new ElasticsearchClient(settings);
//
// builder.Services.AddSingleton(client);
//
// builder.Services.AddScoped<ElasticsearchService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
