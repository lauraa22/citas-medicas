describe('Citas', () => {
  beforeEach(() => {
    cy.visit('/citas');
  });

  it('filtra por médico y crea una cita', () => {
    // =========================
    // FILTRO POR MÉDICO
    // =========================

    cy.get('[data-cy="appointment-filter"]').should('exist');

    // Seleccionamos el primer médico disponible
    cy.get('[data-cy="appointment-filter"] option')
      .eq(1)
      .invoke('val')
      .then((doctorId) => {
        cy.get('[data-cy="appointment-filter"]').select(String(doctorId));

        // IMPORTANTE:
        // volvemos a buscar los elementos después
        // de que Angular haya actualizado el DOM.
        cy.get('[data-cy="appointment-row"]').should('have.length.at.least', 1);
      });

    // Volvemos a mostrar todas
    cy.get('[data-cy="appointment-filter"]').select('');

    // =========================
    // CREAR CITA
    // =========================

    cy.get('[data-cy="new-appointment"]').click();

    cy.get('[data-cy="appointment-date"]').type('2026-12-15T10:30');

    cy.get('[data-cy="appointment-patient"]').select(1);

    cy.get('[data-cy="appointment-doctor"]').select(1);

    cy.get('[data-cy="appointment-reason"]').type('Revisión Cypress');

    cy.get('[data-cy="save-appointment"]').should('not.be.disabled');

    cy.get('[data-cy="save-appointment"]').click();

    // Después de guardar Angular vuelve a renderizar,
    // así que hacemos una consulta nueva.
    cy.get('[data-cy="appointment-row"]').should('contain.text', 'Revisión Cypress');
  });
});
