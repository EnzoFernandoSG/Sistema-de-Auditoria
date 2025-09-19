using SistemaAuditoria.Backend.DTOs;
using System;
using System.Collections.Generic;

namespace SistemaAuditoria.Backend.Services
{
    public interface IDealService
    {
        Task<IEnumerable<SupabaseDealDto>> GetDealsAsync(int franchiseId, DateTime? selectedDate = null);
    }
}