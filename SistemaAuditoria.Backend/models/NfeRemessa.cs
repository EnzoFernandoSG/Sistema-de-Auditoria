using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace SistemaAuditoria.Backend.Models.Supabase
{
    [Table("nfe_remessa")]
    public class NfeRemessa : BaseModel
    {
        [PrimaryKey("id", true)] // uuid gerado por default as identity
        public Guid Id { get; set; } // uuid

        [Column("chave_acesso")]
        public string ChaveAcesso { get; set; } = string.Empty;

        [Column("numero_nfe")]
        public int NumeroNfe { get; set; }

        [Column("serie_nfe")]
        public int SerieNfe { get; set; }

        [Column("data_emissao")]
        public DateTime DataEmissao { get; set; } // timestamp with time zone

        [Column("valor_total_nota")]
        public decimal ValorTotalNota { get; set; } // numeric(15, 2)

        [Column("cnpj_emitente")]
        public string CnpjEmitente { get; set; } = string.Empty;

        [Column("nome_emitente")]
        public string NomeEmitente { get; set; } = string.Empty;

        [Column("cnpj_destinatario")]
        public string? CnpjDestinatario { get; set; }

        [Column("nome_destinatario")]
        public string NomeDestinatario { get; set; } = string.Empty;

        [Column("logradouro_emit")]
        public string? LogradouroEmit { get; set; }

        [Column("municipio_emit")]
        public string? MunicipioEmit { get; set; }

        [Column("uf_emit")]
        public string? UfEmit { get; set; }

        [Column("logradouro_dest")]
        public string? LogradouroDest { get; set; }

        [Column("municipio_dest")]
        public string? MunicipioDest { get; set; }

        [Column("uf_dest")]
        public string? UfDest { get; set; }

        [Column("natureza_operacao")]
        public string? NaturezaOperacao { get; set; }

        [Column("protocolo_autorizacao")]
        public string? ProtocoloAutorizacao { get; set; }

        [Column("xml_completo")]
        public string? XmlCompleto { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } // timestamp with time zone

        [Column("rental")]
        public string? Rental { get; set; } // character varying, foreign key to Franchise (cnpj)
    }
}