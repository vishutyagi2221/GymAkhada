# Gym Akhada & Tournament Management System 💪🏆

A modern, responsive, and robust Gym and Tournament Management web application built with ASP.NET Core MVC (C#) and Entity Framework Core. This application features a stunning dark-theme UI and provides comprehensive tools for Admins to manage members, tournament categories, and tournament registrations, alongside a seamless User Portal for athletes.

## Key Features ✨

*   **Dark-Themed Modern UI**: Beautifully crafted interface using HTML5, CSS3, and Bootstrap 5 for a premium feel.
*   **Role-Based Access Control**: Secure login and dashboard for `Admin` and `Member` roles.
*   **Gym Member Management**: Full CRUD operations for managing gym members.
*   **Tournament Organization**: Create and manage tournaments with distinct categories (Age, Weight, Open).
*   **User Portal**: A dedicated portal for members to browse upcoming tournaments, view prize distributions (Gold, Silver, Bronze), and register seamlessly.
*   **Registration Management**: Admins can easily approve or deny tournament applications with categorized views.
*   **Automated Notifications**: Real-time Email and WhatsApp (via Twilio) notifications for tournament approval/denial and password resets.
*   **Secure Password Reset**: Forgot Password flow with dynamic links sent via Email or WhatsApp.
*   **Professional PDF Exports**: Generate perfect, text-extractable PDF reports with 'AKHADA' watermarks directly from the browser using jsPDF.

## Tech Stack 🛠️
*   **Backend**: C#, ASP.NET Core 8.0 MVC
*   **Database**: Microsoft SQL Server / Entity Framework Core (Code-First)
*   **Frontend**: Razor Pages, HTML, CSS (Bootstrap 5), JavaScript, jsPDF
*   **Integrations**: SMTP (Email Services), Twilio (WhatsApp Messaging)

## Installation & Setup 🚀

1.  **Clone the repository**
    ```bash
    git clone https://github.com/yourusername/GymAkhada.git
    cd GymAkhada
    ```

2.  **Update AppSettings**
    Open `appsettings.json` and configure:
    *   `DefaultConnection` (Your SQL Server connection string)
    *   `EmailSettings` (Your SMTP credentials)
    *   `TwilioSettings` (Your Twilio credentials for WhatsApp)

3.  **Apply Database Migrations**
    The app is configured to apply migrations automatically on startup, but you can also run:
    ```bash
    dotnet ef database update
    ```

4.  **Run the Application**
    ```bash
    dotnet run
    ```

5.  **Default Admin Login**
    *   Username: `admin`
    *   Password: `admin123`

## Author 👨‍🎓
**ANSH**
*Roll No: 2823361*
*Developed as a Major Academic Project.*
