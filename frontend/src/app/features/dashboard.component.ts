import { Component, inject } from '@angular/core';

import { UsuarioService } from '../services/usuario.service';
import { PacienteService } from '../services/paciente.service';
import { MedicoService } from '../services/medico.service';
import { CitaService } from '../services/cita.service';
import { DiagnosticoService } from '../services/diagnostico.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,

  template: `
    <div class="page-head">
      <div>
        <h1>Dashboard</h1>
        <p>Resumen sencillo de la aplicación.</p>
      </div>
    </div>

    <div class="cards">
      <article>
        <b>{{ u.total() }}</b>
        <span>Usuarios</span>
      </article>

      <article>
        <b>{{ p.total() }}</b>
        <span>Pacientes</span>
      </article>

      <article>
        <b>{{ m.total() }}</b>
        <span>Médicos</span>
      </article>

      <article>
        <b>{{ c.total() }}</b>
        <span>Citas</span>
      </article>

      <article>
        <b>{{ d.total() }}</b>
        <span>Diagnósticos</span>
      </article>
    </div>
  `,

  styles: [
    `
      .cards {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
        gap: 14px;
      }

      .cards article {
        background: white;
        border: 1px solid #e2e8f0;
        border-radius: 12px;
        padding: 20px;
      }

      .cards b {
        font-size: 30px;
        color: #17365d;
        display: block;
      }

      .cards span {
        color: #667085;
      }
    `,
  ],
})
export class DashboardComponent {
  u = inject(UsuarioService);
  p = inject(PacienteService);
  m = inject(MedicoService);
  c = inject(CitaService);
  d = inject(DiagnosticoService);
}