// ------------------------- AuditData.cs -------------------------
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SistemaAuditoria.Backend.Models
{
    public class AuditData
    {
        [JsonPropertyName("franqueadoId")]
        public string FranqueadoId { get; set; } = string.Empty;

        [JsonPropertyName("unidadeFranqueada")]
        public string UnidadeFranqueada { get; set; } = string.Empty;

        [JsonPropertyName("proprietario")]
        public string? Proprietario { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("enderecoEstoque")]
        public string? EnderecoEstoque { get; set; }

        [JsonPropertyName("cidade")]
        public string? Cidade { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("whatsapp")]
        public string? Whatsapp { get; set; }

        [JsonPropertyName("cnpj")]
        public string? Cnpj { get; set; }

        [JsonPropertyName("razaoSocial")]
        public string? RazaoSocial { get; set; }

        [JsonPropertyName("endereco")]
        public string? Endereco { get; set; }

        [JsonPropertyName("technicianId")]
        public int? TechnicianId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("dataVisitaTecnica")]
        public string? DataVisitaTecnica { get; set; }

        [JsonPropertyName("m2InstaladosCampo")]
        public string? M2InstaladosCampo { get; set; }

        [JsonPropertyName("estoqueOciosoCampo")]
        public double? EstoqueOciosoCampo { get; set; }

        [JsonPropertyName("placasDanificadasCampo")]
        public double? PlacasDanificadasCampo { get; set; }

        [JsonPropertyName("observacoesGerais")]
        public string? ObservacoesGerais { get; set; }

        [JsonPropertyName("quantidadeEstoqueSistema")]
        public string? QuantidadeEstoqueSistema { get; set; }

        [JsonPropertyName("m2InstaladosSistema")]
        public string? M2InstaladosSistema { get; set; }

        [JsonPropertyName("estoqueOciosoSistema")]
        public string? EstoqueOciosoSistema { get; set; }

        [JsonPropertyName("images")]
        public List<string>? Images { get; set; }

        [JsonPropertyName("imageNames")]
        public List<string>? ImageNames { get; set; }

        [JsonPropertyName("obrasProcessadasIds")]
        public List<long>? ObrasProcessadasIds { get; set; }

        [JsonPropertyName("contagemM2Campo")]
        public List<string>? ContagemM2Campo { get; set; }

        [JsonPropertyName("observacoesPorOrdemValues")]
        public List<string>? ObservacoesPorOrdemValues { get; set; }

        [JsonPropertyName("obrasNaoFornecidasJson")]
        public string? ObrasNaoFornecidasJson { get; set; }

        [JsonPropertyName("customCreatedAt")]
        public DateTime? CustomCreatedAt { get; set; }
    }
}
