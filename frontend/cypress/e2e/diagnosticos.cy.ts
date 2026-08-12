describe('Diagnósticos', () => {
  it('crea y elimina un diagnóstico', () => {
    const unique = Date.now();

    const enfermedad =
      `Alergia Cypress ${unique}`;

    cy.visit('/diagnosticos');

    // =========================
    // CREAR DIAGNÓSTICO
    // =========================

    cy.get('[data-cy="new-diagnosis"]')
      .click();

    cy.get('[data-cy="diagnosis-disease"]')
      .type(enfermedad);

    cy.get(
      '[data-cy="diagnosis-assessment"]',
    ).type(
      'Evitar alérgeno y realizar seguimiento.',
    );

    cy.get('[data-cy="save-diagnosis"]')
      .should('not.be.disabled')
      .click();

    cy.contains(
      '[data-cy="diagnosis-row"]',
      enfermedad,
    ).should('exist');

    // =========================
    // ELIMINAR
    // =========================

    cy.contains(
      '[data-cy="diagnosis-row"]',
      enfermedad,
    )
      .within(() => {
        cy.contains('Eliminar').click();
      });

    cy.get('[data-cy="confirm-delete"]')
      .click();

    cy.contains(
      '[data-cy="diagnosis-row"]',
      enfermedad,
    ).should('not.exist');
  });
});