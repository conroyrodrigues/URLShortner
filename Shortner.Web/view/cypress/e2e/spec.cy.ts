// cypress/integration/url-shortener.spec.js

describe('URL Shortener', () => {
  beforeEach(() => {
    // Visit the main page of your Angular application
    cy.visit('http://localhost:4200'); // Change this URL based on your Angular app's URL
  });

  it('should shorten a URL', () => {
    // Assuming you have an input with the placeholder "Enter URL"
    const originalUrl = 'https://www.example22.com';

    // Type the URL into the input field
    cy.get('#originalUrl').type(originalUrl);

    // Click the button to shorten the URL (change the button selector accordingly)
    cy.get('#btnShorten').contains('Shorten').click(); // Change the button text based on your app

    // Verify that the shortened URL appears
     // Change this selector to match where the short URL appears in your UI
    cy.get('#resultcontainer')
      .should('be.visible')
      .and('contain', originalUrl); 
  });
});
