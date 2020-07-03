describe('Mi primer test', function() {
  it('Visita la página principal', function() {
    cy.visit('http://localhost:4200/');
    cy.get('h1').should('be.visible');
    cy.get('h1').invoke('text').should('contain', 'Bienvenid@. Esta es una cadena traducida');
  });
});
