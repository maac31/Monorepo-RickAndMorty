import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonajeDetalle } from './personaje-detalle';

describe('PersonajeDetalle', () => {
  let component: PersonajeDetalle;
  let fixture: ComponentFixture<PersonajeDetalle>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PersonajeDetalle]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PersonajeDetalle);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
