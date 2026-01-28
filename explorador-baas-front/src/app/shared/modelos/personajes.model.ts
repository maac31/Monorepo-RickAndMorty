export interface FiltroPersonajes {
  nombre?: string;
  estado?: string;
  especie?: string;
  pagina: number;
}

export interface PersonajeResumen {
  id: number;
  nombre: string;
  estado: string;
  especie: string;
  ubicacion: string;
  imagenUrl: string;
}

export interface PaginaPersonajes {
  totalRegistros: number;
  totalPaginas: number;
  paginaActual: number;
  resultados: PersonajeResumen[];
}

export interface PersonajeDetalle {
  id: number;
  nombre: string;
  estado: string;
  especie: string;
  tipo: string;
  genero: string;
  origen: string;
  ubicacion: string;
  imagenUrl: string;
  episodios: string[]; // urls
}

export interface Episodio {
  id: number;
  nombre: string;
  codigo: string;
  fechaEmision: string;
}
