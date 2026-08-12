import { Usuario } from './usuario.model';

export class Paciente extends Usuario {
  constructor(
    id: number,
    nombre: string,
    apellidos: string,
    usuario: string,
    clave: string,
    public nss: string,
    public numTarjeta: string,
    public telefono: string,
    public direccion: string,
    public medicoIds: number[] = [],
  ) {
    super(id, nombre, apellidos, usuario, clave);
  }
}
