using Microsoft.AspNetCore.Http;

namespace FiscalManager.Application.DTOs;

//public record UpdateInvoiceDto
//{
//    public string Description { get; init; } = string.Empty;
//    public decimal Amount { get; init; }
//    public DateTime Date { get; init; }
//    public IFormFile? File { get; init; }
//}

public record UpdateInvoiceDto
(
    string Description,
    decimal Amount,
    DateTime Date,
    IFormFile? File
);