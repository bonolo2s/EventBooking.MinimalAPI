# EventBooking.MinimalAPI

A scalable, RESTful API built using **.NET Minimal API** for managing events and bookings. This is my **playground** where I prototype quickly, test advanced patterns, and experiment with modern .NET features.

## Features

- **Entities**: Event, User, Booking
- **Authentication**: JWT-based, role management
- **Endpoints**:
  - Create, update, delete, and list events
  - Book or cancel tickets
  - Query events with filters and pagination
- **DI Services**: Repositories, logging, notifications
- **Middleware**: Logging, error handling, authentication
- **Notifications**: Email/SMS alerts on booking
- **Testable**: Services can be mocked for unit tests
- **LINQ**: For filtering, sorting, and paging

## Getting Started

1. Clone the repo  
2. Configure `appsettings.json` (JWT, SMTP, etc.)  
3. Run the app with `dotnet run`  
4. API endpoints exposed under `/api`  

## Example Routes

- `GET /events` – List events  
- `GET /events/{id}` – Get event by ID  
- `POST /events` – Create event  
- `POST /bookings` – Book a ticket  

## Why Minimal API?

- Lightweight and fast setup  
- Focused on routes, DI, and middleware  
- Ideal for learning modern .NET API patterns without controllers
