import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { NgIconsModule } from '@ng-icons/core';
import { RouterLink } from '@angular/router';
import {
  lucideEdit, lucideEye, lucideTrash2, lucideLoader, lucideAlertTriangle,
  lucideCalendar, lucideDatabase, lucideFileText, lucideUser, lucideRefreshCcw, lucideCircleCheck,
  lucideX, lucideSave // Adicionar lucideX e lucideSave
} from '@ng-icons/lucide';

import { AuditoriaApiService } from '../../services/auditoria-api';
import type { HistoricAuditDto, AuditSaveResult } from '../../services/auditoria-api';
import type { TechnicianDto } from '../../models/supabase-dtos';
import type { AuditDataForm } from '../../models/audit-form.model'; // Importar AuditDataForm

@Component({
  selector: 'app-historico-auditoria',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgIconsModule,
    RouterLink,
    lucideEdit, lucideEye, lucideTrash2, lucideLoader, lucideAlertTriangle,
    lucideCalendar, lucideDatabase, lucideFileText, lucideUser, lucideRefreshCcw, lucideCircleCheck,
    lucideX, lucideSave // Adicionar aqui
  ],
  templateUrl: './historico-auditoria.html',
  styleUrl: './historico-auditoria.scss'
})
export class HistoricoAuditoria implements OnInit, OnDestroy {
  loadingAuditorias = false;
  auditorias: HistoricAuditDto[] = [];
  erro: string | null = null;
  sucesso: string | null = null;
  auditoriaSelecionada: HistoricAuditDto | null = null;
  modoEdicao: boolean = false;

  // Propriedade para os dados do formulário de edição
  editFormData: AuditDataForm = {}; // Inicializar com objeto vazio


  private subscriptions: Subscription = new Subscription();

  constructor(private auditoriaApi: AuditoriaApiService) { }

