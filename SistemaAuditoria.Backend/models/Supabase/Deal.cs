using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("Deals")]
    public class Deal : BaseModel // Renomeado para 'Deal' para evitar conflito com DTO 'SupabaseDealDto'
    {
        [PrimaryKey("id", true)]
        public long Id { get; set; } // bigint

        [Column("amount")]
        public double? Amount { get; set; } // double precision null

        [Column("id_currency")]
        public int? IdCurrency { get; set; } // integer null

        [Column("id_customer")]
        public long? IdCustomer { get; set; } // bigint null

        [Column("id_status")]
        public int? IdStatus { get; set; } // integer null

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } // timestamp with time zone not null

        [Column("period")]
        public int? Period { get; set; } // integer null

        [Column("cep")]
        public string? Cep { get; set; } // text null

        [Column("street_number")]
        public string? StreetNumber { get; set; } // text null

        [Column("street_name")]
        public string? StreetName { get; set; } // text null

        [Column("district")]
        public string? District { get; set; } // text null

        [Column("state")]
        public string? State { get; set; } // text null

        [Column("city")]
        public string? City { get; set; } // text null

        [Column("document_url")]
        public string? DocumentUrl { get; set; } // text null

        [Column("lastupdatedate")]
        public DateTime? LastUpdateDate { get; set; } // timestamp without time zone null

        [Column("franchise_cod")]
        public int? FranchiseCod { get; set; } // integer null, foreign key to Franchise (id)

        [Reference(typeof(Product))]
        public List<Product>? Products { get; set; } // Deixando como List<Product>

        [Reference(typeof(Customer))]
        public Customer? Customer { get; set; }

        [JsonIgnore]
        public long DealId => Id; // Alias para compatibilidade com DTO

        [JsonIgnore]
        public string Cliente { get; set; } = string.Empty;

        [JsonIgnore]
        public DateTime? CreateDateDto => CreatedAt; // Alias para CreateDate no DTO (se o frontend usar esse nome)

        [JsonIgnore]
        public double M2Instalados { get; set; }

        [JsonIgnore]
        public List<Product> AreasProtegidas { get; set; } = new List<Product>();

        [JsonIgnore]
        public int? PeriodoDto => Period; // Alias para Periodo no DTO (se o frontend usar esse nome)

        [JsonIgnore]
        public string? DataTermino { get; set; }

        [JsonIgnore]
        public string Status { get; set; } = string.Empty;

        [JsonIgnore]
        public bool Expirada { get; set; } = false;
    }
}