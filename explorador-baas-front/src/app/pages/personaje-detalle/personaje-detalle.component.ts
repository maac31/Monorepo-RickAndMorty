import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { PersonajesService } from '../../core/servicios/personajes.service';
import { Episodio, PersonajeDetalle } from '../../shared/modelos/personajes.model';

@Component({
  selector: 'app-personaje-detalle',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './personaje-detalle.component.html',
  styleUrl: './personaje-detalle.component.css'
})
export class PersonajeDetalleComponent implements OnInit {
  cargando = false;
  errorMensaje: string | null = null;

  personaje: PersonajeDetalle | null = null;
  episodios: Episodio[] = [];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly personajesService: PersonajesService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.errorMensaje = 'Id inválido.';
      return;
    }

    this.cargar(id);
  }

  private cargar(id: number): void {
    this.cargando = true;
    this.errorMensaje = null;

    forkJoin({
      detalle: this.personajesService.obtenerDetalle(id),
      episodios: this.personajesService.obtenerEpisodios(id)
    }).subscribe({
      next: (data) => {
        this.personaje = data.detalle;
        this.episodios = data.episodios ?? [];
        this.cargando = false;
      },
      error: (err) => {
        this.errorMensaje = 'Ocurrió un error consultando el backend.';
        if (err?.error?.mensaje) this.errorMensaje = err.error.mensaje;

        this.personaje = null;
        this.episodios = [];
        this.cargando = false;
      }
    });
  }
}
