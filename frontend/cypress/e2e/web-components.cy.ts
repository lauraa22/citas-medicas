describe('Web Components integrados', () => {
  it('muestra paciente-resumen en el detalle de paciente', () => {
    cy.visit('/pacientes');

    cy.get('[data-cy="patient-row"]')
      .first()
      .within(() => {
        cy.contains('Ver').click();
      });

    cy.get('paciente-resumen').should('exist');
  });

  it('muestra medico-resumen en el detalle de médico', () => {
    cy.visit('/medicos');

    cy.get('tbody tr')
      .first()
      .within(() => {
        cy.contains('Ver').click();
      });

    cy.get('medico-resumen').should('exist');
  });

  it('muestra cita-resumen en el detalle de cita', () => {
    cy.visit('/citas');

    cy.get('[data-cy="appointment-row"]')
      .first()
      .within(() => {
        cy.contains('Ver').click();
      });

    cy.get('cita-resumen').should('exist');
  });
});
