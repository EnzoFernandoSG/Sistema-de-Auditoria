using Supabase;
using SistemaAuditoria.Backend.Models.Supabase;
using SistemaAuditoria.Backend.DTOs;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Supabase.Postgrest.Responses;

namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class TechnicianService : ITechnicianService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<TechnicianService> _logger;

        public TechnicianService(Supabase.Client supabaseClient, ILogger<TechnicianService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<IEnumerable<TechnicianDto>> GetTechniciansAsync()
        {
            _logger.LogInformation("TechnicianService: Buscando técnicos do Supabase.");
            try
            {
                var response = await _supabaseClient.From<Technician>().Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"TechnicianService: Erro do Supabase ao buscar técnicos: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    throw new ApplicationException($"Falha ao conectar com Supabase (Técnicos): {errorContent}");
                }

                return response.Models?.Select(t => new TechnicianDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Phone = t.Phone,
                    CreatedAt = t.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Cpf = t.Cpf,
                    Rg = t.Rg,
                    Born = t.Born?.ToString("yyyy-MM-dd")
                }).ToList() ?? Enumerable.Empty<TechnicianDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "TechnicianService: Erro de requisição HTTP ao Supabase para técnicos.");
                throw new ApplicationException($"Erro de rede ou HTTP para Supabase (Técnicos): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TechnicianService: Erro inesperado ao buscar técnicos do Supabase.");
                throw new ApplicationException($"Erro genérico ao buscar técnicos: {ex.Message}", ex);
            }
        }
    }
}