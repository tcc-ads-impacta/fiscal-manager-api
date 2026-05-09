using FiscalManager.Application.DTOs;


namespace FiscalManager.Application.Interfaces;

public interface IInvoiceService
{
    Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto);
    Task<IEnumerable<InvoiceItemDto>> GetAllAsync(int? month = null, int? year = null);
    Task<List<InvoiceItemDto>> SearchInvoicesAsync(string? text = null);
    Task<bool> DeleteAsync(int id);
    Task<InvoiceResponseDto?> UpdateAsync(int id, UpdateInvoiceDto dto);
}

