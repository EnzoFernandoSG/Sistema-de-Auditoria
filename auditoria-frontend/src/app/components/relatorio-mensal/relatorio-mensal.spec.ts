import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RelatorioMensal } from './relatorio-mensal';

describe('RelatorioMensal', () => {
  let component: RelatorioMensal;
  let fixture: ComponentFixture<RelatorioMensal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RelatorioMensal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RelatorioMensal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
