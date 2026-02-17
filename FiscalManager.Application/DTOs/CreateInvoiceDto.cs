using Microsoft.AspNetCore.Http;

namespace FiscalManager.Application.DTOs;

public record CreateInvoiceDto
(
    string Description,
    decimal Amount,
    DateTime Date,
    IFormFile File
);

public record InvoiceResponseDto(
    int Id,
    string Description,
    decimal Amount,
    DateTime Date,
    string FilePath
);