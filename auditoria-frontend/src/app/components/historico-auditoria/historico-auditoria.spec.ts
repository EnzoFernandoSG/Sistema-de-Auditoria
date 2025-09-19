import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricoAuditoria } from './historico-auditoria';

describe('HistoricoAuditoria', () => {
  let component: HistoricoAuditoria;
  let fixture: ComponentFixture<HistoricoAuditoria>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistoricoAuditoria]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistoricoAuditoria);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
