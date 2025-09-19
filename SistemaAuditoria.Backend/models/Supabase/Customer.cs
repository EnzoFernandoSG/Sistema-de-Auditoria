using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("Customers")]
    public class Customer : BaseModel
    {
        [PrimaryKey("id", true)]
        public long Id { get; set; } // bigint

        [Column("name")]
        public string Name { get; set; } = string.Empty; // not null

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("street")]
        public string? Street { get; set; }

        [Column("neighborhood")]
        public string? Neighborhood { get; set; }

        [Column("number")]
        public long? Number { get; set; } // bigint

        [Column("cep")]
        public string? Cep { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("cpf")]
        public string? Cpf { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } // timestamp without time zone

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; } // timestamp without time zone

        [Column("user_id")]
        public long? UserId { get; set; } // bigint, foreign key to Users (id)

        [Column("franchise_cod")]
        public string? FranchiseCod { get; set; } // text

        [Column("company_name")]
        public string? CompanyName { get; set; } // text

        [Column("complement")]
        public string? Complement { get; set; } // text

        [Column("city")]
        public string? City { get; set; } // text

        [Column("state")]
        public string? State { get; set; } // text
    }
}