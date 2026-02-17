using FiscalManager.Application.DTOs;
using FiscalManager.Application.Interfaces;
using FiscalManager.Domain.Entities;
using FiscalManager.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalManager.Infrastructure.Services;
public class InvoiceService : IInvoiceService
{
    private readonly FiscalDbContext _context;
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

    public InvoiceService(FiscalDbContext context)
    {
        _context = context;

        //Garantir que a pasta exista
        if(!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }
    public async Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto)
    {
        // 1. Upload do arquivo (File System)
        var extension = Path.GetExtension(dto.File.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        // 2. Mapeamento para Entidade de Domínio
        var entity = new Invoice
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            FilePath = fullPath, // Salva o caminho do arquivo
            CreatedAt = DateTime.UtcNow
        };

        // 3. Persistência no Banco
        _context.Invoices.Add(entity);
        await _context.SaveChangesAsync();

        var publicUrl = $"/files/{Path.GetFileName(entity.FilePath)}"; // Cria a URL relativa

        // 4. Retorno mapeado com a dto de resposta
        return new InvoiceResponseDto(
            entity.Id,
            entity.Description,
            entity.Amount,
            entity.Date,
            publicUrl
        );
    }
}

