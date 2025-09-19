using SistemaAuditoria.Backend.Models; 

namespace SistemaAuditoria.Backend.Services
{
    public interface INfeService
    {
        Task<StockQueryResult> GetStockByCnpjAsync(string cnpj);
    }
}