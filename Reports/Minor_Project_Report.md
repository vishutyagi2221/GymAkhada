<style>
body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; padding: 20px; }
h1, h2 { color: #333; }
.cover-page { text-align: center; margin-top: 100px; margin-bottom: 100px; page-break-after: always; }
.cover-page h1 { font-size: 2.5em; margin-bottom: 10px; }
.cover-page h2 { font-size: 1.2em; font-weight: normal; margin-bottom: 50px; }
</style>

<div class="cover-page">
    <h1>MINOR PROJECT REPORT</h1>
    <h2>Gym Akhada System (Core Modules)</h2>
    <div style="margin-top: 50px; font-size: 1.2em;">
        <p><strong>Submitted by:</strong> ANSH</p>
        <p><strong>Roll No:</strong> 2823361</p>
    </div>
</div>

## 1. Introduction
This minor report focuses on the core foundational modules developed for the "Gym Akhada" project. Before integrating complex tournament structures and automated notifications, the core member management and authentication modules were designed.

## 2. Objectives
- To build a secure user authentication system using C# and ASP.NET Core.
- To design an Entity Framework Core database schema for Gym Members.
- To create a responsive, dark-themed User Interface.

## 3. Implementation Details
The system utilizes a SQL Server backend. The `AppUser` and `GymMember` tables are linked via a Foreign Key, ensuring that login credentials are mathematically tied to physical member profiles. 

Features implemented in this phase:
- **Custom Authentication**: Instead of relying on boilerplate Identity, a custom Cookie-based authentication controller was built featuring SHA-256 password hashing.
- **Member CRUD**: Full Create, Read, Update, and Delete operations for managing gym goers.
- **UI Design**: Bootstrap 5 was heavily customized to provide a dark, athletic aesthetic fitting for an "Akhada" (Gym).

## 4. Conclusion
The minor phase successfully established the robust architecture needed. The secure login flow and clean database design paved the way for the advanced tournament features implemented in the major phase.
