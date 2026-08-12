describe('Citas', () => {
  it('filtra por médico y crea una cita', () => {
    const unique = Date.now();

    const motivo =
      `Revision Cypress ${unique}`;

    cy.visit('/citas');

    // =========================
    // COMPROBAR FILTRO
    // =========================

    cy.get(
      '[data-cy="appointment-doctor-filter"]',
    ).should('exist');

    cy.get(
      '[data-cy="appointment-doctor-filter"] option',
    ).should(
      'have.length.greaterThan',
      1,
    );

    cy.get(
      '[data-cy="appointment-doctor-filter"] option',
    )
      .eq(1)
      .invoke('val')
      .then((doctorId) => {
        cy.get(
          '[data-cy="appointment-doctor-filter"]',
        ).select(String(doctorId));
      });

    // Volvemos a mostrar todas las citas.
    cy.get(
      '[data-cy="appointment-doctor-filter"]',
    ).select('');

    // =========================
    // ABRIR FORMULARIO
    // =========================

    cy.get(
      '[data-cy="new-appointment"]',
    ).click();

    // Ahora sí existen estos selects.
    cy.get(
      '[data-cy="appointment-patient"]',
    ).should('exist');

    cy.get(
      '[data-cy="appointment-doctor"]',
    ).should('exist');

    // =========================
    // CREAR CITA
    // =========================

    cy.get(
      '[data-cy="appointment-date"]',
    ).type('2026-12-15T10:30');

    cy.get(
      '[data-cy="appointment-reason"]',
    ).type(motivo);

    // Seleccionar primer paciente disponible.
    cy.get(
      '[data-cy="appointment-patient"] option',
    ).should(
      'have.length.greaterThan',
      1,
    );

    cy.get(
      '[data-cy="appointment-patient"] option',
    )
      .eq(1)
      .invoke('val')
      .then((patientId) => {
        cy.get(
          '[data-cy="appointment-patient"]',
        ).select(String(patientId));
      });

    // Seleccionar primer médico disponible.
    cy.get(
      '[data-cy="appointment-doctor"] option',
    ).should(
      'have.length.greaterThan',
      1,
    );

    cy.get(
      '[data-cy="appointment-doctor"] option',
    )
      .eq(1)
      .invoke('val')
      .then((doctorId) => {
        cy.get(
          '[data-cy="appointment-doctor"]',
        ).select(String(doctorId));
      });

    // No seleccionamos diagnóstico:
    // debe poder crearse con diagnosticoId = null.

    cy.get(
      '[data-cy="save-appointment"]',
    )
      .should('not.be.disabled')
      .click();

    // =========================
    // COMPROBAR CITA CREADA
    // =========================

    cy.contains(
      '[data-cy="appointment-row"]',
      motivo,
    ).should('exist');
  });
});