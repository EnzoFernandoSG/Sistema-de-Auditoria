using Supabase;
using SistemaAuditoria.Backend.Models.Supabase;
using SistemaAuditoria.Backend.DTOs;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Supabase.Postgrest.Responses;

namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class DealService : IDealService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<DealService> _logger;

        public DealService(Supabase.Client supabaseClient, ILogger<DealService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<IEnumerable<SupabaseDealDto>> GetDealsAsync(int franchiseId, DateTime? selectedDate = null)
        {
            _logger.LogInformation($"DealService: Buscando deals para FranchiseId: {franchiseId} com data: {selectedDate?.ToString() ?? "atual"} do Supabase.");
            try
            {
                var dealsQuery = _supabaseClient.From<Deal>()
                                                .Where(d => d.FranchiseCod == franchiseId);

                var dealsResponse = await dealsQuery.Get();

                if (!dealsResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await dealsResponse.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"DealService: Erro do Supabase ao buscar deals: Status {dealsResponse.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    throw new ApplicationException($"Falha ao buscar deals: {errorContent}");
                }

                if (dealsResponse.Models == null)
                {
                    _logger.LogInformation($"DealService: Nenhuns deals encontrados para FranchiseId: {franchiseId}.");
                    return Enumerable.Empty<SupabaseDealDto>();
                }

                DateTime auditDate = selectedDate ?? DateTime.UtcNow.Date;

                var dealsToProcess = dealsResponse.Models.Where(deal => deal.CreatedAt.Date <= auditDate.Date).ToList();
                _logger.LogInformation($"DealService: Após filtragem de CreatedAt (em memória): {dealsToProcess.Count} deals.");


                var activeDealsDto = new List<SupabaseDealDto>();

                foreach(var deal in dealsToProcess)
                {
                    _logger.LogDebug($"DealService: Processando deal ID: {deal.Id}");

                    var productsResponse = await _supabaseClient.From<Product>().Where(p => p.DealId == deal.Id).Get();
                    deal.Products = productsResponse?.Models?.ToList() ?? new List<Product>();
                    _logger.LogDebug($"DealService: Deal ID: {deal.Id} - Products: {deal.Products.Count}");

                    Customer? customer = null;
                    if (deal.IdCustomer.HasValue)
                    {
                        var customerResponse = await _supabaseClient.From<Customer>().Where(c => c.Id == deal.IdCustomer.Value).Get();
                        customer = customerResponse?.Models?.FirstOrDefault();
                        if (customer == null)
                        {
                            _logger.LogWarning($"DealService: Cliente (ID: {deal.IdCustomer.Value}) não encontrado para Deal ID: {deal.Id}.");
                        }
                    }
                    else
                    {
                        _logger.LogDebug($"DealService: Deal ID: {deal.Id} sem IdCustomer.");
                    }


                    DateTime? finalDateForCalc = deal.LastUpdateDate;
                    if (!finalDateForCalc.HasValue)
                    {
                        finalDateForCalc = deal.CreatedAt;
                    }

                    if (!finalDateForCalc.HasValue || !deal.Period.HasValue)
                    {
                        _logger.LogWarning($"DealService: Deal ID: {deal.Id} sem LastUpdateDate/CreatedAt ou Periodo para cálculo de atividade. Ignorando.");
                        continue;
                    }

                    var startDateForEndDateCalc = finalDateForCalc.Value.Date;
                    var endDate = startDateForEndDateCalc.AddDays(deal.Period.Value);

                    if (endDate.Date >= auditDate.Date)
                    {
                        _logger.LogDebug($"DealService: Deal ID: {deal.Id} ATIVO. Adicionando ao DTO.");
                        activeDealsDto.Add(new SupabaseDealDto
                        {
                            Id = deal.Id,
                            DealId = deal.Id,
                            Amount = deal.Amount,
                            IdCurrency = deal.IdCurrency,
                            IdCustomer = deal.IdCustomer,
                            IdStatus = deal.IdStatus,
                            CreateDate = deal.CreatedAt.ToString("yyyy-MM-dd"),
                            Periodo = deal.Period,
                            Cep = deal.Cep,
                            StreetNumber = deal.StreetNumber,
                            StreetName = deal.StreetName,
                            District = deal.District,
                            State = deal.State,
                            City = deal.City,
                            DocumentUrl = deal.DocumentUrl,
                            LastUpdateDate = deal.LastUpdateDate?.ToString("yyyy-MM-dd") ?? "",
                            FranchiseCod = deal.FranchiseCod,

                            Customer = (customer != null) ? new CustomerDto
                            {
                                Id = customer.Id,
                                Name = customer.Name,
                                Phone = customer.Phone,
                                Email = customer.Email,
                                Street = customer.Street,
                                Neighborhood = customer.Neighborhood,
                                Number = customer.Number,
                                Cep = customer.Cep,
                                Cnpj = customer.Cnpj,
                                Cpf = customer.Cpf,
                                CreatedAt = customer.CreatedAt?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                UpdatedAt = customer.UpdatedAt?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                UserId = customer.UserId,
                                FranchiseCod = customer.FranchiseCod,
                                CompanyName = customer.CompanyName,
                                Complement = customer.Complement,
                                City = customer.City,
                                State = customer.State
                            } : null,

                            Cliente = customer?.CompanyName ?? customer?.Name ?? $"Cliente do Deal #{deal.Id}",
                            M2Instalados = deal.Products?.Sum(p => p.Quantity ?? 0) ?? 0,
                            AreasProtegidas = deal.Products?.Select(p => new ProductDto {
                                Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice, Total = p.Total, Quantity = p.Quantity,
                                DealId = p.DealId, CurrencyId = p.CurrencyId, CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                            }).ToList() ?? new List<ProductDto>(),
                            DataTermino = deal.Period.HasValue ? deal.CreatedAt.AddDays(deal.Period.Value).ToString("yyyy-MM-dd") : null,
                            Status = selectedDate.HasValue ? "ativa_no_passado" : "ativa",
                            Expirada = false
                        });
                    } else {
                        _logger.LogDebug($"DealService: Deal ID: {deal.Id} INATIVO (expirado ou fora do período).");
                    }
                }
                _logger.LogInformation($"DealService: Retornando {activeDealsDto.Count} deals DTOs ativos.");
                return activeDealsDto.OrderByDescending(d => DateTime.TryParse(d.LastUpdateDate, out var lDate) ? lDate : (DateTime.TryParse(d.CreateDate, out var cDate) ? cDate : DateTime.MinValue)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DealService: Erro fatal ao buscar obras para FranchiseId: {franchiseId} ou data: {selectedDate?.ToString() ?? "atual"}.");
                throw new ApplicationException($"Erro ao buscar obras: {ex.Message}", ex);
            }
        }
    }
}