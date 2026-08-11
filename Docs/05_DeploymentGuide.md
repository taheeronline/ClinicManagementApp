# 🚀 Deployment Guide

**Target Audience:** DevOps, IT Admin, or Hosting Provider.
**Purpose:** Instructions for installing the software in a production environment.

## 1. Prerequisites
* Windows Server with IIS (or Azure App Service).
* SQL Server (Express, Standard, or Azure SQL).
* .NET 8 Hosting Bundle installed on the server.

## 2. Initial Setup
1. **Database Connection:** Update the `appsettings.json` in the Server project with the production SQL Server connection string.
2. **Migrations:** Run `Update-Database` in the Package Manager Console to generate the SQL tables and seed the default `Clinic` data row.
3. **Publishing:** Right-click the Server project in Visual Studio -> Publish. Ensure the deployment mode is set to "Framework Dependent" for smaller file sizes.
4. **CORS & Security:** If hosting the API and WebAssembly client on different domains, ensure CORS policies are updated in `Program.cs` to allow the client domain.