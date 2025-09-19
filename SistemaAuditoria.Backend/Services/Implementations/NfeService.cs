using Supabase;
using SistemaAuditoria.Backend.Models; // Para StockQueryResult
using SistemaAuditoria.Backend.Models.Supabase; // Para NfeRemessa, NfeItem
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Collections.Generic;
using Supabase.Postgrest.Responses;


namespace SistemaAuditoria.Backend.Services.Implementations
{
    public class NfeService : INfeService
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger<NfeService> _logger;

        public NfeService(Supabase.Client supabaseClient, ILogger<NfeService> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<StockQueryResult> GetStockByCnpjAsync(string cnpj)
        {
            _logger.LogInformation($"NfeService: Buscando quantidade de estoque para CNPJ: {cnpj} no Supabase (NFe).");

            if (string.IsNullOrEmpty(cnpj))
            {
                return new StockQueryResult { Quantidade = 0, Erro = "CNPJ não pode ser vazio." };
            }

            try
            {
                decimal quantidadePositiva = 0;
                decimal quantidadeNegativa = 0; // Declarar uma única vez aqui

                // Buscar remessas POSITIVAS (Comodato/Locação + Remessa de Comodato)
                _logger.LogInformation($"NfeService: 📥 Buscando remessas POSITIVAS para CNPJ: {cnpj}...");
                var remessasPositivasResponse = await _supabaseClient.From<NfeRemessa>()
                                                                    .Where(n => n.CnpjDestinatario == cnpj)
                                                                    .Filter("natureza_operacao", Supabase.Postgrest.Constants.Operator.In, new List<string> { "Remessa de Bem por Conta de Contrato de Comodato ou Locacao", "REMESSA DE COMODATO" })
                                                                    .Get();

                if (!remessasPositivasResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await remessasPositivasResponse.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"NfeService: Erro Supabase ao buscar remessas positivas: Status {remessasPositivasResponse.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    return new StockQueryResult { Quantidade = 0, Erro = $"Erro ao buscar remessas positivas: {errorContent}" };
                }

                if (remessasPositivasResponse.Models != null && remessasPositivasResponse.Models.Any())
                {
                    _logger.LogInformation($"NfeService: ✅ Encontradas {remessasPositivasResponse.Models.Count} remessas positivas.");
                    var idsRemessasPositivas = remessasPositivasResponse.Models.Select(n => n.Id).ToList();

                    var itensPositivosResponse = await _supabaseClient.From<NfeItem>()
                                                                    .Filter("nfe_id", Supabase.Postgrest.Constants.Operator.In, idsRemessasPositivas) // Qualificado
                                                                    .Get();

                    if (!itensPositivosResponse.ResponseMessage.IsSuccessStatusCode)
                    {
                        string? errorContent = await itensPositivosResponse.ResponseMessage.Content.ReadAsStringAsync();
                        _logger.LogError($"NfeService: Erro Supabase ao buscar itens positivos: Status {itensPositivosResponse.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                        return new StockQueryResult { Quantidade = 0, Erro = $"Erro ao buscar itens positivos: {errorContent}" };
                    }

                    quantidadePositiva = itensPositivosResponse.Models?.Sum(item => item.QuantidadeComercial ?? 0) ?? 0;
                    _logger.LogInformation($"NfeService: 📊 Quantidade positiva (Comodato/Locação): {quantidadePositiva}");
                }
                else
                {
                    _logger.LogInformation("NfeService: 📭 Nenhuma remessa positiva encontrada.");
                }

                // Buscar remessas NEGATIVAS (Retorno de Comodato + Retorno de Bem Remetido)
                _logger.LogInformation($"NfeService: 📤 Buscando remessas NEGATIVAS para CNPJ: {cnpj}...");
                var remessasNegativasResponse = await _supabaseClient.From<NfeRemessa>()
                                                                    .Where(n => n.CnpjDestinatario == cnpj)
                                                                    .Filter("natureza_operacao", Supabase.Postgrest.Constants.Operator.In, new List<string> { "RETORNO DE COMODATO", "Retorno de Bem Remetido - Conta de Contrato Comodato/locacao" })
                                                                    .Get();

                if (!remessasNegativasResponse.ResponseMessage.IsSuccessStatusCode)
                {
                    string? errorContent = await remessasNegativasResponse.ResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogError($"NfeService: Erro Supabase ao buscar remessas negativas: Status {remessasNegativasResponse.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                    return new StockQueryResult { Quantidade = 0, Erro = $"Erro ao buscar remessas negativas: {errorContent}" };
                }

                if (remessasNegativasResponse.Models != null && remessasNegativasResponse.Models.Any())
                {
                    _logger.LogInformation($"NfeService: ✅ Encontradas {remessasNegativasResponse.Models.Count} remessas negativas.");
                    var idsRemessasNegativas = remessasNegativasResponse.Models.Select(n => n.Id).ToList();

                    var itensNegativosResponse = await _supabaseClient.From<NfeItem>()
                                                                    .Filter("nfe_id", Supabase.Postgrest.Constants.Operator.In, idsRemessasNegativas)
                                                                    .Get();

                    if (!itensNegativosResponse.ResponseMessage.IsSuccessStatusCode)
                    {
                        string? errorContent = await itensNegativosResponse.ResponseMessage.Content.ReadAsStringAsync();
                        _logger.LogError($"NfeService: Erro Supabase ao buscar itens negativos: Status {itensNegativosResponse.ResponseMessage.StatusCode}, Conteúdo: {errorContent}");
                        return new StockQueryResult { Quantidade = 0, Erro = $"Erro ao buscar itens negativos: {errorContent}" };
                    }

                    quantidadeNegativa = itensNegativosResponse.Models?.Sum(item => item.QuantidadeComercial ?? 0) ?? 0;
                    _logger.LogInformation($"NfeService: 📊 Quantidade negativa (Retorno Comodato): {quantidadeNegativa}");
                }
                else
                {
                    _logger.LogInformation("NfeService: 📭 Nenhuma remessa negativa encontrada.");
                }

                decimal quantidadeFinal = quantidadePositiva - quantidadeNegativa;

                _logger.LogInformation($"NfeService: 🧮 Cálculo final: {quantidadeFinal}");
                return new StockQueryResult { Quantidade = (int)quantidadeFinal, Erro = null };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NfeService: 💥 Exceção ao buscar quantidade de estoque (NFe): {Message}", ex.Message);
                return new StockQueryResult { Quantidade = 0, Erro = $"Exceção: {ex.Message}" };
            }
        }
    }
}