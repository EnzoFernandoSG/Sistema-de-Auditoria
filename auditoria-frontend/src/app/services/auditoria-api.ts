import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { FranchiseDto, TechnicianDto, SupabaseDealDto, ProductDto } from '../models/supabase-dtos';

export interface StockQueryResult {
  quantidade: number;
  erro: string | null;
}

export interface AuditSaveResult {
  success: boolean;
  message: string;
}

export interface HistoricAuditDto { 
  id: string; // UUID
  cnpj?: string;
  unidadeFranqueada?: string;
  proprietario?: string;
  email?: string;
  whatsapp?: string;
  razaoSocial?: string;
  endereco?: string;
  cidade?: string;
  estado?: string;

  statusText?: string;
  dataVisitaTecnica?: string;
  createdAt?: string;

  qntEstoqueSistema?: number;
  m2InstaladosSistema?: number;
  estoqueOciosoSistema?: number;
  m2InstaladosCampo?: number;
  estoqueOciosoCampo?: number;
  placasDanificadasCampo?: number;

  imageUrls?: string[];
  imageNames?: string[];

  idsObrasProcessadas?: number[]; // long[] -> number[]
  contagemM2Campo?: number[]; // double[] -> number[]
  notReleasedJson?: string;
  notes?: string[];

  technician?: TechnicianDto; // DTO do técnico
  technicianId?: number; // long -> number

  observacoesGerais?: string;
  obrasNaoFornecidas?: HistoricAuditDtoObraNaoFornecida[]; // Nova interface abaixo
}

export interface HistoricAuditDtoObraNaoFornecida { // Renomeado
    contagemM2?: string;
    cliente?: string;
    endereco?: string;
    periodo?: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuditoriaApiService {
  private backendUrl = 'http://localhost:5250/api';

  constructor(private http: HttpClient) { }

  getFranchises(): Observable<FranchiseDto[]> {
    return this.http.get<FranchiseDto[]>(`${this.backendUrl}/AberturaAuditoria/franchises`);
  }

  getTechnicians(): Observable<TechnicianDto[]> {
    return this.http.get<TechnicianDto[]>(`${this.backendUrl}/AberturaAuditoria/technicians`);
  }

  getDeals(franchiseId: number, selectedDate: string | null = null): Observable<SupabaseDealDto[]> {
    let params: { [key: string]: string } = { franchiseId: franchiseId.toString() };
    if (selectedDate) {
      params['selectedDate'] = selectedDate;
    }
    return this.http.get<SupabaseDealDto[]>(`${this.backendUrl}/AberturaAuditoria/deals`, { params });
  }

  getStockByCnpj(cnpj: string): Observable<StockQueryResult> {
    return this.http.get<StockQueryResult>(`${this.backendUrl}/AberturaAuditoria/stock-by-cnpj`, {
      params: { cnpj }
    });
  }

  submitAudit(auditData: any): Observable<AuditSaveResult> {
    return this.http.post<AuditSaveResult>(`${this.backendUrl}/AberturaAuditoria/submit-audit`, auditData);
  }

  // NOVOS MÉTODOS PARA AuditoriaController
  getAuditorias(): Observable<HistoricAuditDto[]> {
    return this.http.get<HistoricAuditDto[]>(`${this.backendUrl}/HistoricAuditoria`); 
  }

  getAuditoriaById(id: string): Observable<HistoricAuditDto> {
    return this.http.get<HistoricAuditDto>(`${this.backendUrl}/HistoricAuditoria/${id}`); 
  }

  updateAuditoria(id: string, auditData: any): Observable<AuditSaveResult> {
    return this.http.put<AuditSaveResult>(`${this.backendUrl}/Auditoria/${id}`, auditData);
  }

  deleteAuditoria(id: string): Observable<AuditSaveResult> {
    return this.http.delete<AuditSaveResult>(`${this.backendUrl}/Auditoria/${id}`); 
  }
}