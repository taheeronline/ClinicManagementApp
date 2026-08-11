# 📄 Functional Specification

**Target Audience:** Product Managers, Business Analysts, and Stakeholders.
**Purpose:** Defines *what* the system does and the business rules it enforces.

## 1. Core Features
* **Unified Dashboard:** Must provide real-time metrics (Appointments Today, Pending Bills) and allow operational actions without navigating away from the page.
* **Entity CRUD:** Create, Read, Update, and Delete operations for Doctors, Patients, Appointments, and Bills.
* **Dynamic UI Validation:** All forms must validate required fields, email formats, and number lengths before submission.

## 2. Strict Business Rules (Enforced by System)
1. **Immutability of EMR:** Once a Patient Record/Prescription is generated, it becomes read-only. It cannot be modified or deleted.
2. **Financial Lock:** Once a Consultation Bill is marked as 'Paid', it cannot be modified or deleted.
3. **Double-Booking Prevention:** The system blocks scheduling an appointment if the selected doctor already has a booking within a 30-minute window of the requested time.
4. **Referential Integrity (Data Retention):** A Doctor or Patient cannot be deleted if they have historical appointments or medical records tied to them.
5. **Appointment State Lock:** Appointments marked as `Completed` or `Closed` cannot be altered or deleted.
6. **Automated No-Shows:** Appointments left in `Scheduled` status that are past due by 2+ hours can be auto-marked as `NoShow` via the Dashboard Clean-Up tool.