import { Injectable, signal } from '@angular/core';

export interface AppNotification {
  text: string;
  type: 'success' | 'error';
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  readonly notification = signal<AppNotification | null>(null);
  success(text: string): void {
    this.notification.set({ text, type: 'success' });
  }
  error(text: string): void {
    this.notification.set({ text, type: 'error' });
  }
  clear(): void {
    this.notification.set(null);
  }
}
