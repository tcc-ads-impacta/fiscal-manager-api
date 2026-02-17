using FiscalManager.Api.Endpoints;
using FiscalManager.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Configuração com o banco de dados (infrastructure)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString!);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("documentation");
}

app.UseHttpsRedirection();

//Configurações para a pasta de uploads
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

//Habilita o acesso via http://localhost:PORTA/files/nome-do-arquivo.pdf
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/files"
});

// REGISTRO DE ROTAS
app.MapInvoiceEndpoints();

app.Run();

