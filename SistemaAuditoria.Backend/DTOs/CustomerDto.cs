namespace SistemaAuditoria.Backend.DTOs
{
    public class CustomerDto
    {
        public long Id { get; set; } // bigint mapeia para long
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? Neighborhood { get; set; }
        public long? Number { get; set; } // bigint
        public string? Cep { get; set; }
        public string? Cnpj { get; set; }
        public string? Cpf { get; set; }
        public string? CreatedAt { get; set; } // timestamp without time zone
        public string? UpdatedAt { get; set; } // timestamp without time zone
        public long? UserId { get; set; } // bigint
        public string? FranchiseCod { get; set; }
        public string? CompanyName { get; set; }
        public string? Complement { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}