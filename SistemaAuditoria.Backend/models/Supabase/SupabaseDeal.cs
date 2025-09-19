using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("Deals")]
    public class SupabaseDeal : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("lastupdatedate")]
        public DateTime? LastUpdateDate { get; set; }

        [Column("id_status")]
        public int? IdStatus { get; set; }

        [Column("street_name")]
        public string? StreetName { get; set; }

        [Column("street_number")]
        public string? StreetNumber { get; set; }

        [Column("district")]
        public string? District { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("state")]
        public string? State { get; set; }

        [Column("franchise_cod")]
        public int? FranchiseCod { get; set; }

        [Column("period")]
        public int? Period { get; set; }

        [Column("document_url")]
        public string? DocumentUrl { get; set; }

        [Reference(typeof(Product))]
        public List<Product>? Products { get; set; }

        [Reference(typeof(Customer))]
        public List<Customer>? Customers { get; set; }

        // PROPRIEDADES CORRIGIDAS para ter setters
        // Elas serão populadas no serviço SupabaseService.GetPastDealsAsync
        [JsonIgnore]
        public int DealId { get; set; } // De => Id para { get; set; }

        [JsonIgnore]
        public string Cliente { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime? CreateDate { get; set; } // De => CreatedAt para { get; set; }

        [JsonIgnore]
        public double M2Instalados { get; set; }

        [JsonIgnore]
        public List<Product> AreasProtegidas { get; set; } = new List<Product>();

        [JsonIgnore]
        public int? Periodo { get; set; } // De => Period para { get; set; }

        [JsonIgnore]
        public string? DataTermino { get; set; }

        [JsonIgnore]
        public string Status { get; set; } = string.Empty;

        [JsonIgnore]
        public bool Expirada { get; set; } = false;
    }
}