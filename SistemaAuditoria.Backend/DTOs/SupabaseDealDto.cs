using System.Text.Json.Serialization;

namespace SistemaAuditoria.Backend.DTOs
{
    public class SupabaseDealDto
    {
        public long Id { get; set; }
        public long DealId { get; set; }

        public double? Amount { get; set; }
        public int? IdCurrency { get; set; }
        public long? IdCustomer { get; set; }
        public int? IdStatus { get; set; }

        public string CreateDate { get; set; } = string.Empty;
        public int? Periodo { get; set; }
        public string? Cep { get; set; }
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? DocumentUrl { get; set; }
        public string? LastUpdateDate { get; set; }
        public int? FranchiseCod { get; set; }

        public string Cliente { get; set; } = string.Empty;
        public double M2Instalados { get; set; }
        public List<ProductDto> AreasProtegidas { get; set; } = new List<ProductDto>();
        public string? DataTermino { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool Expirada { get; set; }

        public CustomerDto? Customer { get; set; } 
    }
}