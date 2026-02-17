using FiscalManager.Application.DTOs;


namespace FiscalManager.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto);
    Task<IEnumerable<InvoiceItemDto>> GetAllAsync(int? month = null, int? year = null);
    Task<bool> DeleteAsync(int id);
}

