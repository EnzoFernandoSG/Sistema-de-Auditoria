namespace SistemaAuditoria.Backend.Models
{
    public class StockQueryResult
    {
        public int Quantidade { get; set; }
        public string? Erro { get; set; }
    }

    public class AuditSaveResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}