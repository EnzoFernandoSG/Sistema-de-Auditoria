using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("technician")]
    public class Technician : BaseModel
    {
        [PrimaryKey("id", true)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("cpf")]
        public string? Cpf { get; set; }

        [Column("rg")]
        public string? Rg { get; set; }

        [Column("born")]
        public DateOnly? Born { get; set; } 
    }
}