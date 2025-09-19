using Supabase;
using SistemaAuditoria.Backend.Models.Supabase;
using SistemaAuditoria.Backend.DTOs;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Supabase.Postgrest.Responses;

namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class FranchiseService : IFranchiseService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<FranchiseService> _logger;

        public FranchiseService(Supabase.Client supabaseClient, ILogger<FranchiseService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<IEnumerable<FranchiseDto>> GetFranchisesAsync()
        {
            _logger.LogInformation("FranchiseService: Buscando todas as franquias do Supabase.");
            try
            {
                var response = await _supabaseClient.From<Franchise>().Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"FranchiseService: Erro Supabase ao buscar franquias: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    throw new ApplicationException($"Falha ao buscar franquias: {errorContent}");
                }
                return response.Models?.Select(f => new FranchiseDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Code = f.Code,
                    Cnpj = f.Cnpj,
                    CorporateName = f.CorporateName,
                    Address = f.Address,
                    City = f.City,
                    State = f.State,
                    Email = f.Email,
                    Whatsapp = f.Whatsapp,
                    Owner = f.Owner,
                    CreatedAt = f.CreatedAt?.ToString("yyyy-MM-dd"),
                    District = f.District,
                    Website = f.Website,
                    Note = f.Note,
                    ZipCode = f.ZipCode,
                    Instagram = f.Instagram,
                    Type = f.Type,
                    IdPloomes = f.IdPloomes
                }).ToList() ?? Enumerable.Empty<FranchiseDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FranchiseService: Erro ao buscar franquias.");
                throw new ApplicationException($"Erro ao buscar franquias: {ex.Message}", ex);
            }
        }

        public async Task<FranchiseDto?> GetFranchiseByIdPloomesAsync(string idPloomes)
        {
            _logger.LogInformation($"FranchiseService: Buscando franquia pelo IdPloomes: {idPloomes}");
            try
            {
                var response = await _supabaseClient.From<Franchise>()
                                                    .Where(f => f.IdPloomes == idPloomes)
                                                    .Get();

                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"FranchiseService: Erro Supabase ao buscar franquia por IdPloomes: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    throw new ApplicationException($"Falha ao buscar franquia por IdPloomes: {errorContent}");
                }
                return response.Models?.Select(f => new FranchiseDto
                {
                    Id = f.Id, Name = f.Name, Code = f.Code, Cnpj = f.Cnpj, CorporateName = f.CorporateName,
                    Address = f.Address, City = f.City, State = f.State, Email = f.Email, Whatsapp = f.Whatsapp,
                    Owner = f.Owner, CreatedAt = f.CreatedAt?.ToString("yyyy-MM-dd"), District = f.District,
                    Website = f.Website, Note = f.Note, ZipCode = f.ZipCode, Instagram = f.Instagram, Type = f.Type,
                    IdPloomes = f.IdPloomes
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FranchiseService: Erro ao buscar franquia por IdPloomes.");
                throw new ApplicationException($"Erro ao buscar franquia por IdPloomes: {ex.Message}", ex);
            }
        }
    }
}