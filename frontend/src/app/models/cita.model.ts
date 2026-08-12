export class Cita {
  constructor(
    public id: number,
    public fechaHora: string,
    public motivoCita: string,
    public pacienteId: number,
    public medicoId: number,
    public diagnosticoId: number | null = null,
  ) {}
}
