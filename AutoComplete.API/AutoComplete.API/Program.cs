using AutoComplete.API.Model;
using AutoComplete.API.Service;
using Elasticsearch.Net;
using Nest;

var builder = WebApplication.CreateBuilder(args);

var node = new Uri("https://localhost:9200");
var username = "elastic"; // Replace with your Elasticsearch username
var password = "5LBR-s7dDQbefu2MtA2m"; // Replace with your Elasticsearch password
var connectionPool = new SingleNodeConnectionPool(node);
var settings = new ConnectionSettings(connectionPool)
    .BasicAuthentication(username, password)
    .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
    .DefaultIndex("myindex");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(settings);
builder.Services.AddTransient<IAutocompleteService, AutocompleteService>();

var app = builder.Build();

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

