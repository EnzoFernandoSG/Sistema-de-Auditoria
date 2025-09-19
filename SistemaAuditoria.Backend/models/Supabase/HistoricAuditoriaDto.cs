using System;
using System.Collections.Generic;

namespace SistemaAuditoria.Backend.DTOs
{
    public class HistoricAuditoriaDto
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

        public string? StatusText { get; set; } // "Congruente" ou "Incongruente"
        public string? DataVisitaTecnica { get; set; } // Formato "YYYY-MM-DD"
        public string? CreatedAt { get; set; } // Data de criação da auditoria no DB

        public double? QntEstoqueSistema { get; set; }
        public double? M2InstaladosSistema { get; set; }
        public double? EstoqueOciosoSistema { get; set; }
        public double? M2InstaladosCampo { get; set; }
        public double? EstoqueOciosoCampo { get; set; }
        public double? PlacasDanificadasCampo { get; set; }

        public List<string>? ImageUrls { get; set; } // URLs das imagens se você as salvar em um Storage como Supabase Storage
        public List<string>? ImageNames { get; set; }

        public List<long>? IdsObrasProcessadas { get; set; } // IDs dos Deals
        public List<double>? ContagemM2Campo { get; set; }
        public string? NotReleasedJson { get; set; } // JSON string de Obras Não Fornecidas
        public List<string>? Notes { get; set; }

        public TechnicianDto? Technician { get; set; } // Objeto Technician completo
        public long? TechnicianId { get; set; } // Para mapeamento

        // Propriedades adicionais para facilitar o frontend (pode não vir do DB diretamente)
        public string? ObservacoesGerais { get; set; } // Mapeado do último item de 'Notes'
        public List<ObraNaoFornecidaDto>? ObrasNaoFornecidas { get; set; } // Objeto deserializado de NotReleasedJson

        // DTOs Aninhados para obras não fornecidas (se precisar de uma representação de objeto)
        public class ObraNaoFornecidaDto
        {
            public string? ContagemM2 { get; set; }
            public string? Cliente { get; set; }
            public string? Endereco { get; set; }
            public string? Periodo { get; set; }
        }
    }
}