namespace SistemaAuditoria.Backend.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double? UnitPrice { get; set; }
        public double? Total { get; set; } 
        public double? Quantity { get; set; } 
        public long? DealId { get; set; } 
        public int? CurrencyId { get; set; } 
        public string? CreatedAt { get; set; } 
        public string? ProductName => Name;
    }
}