import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Subscription, finalize, timeout } from 'rxjs';
import { PersonajesService } from '../../core/servicios/personajes.service';
import { FiltroPersonajes, PaginaPersonajes } from '../../shared/modelos/personajes.model';

@Component({
  selector: 'app-personajes-lista',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './personajes-lista.component.html',
  styleUrl: './personajes-lista.component.css'
})
export class PersonajesListaComponent implements OnInit {
  nombre = '';
  estado = ''; // alive/dead/unknown/''
  especie = '';

  estadosDisponibles: Array<{ valor: string; texto: string }> = [
    { valor: '', texto: 'Todos' },
    { valor: 'alive', texto: 'Alive' },
    { valor: 'dead', texto: 'Dead' },
    { valor: 'unknown', texto: 'Unknown' }
  ];

  cargando = false;
  errorMensaje: string | null = null;
  sinResultados = false;

  pagina: PaginaPersonajes | null = null;

  private suscripcionBusqueda: Subscription | null = null;

  constructor(private readonly personajesService: PersonajesService) {}

  ngOnInit(): void {
    this.cargar(1);
  }

  buscar(): void {
    this.cargar(1);
  }

  limpiar(): void {
    this.nombre = '';
    this.estado = '';
    this.especie = '';
    this.cargar(1);
  }

  anterior(): void {
    if (!this.pagina || this.cargando) return;
    if (this.pagina.paginaActual <= 1) return;
    this.cargar(this.pagina.paginaActual - 1);
  }

  siguiente(): void {
    if (!this.pagina || this.cargando) return;
    if (this.pagina.paginaActual >= this.pagina.totalPaginas) return;
    this.cargar(this.pagina.paginaActual + 1);
  }

  get puedeIrAnterior(): boolean {
    return !!this.pagina && !this.cargando && this.pagina.paginaActual > 1;
  }

  get puedeIrSiguiente(): boolean {
    return !!this.pagina && !this.cargando && this.pagina.paginaActual < this.pagina.totalPaginas;
  }

  private cargar(paginaNumero: number): void {
    // Cancela la búsqueda anterior si seguía en curso
    this.suscripcionBusqueda?.unsubscribe();
    this.suscripcionBusqueda = null;

    const estadoNormalizado =
      this.estado?.toLowerCase() === 'todos' ? '' : this.estado;

    const filtro: FiltroPersonajes = {
      nombre: this.nombre,
      estado: estadoNormalizado,
      especie: this.especie,
      pagina: paginaNumero
    };

    this.cargando = true;
    this.errorMensaje = null;
    this.sinResultados = false;

    this.suscripcionBusqueda = this.personajesService
      .obtenerPersonajes(filtro)
      .pipe(
        // Si el backend/proxy se cuelga, corta y muestra error
        timeout(15000),
        // Pase lo que pase, apaga el cargando
        finalize(() => {
          this.cargando = false;
        })
      )
      .subscribe({
        next: (data) => {
          this.pagina = data;
          this.sinResultados = (data.resultados?.length ?? 0) === 0;
        },
        error: (err) => {
          // Timeout de RxJS
          if (err?.name === 'TimeoutError') {
            this.errorMensaje = 'La consulta tardó demasiado. Revisa el backend/proxy.';
          } else if (err?.status === 0) {
            this.errorMensaje = 'No se pudo conectar con el backend. Verifica que esté encendido.';
          } else if (err?.error?.mensaje) {
            this.errorMensaje = err.error.mensaje;
          } else {
            this.errorMensaje = 'Ocurrió un error consultando el backend.';
          }

          this.pagina = null;
          this.sinResultados = false;
        }
      });
  }
}
