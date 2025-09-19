using SistemaAuditoria.Backend.DTOs;

namespace SistemaAuditoria.Backend.Services
{
    public interface IFranchiseService
    {
        Task<IEnumerable<FranchiseDto>> GetFranchisesAsync();
        Task<FranchiseDto?> GetFranchiseByIdPloomesAsync(string idPloomes);
    }
}