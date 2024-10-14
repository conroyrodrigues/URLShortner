# URL Shortener

A simple URL shortener service that allows users to shorten long URLs and track the usage of those shortened links. Built with C# and Angular, using Microsoft SQL Server as the database.

## Application Overview

Designing and implementing a URL shortener service involves several key considerations and design choices to ensure it is functional, scalable, and user-friendly. Here’s an overview of my approach:

1. Requirements Gathering
    - Functionality: The service should shorten long URLs, redirect to the original URLs, track usage metrics (click counts, referrer information, etc.), and provide a user interface
    - User Authentication: This service will allow anonymous users to create shortened URLs.
2.  High-Level Architecture
    - Frontend: An Angular web interface to allow users to input long URLs and receive shortened links
    - Backend: A RESTful API built with ASP.NET Core to handle URL shortening.
    - Database: An MSSQL database to store URL mappings
3. Key Components
    - URL Mapping
        - Each shortened URL will map to its original URL in the database.
        - Use of a unique identifier (short code) for each URL, which is generated for a unique URL.
    - Error Handling
        - Provide meaningful error responses when a  URl can't be generated.
4. Database Schema Design
    - URL Table (UrlShorteners): Stores mappings between shortcodes and original URLs, along with creation timestamps.
5. URL Shortening Algorithm
    - Implement an algorithm for generating shortcodes. Common approaches include:
        - Hashing: Use hashing algorithms (GUID) to create a short, unique identifier based on the original URL.
6. Security Considerations
    - Validation: Ensured that input URLs were valid and sanitize user inputs to prevent injection attacks.
7. Deployment and Scalability
    -  Containerization: Use Docker to containerize the application, making it easier to deploy and scale.
    -  Load Balancing: Opportunity for using a load balancer if expecting high traffic to distribute incoming requests across multiple server instances.
    -  Database Scaling: Opportunity for database scaling using techniques like replication or partitioning if needed
8. Monitoring and Maintenance
    - Logging: Implemented logging to monitor application behaviour and diagnose issues.
9. Testing
    - Unit Testing: Implemented unit tests for key functions such as URL shortening and fetching all the URLs
    - End-to-end Testing: E2E testing is implemented using Cypress for key functionality.
    - Frontend Test: Implemented frontend test using karma

### Application Insight

The following diagram tries to explain the working of the underlying code.

 1. Class Diagram
 [Class Diagram](/img/ClassDiagram.png)

 2. ER Diagram
 `![ER Diagram](/img/ERDiagram.png)`

 3. Sequence Diagram
 `![ER Diagram](/img/Sequence_Diagaram.png)`
  

## Features

- Shorten long URLs
- Redirect to original URLs
- RESTful API endpoints for integration

## Technologies Used

- **Backend**: C# with ASP.NET Core, Xunit, Moq
- **Database**: Microsoft SQL Server
- **Containerization**: Docker and Docker Compose
- **Frontend**:  Angular, Karma, Cypress

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/)
- .NET 8 SDK (for development)

### Clone the Repository
```bash
git clone https://github.com/conroyrodrigues/URLShortner.git
cd URLShortner
```

### Project Structure

The structure of the application is as follows

`![Project Structure](/img/ProjectStructure.png)`
 
 - Database: Contains the docker setup file for required for setting the database with sample data
 - img: Files used in the documentation process
 - Shortner.Web: The Web Application in C#
    - view: The angular View Setup
- Tests/Shortner.Tests: Unit Test for testing the backend Services, this project runs on xUnit, Moq and EF. For the purpose of testing an InMemory database is used.
- worflows: Contains CI required to build and run the tests.
- docker-compose: contains the entire setup required to run the application

### How to Run the Application

1. git clone https://github.com/conroyrodrigues/URLShortner.git
2. docker-compose up -d
3. navigate to [http://localhost:8080/] (http://localhost:8080/) on the browser

You should see the following,
`![RunningApp](/img/RunningApp.png)`

An end to end Test should look so,
`![e2e](/img/e2eTests.png)`
