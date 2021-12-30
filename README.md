# VPM
## Visitor Parking Manager

VPM is a cloud-based, mobile-ready, web-application; a solution for buildings to manage their visitor parkings.

- Allow visitors to make reservations online
- Allows building managers to visitor parking reservations online

## Features

- To-be-added

## Tech
- [.NET 6] - Razor Pages

## Installation

VPM requires [.NET 6 Run-time](https://dotnet.microsoft.com/en-us/download)  to run.

Once the runtime is intalled, in order for the application to start, it needs to be initialized.

```
UpdateDatabase.ps1 dev
```

This will create the necessary database tables for authentication and authorization. 
After that, start the application (e.g. using Visual Studio) and navigation to `/Init`. Use the MasterPasswrord to initialize the application. This will create a default building and an admin user.

