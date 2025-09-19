using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaAuditoria.Backend.Services
{
    public interface IAuditService
    {
        Task<AuditSaveResult> SubmitAuditAsync(AuditData auditData);
        Task<IEnumerable<HistoricAuditDto>> GetAuditoriasAsync();
        Task<HistoricAuditDto?> GetAuditoriaByIdAsync(Guid id);
        Task<AuditSaveResult> UpdateAuditoriaAsync(Guid id, AuditData auditData);
        Task<AuditSaveResult> DeleteAuditoriaAsync(Guid id);
    }
}
