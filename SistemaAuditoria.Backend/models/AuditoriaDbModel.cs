// ------------------------- AuditoriaDbModel.cs -------------------------
using System;
using System.Collections.Generic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Newtonsoft.Json;

namespace SistemaAuditoria.Backend.Models
{
    [Table("auditorias")]
    public class AuditoriaDbModel : BaseModel
    {
        [PrimaryKey("id", true)]
        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        public bool ShouldSerializeId() => Id != Guid.Empty;

        [Column("status")]
        public short? Status { get; set; }

        [Column("cnpj")]
        public string? Cnpj { get; set; }

        [Column("data_visita_tecnica")]
        public DateTime? DataVisitaTecnica { get; set; }

        [Column("qnt_estoque_sistema")]
        public double? QntEstoqueSistema { get; set; }

        [Column("image")]
        public List<string>? ImageM { get; set; }

        [Column("m2_instalados_sistema")]
        public double? M2InstaladosSistema { get; set; }

        [Column("m2_instalados_campo")]
        public double? M2InstaladosCampo { get; set; }

        [Column("placas_danificadas_campo")]
        public double? PlacasDanificadasCampo { get; set; }

        [Column("unidade_franqueada")]
        public string? UnidadeFranqueada { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("proprietario")]
        public string? Proprietario { get; set; }

        [Column("endereco_estoque")]
        public string? EnderecoEstoque { get; set; }

        [Column("whatsapp")]
        public string? Whatsapp { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("estoque_ocioso_campo")]
        public double? EstoqueOciosoCampo { get; set; }

        [Column("estoque_ocioso_sistema")]
        public double? EstoqueOciosoSistema { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("ids_ploomes_orders")]
        public List<long>? IdsPloomesOrders { get; set; }

        [Column("franqueado_id")]
        public long? FranqueadoId { get; set; }

        [Column("contagem_m2_campo")]
        public List<double>? ContagemM2Campo { get; set; }

        [Column("not_released")]
        public string? NotReleased { get; set; }

        [Column("notes")]
        public List<string>? Notes { get; set; }

        [Column("technician_id")]
        public long? TechnicianId { get; set; }

        [Column("razao_social")]
        public string? RazaoSocial { get; set; }

        [Column("endereco")]
        public string? Endereco { get; set; }

        [Column("image_name")]
        public List<string>? ImageName { get; set; }

        [Column("last_update_date")]
        public DateTime? LastUpdateDate { get; set; }
    }
}
