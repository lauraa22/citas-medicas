import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ToastComponent } from '../shared/toast.component';
@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet, ToastComponent],
  template: `
    <header>
      <div><b> Citas Médicas </b></div>
    </header>
    <div class="layout">
      <aside>
        <a routerLink="/dashboard" routerLinkActive="active">Dashboard</a
        ><a routerLink="/usuarios" routerLinkActive="active">Usuarios</a
        ><a routerLink="/pacientes" routerLinkActive="active">Pacientes</a
        ><a routerLink="/medicos" routerLinkActive="active">Médicos</a
        ><a routerLink="/citas" routerLinkActive="active">Citas</a
        ><a routerLink="/diagnosticos" routerLinkActive="active">Diagnósticos</a>
      </aside>
      <main><router-outlet /></main>
    </div>
    <app-toast />
  `,
  styles: [
    `
      header {
        height: 64px;
        background: #17365d;
        color: #fff;
        display: flex;
        align-items: center;
        padding: 0 24px;
      }
      header div {
        display: flex;
        align-items: center;
        gap: 12px;
        font-size: 25px;
      }
      header span {
        font-size: 12px;
        opacity: 0.7;
      }
      .layout {
        display: flex;
        min-height: calc(100vh - 64px);
      }
      aside {
        width: 210px;
        background: #f5f7fa;
        padding: 18px 12px;
        border-right: 1px solid #e1e7ef;
      }
      aside a {
        display: block;
        padding: 10px 12px;
        margin: 4px 0;
        border-radius: 8px;
        text-decoration: none;
        color: #344054;
        font-weight: 600;
      }
      .active,
      aside a:hover {
        background: #e4edfa;
        color: #17365d;
      }
      main {
        flex: 1;
        padding: 28px;
        overflow: auto;
      }
      @media (max-width: 800px) {
        .layout {
          display: block;
        }
        aside {
          width: auto;
          display: flex;
          overflow: auto;
        }
        aside a {
          white-space: nowrap;
        }
        main {
          padding: 18px;
        }
      }
    `,
  ],
})
export class MainLayoutComponent {}
