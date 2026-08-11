# 📐 Design Specification (Technical Architecture)

**Target Audience:** Software Engineers and Technical Leads.
**Purpose:** Defines *how* the system is built under the hood.

## 1. Tech Stack
* **Frontend:** .NET 8 Blazor WebAssembly (InteractiveWebAssemblyRenderMode).
* **Backend:** ASP.NET Core Web API.
* **Database:** Microsoft SQL Server.
* **ORM:** Entity Framework (EF) Core.
* **Styling:** Bootstrap 5 & Bootstrap Icons.

## 2. Design Patterns & Architecture
* **Client-Server Decoupling:** The UI (Client) and Database (Server) are strictly separated. The Client only communicates via HTTP REST API calls.
* **Data Transfer Objects (DTOs):** Shared project contains DTOs. Database models are never exposed directly to the frontend.
* **Service Repository Pattern:** Business logic is isolated inside `Services` (e.g., `AppointmentService.cs`), keeping Controllers thin and focused only on HTTP routing.
* **Global Exception Handling:** The Server implements `IExceptionHandler` to catch business rule violations (`InvalidOperationException`) and translate them into standardized `400 Bad Request` ProblemDetails JSON for the client to display.

## 3. Database Schema (Core Tables)

| Table | Key Relationships | Purpose |
| :--- | :--- | :--- |
| `Clinics` | Single Row (Seeded) | Stores global clinic branding. |
| `Doctors` | 1:M with Appointments | Stores doctor profiles and fees. |
| `Patients` | 1:M with Appointments | Stores patient demographics. |
| `Appointments` | FK to Doctor & Patient | The central transactional hub. |
| `PatientRecords` | 1:1 with Appointment | The immutable medical consultation note. |
| `Prescriptions` | 1:M with Appointment | Medicines tied to a specific consultation. |
| `ConsultationBills`| 1:1 with Appointment | Financial invoice and payment status. |