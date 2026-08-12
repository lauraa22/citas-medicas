import { Component, Input } from '@angular/core';
@Component({
  selector: 'app-cita-element-content',
  standalone: true,
  template: `<div class="box">
    <span>📅</span>
    <div>
      <b>{{ fecha }}</b
      ><br /><small>{{ texto }}</small>
    </div>
  </div>`,
  styles: [
    `
      .box {
        display: flex;
        gap: 9px;
        align-items: center;
        border-left: 4px solid #5b74d6;
        padding: 8px 12px;
        background: #f7f8ff;
        font: 14px Arial;
      }
      .box small {
        color: #60697a;
      }
    `,
  ],
})
export class CitaResumenElementComponent {
  @Input() fecha = 'Sin fecha';
  @Input() texto = 'Cita médica';
}
