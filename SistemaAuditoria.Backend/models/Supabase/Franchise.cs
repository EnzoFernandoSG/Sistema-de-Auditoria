using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("Franchise")]
    public class Franchise : BaseModel
    {
        [PrimaryKey("id", true)] // id é gerado por default como identity
        public int Id { get; set; } // integer

        [Column("name")]
        public string Name { get; set; } = string.Empty; // not null

        [Column("email")]
        public string? Email { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("state")]
        public string? State { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("owner")]
        public int? Owner { get; set; } // foreign key to Users

        [Column("whatsapp")]
        public string? Whatsapp { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("district")]
        public string? District { get; set; }

        [Column("website")]
        public string? Website { get; set; }

        [Column("created_at")]
        public DateOnly? CreatedAt { get; set; } // date null

        [Column("note")]
        public string? Note { get; set; } // text null

        [Column("corporate_name")]
        public string? CorporateName { get; set; }

        [Column("zip_code")]
        public string? ZipCode { get; set; }

        [Column("code")]
        public string? Code { get; set; }

        [Column("instagram")]
        public string? Instagram { get; set; }

        [Column("type")]
        public string? Type { get; set; } // text null

        [Column("id_ploomes")]
        public string? IdPloomes { get; set; } // text null, unique
    }
}