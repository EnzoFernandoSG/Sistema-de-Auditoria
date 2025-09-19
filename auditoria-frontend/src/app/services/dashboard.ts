import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

interface DashboardStats {
  totalFranchises: number;
  monthlyAudits: number;
  userName: string;
}

@Injectable({
  providedIn: 'root'
})
export class Dashboard {

  constructor() { }

  // Simula a busca de dados do dashboard
  getDashboardStats(): Observable<DashboardStats> {
    // Retorna um Observable com dados mockados após um pequeno atraso
    const mockData: DashboardStats = {
      totalFranchises: 45, // Exemplo: 45 franquias
      monthlyAudits: 12,   // Exemplo: 12 auditorias no mês atual (Julho de 2025)
      userName: 'Usuário Teste' // Nome do usuário logado (será dinâmico depois)
    };
    return of(mockData).pipe(delay(500)); // Simula um pequeno atraso de rede
  }

  // Simula a obtenção do nome do usuário logado
  getUserName(): Observable<string> {
    return of('Usuário Teste').pipe(delay(100));
  }
}