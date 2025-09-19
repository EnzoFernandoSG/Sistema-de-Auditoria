import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AberturaAuditoria } from './abertura-auditoria';

describe('AberturaAuditoria', () => {
  let component: AberturaAuditoria;
  let fixture: ComponentFixture<AberturaAuditoria>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AberturaAuditoria]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AberturaAuditoria);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
