import { Usuario } from '../models/usuario.model';
import { Paciente } from '../models/paciente.model';
import { Medico } from '../models/medico.model';
import { Diagnostico } from '../models/diagnostico.model';
import { Cita } from '../models/cita.model';

export const USUARIOS_MOCK: Usuario[] = [
  new Usuario(1, 'Laura', 'Guirao', 'laura', '1234'),
  new Usuario(2, 'Carlos', 'Ruiz', 'carlos', '1234'),
];

export const PACIENTES_MOCK: Paciente[] = [
  new Paciente(
    1,
    'Ana',
    'Martínez',
    'ana.p',
    '1234',
    'NSS001',
    'TS001',
    '600111111',
    'Calle Mayor 1',
    [1],
  ),
  new Paciente(
    2,
    'Pedro',
    'López',
    'pedro.p',
    '1234',
    'NSS002',
    'TS002',
    '600222222',
    'Avenida Sol 12',
    [1, 2],
  ),
  new Paciente(
    3,
    'Marta',
    'Sánchez',
    'marta.p',
    '1234',
    'NSS003',
    'TS003',
    '600333333',
    'Plaza Nueva 3',
    [2],
  ),
];

export const MEDICOS_MOCK: Medico[] = [
  new Medico(1, 'Elena', 'García', 'elena.m', '1234', 'COL001', [1, 2]),
  new Medico(2, 'Miguel', 'Torres', 'miguel.m', '1234', 'COL002', [2, 3]),
];

export const DIAGNOSTICOS_MOCK: Diagnostico[] = [
  new Diagnostico(1, 'Reposo y control en una semana.', 'Migraña'),
  new Diagnostico(2, 'Tratamiento sintomático y seguimiento.', 'Gripe'),
];

export const CITAS_MOCK: Cita[] = [
  new Cita(1, '2026-08-12T10:00', 'Dolor de cabeza', 1, 1, 1),
  new Cita(2, '2026-08-13T12:30', 'Revisión general', 2, 1, null),
  new Cita(3, '2026-08-14T09:15', 'Fiebre', 3, 2, 2),
];
