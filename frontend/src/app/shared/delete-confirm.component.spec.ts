import { TestBed } from '@angular/core/testing';
import { DeleteConfirmComponent } from './delete-confirm.component';
describe('DeleteConfirmComponent', () => {
  it('emite confirmación', async () => {
    await TestBed.configureTestingModule({ imports: [DeleteConfirmComponent] }).compileComponents();
    const f = TestBed.createComponent(DeleteConfirmComponent);
    f.componentRef.setInput('open', true);
    let ok = false;
    f.componentInstance.confirm.subscribe(() => (ok = true));
    f.detectChanges();
    (f.nativeElement.querySelector('[data-cy="confirm-delete"]') as HTMLButtonElement).click();
    expect(ok).toBe(true);
  });
});
