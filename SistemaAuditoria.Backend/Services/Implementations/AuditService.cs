using Supabase;
using SistemaAuditoria.Backend.Configurations;
using SistemaAuditoria.Backend.Models; // Para AuditData, AuditoriaDbModel
using SistemaAuditoria.Backend.Models.Supabase; // Para Technician, Deal, Product, Customer, NfeRemessa, NfeItem
using SistemaAuditoria.Backend.DTOs; // Para DTOs (HistoricAuditDto, TechnicianDto, SupabaseDealDto)
using Microsoft.Extensions.Logging; // Para ILogger
using System.Linq; // Para LINQ (Select, Where, Sum, ToList, Any)
using System; // Para DateTime, Guid
using System.Collections.Generic; // Para List, Dictionary
using System.Globalization; // Para CultureInfo, NumberStyles
using System.Text.Json; // Para JsonSerializer


namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<AuditService> _logger;

        public AuditService(Supabase.Client supabaseClient, ILogger<AuditService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        // MÉTODOS AUXILIARES
        private double? _ParseDoubleNullable(string? value)
        {
            if (value != null && double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
            {
                return parsedValue;
            }
            return null;
        }

        // Helper para mapear AuditoriaDbModel para HistoricAuditDto
        private async Task<HistoricAuditDto?> MapAuditoriaDbModelToDto(AuditoriaDbModel dbModel)
        {
            if (dbModel == null) return null;

            TechnicianDto? technicianDto = null;
            if (dbModel.TechnicianId.HasValue)
            {
                try
                {
                    var techResponse = await _supabaseClient.From<Technician>().Where(t => t.Id == dbModel.TechnicianId.Value).Get();
                    if (techResponse.ResponseMessage.IsSuccessStatusCode && techResponse.Models != null && techResponse.Models.Any())
                    {
                        var tech = techResponse.Models.First();
                        technicianDto = new TechnicianDto {
                            Id = tech.Id, Name = tech.Name, Phone = tech.Phone,
                            CreatedAt = tech.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                            Cpf = tech.Cpf, Rg = tech.Rg, Born = tech.Born?.ToString("yyyy-MM-dd")
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"AuditService: Erro ao buscar detalhes do técnico ID {dbModel.TechnicianId}: {ex.Message}");
                }
            }

            List<HistoricAuditDto.ObraNaoFornecidaDto>? obrasNaoFornecidasDto = null;
            if (!string.IsNullOrEmpty(dbModel.NotReleased))
            {
                try
                {
                    obrasNaoFornecidasDto = JsonSerializer.Deserialize<List<HistoricAuditDto.ObraNaoFornecidaDto>>(dbModel.NotReleased);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"AuditService: Erro ao desserializar NotReleasedJson para Auditoria ID {dbModel.Id}: {ex.Message}");
                }
            }

            string? observacoesGerais = dbModel.Notes != null && dbModel.Notes.Any() ? dbModel.Notes.Last() : null;
            List<string>? notesWithoutGeneral = null;
            if (dbModel.Notes != null)
            {
                if (dbModel.Notes.Any() && dbModel.Notes.Last() == observacoesGerais)
                {
                    notesWithoutGeneral = dbModel.Notes.Take(dbModel.Notes.Count - 1).ToList();
                }
                else
                {
                    notesWithoutGeneral = dbModel.Notes.ToList();
                }
            }
            notesWithoutGeneral = notesWithoutGeneral ?? new List<string>();


            return new HistoricAuditDto
            {
                Id = dbModel.Id,
                Cnpj = dbModel.Cnpj,
                UnidadeFranqueada = dbModel.UnidadeFranqueada,
                Proprietario = dbModel.Proprietario,
                Email = dbModel.Email,
                Whatsapp = dbModel.Whatsapp,
                RazaoSocial = dbModel.RazaoSocial,
                Endereco = dbModel.Endereco,
                Cidade = dbModel.Cidade,
                Estado = dbModel.Estado,

                StatusText = dbModel.Status == 1 ? "Congruente" : "Incongruente",
                DataVisitaTecnica = dbModel.DataVisitaTecnica?.ToString("yyyy-MM-dd"),
                CreatedAt = dbModel.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),

                QntEstoqueSistema = dbModel.QntEstoqueSistema,
                M2InstaladosSistema = dbModel.M2InstaladosSistema,
                EstoqueOciosoSistema = dbModel.EstoqueOciosoSistema,
                M2InstaladosCampo = dbModel.M2InstaladosCampo,
                EstoqueOciosoCampo = dbModel.EstoqueOciosoCampo,
                PlacasDanificadasCampo = dbModel.PlacasDanificadasCampo,

                ImageUrls = dbModel.ImageM,
                ImageNames = dbModel.ImageName,

                IdsObrasProcessadas = dbModel.IdsPloomesOrders,
                ContagemM2Campo = dbModel.ContagemM2Campo,
                NotReleasedJson = dbModel.NotReleased,
                Notes = notesWithoutGeneral,

                Technician = technicianDto,
                TechnicianId = dbModel.TechnicianId,

                ObservacoesGerais = observacoesGerais,
                ObrasNaoFornecidas = obrasNaoFornecidasDto
            };
        }


        // --- IMPLEMENTAÇÃO DOS MÉTODOS DA INTERFACE IAuditService ---

        public async Task<AuditSaveResult> SubmitAuditAsync(AuditData auditData)
        {
            _logger.LogInformation("AuditService: Salvando auditoria no Supabase.");
            try
            {
                // Mapear AuditData (vindo do frontend) para AuditoriaDbModel
                var auditToSave = new AuditoriaDbModel
                {
                    FranqueadoId = long.TryParse(auditData.FranqueadoId, out long fId) ? fId : (long?)null,

                    UnidadeFranqueada = auditData.UnidadeFranqueada,
                    Proprietario = auditData.Proprietario,
                    Email = auditData.Email,
                    EnderecoEstoque = auditData.EnderecoEstoque,
                    Cidade = auditData.Cidade,
                    Estado = auditData.Estado,
                    Whatsapp = auditData.Whatsapp,
                    Cnpj = auditData.Cnpj,
                    RazaoSocial = auditData.RazaoSocial,
                    Endereco = auditData.Endereco,
                    TechnicianId = (long?)auditData.TechnicianId,

                    Status = (short?)(auditData.Status == "congruente" ? 1 : 0),

                    ImageM = auditData.Images ?? new List<string>(), 
                    ImageName = auditData.ImageNames ?? new List<string>(),

                    QntEstoqueSistema = _ParseDoubleNullable(auditData.QuantidadeEstoqueSistema),
                    DataVisitaTecnica = auditData.DataVisitaTecnica != null && DateTime.TryParse(auditData.DataVisitaTecnica, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime valDVT) ? valDVT : (DateTime?)null,
                    M2InstaladosCampo = _ParseDoubleNullable(auditData.M2InstaladosCampo),
                    EstoqueOciosoCampo = auditData.EstoqueOciosoCampo,
                    PlacasDanificadasCampo = auditData.PlacasDanificadasCampo,
                    M2InstaladosSistema = _ParseDoubleNullable(auditData.M2InstaladosSistema), 
                    EstoqueOciosoSistema = _ParseDoubleNullable(auditData.EstoqueOciosoSistema), 
                    
                    IdsPloomesOrders = auditData.ObrasProcessadasIds?.Select(id => (long)id).ToList() ?? new List<long>(), 
                    ContagemM2Campo = auditData.ContagemM2Campo?.Select(s => _ParseDoubleNullable(s) ?? 0.0).ToList() ?? new List<double>(), 
                    NotReleased = auditData.ObrasNaoFornecidasJson,
                    Notes = auditData.ObservacoesPorOrdemValues ?? new List<string>(),

                    CreatedAt = auditData.CustomCreatedAt ?? DateTime.UtcNow,
                };

                // Consolidar observações gerais e observações por ordem no campo 'Notes' do DB
                List<string> allNotes = new List<string>();
                if (auditToSave.Notes != null && auditToSave.Notes.Any())
                {
                    allNotes.AddRange(auditToSave.Notes);
                }
                if (!string.IsNullOrEmpty(auditData.ObservacoesGerais)) // ObservacoesGerais do AuditData
                {
                    allNotes.Add(auditData.ObservacoesGerais);
                }
                auditToSave.Notes = allNotes.Any() ? allNotes : null; // Atribui a lista final


                // Lógica de cálculo (m2_instalados_sistema e estoque_ocioso_sistema)
                // Se o frontend envia m2InstaladosSistema e estoqueOciosoSistema (como string),
                // as linhas acima já os parsearam e atribuíram. A lógica abaixo é para RECALCULAR no backend.
                if (auditToSave.IdsPloomesOrders != null && auditToSave.IdsPloomesOrders.Any())
                {
                    var dealsResponse = await _supabaseClient.From<Deal>()
                                                            .Filter("id", Supabase.Postgrest.Constants.Operator.In, auditToSave.IdsPloomesOrders.Select(id => (long)id).ToList())
                                                            .Get();
                    if (dealsResponse.ResponseMessage.IsSuccessStatusCode && dealsResponse.Models != null)
                    {
                        var m2DealsProcessados = 0.0;
                        foreach (var deal in dealsResponse.Models)
                        {
                            var productsResponse = await _supabaseClient.From<Product>().Where(p => p.DealId == deal.Id).Get();
                            deal.Products = productsResponse?.Models?.ToList() ?? new List<Product>();
                            m2DealsProcessados += deal.Products?.Sum(p => p.Quantity ?? 0) ?? 0;
                        }
                        auditToSave.M2InstaladosSistema = m2DealsProcessados;
                    }
                } else {
                    auditToSave.M2InstaladosSistema = 0.0;
                }

                auditToSave.EstoqueOciosoSistema = (auditToSave.QntEstoqueSistema ?? 0.0) - (auditToSave.M2InstaladosSistema ?? 0.0);


                _logger.LogInformation("AuditService: Dados de auditoria mapeados para AuditoriaDbModel. Enviando para Supabase...");
                var response = await _supabaseClient.From<AuditoriaDbModel>().Insert(auditToSave);

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation("AuditService: Auditoria salva com sucesso no Supabase!");
                    return new AuditSaveResult { Success = true, Message = "Auditoria salva com sucesso!" };
                }
                else
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"AuditService: Erro ao salvar auditoria no Supabase: Status {response.ResponseMessage.StatusCode} - {response.ResponseMessage.ReasonPhrase} - Conteúdo: {errorContent}");
                    return new AuditSaveResult { Success = false, Message = $"Erro ao salvar auditoria: {response.ResponseMessage.ReasonPhrase} - {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuditService: 💥 Exceção ao salvar auditoria: {Message}", ex.Message);
                return new AuditSaveResult { Success = false, Message = $"Exceção ao salvar auditoria: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<HistoricAuditDto>> GetAuditoriasAsync()
        {
            _logger.LogInformation("AuditService: Buscando todas as auditorias do Supabase.");
            try
            {
                var response = await _supabaseClient.From<AuditoriaDbModel>().Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"AuditService: Erro Supabase ao buscar auditorias: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    throw new ApplicationException($"Falha ao buscar auditorias: {errorContent}");
                }

                List<HistoricAuditDto> auditoriasDto = new List<HistoricAuditDto>();
                if (response?.Models != null)
                {
                    foreach (var dbModel in response.Models)
                    {
                        var dto = await MapAuditoriaDbModelToDto(dbModel);
                        if (dto != null) auditoriasDto.Add(dto);
                    }
                }
                return auditoriasDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuditService: Erro ao buscar auditorias.");
                throw new ApplicationException($"Erro ao buscar auditorias: {ex.Message}", ex);
            }
        }

        public async Task<HistoricAuditDto?> GetAuditoriaByIdAsync(Guid id)
        {
            _logger.LogInformation($"AuditService: Buscando auditoria ID: {id} do Supabase.");
            try
            {
                var response = await _supabaseClient.From<AuditoriaDbModel>().Where(a => a.Id == id).Get();
                if (!response.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"AuditService: Erro Supabase ao buscar auditoria {id}: Status {response.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    return null;
                }
                var dbModel = response.Models?.FirstOrDefault();
                if (dbModel == null) return null;

                return await MapAuditoriaDbModelToDto(dbModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AuditService: Erro ao buscar auditoria ID: {id}.");
                throw new ApplicationException($"Erro ao buscar auditoria: {ex.Message}", ex);
            }
        }

        public async Task<AuditSaveResult> UpdateAuditoriaAsync(Guid id, AuditData auditData)
        {
            _logger.LogInformation($"AuditService: Atualizando auditoria ID: {id} no Supabase.");
            try
            {
                // Mapeamento de AuditData para AuditoriaDbModel para atualização
                var auditToUpdate = new AuditoriaDbModel
                {
                    Status = (short?)(auditData.Status == "congruente" ? 1 : 0),
                    Cnpj = auditData.Cnpj,
                    DataVisitaTecnica = auditData.DataVisitaTecnica != null && DateTime.TryParse(auditData.DataVisitaTecnica, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out DateTime valDVT) ? valDVT : (DateTime?)null,
                    QntEstoqueSistema = _ParseDoubleNullable(auditData.QuantidadeEstoqueSistema),
                    ImageM = auditData.Images ?? new List<string>(),
                    ImageName = auditData.ImageNames ?? new List<string>(),
                    M2InstaladosSistema = _ParseDoubleNullable(auditData.M2InstaladosSistema),
                    M2InstaladosCampo = _ParseDoubleNullable(auditData.M2InstaladosCampo),
                    PlacasDanificadasCampo = auditData.PlacasDanificadasCampo,
                    UnidadeFranqueada = auditData.UnidadeFranqueada,
                    Email = auditData.Email,
                    Proprietario = auditData.Proprietario,
                    EnderecoEstoque = auditData.EnderecoEstoque,
                    Whatsapp = auditData.Whatsapp,
                    Cidade = auditData.Cidade,
                    Estado = auditData.Estado,
                    EstoqueOciosoCampo = auditData.EstoqueOciosoCampo,
                    EstoqueOciosoSistema = _ParseDoubleNullable(auditData.EstoqueOciosoSistema),
                    IdsPloomesOrders = auditData.ObrasProcessadasIds?.Select(oid => (long)oid).ToList() ?? new List<long>(),
                    FranqueadoId = long.TryParse(auditData.FranqueadoId, out long fId) ? fId : (long?)null,
                    ContagemM2Campo = auditData.ContagemM2Campo?.Select(s => _ParseDoubleNullable(s) ?? 0.0).ToList() ?? new List<double>(),
                    NotReleased = auditData.ObrasNaoFornecidasJson,
                    Notes = auditData.ObservacoesPorOrdemValues != null ? new List<string>(auditData.ObservacoesPorOrdemValues) : null,
                    TechnicianId = (long?)auditData.TechnicianId,
                    RazaoSocial = auditData.RazaoSocial,
                    Endereco = auditData.Endereco,
                    LastUpdateDate = DateTime.UtcNow // Atualizar a data de modificação
                };

                // Consolidar observações gerais e observações por ordem no campo 'Notes' do DB
                List<string> allNotes = new List<string>();
                if (auditToUpdate.Notes != null && auditToUpdate.Notes.Any())
                {
                    allNotes.AddRange(auditToUpdate.Notes);
                }
                if (!string.IsNullOrEmpty(auditData.ObservacoesGerais))
                {
                    allNotes.Add(auditData.ObservacoesGerais);
                }
                auditToUpdate.Notes = allNotes.Any() ? allNotes : null;


                var response = await _supabaseClient.From<AuditoriaDbModel>()
                                                    .Where(a => a.Id == id)
                                                    .Update(auditToUpdate); // Update opera com o objeto completo

                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"AuditService: Auditoria ID {id} atualizada com sucesso no Supabase!");
                    return new AuditSaveResult { Success = true, Message = "Auditoria atualizada com sucesso!" };
                }
                else
                {
                    string? errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"AuditService: Erro ao atualizar auditoria ID {id} no Supabase: Status {response.ResponseMessage.StatusCode} - {response.ResponseMessage.ReasonPhrase} - Conteúdo: {errorContent}");
                    return new AuditSaveResult { Success = false, Message = $"Erro ao atualizar auditoria: {response.ResponseMessage.ReasonPhrase} - {errorContent}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AuditService: 💥 Exceção ao atualizar auditoria ID {id}: {ex.Message}", ex);
                return new AuditSaveResult { Success = false, Message = $"Exceção ao atualizar auditoria: {ex.Message}" };
            }
        }

        public async Task<AuditSaveResult> DeleteAuditoriaAsync(Guid id)
        {
            _logger.LogInformation($"AuditService: Excluindo auditoria ID: {id} do Supabase.");
            try
            {
                await _supabaseClient.From<AuditoriaDbModel>()
                                     .Where(a => a.Id == id)
                                     .Delete();


                _logger.LogInformation($"AuditService: Auditoria ID {id} excluída com sucesso do Supabase!");
                return new AuditSaveResult { Success = true, Message = "Auditoria excluída com sucesso!" };
            }
            catch (Exception ex) // Captura qualquer exceção durante a operação de exclusão
            {
                _logger.LogError(ex, $"AuditService: 💥 Exceção ao excluir auditoria ID {id}: {ex.Message}");
                if (ex is Supabase.Postgrest.Exceptions.PostgrestException pgEx)
                {
                    return new AuditSaveResult { Success = false, Message = $"Erro do banco de dados ao excluir: {pgEx.Message}" };
                }
                return new AuditSaveResult { Success = false, Message = $"Exceção ao excluir auditoria: {ex.Message}" };
            }
        }
    }
}