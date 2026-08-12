describe('Dashboard',()=>{it('muestra los contadores y navegación',()=>{cy.visit('/dashboard');cy.contains('Dashboard');cy.contains('Pacientes');cy.contains('Médicos');cy.contains('Citas')})});
