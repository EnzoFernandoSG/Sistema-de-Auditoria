using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization; // Para JsonIgnore

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("Products")]
    public class Product : BaseModel
    {
        [PrimaryKey("id", true)]
        public long Id { get; set; } // bigint

        [Column("name")]
        public string? Name { get; set; } // text null

        [Column("unit_price")]
        public double? UnitPrice { get; set; } // double precision null

        [Column("total")]
        public double? Total { get; set; } // double precision null

        [Column("quantity")]
        public double? Quantity { get; set; } // double precision null

        [Column("deal_id")]
        public long? DealId { get; set; } // bigint null, foreign key to Deals (id)

        [Column("currency_id")]
        public int? CurrencyId { get; set; } // integer null, foreign key to Currency (id)

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } // timestamp with time zone not null

        [JsonIgnore] // Para compatibilidade com PloomesProduct no frontend
        public string ProductName => Name ?? string.Empty;
    }
}