
using FiscalManager.Application.DTOs;
using FiscalManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiscalManager.Api.Endpoints;

public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        //Todas as rotas abaixo começarão com /api/invoices
        var group = app.MapGroup("/api/invoices")
                       .DisableAntiforgery();

        // POST: /api/invoices
        group.MapPost("/cadastrar", CreateInvoice);

        // GET: /api/invoices (Faremos a seguir)
        // group.MapGet("/", GetAllInvoices)
    }

    private static async Task<IResult> CreateInvoice(
        [FromForm] CreateInvoiceDto dto,
        IInvoiceService service)
    {
        var result = await service.CreateAsync(dto);
        return Results.Created($"/api/invoices/{result.Id}", result);
    }
}

