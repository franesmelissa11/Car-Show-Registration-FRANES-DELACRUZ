## Car Show Registration System

A web-based Car Show Registration System developed for Integrative Programming 2.
This project is designed to help event organizers manage car show registrations, 
participant information,registration fees, and payments.
The system uses a React frontend and an ASP.NET Core Web API backend with SQLite database storage.


## Team Members and Roles

| Team Member | Role |
|---|---|
| Daniela Joy Dela Cruz | Backend Developer |
| Melisa Mae Franes | Frontend Developer |


## Technologies Used

Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* Vertical Slice Architecture
* Scalar API Documentation

Frontend

* React JS
* Vite
• JavaScript
* Tailwind CSS 

## Project Structure

```Project Structure

Car-Show-Registration-FRANES-DELACRUZ/
│
├── backend/
│   ├── Api/
│   ├── Application/
│   ├── Domain/
│   └── Infrastructure/
│
├── docs/
│   └── CarShowRegistration-Franes_DelaCruzscreenshots.docx
│
├── public/
├── src/
├── .gitignore
├── README.md
└── package.json
```
## Main Features

* Register car show participants.
* Generate unique registration numbers.
* Automatically calculate registration fees.
* Validate participant information.
* Prevent duplicate car plate numbers.
* View all registrations.
* Search and filter registrations.
* View registration details using an ID.
* Track payment status as Unpaid, Partial, or Paid.
* Display registration summary and payment totals.

## Registration Types and Fees

Registration Type	Fee

Exhibitor	₱1,500

Competitor	₱2,500

VIP	₱5,000

## API Endpoints

Method	Endpoint	Description

POST	/api/registrations	Create a new registration

GET	/api/registrations	Get all registrations

GET	/api/registrations/{id}	Get registration by ID

GET	/api/registrations/summary	Get registration summary

The registrations endpoint also supports filtering and pagination.

## Database

The system uses SQLite with Entity Framework Core to store registration records.

The database keeps registration information even after the API is restarted.

## How to Run the Project

Backend

1. Open the project repository.
   
2. Open the backend solution using Visual Studio.
 
3. Make sure the required .NET SDK is installed.
 
4. Configure the database connection if needed.
 
5. Run the API project.

The Scalar API documentation is available at:

http://localhost:5205/scalar/v1

Frontend

1. Open the project folder in Visual Studio Code.

2. Open the terminal in the repository root folder.

3. Install the required packages:
   npm install

4. Start the frontend:
   npm run dev

5. Open the local URL displayed in the terminal.

## Documentation

The project documentation and screenshots are available in the docs folder.




GitHub: https://github.com/franesmelissa11/Car-Show-Registration-FRANES-DELACRUZ
