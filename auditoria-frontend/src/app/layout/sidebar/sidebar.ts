
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgIconsModule } from '@ng-icons/core'; // Importe NgIconsModule aqui!

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule, FormsModule, NgIconsModule], // NgIconsModule deve estar aqui
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss'
})
export class Sidebar {
  searchTerm: string = '';

  navItems = [
    { path: '/home', label: 'Home', icon: 'lucideHouse', category: 'Principal' }, // Nome Lucide
    { path: '/abertura', label: 'Abertura de Auditoria', icon: 'lucideFileText', category: 'Auditoria' }, // Nome Lucide
    { path: '/historico-auditoria', label: 'Histórico de Auditorias', icon: 'lucideHistory', category: 'Auditoria' }, // Nome Lucide
    { path: '/relatorio-mensal', label: 'Relatório Mensal', icon: 'lucideChartBar', category: 'Relatórios' }, // Nome Lucide
    { path: '/relatorio-ranking', label: 'Ranking', icon: 'lucideAward', category: 'Relatórios' }, // Nome Lucide
    { path: '/tarefas', label: 'Tarefas', icon: 'lucideClipboardCheck', category: 'Outros' }, // Nome Lucide
    { path: '/deslocamento', label: 'Deslocamento', icon: 'lucidePlane', category: 'Outros' }, // Nome Lucide
    { path: '/hospedagem', label: 'Hospedagem', icon: 'lucideHotel', category: 'Outros' }, // Nome Lucide
    { path: '/juridico', label: 'Jurídico', icon: 'lucideGavel', category: 'Outros' } // Nome Lucide
  ];

  categorizedNavItems: { category: string; items: any[] }[] = [];

  constructor() {
    this.updateCategorizedNavItems();
  }

  filterNavItems(): void {
    const lowerCaseSearchTerm = this.searchTerm.toLowerCase();
    const filteredItems = this.navItems.filter(item =>
      item.label.toLowerCase().includes(lowerCaseSearchTerm) ||
      item.category.toLowerCase().includes(lowerCaseSearchTerm)
    );
    this.groupAndSetItems(filteredItems);
  }

  private updateCategorizedNavItems(): void {
    this.groupAndSetItems(this.navItems);
  }

  private groupAndSetItems(items: any[]): void {
    const grouped: { [key: string]: any[] } = {};
    items.forEach(item => {
      if (!grouped[item.category]) {
        grouped[item.category] = [];
      }
      grouped[item.category].push(item);
    });

    this.categorizedNavItems = Object.keys(grouped).map(category => ({
      category,
      items: grouped[category]
    })).sort((a, b) => {
      const order = ['Principal', 'Auditoria', 'Relatórios', 'Outros'];
      return order.indexOf(a.category) - order.indexOf(b.category);
    });
  }
}
