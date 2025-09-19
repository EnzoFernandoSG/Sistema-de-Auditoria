using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.DTOs; 

namespace SistemaAuditoria.Backend.Services
{
    public interface IDashboardService
    {
        Task<int> GetTotalFranchisesAsync();
        Task<int> GetMonthlyAuditsAsync();
    }
}