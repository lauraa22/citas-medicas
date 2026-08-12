describe('Usuarios', () => {
  it('crea, edita y elimina un usuario', () => {
    const unique =
      Date.now();

    const username =
      `eva.${unique}`;

    cy.visit('/usuarios');

    cy.get(
      '[data-cy="new-user"]',
    ).click();

    cy.get(
      '[data-cy="user-name"]',
    ).type('Eva');

    cy.get(
      '[data-cy="user-lastname"]',
    ).type('Test');

    cy.get(
      '[data-cy="user-username"]',
    ).type(username);

    cy.get(
      '[data-cy="user-password"]',
    ).type('1234');

    cy.get(
      '[data-cy="save-user"]',
    )
      .should('not.be.disabled')
      .click();

    cy.contains(
      '[data-cy="user-row"]',
      'Eva Test',
    ).should('exist');

    // EDITAR
    cy.contains(
      '[data-cy="user-row"]',
      'Eva Test',
    )
      .within(() => {
        cy.contains(
          'Editar',
        ).click();
      });

    cy.get(
      '[data-cy="user-name"]',
    )
      .clear()
      .type('Evelyn');

    /*
     * La API no devuelve la clave,
     * por lo que al editar debemos
     * introducirla nuevamente.
     */
    cy.get(
      '[data-cy="user-password"]',
    ).type('1234');

    cy.get(
      '[data-cy="save-user"]',
    ).click();

    cy.contains(
      '[data-cy="user-row"]',
      'Evelyn Test',
    ).should('exist');

    // ELIMINAR
    cy.contains(
      '[data-cy="user-row"]',
      'Evelyn Test',
    )
      .within(() => {
        cy.contains(
          'Eliminar',
        ).click();
      });

    cy.get(
      '[data-cy="confirm-delete"]',
    ).click();

    cy.contains(
      '[data-cy="user-row"]',
      'Evelyn Test',
    ).should('not.exist');
  });
});