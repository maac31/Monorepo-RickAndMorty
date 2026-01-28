import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Episodio,
  FiltroPersonajes,
  PaginaPersonajes,
  PersonajeDetalle
} from '../../shared/modelos/personajes.model';

@Injectable({
  providedIn: 'root'
})
export class PersonajesService {
  private readonly baseUrl = '/api/personajes';

  constructor(private readonly http: HttpClient) {}

  obtenerPersonajes(filtro: FiltroPersonajes): Observable<PaginaPersonajes> {
    
    let params = new HttpParams().set('pagina', String(filtro.pagina));

    if (filtro.nombre?.trim()) params = params.set('nombre', filtro.nombre.trim());
    if (filtro.estado?.trim()) params = params.set('estado', filtro.estado.trim());
    if (filtro.especie?.trim()) params = params.set('especie', filtro.especie.trim());

    return this.http.get<PaginaPersonajes>(this.baseUrl, { params });
  }

  obtenerDetalle(id: number): Observable<PersonajeDetalle> {
    return this.http.get<PersonajeDetalle>(`${this.baseUrl}/${id}`);
  }

  obtenerEpisodios(id: number): Observable<Episodio[]> {
    return this.http.get<Episodio[]>(`${this.baseUrl}/${id}/episodios`);
  }
}
