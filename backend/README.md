Backend – Car Show Registration System

Project Overview

The backend is developed using ASP.NET Core Web API with C#. It handles participant registrations, validates information, and stores registration data using an SQLite database.

Technologies Used

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* Scalar
* Vertical Slice Architecture

Main Features

* Create car show registrations.
* Generate registration numbers.
* Validate participant information.
* Prevent duplicate plate numbers.
* View all registrations.
* Search and filter registrations.
* View registration details.
* View registration summary.

API Endpoints

Method	Endpoint	Description
POST	/api/registrations	Create registration
GET	/api/registrations	View all registrations
GET	/api/registrations/{id}	View registration details
GET	/api/registrations/summary	View registration summary

How to Run

1. Open the backend folder.
2. Open the terminal.
3. Run dotnet restore.
4. Run dotnet build.
5. Run dotnet run --project Api.

API Testing

Use Scalar to test the API endpoints:

http://localhost:5205/scalar/v1

Project Structure

* Api – Handles API endpoints.
* Application – Contains application features and handlers.
* Domain – Contains entities and business rules.
* Infrastructure – Handles database operations.
