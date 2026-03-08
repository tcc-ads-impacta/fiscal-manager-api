using FiscalManager.Api.Endpoints;
using FiscalManager.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Microsoft.Extensions.FileProviders;
using FiscalManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string angularPolicy = "AllowAngularOrigin";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: angularPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Configura��o com o banco de dados (infrastructure)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString!);

var app = builder.Build();

// Aplica migrations pendentes automaticamente na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FiscalDbContext>();
    db.Database.Migrate();
}

app.UseCors(angularPolicy);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("documentation");
}

app.UseHttpsRedirection();

//Configura��es para a pasta de uploads
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

