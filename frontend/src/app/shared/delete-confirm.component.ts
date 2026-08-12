import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-delete-confirm',
  standalone: true,
  template: `
    @if (open()) {
      <div class="modal-backdrop" data-cy="delete-modal">
        <section class="modal">
          <h3>Confirmar eliminación</h3>
          <p>
            ¿Seguro que quieres eliminar <strong>{{ label() }}</strong
            >?
          </p>
          <div class="actions">
            <button class="secondary" (click)="cancel.emit()">Cancelar</button
            ><button class="danger" data-cy="confirm-delete" (click)="confirm.emit()">
              Eliminar
            </button>
          </div>
        </section>
      </div>
    }
  `,
  styles: [
    `
      .modal-backdrop {
        position: fixed;
        inset: 0;
        background: #0008;
        display: grid;
        place-items: center;
        z-index: 50;
      }
      .modal {
        background: #fff;
        border-radius: 14px;
        padding: 24px;
        max-width: 420px;
        width: 90%;
        box-shadow: 0 20px 70px #0003;
      }
      .actions {
        display: flex;
        justify-content: flex-end;
        gap: 10px;
      }
      .danger {
        background: #c62828;
        color: white;
      }
      .secondary {
        background: #eef2f7;
      }
    `,
  ],
})
export class DeleteConfirmComponent {
  open = input(false);
  label = input('este elemento');
  confirm = output<void>();
  cancel = output<void>();
}
