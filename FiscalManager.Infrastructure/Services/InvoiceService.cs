using FiscalManager.Application.DTOs;
using FiscalManager.Application.Interfaces;
using FiscalManager.Domain.Entities;
using FiscalManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
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
    
    public async Task<IEnumerable<InvoiceItemDto>> GetAllAsync(int? month = null, int? year = null)
    {
        // 1. Permite filtrar no banco antes de trazer para memória
        var query = _context.Invoices.AsQueryable();

        // 2. Aplicação de Filtros (Se informados)
        if (month.HasValue)
            query = query.Where(x => x.Date.Month == month.Value);

        if (year.HasValue)
            query = query.Where(x => x.Date.Year == year.Value);

        // Ordenar por data (mais antigas ou urgentes primeiro)
        query = query.OrderBy(x => x.Date);

        // 3. Execução no Banco
        var entities = await query.ToListAsync();

        // 4. Mapeamento e Cálculo de Status (Em Memória)
        var today = DateTime.UtcNow.Date; // Data de hoje zerada (sem horas)

        return entities.Select(entity =>
        {
            var fileName = Path.GetFileName(entity.FilePath);
            var publicUrl = $"/files/{fileName}";

            // Lógica de Status
            var daysDiff = (entity.Date.Date - today).TotalDays;
            string status;
            string severity;

            if (daysDiff < 0)
            {
                status = "Vencida";
                severity = "danger"; // Vermelho
            }
            else if (daysDiff <= 3) // 3 dias para vencer
            {
                status = "Próxima do Vencimento";
                severity = "warning"; // Amarelo
            }
            else
            {
                status = "Em Aberto";
                severity = "success"; // Verde ou Azul
            }

            return new InvoiceItemDto(
                entity.Id,
                entity.Description,
                entity.Amount,
                entity.Date,
                publicUrl,
                status,
                severity,
                (int)daysDiff
            );
        });

    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        // 1. Busca no Banco
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice is null) return false;

        // 2. Apaga o Arquivo Físico (Se existir)
        if (!string.IsNullOrEmpty(invoice.FilePath) && File.Exists(invoice.FilePath))
        {
            try
            {
                File.Delete(invoice.FilePath);
            }
            catch (IOException)
            {
                // Para este MVP, continuarei para garantir que o registro saia do banco.
                // Mas usariamos um sistema de log ou tratativa especifica
            }
        }

        // 3. Remove do Banco
        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();

        return true;
    }
}

