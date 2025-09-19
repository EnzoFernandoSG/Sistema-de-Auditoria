import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Hospedagem } from './hospedagem';

describe('Hospedagem', () => {
  let component: Hospedagem;
  let fixture: ComponentFixture<Hospedagem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Hospedagem]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Hospedagem);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
