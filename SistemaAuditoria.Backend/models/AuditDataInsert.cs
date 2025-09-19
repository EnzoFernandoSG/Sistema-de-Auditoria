// SistemaAuditoria.Backend/Models/AuditDataInsert.cs

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SistemaAuditoria.Backend.Models
{
    // DTO específico para a inserção de auditoria, NÃO inclui o 'Id'
    public class AuditDataInsert
    {

        [JsonPropertyName("status")]
        public short? Status { get; set; } // smallint

        [JsonPropertyName("cnpj")]
        public string? Cnpj { get; set; } // character varying

        [JsonPropertyName("dataVisitaTecnica")]
        public DateTime? DataVisitaTecnica { get; set; } // timestamp without time zone

        [JsonPropertyName("qntEstoqueSistema")]
        public double? QntEstoqueSistema { get; set; } // double precision

        [JsonPropertyName("images")] // Text[]
        public List<string>? Image { get; set; } // Renamed to Image to match DB column 'image'

        [JsonPropertyName("m2InstaladosSistema")]
        public double? M2InstaladosSistema { get; set; } // double precision

        [JsonPropertyName("m2InstaladosCampo")]
        public double? M2InstaladosCampo { get; set; } // double precision

        [JsonPropertyName("placasDanificadasCampo")]
        public double? PlacasDanificadasCampo { get; set; } // double precision

        [JsonPropertyName("unidadeFranqueada")]
        public string? UnidadeFranqueada { get; set; } // character varying

        [JsonPropertyName("email")]
        public string? Email { get; set; } // character varying

        [JsonPropertyName("proprietario")]
        public string? Proprietario { get; set; } // character varying

        [JsonPropertyName("enderecoEstoque")]
        public string? EnderecoEstoque { get; set; } // character varying

        [JsonPropertyName("whatsapp")]
        public string? Whatsapp { get; set; } // character varying

        [JsonPropertyName("cidade")]
        public string? Cidade { get; set; } // character varying

        [JsonPropertyName("estado")]
        public string? Estado { get; set; } // character varying

        [JsonPropertyName("estoqueOciosoCampo")]
        public double? EstoqueOciosoCampo { get; set; } // double precision

        [JsonPropertyName("estoqueOciosoSistema")]
        public double? EstoqueOciosoSistema { get; set; } // double precision

        [JsonPropertyName("createdAt")] // Will be used to map customCreatedAt from frontend
        public DateTime? CreatedAt { get; set; } // timestamp without time zone not null default now()

        [JsonPropertyName("idsObrasProcessadas")] // Assuming this maps to ids_ploomes_orders
        public List<long>? IdsObrasProcessadas { get; set; } // bigint[]

        [JsonPropertyName("franqueadoId")]
        public long? FranqueadoId { get; set; } // bigint

        [JsonPropertyName("contagemM2Campo")]
        public List<double>? ContagemM2Campo { get; set; } // double precision[]

        [JsonPropertyName("notReleased")] // Assuming this maps to not_released
        public string? NotReleased { get; set; } // character varying (JSON string)

        [JsonPropertyName("notes")] // Assuming this maps to notes
        public List<string>? Notes { get; set; } // character varying[]

        [JsonPropertyName("technicianId")]
        public long? TechnicianId { get; set; } // bigint (FK)

        [JsonPropertyName("razaoSocial")]
        public string? RazaoSocial { get; set; } // character varying

        [JsonPropertyName("endereco")]
        public string? Endereco { get; set; } // character varying

        [JsonPropertyName("imageNames")] // Assuming this maps to image_name
        public List<string>? ImageName { get; set; } // text[]
    }
}