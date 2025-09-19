import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatorioRanking } from './relatorio-ranking';

describe('RelatorioRanking', () => {
  let component: RelatorioRanking;
  let fixture: ComponentFixture<RelatorioRanking>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RelatorioRanking]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RelatorioRanking);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
