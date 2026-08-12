import { Component, inject } from '@angular/core';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  template: `@if (service.notification(); as n) {
    <div class="toast" [class.error]="n.type === 'error'" data-cy="toast">
      {{ n.text }} <button (click)="service.clear()">×</button>
    </div>
  }`,
  styles: [
    `
      .toast {
        position: fixed;
        right: 24px;
        bottom: 24px;
        background: #197a4a;
        color: #fff;
        padding: 14px 16px;
        border-radius: 10px;
        box-shadow: 0 10px 30px #0003;
        z-index: 60;
      }
      .toast.error {
        background: #b3261e;
      }
      .toast button {
        background: none;
        color: white;
        padding: 0 0 0 12px;
      }
    `,
  ],
})
export class ToastComponent {
  service = inject(NotificationService);
}
