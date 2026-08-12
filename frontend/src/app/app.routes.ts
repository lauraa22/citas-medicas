import { Routes } from '@angular/router';
import { MainLayoutComponent } from './core/main-layout.component';
export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'usuarios',
        loadComponent: () =>
          import('./features/usuarios.component').then((m) => m.UsuariosComponent),
      },
      {
        path: 'pacientes',
        loadComponent: () =>
          import('./features/pacientes.component').then((m) => m.PacientesComponent),
      },
      {
        path: 'medicos',
        loadComponent: () => import('./features/medicos.component').then((m) => m.MedicosComponent),
      },
      {
        path: 'citas',
        loadComponent: () => import('./features/citas.component').then((m) => m.CitasComponent),
      },
      {
        path: 'diagnosticos',
        loadComponent: () =>
          import('./features/diagnosticos.component').then((m) => m.DiagnosticosComponent),
      },
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];
