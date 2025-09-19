
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Dashboard } from '../../services/dashboard';
import { Observable, of } from 'rxjs'; // Adicione 'of' aqui
import { map } from 'rxjs/operators'; // Adicione 'map' para usar com pipe
import { NgIconsModule } from '@ng-icons/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, NgIconsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {
  userName$: Observable<string> | undefined;
  totalFranchises$: Observable<number> | undefined;
  monthlyAudits$: Observable<number> | undefined;

  constructor(private dashboardService: Dashboard) { }

  ngOnInit(): void {
    // Para o userName, você pode assinar diretamente ou usar o async pipe
    this.userName$ = this.dashboardService.getUserName();

    // Para totalFranchises e monthlyAudits, vamos usar o pipe e map para extrair os valores
    this.totalFranchises$ = this.dashboardService.getDashboardStats().pipe(
      map(stats => stats.totalFranchises)
    );
    this.monthlyAudits$ = this.dashboardService.getDashboardStats().pipe(
      map(stats => stats.monthlyAudits)
    );
  }
}