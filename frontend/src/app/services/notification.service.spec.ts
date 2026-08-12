import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';
describe('NotificationService', () => {
  it('muestra y limpia notificaciones', () => {
    const s = TestBed.inject(NotificationService);
    s.success('OK');
    expect(s.notification()?.text).toBe('OK');
    expect(s.notification()?.type).toBe('success');
    s.clear();
    expect(s.notification()).toBeNull();
  });
});
