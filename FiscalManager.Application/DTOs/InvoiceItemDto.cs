namespace FiscalManager.Application.DTOs;

public record InvoiceItemDto(
    int Id,
    string Description,
    decimal Amount,
    DateTime Date,
    string PublicUrl, // URL tratada (/files/...)
    string Status,    // "Vencida", "Perto do Vencimento", "Em Aberto"
    string Severity,  // "danger", "warning", "info" (Para cor no frontend)
    int DaysUntilDue // Dias restantes (Negativo = Atrasado)
);

public record InvoiceFilterRequest(int? Month, int? Year);
