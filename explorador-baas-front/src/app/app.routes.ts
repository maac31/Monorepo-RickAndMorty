import { Routes } from '@angular/router';
import { PersonajesListaComponent } from './pages/personajes-lista/personajes-lista.component';
import { PersonajeDetalleComponent } from './pages/personaje-detalle/personaje-detalle.component';


export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'characters' },

  
  { path: 'characters', component: PersonajesListaComponent },
  { path: 'characters/:id', component: PersonajeDetalleComponent },

  { path: '**', redirectTo: 'characters' }
];
