import { Routes } from '@angular/router';
import { Home } from './components/home/home';
import { AberturaAuditoria } from './components/abertura-auditoria/abertura-auditoria';
import { HistoricoAuditoria } from './components/historico-auditoria/historico-auditoria';
import { RelatorioMensal } from './components/relatorio-mensal/relatorio-mensal';
import { RelatorioRanking } from './components/relatorio-ranking/relatorio-ranking';
import { Tarefas } from './components/tarefas/tarefas';
import { Deslocamento } from './components/deslocamento/deslocamento';
import { Hospedagem } from './components/hospedagem/hospedagem';
import { Juridico } from './components/juridico/juridico';


export const routes: Routes = [
  { path: 'home', component: Home },
  { path: 'abertura', component: AberturaAuditoria },
  { path: 'historico-auditoria', component: HistoricoAuditoria },
  { path: 'relatorio-mensal', component: RelatorioMensal },
  { path: 'relatorio-ranking', component: RelatorioRanking },
  { path: 'tarefas', component: Tarefas },
  { path: 'deslocamento', component: Deslocamento },
  { path: 'hospedagem', component: Hospedagem },
  { path: 'juridico', component: Juridico },
  { path: '**', redirectTo: '/home' },
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];