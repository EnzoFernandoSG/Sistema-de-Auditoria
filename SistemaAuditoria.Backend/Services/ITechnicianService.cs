using SistemaAuditoria.Backend.DTOs;

namespace SistemaAuditoria.Backend.Services
{
    public interface ITechnicianService
    {
        Task<IEnumerable<TechnicianDto>> GetTechniciansAsync();
    }
}