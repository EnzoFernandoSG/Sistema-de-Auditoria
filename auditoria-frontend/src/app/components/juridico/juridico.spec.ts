import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Juridico } from './juridico';

describe('Juridico', () => {
  let component: Juridico;
  let fixture: ComponentFixture<Juridico>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Juridico]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Juridico);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
