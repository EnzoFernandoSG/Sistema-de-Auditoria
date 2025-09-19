using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("nfe_itens")]
    public class NfeItem : BaseModel
    {
        [PrimaryKey("id", true)] // uuid gerado por default as identity
        public Guid Id { get; set; } // uuid

        [Column("nfe_id")]
        public Guid? NfeId { get; set; } // uuid null, foreign key to nfe_remessa (id)

        [Column("numero_item")]
        public int NumeroItem { get; set; }

        [Column("codigo_produto")]
        public string CodigoProduto { get; set; } = string.Empty;

        [Column("descricao_produto")]
        public string DescricaoProduto { get; set; } = string.Empty;

        [Column("ncm")]
        public string? Ncm { get; set; }

        [Column("cfop")]
        public string? Cfop { get; set; }

        [Column("unidade_comercial")]
        public string? UnidadeComercial { get; set; }

        [Column("quantidade_comercial")]
        public decimal? QuantidadeComercial { get; set; } // numeric(15, 4)

        [Column("valor_unitario_comercial")]
        public decimal? ValorUnitarioComercial { get; set; } // numeric(15, 2)

        [Column("valor_total_produto")]
        public decimal? ValorTotalProduto { get; set; } // numeric(15, 2)

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } // timestamp with time zone
    }
}