  ngOnInit(): void {
    this.carregarAuditorias();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  setErro(message: string | null): void {
    this.erro = message;
    if (message) this.setSucesso(null);
  }

  setSucesso(message: string | null): void {
    this.sucesso = message;
    if (message) this.setErro(null);
  }

  carregarAuditorias(): void {
    this.loadingAuditorias = true;
    this.setErro(null);
    this.subscriptions.add(
      this.auditoriaApi.getAuditorias().subscribe({
        next: (data: HistoricAuditDto[]) => {
          this.auditorias = data;
        },
        error: (err: any) => {
          console.error("Erro ao carregar histórico de auditorias:", err);
          this.setErro(`Falha ao carregar auditorias: ${err.message}`);
        },
        complete: () => this.loadingAuditorias = false
      })
    );
  }

  verDetalhes(auditoria: HistoricAuditDto): void {
    this.auditoriaSelecionada = auditoria;
    this.modoEdicao = false;
  }

  editarAuditoria(auditoria: HistoricAuditDto): void {
    // Ao editar, mapeie HistoricAuditDto para AuditDataForm
    this.auditoriaSelecionada = { ...auditoria }; // Criar uma cópia para não alterar diretamente o objeto da lista

    // Mapear HistoricAuditDto para editFormData (AuditDataForm)
    this.editFormData = {
      franqueadoId: auditoria.franqueadoId?.toString(),
      unidadeFranqueada: auditoria.unidadeFranqueada,
      proprietario: auditoria.proprietario,
      email: auditoria.email,
      whatsapp: auditoria.whatsapp,
      cnpj: auditoria.cnpj,
      razaoSocial: auditoria.razaoSocial,
      endereco: auditoria.endereco,
      cidade: auditoria.cidade,
      estado: auditoria.estado,
      // enderecoEstoque não está no HistoricAuditDto, manter como mock se necessário
      enderecoEstoque: "Não informado",

      technicianId: auditoria.technicianId,
      status: auditoria.statusText === "Congruente" ? "congruente" : "incongruente",

      dataVisitaTecnica: auditoria.dataVisitaTecnica,
      m2InstaladosCampo: auditoria.m2InstaladosCampo?.toString(),
      estoqueOciosoCampo: auditoria.estoqueOciosoCampo,
      placasDanificadasCampo: auditoria.placasDanificadasCampo,
      observacoesGerais: auditoria.ObservacoesGerais,

      quantidadeEstoqueSistema: auditoria.qntEstoqueSistema?.toString(),
      m2InstaladosSistema: auditoria.m2InstaladosSistema?.toString(),
      estoqueOciosoSistema: auditoria.estoqueOciosoSistema?.toString(),

      images: auditoria.imageUrls,
      imageNames: auditoria.imageNames,

      obrasProcessadasIds: auditoria.idsObrasProcessadas,
      contagemM2Campo: auditoria.contagemM2Campo?.map(String), // Mapear number[] para string[]
      observacoesPorOrdemValues: auditoria.notes,

      obrasNaoFornecidasJson: auditoria.notReleasedJson,
      customCreatedAt: auditoria.createdAt ? new Date(auditoria.createdAt) : null,
    };

    this.modoEdicao = true;
  }

  salvarEdicao(auditoriaAtualizada: HistoricAuditDto): void {
    if (!auditoriaAtualizada.id) {
        this.setErro("ID da auditoria ausente para salvar.");
        return;
    }
    this.loadingAuditorias = true;
    this.setErro(null);
    this.setSucesso(null);

    // Mapear editFormData (AuditDataForm) de volta para AuditData
    const auditDataParaAtualizar: AuditDataForm = {
      franqueadoId: this.editFormData.franqueadoId,
      unidadeFranqueada: this.editFormData.unidadeFranqueada,
      proprietario: this.editFormData.proprietario,
      email: this.editFormData.email,
      whatsapp: this.editFormData.whatsapp,
      cnpj: this.editFormData.cnpj,
      razaoSocial: this.editFormData.razaoSocial,
      endereco: this.editFormData.endereco,
      cidade: this.editFormData.cidade,
      estado: this.editFormData.estado,
      enderecoEstoque: this.editFormData.enderecoEstoque,

      technicianId: this.editFormData.technicianId,
      status: this.editFormData.status,

      dataVisitaTecnica: this.editFormData.dataVisitaTecnica,
      m2InstaladosCampo: this.editFormData.m2InstaladosCampo,
      estoqueOciosoCampo: this.editFormData.estoqueOciosoCampo,
      placasDanificadasCampo: this.editFormData.placasDanificadasCampo,
      observacoesGerais: this.editFormData.observacoesGerais,

      quantidadeEstoqueSistema: this.editFormData.quantidadeEstoqueSistema,
      m2InstaladosSistema: this.editFormData.m2InstaladosSistema,
      estoqueOciosoSistema: this.editFormData.estoqueOciosoSistema,

      images: this.editFormData.images,
      imageNames: this.editFormData.imageNames,

      obrasProcessadasIds: this.editFormData.obrasProcessadasIds,
      contagemM2Campo: this.editFormData.contagemM2Campo,
      observacoesPorOrdemValues: this.editFormData.observacoesPorOrdemValues,

      obrasNaoFornecidasJson: this.editFormData.obrasNaoFornecidasJson,
      customCreatedAt: this.editFormData.customCreatedAt,
    };


    this.subscriptions.add(
      this.auditoriaApi.updateAuditoria(auditoriaAtualizada.id, auditDataParaAtualizar).subscribe({
        next: (result: AuditSaveResult) => {
          if (result.success) {
            this.setSucesso("Auditoria atualizada com sucesso!");
            this.carregarAuditorias();
            this.auditoriaSelecionada = null;
            this.modoEdicao = false;
          } else {
            this.setErro(result.message || "Erro ao atualizar auditoria.");
          }
        },
        error: (err: any) => {
          console.error("Erro ao atualizar auditoria:", err);
          this.setErro(`Falha ao atualizar: ${err.message}`);
        },
        complete: () => this.loadingAuditorias = false
      })
    );
  }

  excluirAuditoria(id: string): void {
    if (!confirm("Tem certeza que deseja excluir esta auditoria?")) {
      return;
    }
    this.loadingAuditorias = true;
    this.setErro(null);
    this.setSucesso(null);

    this.subscriptions.add(
      this.auditoriaApi.deleteAuditoria(id).subscribe({
        next: (result: AuditSaveResult) => {
          if (result.success) {
            this.setSucesso("Auditoria excluída com sucesso!");
            this.carregarAuditorias();
          } else {
            this.setErro(result.message || "Erro ao excluir auditoria.");
          }
        },
        error: (err: any) => {
          console.error("Erro ao excluir auditoria:", err);
          this.setErro(`Falha ao excluir: ${err.message}`);
        },
        complete: () => this.loadingAuditorias = false
      })
    );
  }

  fecharDetalhes(): void {
    this.auditoriaSelecionada = null;
    this.modoEdicao = false;
  }

  // FUNÇÃO TRACKBY PARA *ngFor
  trackByAuditoriaId(index: number, auditoria: HistoricAuditDto): string {
    return auditoria.id; // Retorna o ID único da auditoria
  }

  // Helper para formatar o valor de estoque ocioso do campo
  formatarEstoqueOciosoCampo(value: number | null | undefined): string {
    return (value === null || value === undefined) ? 'N/A' : value.toString();
  }

  // Helper para formatar o valor de placas danificadas do campo
  formatarPlacasDanificadasCampo(value: number | null | undefined): string {
    return (value === null || value === undefined) ? 'N/A' : value.toString();
  }

  // Helper para formatar o valor de m2 instalados do sistema
  formatarM2InstaladosSistema(value: number | null | undefined): string {
    return (value === null || value === undefined) ? 'N/A' : value.toString();
  }
}