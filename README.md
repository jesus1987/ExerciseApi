User Story: 
	Developing a WEB API
Title:

	As a developer, I want to build a WEB API and robust data layer, so that users can efficiently access and manage data.

Description:

	The goal is to create a simple scalable web application using .NET C#, ASP.NET MVC, and Web API. 
	The application should integrate with a database for persistent data management.
	The development process should adhere to clean architecture principles to ensure maintainability and scalability and utilize Test-Driven Development (TDD).

Acceptance Criteria:

	API Layer:

		The API should follow RESTful principles.
		The API should support CRUD operations.
		The API should support Log in/Log out operations.
		API endpoints should be properly documented (e.g., using Swagger).
		The API endpoint should support natural language query(use AI)
		
	Business Layer:
		The business layer should validate data and handle business models
	Data Layer:
	
		The data access layer should create data base and tables
		The data access layer should use a repository pattern to interact with the database.

	Clean Architecture:

		The project should be structured into distinct layers: Api Layer, Data Layer.
	
	Test-Driven Development:

		Unit tests should cover key business logic.
		Integration tests should validate the interaction between components.
		Tests should be written before implementing any functionality.


How It Was Made:

The API was developed using .NET Core 8 and Visual Studio 2022.

Architecture:
	Dependency Injection: The architecture incorporates dependency injection to promote loose coupling and modularity.
	
	Layered Structure: The application is organized into three layers:
		1. API Layer: Contains controllers that handle HTTP requests.
		2. Business Layer: Encapsulates business logic and application rules.
		3. Data Layer: Responsible for interacting with the database.
	Database Interaction: Data is saved to the database using raw SQL queries.
	
	Mapping Helpers: Custom helpers were created to:
		1. Map database query results to objects.
		2. Convert view models into entities for better data management.

Testing:
	A dedicated unit test project was implemented, including:
		1. Unit Tests: To validate individual components and ensure functionality.
		2. Integration Tests: Written using Test-Driven Development (TDD) practices to verify end-to-end behavior and expected results.

