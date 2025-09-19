using Supabase;
using SistemaAuditoria.Backend.Models;
using SistemaAuditoria.Backend.Models.Supabase; // Para modelos DB como Franchise, AuditoriaDbModel
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Supabase.Postgrest.Responses;
//using Supabase.Postgrest.Constants;

namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(Supabase.Client supabaseClient, ILogger<DashboardService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<int> GetTotalFranchisesAsync()
        {
            _logger.LogInformation("DashboardService: Buscando total de franquias do Supabase.");
            try
            {
                var response = await _supabaseClient.From<Franchise>().Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"DashboardService: Erro Supabase ao contar franquias: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    return 0;
                }
                return response.Models?.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DashboardService: Erro ao buscar total de franquias.");
                return 0;
            }
        }

        public async Task<int> GetMonthlyAuditsAsync()
        {
            _logger.LogInformation("DashboardService: Buscando auditorias do mês atual do Supabase.");
            try
            {
                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

                var response = await _supabaseClient.From<AuditoriaDbModel>()
                                                    .Filter("created_at", Supabase.Postgrest.Constants.Operator.GreaterThanOrEqual, startOfMonth.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                                                    .Filter("created_at", Supabase.Postgrest.Constants.Operator.LessThanOrEqual, endOfMonth.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
                                                    .Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"DashboardService: Erro Supabase ao contar auditorias do mês: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    return 0;
                }
                return response.Models?.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DashboardService: Erro ao buscar auditorias do mês.");
                return 0;
            }
        }
    }
}