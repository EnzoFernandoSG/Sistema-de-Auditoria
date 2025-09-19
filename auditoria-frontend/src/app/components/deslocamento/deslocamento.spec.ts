import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Deslocamento } from './deslocamento';

describe('Deslocamento', () => {
  let component: Deslocamento;
  let fixture: ComponentFixture<Deslocamento>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Deslocamento]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Deslocamento);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
