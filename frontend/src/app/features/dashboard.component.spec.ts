import { TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard.component';
describe('DashboardComponent', () => {
  beforeEach(() => localStorage.clear());
  it('muestra las tarjetas del dashboard', async () => {
    await TestBed.configureTestingModule({ imports: [DashboardComponent] }).compileComponents();
    const f = TestBed.createComponent(DashboardComponent);
    f.detectChanges();
    expect(f.nativeElement.textContent).toContain('Pacientes');
    expect(f.nativeElement.textContent).toContain('Médicos');
    expect(f.nativeElement.textContent).toContain('Citas');
  });
});
