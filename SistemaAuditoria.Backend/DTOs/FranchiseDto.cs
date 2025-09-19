namespace SistemaAuditoria.Backend.DTOs
{
    public class FranchiseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Cnpj { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public int? Owner { get; set; }
        public string? Whatsapp { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? Website { get; set; }
        public string? CreatedAt { get; set; }
        public string? Note { get; set; }
        public string? CorporateName { get; set; }
        public string? ZipCode { get; set; }
        public string? Instagram { get; set; }
        public string? Type { get; set; }
        public string? IdPloomes { get; set; }
    }
}