using FiscalManager.Application.DTOs;


namespace FiscalManager.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto);
}

