import { CommonModule } from '@angular/common';
import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  computed,
  inject,
  signal
} from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PacienteService } from '../services/paciente.service';
import { MedicoService } from '../services/medico.service';
import { NotificationService } from '../services/notification.service';
import { Paciente } from '../models/paciente.model';
import { DeleteConfirmComponent } from '../shared/delete-confirm.component';
@Component({
  selector: 'app-pacientes',
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [CommonModule, ReactiveFormsModule, DeleteConfirmComponent],
  template: `
    <div class="page-head">
      <div>
        <h1>Pacientes</h1>
      </div>
      <button data-cy="new-patient" (click)="newItem()">+ Nuevo</button>
    </div>
    <label class="search"
      >Buscar paciente
      <input
        data-cy="patient-search"
        [value]="search()"
        (input)="search.set($any($event.target).value)"
        placeholder="Nombre, apellidos o NSS"
    /></label>
    @if (formVisible()) {
      <section class="panel">
        <h2>{{ editingId() ? 'Editar' : 'Nuevo' }} paciente</h2>
        <form [formGroup]="form" (ngSubmit)="save()">
          <div class="grid">
            <label>Nombre<input data-cy="patient-name" formControlName="nombre" /></label
            ><label>Apellidos<input formControlName="apellidos" /></label
            ><label>Usuario<input formControlName="usuario" /></label
            ><label>Clave<input type="password" formControlName="clave" /></label
            ><label>NSS<input data-cy="patient-nss" formControlName="nss" /></label
            ><label>Tarjeta<input formControlName="numTarjeta" /></label
            ><label>Teléfono<input formControlName="telefono" /></label
            ><label>Dirección<input formControlName="direccion" /></label>
          </div>
          <fieldset>
            <legend>Médicos relacionados </legend>
            @for (m of medicos.medicos(); track m.id) {
              <label class="check"
                ><input
                  type="checkbox"
                  [checked]="selectedDoctors().includes(m.id)"
                  (change)="toggleDoctor(m.id, $any($event.target).checked)"
                />{{ m.nombre }} {{ m.apellidos }}</label
              >
            }
          </fieldset>
          <div class="actions">
            <button type="button" class="secondary" (click)="closeForm()">Cancelar</button
            ><button data-cy="save-patient" [disabled]="form.invalid">Guardar</button>
          </div>
        </form>
      </section>
    }
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Paciente</th>
          <th>NSS</th>
          <th>Médicos</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        @for (x of filtered(); track x.id) {
          <tr data-cy="patient-row">
            <td>{{ x.id }}</td>
            <td>{{ x.nombre }} {{ x.apellidos }}</td>
            <td>{{ x.nss }}</td>
            <td>{{ doctorNames(x) }}</td>
            <td>
              <button class="link" (click)="detail.set(x)">Ver</button
              ><button class="link" (click)="edit(x)">Editar</button
              ><button class="link danger-text" (click)="pendingDelete.set(x)">Eliminar</button>
            </td>
          </tr>
        }
      </tbody>
    </table>
    @if (detail(); as x) {
      <section class="panel detail">
        <h2>Detalle paciente #{{ x.id }}</h2>

        <paciente-resumen
          [attr.nombre]="x.nombre + ' ' + x.apellidos"
          [attr.nss]="x.nss"
        >
        </paciente-resumen>

        <p><b>Nombre:</b> {{ x.nombre }} {{ x.apellidos }}</p>
        <p><b>NSS:</b> {{ x.nss }}</p>
        <p><b>Tarjeta:</b> {{ x.numTarjeta }}</p>
        <p><b>Teléfono:</b> {{ x.telefono }}</p>
        <p><b>Dirección:</b> {{ x.direccion }}</p>
        <p><b>Médicos:</b> {{ doctorNames(x) }}</p>

        <button class="secondary" (click)="detail.set(null)">
          Cerrar
        </button>
      </section>
    }
    <app-delete-confirm
      [open]="!!pendingDelete()"
      [label]="pendingDelete()?.nombre || ''"
      (cancel)="pendingDelete.set(null)"
      (confirm)="confirmDelete()"
    />
  `,
})
export class PacientesComponent {
  service = inject(PacienteService);
  medicos = inject(MedicoService);
  notify = inject(NotificationService);
  fb = inject(FormBuilder);
  search = signal('');
  filtered = computed(() => this.service.search(this.search()));
  formVisible = signal(false);
  editingId = signal<number | null>(null);
  detail = signal<Paciente | null>(null);
  pendingDelete = signal<Paciente | null>(null);
  selectedDoctors = signal<number[]>([]);
  form = this.fb.nonNullable.group({
    nombre: ['', Validators.required],
    apellidos: ['', Validators.required],
    usuario: ['', Validators.required],
    clave: ['', Validators.required],
    nss: ['', Validators.required],
    numTarjeta: ['', Validators.required],
    telefono: ['', [Validators.required, Validators.pattern(/^[679]\d{8}$/)]],
    direccion: ['', Validators.required],
  });
  newItem() {
    this.editingId.set(null);
    this.selectedDoctors.set([]);
    this.form.reset();
    this.formVisible.set(true);
  }
  edit(x: Paciente) {
    this.editingId.set(x.id);
    this.selectedDoctors.set([...x.medicoIds]);
    this.form.setValue({
      nombre: x.nombre,
      apellidos: x.apellidos,
      usuario: x.usuario,
      clave: x.clave,
      nss: x.nss,
      numTarjeta: x.numTarjeta,
      telefono: x.telefono,
      direccion: x.direccion,
    });
    this.formVisible.set(true);
  }
  toggleDoctor(id: number, on: boolean) {
    this.selectedDoctors.update((v) => (on ? [...v, id] : v.filter((x) => x !== id)));
  }
  closeForm() {
    this.formVisible.set(false);
  }
  save() {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const id = this.editingId();
    const x = new Paciente(
      id ?? 0,
      v.nombre,
      v.apellidos,
      v.usuario,
      v.clave,
      v.nss,
      v.numTarjeta,
      v.telefono,
      v.direccion,
      this.selectedDoctors(),
    );
    if (id) this.service.update(x);
    else this.service.create(x as any);
    this.notify.success('Paciente guardado correctamente');
    this.closeForm();
  }
  doctorNames(p: Paciente) {
    return (
      p.medicoIds
        .map((id) => this.medicos.getById(id))
        .filter(Boolean)
        .map((x) => `${x!.nombre} ${x!.apellidos}`)
        .join(', ') || 'Sin asignar'
    );
  }
  confirmDelete() {
    const x = this.pendingDelete();
    if (x) {
      this.service.delete(x.id);
      this.notify.success('Paciente eliminado');
      this.pendingDelete.set(null);
    }
  }
}
