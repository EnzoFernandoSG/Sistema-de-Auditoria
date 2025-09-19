using System;
using System.Collections.Generic;

namespace SistemaAuditoria.Backend.DTOs
{
    public class HistoricAuditDto // <-- NOME DA CLASSE RENOMEADO
    {
        public Guid Id { get; set; }
        public string? Cnpj { get; set; }
        public string? UnidadeFranqueada { get; set; }
        public string? Proprietario { get; set; }
        public string? Email { get; set; }
        public string? Whatsapp { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }

        public string? StatusText { get; set; }
        public string? DataVisitaTecnica { get; set; }
        public string? CreatedAt { get; set; }

        public double? QntEstoqueSistema { get; set; }
        public double? M2InstaladosSistema { get; set; }
        public double? EstoqueOciosoSistema { get; set; }
        public double? M2InstaladosCampo { get; set; }
        public double? EstoqueOciosoCampo { get; set; }
        public double? PlacasDanificadasCampo { get; set; }

        public List<string>? ImageUrls { get; set; }
        public List<string>? ImageNames { get; set; }

        public List<long>? IdsObrasProcessadas { get; set; }
        public List<double>? ContagemM2Campo { get; set; }
        public string? NotReleasedJson { get; set; }
        public List<string>? Notes { get; set; }

        public TechnicianDto? Technician { get; set; }
        public long? TechnicianId { get; set; }

        public string? ObservacoesGerais { get; set; }
        public List<HistoricAuditDto.ObraNaoFornecidaDto>? ObrasNaoFornecidas { get; set; } 

        public class ObraNaoFornecidaDto 
        {
            public string? ContagemM2 { get; set; }
            public string? Cliente { get; set; }
            public string? Endereco { get; set; }
            public string? Periodo { get; set; }
        }
    }
}