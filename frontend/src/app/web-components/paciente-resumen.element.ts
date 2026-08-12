import { Component, Input } from '@angular/core';
@Component({
  selector: 'app-paciente-element-content',
  standalone: true,
  template: `<span class="chip"
    ><b>{{ nombre }}</b> · NSS {{ nss }}</span
  >`,
  styles: [
    `
      .chip {
        display: inline-block;
        padding: 7px 10px;
        border-radius: 999px;
        background: #e8f1ff;
        color: #164b84;
        font: 14px Arial;
      }
    `,
  ],
})
export class PacienteResumenElementComponent {
  @Input() nombre = 'Paciente';
  @Input() nss = '-';
}
