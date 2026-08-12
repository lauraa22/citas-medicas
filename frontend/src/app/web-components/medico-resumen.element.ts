import { Component, Input } from '@angular/core';
@Component({
  selector: 'app-medico-element-content',
  standalone: true,
  template: `<article>
    <strong>Dr/a. {{ nombre }}</strong
    ><small>Colegiado: {{ colegiado }}</small>
  </article>`,
  styles: [
    `
      article {
        border: 1px solid #d8e3ef;
        border-radius: 10px;
        padding: 10px 12px;
        background: white;
        font: 14px Arial;
      }
      small {
        display: block;
        color: #637083;
        margin-top: 4px;
      }
    `,
  ],
})
export class MedicoResumenElementComponent {
  @Input() nombre = 'Médico';
  @Input() colegiado = '-';
}
