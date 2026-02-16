

namespace FiscalManager.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma nota fiscal
    /// </summary>
    public class Invoice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string FilePath { get; set; } = string.Empty; // Caminho do arquivo no disco
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
