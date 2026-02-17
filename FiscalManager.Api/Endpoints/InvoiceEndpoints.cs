
using FiscalManager.Application.DTOs;
using FiscalManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiscalManager.Api.Endpoints;

public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/invoices")
                       .DisableAntiforgery();

        // Cadastro
        group.MapPost("/", CreateInvoice);
        // Recuperar notas
        group.MapGet("/", GetAllInvoices);
        // Apagar
        group.MapDelete("/{id}", DeleteInvoice);
        // Atualizar
        group.MapPut("/{id}", UpdateInvoice).DisableAntiforgery();
    }

    private static async Task<IResult> CreateInvoice(
        [FromForm] CreateInvoiceDto dto,
        IInvoiceService service)
    {
        var result = await service.CreateAsync(dto);
        return Results.Created($"/api/invoices/{result.Id}", result);
    }

    private static async Task<IResult> GetAllInvoices(
    IInvoiceService service,
    [AsParameters] InvoiceFilterRequest filters)
    {
        // Chama o serviço passando mês e ano (se vierem nulos, traz tudo)
        var result = await service.GetAllAsync(filters.Month, filters.Year);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteInvoice(int id, IInvoiceService service)
    {
        var deleted = await service.DeleteAsync(id);

        return deleted ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> UpdateInvoice(
    int id,
    [FromForm] UpdateInvoiceDto dto,
    IInvoiceService service)
    {
        var result = await service.UpdateAsync(id, dto);

        if (result is null)
            return Results.NotFound();

        return Results.Ok(result);
    }
}

