namespace SistemaAuditoria.Backend.DTOs
{
    public class TechnicianDto
    {
        public long Id { get; set; } // bigint mapeia para number
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CreatedAt { get; set; } // timestamp with time zone
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public string? Born { get; set; } // DateOnly mapeia para string
    }
}