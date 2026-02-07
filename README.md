# BlueCraeft Bowl Parlay App

BlueCraeft Bowl is a .NET 8 Web Application designed for family and friends to engage in a parlay betting game during an NFL Football Game. Participants can predict events and stats in real-time, with live leaderboard updates powered by SignalR.

## Features

- **Parlay Types**: Supports A/B choices and Over/Under predictions.
- **Real-time Updates**: Live leaderboard standings using SignalR.
- **Admin Dashboard**:
    - Manage Parlay items (Create/Delete).
    - User Management (Activate/Deactivate participants).
    - Game Control (Lock selections and enter results).
- **Authentication**: Integrated with Google Identity Provider.
- **Responsive UI**: Built with Razor Pages and Bootstrap 5 for mobile and desktop support.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB supported for development)
- Google OAuth Credentials (for Authentication)

### Initial Setup

1. Clone the repository.
2. Update `appsettings.json` with your Google `ClientId` and `ClientSecret`.
3. Run the application:
   ```bash
   dotnet run
   ```

## Default Admin Credentials

The application automatically seeds a default administrator account on the first run.

- **Email**: `admin@bluecraeftbowl.com`
- **Default Password**: `Admin123!`

> [!CAUTION]
> **ADVISORY**: Please change the default administrator password immediately after your first login to ensure the security of your application.

## Deployment

The application is designed to be deployed as an **Azure Container App** with an **Azure SQL Database**. Ensure connection strings and authentication secrets are properly configured in your production environment.
