# 🧪 Test Plan

**Target Audience:** QA Engineers and Developers.
**Purpose:** Ensures the software functions securely and meets business requirements.

## 1. UI & Usability Testing
* Verify the Dashboard KPI counters match the actual records in the database.
* Verify that Dashboard scrollable lists do not exceed their `max-height` (250px).
* Verify that search bars filter lists in real-time (case-insensitive).
* Verify that a globally updated Clinic Name reflects instantly in the top Navigation bar.

## 2. Business Logic Edge-Case Testing

| Test ID | Scenario | Expected Result | Status |
| :--- | :--- | :--- | :--- |
| **TC-01** | Create two appointments for Dr. A at 10:00 AM. | System rejects the second booking with a 400 Bad Request and shows a UI alert. | Pending |
| **TC-02** | Attempt to delete a Patient who has past appointments. | Deletion blocked. UI shows data retention error message. | Pending |
| **TC-03** | Attempt to edit an Appointment marked as "Completed". | UI Edit buttons are hidden (Padlock shown). Direct URL access redirects to list. | Pending |
| **TC-04** | Click "Clean Up" on Dashboard when an appointment is 3 hours overdue. | Status changes to "NoShow". Dashboard schedule refreshes instantly. | Pending |
| **TC-05** | View a Medical Record after generation. | Only Read-Only view is available. No Save/Edit buttons exist. | Pending |