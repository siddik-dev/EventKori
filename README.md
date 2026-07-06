# EventKori

A full-stack event marketplace that connects event customers with vetted service providers for discovery, planning, quotations, and bookings — all in one clean workflow.
 
[![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-TypeScript-61DAFB?logo=react&logoColor=black)](https://react.dev/)
[![Vite](https://img.shields.io/badge/Vite-Frontend-646CFF?logo=vite&logoColor=white)](https://vitejs.dev/)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-Styling-06B6D4?logo=tailwindcss&logoColor=white)](https://tailwindcss.com/)

---
​
## Overview

EventKori brings event customers and service providers into one place. Customers post event requests (weddings, corporate events, birthdays, and more), browse verified providers, compare packages, request quotations, confirm bookings, and leave reviews. Service providers manage their business profile, showcase a portfolio, publish packages, and respond to booking requests.
​
The platform is built as a **.NET (ASP.NET Core) Web API** following clean architecture principles, paired with a modern **React + TypeScript** single-page front end.
​
**Key concepts**
​
- **Customers** create and manage event requests and bookings.
- **Service Providers** publish packages, portfolios, and respond to bookings.
- **Bookings / Quotations** link an event, a provider, and (optionally) a package with a total amount and status.
- **Reviews** let customers rate completed bookings, with an optional provider response.
​
---
​
## Features

### For Customers
- Register and sign in with a Customer role
- Create, update, and delete **event requests** with type, date, location, budget, attendee count, and special requirements
- Browse and search **service providers**, including featured providers
- Compare provider **packages** and portfolios
- Request and track **bookings / quotations** with live status
​
### For Service Providers
- Register and sign in with a Service Provider role
- Create and manage a **business profile** (company details, services, starting price, experience, verification)
- Publish and manage **packages** (pricing, features, duration, max attendees)
- Showcase a **portfolio** of past work
- Receive and update the status of **bookings**
​
### Platform
- **JWT-based authentication** with role-based access control (Customer / Service Provider)
- **Dashboard** with aggregated metrics: total & active event requests, verified organizers, pending quotations, and total quoted value
- **Search & filtering** by event type, status, location, and verification
- **Server-side validation** with FluentValidation
- **Database seeding** for realistic demo data
​
---
​
## Tech Stack

| Layer          | Technologies                                   |
| -------------- | ---------------------------------------------- |
| Backend        | .NET / ASP.NET Core Web API, C#                |
| Data           | Entity Framework Core, SQL Server              |
| Auth           | ASP.NET Core Identity, JWT Bearer tokens       |
| Validation     | FluentValidation (auto-validation)             |
| API Docs       | OpenAPI                              |
| Frontend       | React, TypeScript, Vite                        |
| Styling        | Tailwind CSS, PostCSS, lucide-react icons      |
| Data fetching  | TanStack Query (React Query), Axios            |
| State          | Zustand (persisted session)                    |
| Forms          | React Hook Form, Zod, Sonner (toasts)          |
| Routing        | React Router                                   |

---
​
## Architecture
​
The backend follows **Clean Architecture**, separating concerns into independent layers:
​
```text
EventKori.Domain          -> Entities, enums, repository interfaces (no dependencies)
EventKori.Application      -> DTOs, services, mappers, validators, interfaces
EventKori.Infrastructure   -> EF Core DbContext, repositories, Identity, JWT, DI
EventKori.WebAPI           -> Controllers, middleware, seeding, app startup
EventKori.Frontend         -> React + TypeScript SPA (Vite)
```

---
​
## Screenshots

<div align="center">
  
  ### Home Page
  <img src="https://github.com/user-attachments/assets/638a4952-768d-4e6d-b8ea-cc4b12cbc5d2" alt="Home Page" width="800">
  
  ### Shop Page
  <img src="https://github.com/user-attachments/assets/e92d78ea-a085-4dfb-96d8-acdd15b9cefd" alt="Shop Page" width="800">
  
  ### Details Page
  <img src="https://github.com/user-attachments/assets/85b2ded6-4fb3-4c40-86a9-37c1d4407806" alt="Details Page" width="800">
  
  ### View Cart Page
  <img src="https://github.com/user-attachments/assets/1d206380-974a-47e0-9a05-3c6ff81d4265" alt="View Cart Page" width="800">
  
  ### Checkout Page
  <img src="https://github.com/user-attachments/assets/0588a40c-a7e2-4888-988e-5ce69371db13" alt="Checkout Page" width="800">
  
  ### Order Placed Page
  <img src="https://github.com/user-attachments/assets/ad28fba1-99ae-428e-94e0-dec4f7fc2da9" alt="Order Placed Page" width="800">
</div>

---
​
## Getting Started

### Prerequisites
- [.NET ](https://dotnet.microsoft.com/download) 9
- [Node.js](https://nodejs.org/) 18+ and npm
- SQL Server
​
### 1. Clone the repository

```bash
git clone https://github.com/siddik-dev/EventKori.git
cd EventKori
```

### 2. Configure the backend

Update `EventKori.WebAPI/appsettings.json` with your database connection string and JWT settings:
​
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EventKori;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JWT": {
    "Key": "your-super-secret-signing-key",
    "Issuer": "EventKori",
    "Audience": "EventKoriClient"
  }
}
```

### 3. Apply migrations & run the API

```bash
cd EventKori.WebAPI
dotnet restore
dotnet ef database update
dotnet run
```

The database is seeded with demo data on startup. Explore the endpoints via Swagger at `/swagger`.

### 4. Run the frontend

```bash
cd EventKori.Frontend
npm install
npm run dev
```
​
Create a `.env` file in `EventKori.Frontend` to point at your API:
​
```bash
VITE_API_BASE_URL=http://localhost:5000/api
```

---
​
## Usage

1. Open the app at `http://localhost:5173`.
2. **Register** as either a *Customer* or a *Service Provider*.
3. As a **Customer**: create an event request, browse providers, request a booking, and review completed bookings.
4. As a **Service Provider**: set up your profile, add packages and portfolio items, and manage incoming bookings.
5. Track everything from the **Dashboard**.
​
---
​
## Roadmap

- [ ] Payment gateway integration
- [ ] Real-time notifications for booking updates
- [ ] Provider analytics & reporting
- [ ] Image/file uploads for portfolios
- [ ] Automated tests & CI/CD pipeline
​
---
