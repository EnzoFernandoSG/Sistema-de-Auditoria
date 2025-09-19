using Microsoft.AspNetCore.Mvc;
using SistemaAuditoria.Backend.Services;
using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.DTOs;
using System;
using System.Collections.Generic;

namespace SistemaAuditoria.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class HistoricAuditoriaController : ControllerBase 
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<HistoricAuditoriaController> _logger; 

        public HistoricAuditoriaController(IAuditService auditService, ILogger<HistoricAuditoriaController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [HttpGet] // GET /api/HistoricAuditoria
        public async Task<ActionResult<IEnumerable<HistoricAuditDto>>> GetAuditorias()
        {
            _logger.LogInformation("Recebida requisição para obter todas as auditorias.");
            var auditorias = await _auditService.GetAuditoriasAsync();
            return Ok(auditorias);
        }

        [HttpGet("{id}")] // GET /api/HistoricAuditoria/{id}
        public async Task<ActionResult<HistoricAuditDto>> GetAuditoria(Guid id)
        {
            _logger.LogInformation($"Recebida requisição para obter auditoria ID: {id}.");
            var auditoria = await _auditService.GetAuditoriaByIdAsync(id);
            if (auditoria == null)
            {
                return NotFound($"Auditoria com ID {id} não encontrada.");
            }
            return Ok(auditoria);
        }

        [HttpPut("{id}")] // PUT /api/HistoricAuditoria/{id}
        public async Task<ActionResult<AuditSaveResult>> UpdateAuditoria(Guid id, [FromBody] AuditData auditData)
        {
            _logger.LogInformation($"Recebida requisição para atualizar auditoria ID: {id}.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _auditService.UpdateAuditoriaAsync(id, auditData);
            if (!result.Success)
            {
                return StatusCode(500, result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")] // DELETE /api/HistoricAuditoria/{id}
        public async Task<ActionResult<AuditSaveResult>> DeleteAuditoria(Guid id)
        {
            _logger.LogInformation($"Recebida requisição para excluir auditoria ID: {id}.");
            var result = await _auditService.DeleteAuditoriaAsync(id);
            if (!result.Success)
            {
                return StatusCode(500, result);
            }
            return Ok(result);
        }
    }
}