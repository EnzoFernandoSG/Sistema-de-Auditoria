using SistemaAuditoria.Backend.Models.Supabase;
using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.DTOs; 
namespace SistemaAuditoria.Backend.Services
{
    public interface ISupabaseService
    {

        Task<int> GetTotalFranchisesAsync();
        Task<int> GetMonthlyAuditsAsync();
        Task<IEnumerable<FranchiseDto>> GetFranchisesAsync();
        Task<FranchiseDto?> GetFranchiseByIdPloomesAsync(string idPloomes);
        Task<IEnumerable<TechnicianDto>> GetTechniciansAsync();
        Task<IEnumerable<SupabaseDealDto>> GetDealsAsync(int franchiseId, DateTime? selectedDate = null);
        Task<StockQueryResult> GetStockByCnpjAsync(string cnpj);
        Task<AuditSaveResult> SubmitAuditAsync(SistemaAuditoria.Backend.Models.AuditData auditData);
        Task<IEnumerable<HistoricAuditoriaDto>> GetAuditoriasAsync();
        Task<HistoricAuditoriaDto?> GetAuditoriaByIdAsync(Guid id);
        Task<AuditSaveResult> UpdateAuditoriaAsync(Guid id, AuditData auditData);
        Task<AuditSaveResult> DeleteAuditoriaAsync(Guid id);
    }
}