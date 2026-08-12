describe('Pacientes', () => {
  it('crea, busca y elimina un paciente', () => {
    const unique = Date.now();

    const nombre = `Lucia${unique}`;
    const usuario = `lucia.${unique}`;
    const nss = `NSS${unique}`;
    const tarjeta = `TS${unique}`;

    cy.visit('/pacientes');

    // =========================
    // CREAR PACIENTE
    // =========================

    cy.get('[data-cy="new-patient"]').click();

    cy.get('[data-cy="patient-name"]')
      .type(nombre);

    cy.get('input[formcontrolname="apellidos"]')
      .type('Prueba');

    cy.get('input[formcontrolname="usuario"]')
      .type(usuario);

    cy.get('input[formcontrolname="clave"]')
      .type('1234');

    cy.get('[data-cy="patient-nss"]')
      .type(nss);

    cy.get('input[formcontrolname="numTarjeta"]')
      .type(tarjeta);

    cy.get('input[formcontrolname="telefono"]')
      .type('600999999');

    cy.get('input[formcontrolname="direccion"]')
      .type('Calle Test');

    cy.get('[data-cy="save-patient"]')
      .should('not.be.disabled')
      .click();

    // El paciente debe aparecer en la tabla.
    cy.contains(
      '[data-cy="patient-row"]',
      `${nombre} Prueba`,
    ).should('exist');

    // =========================
    // BUSCADOR SIGNAL
    // =========================

    cy.get('[data-cy="patient-search"]')
      .clear()
      .type(nombre);

    cy.get('[data-cy="patient-row"]')
      .should('have.length', 1)
      .and('contain.text', nombre);

    cy.get('[data-cy="patient-search"]')
      .clear();

    // =========================
    // ELIMINAR
    // =========================

    cy.contains(
      '[data-cy="patient-row"]',
      nombre,
    )
      .within(() => {
        cy.contains('Eliminar').click();
      });

    cy.get('[data-cy="confirm-delete"]')
      .click();

    cy.contains(
      '[data-cy="patient-row"]',
      nombre,
    ).should('not.exist');
  });
});