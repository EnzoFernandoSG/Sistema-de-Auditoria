using Microsoft.AspNetCore.Mvc;
using SistemaAuditoria.Backend.Services;
using SistemaAuditoria.Backend.Models; 
using SistemaAuditoria.Backend.DTOs; 
using System.Collections.Generic; 
using System; 
using System.Text.Json; 

namespace SistemaAuditoria.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AberturaAuditoriaController : ControllerBase
    {
        private readonly IFranchiseService _franchiseService; 
        private readonly IDealService _dealService;           
        private readonly INfeService _nfeService;             
        private readonly ILogger<AberturaAuditoriaController> _logger;

        public AberturaAuditoriaController(
            IFranchiseService franchiseService, // Injete
            IDealService dealService,           // Injete
            INfeService nfeService,             // Injete
            ILogger<AberturaAuditoriaController> logger)
        {
            _franchiseService = franchiseService;
            _dealService = dealService;
            _nfeService = nfeService;
            _logger = logger;
        }

        [HttpGet("franchises")]
        public async Task<ActionResult<IEnumerable<FranchiseDto>>> GetFranchises()
        {
            _logger.LogInformation("Recebida requisição para obter todas as franquias do Supabase.");
            var franchises = await _franchiseService.GetFranchisesAsync(); // <-- Usar o novo serviço
            return Ok(franchises);
        }

        [HttpGet("deals")]
        public async Task<ActionResult<IEnumerable<SupabaseDealDto>>> GetDeals(
            [FromQuery] int franchiseId,
            [FromQuery] DateTime? selectedDate = null)
        {
            _logger.LogInformation($"Recebida requisição para obter obras para FranchiseId: {franchiseId} e Data: {selectedDate?.ToString() ?? "atual"}.");
            if (franchiseId <= 0)
            {
                return BadRequest("O ID da franquia é obrigatório.");
            }
            var deals = await _dealService.GetDealsAsync(franchiseId, selectedDate); 
            return Ok(deals);
        }

        [HttpGet("technicians")]
        public async Task<ActionResult<IEnumerable<TechnicianDto>>> GetTechnicians(
            ITechnicianService technicianService) 
        {
            _logger.LogInformation("Recebida requisição para obter Técnicos do Supabase.");
            var technicians = await technicianService.GetTechniciansAsync(); 
            return Ok(technicians);
        }

        [HttpGet("stock-by-cnpj")]
        public async Task<ActionResult<StockQueryResult>> GetStockByCnpj([FromQuery] string cnpj)
        {
            _logger.LogInformation("Recebida requisição para obter estoque por CNPJ.");
            if (string.IsNullOrEmpty(cnpj))
            {
                return BadRequest("O CNPJ é obrigatório.");
            }
            var result = await _nfeService.GetStockByCnpjAsync(cnpj);
            return Ok(result);
        }

        [HttpPost("submit-audit")]
        public async Task<ActionResult<AuditSaveResult>> SubmitAudit(
            [FromBody] AuditData auditData,
            [FromServices] IAuditService auditService) 
        {
            _logger.LogInformation("Recebida requisição para submeter auditoria.");
            _logger.LogDebug("JSON de AuditData recebido: {AuditDataJson}", JsonSerializer.Serialize(auditData, new JsonSerializerOptions { WriteIndented = true }));

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                                       .ToDictionary(
                                           kvp => kvp.Key,
                                           kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                                       );
                _logger.LogError("Erros de validação do modelo: {ValidationErrors}", JsonSerializer.Serialize(errors, new JsonSerializerOptions { WriteIndented = true }));
                return BadRequest(ModelState);
            }

            var result = await auditService.SubmitAuditAsync(auditData);
            if (!result.Success)
            {
                return StatusCode(500, result);
            }
            return Ok(result);
        }
    }
}