import { Usuario } from './usuario.model';

export class Medico extends Usuario {
  constructor(
    id: number,
    nombre: string,
    apellidos: string,
    usuario: string,
    clave: string,
    public numColegiado: string,
    public pacienteIds: number[] = [],
  ) {
    super(id, nombre, apellidos, usuario, clave);
  }
}
