$loginB64 = Get-Content "D:\GymAkhada\Reports\login_b64.txt" -Raw
$subcatB64 = Get-Content "D:\GymAkhada\Reports\subcat_b64.txt" -Raw
$attendB64 = Get-Content "D:\GymAkhada\Reports\attend_b64.txt" -Raw

$html = @"
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Major Project Report - Gym Akhada</title>
    <style>
        body { font-family: 'Times New Roman', Times, serif; line-height: 1.6; margin: 0; padding: 0; background-color: #f4f4f4; color: #000; }
        .page { background: white; width: 210mm; min-height: 297mm; margin: 20px auto; padding: 20mm; box-shadow: 0 0 10px rgba(0,0,0,0.1); box-sizing: border-box; }
        @media print { body { background: white; margin: 0; } .page { margin: 0; padding: 20mm; box-shadow: none; page-break-after: always; } }
        .text-center { text-align: center; }
        .title { font-size: 28pt; font-weight: bold; margin-top: 100px; color: #111; }
        .subtitle { font-size: 18pt; margin-top: 20px; }
        .details { margin-top: 150px; font-size: 14pt; }
        h1 { font-size: 20pt; border-bottom: 2px solid #000; padding-bottom: 5px; margin-top: 40px; page-break-before: always; }
        h2 { font-size: 16pt; margin-top: 30px; }
        h3 { font-size: 14pt; margin-top: 20px; }
        p { font-size: 12pt; text-align: justify; }
        ul { font-size: 12pt; }
        table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12pt; }
        th, td { border: 1px solid #000; padding: 8px; text-align: left; }
        th { background-color: #eaeaea; }
        .img-container { text-align: center; margin: 30px 0; }
        .img-container img { width: 100%; max-height: 400px; object-fit: contain; border: 1px solid #ccc; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }
        .caption { text-align: center; font-style: italic; font-size: 11pt; color: #444; margin-top: 10px; }
    </style>
</head>
<body>

    <!-- Cover Page -->
    <div class="page text-center">
        <div class="title">MAJOR PROJECT REPORT</div>
        <div class="subtitle">ON</div>
        <div class="title" style="margin-top: 20px; font-size: 24pt;">GYM AKHADA & TOURNAMENT MANAGEMENT SYSTEM</div>
        <div class="details">
            <p>Submitted in partial fulfillment of the requirements for the Degree</p>
            <br><br><br>
            <p><strong>Submitted By:</strong></p>
            <p><strong>Name:</strong> ANSH</p>
            <p><strong>Roll No:</strong> 2823361</p>
        </div>
    </div>

    <!-- Acknowledgement Page -->
    <div class="page">
        <h1 style="page-break-before: avoid; margin-top: 0;">Acknowledgement</h1>
        <p>I would like to express my profound gratitude to all those who have been instrumental in the successful completion of this major project. I am deeply thankful to my mentors and faculty members for their continuous guidance, support, and invaluable feedback throughout the development of the "Gym Akhada & Tournament Management System".</p>
        <p>Developing a full-stack, enterprise-level application has been a tremendous learning experience. The challenges faced during the integration of server-side logic, database management, and automated APIs have significantly improved my technical acumen. I am grateful for the opportunity to apply theoretical concepts to a practical, real-world application.</p>
        <br><br><br>
        <p style="text-align: right; font-weight: bold;">ANSH<br>Roll No: 2823361</p>
    </div>

    <!-- Abstract & Intro -->
    <div class="page">
        <h1 style="margin-top: 0;">1. Abstract</h1>
        <p>The "Gym Akhada & Tournament Management System" is an advanced web-based application designed to bridge the gap between daily gym administration and the complexities of organizing athletic tournaments. Traditionally, gyms manage member records and tournament entries through fragmented systems or manual spreadsheets, leading to data loss, miscommunication, and inefficiencies.</p>
        
        <div class="img-container">
            <img src="data:image/png;base64,$loginB64">
            <div class="caption">Fig 1: The Modern Dark-Themed Home Page & Login Portal</div>
        </div>

        <p>This project introduces a centralized platform featuring Role-Based Access Control (RBAC). It allows administrators to seamlessly manage gym members, dynamically categorize tournaments (e.g., by age and weight), and process applications with a click of a button.</p>

        <h2>2. System Objectives</h2>
        <ul>
            <li>To digitize the manual process of tournament registrations.</li>
            <li>To provide a secure, persistent authentication system without relying on heavy third-party identity frameworks.</li>
            <li>To implement an automated notification engine (Email/WhatsApp) for registration approvals, denials, and password resets.</li>
            <li>To enable dynamic, client-side PDF report generation with data extraction capabilities.</li>
        </ul>
    </div>

    <!-- Technology & Server Handling -->
    <div class="page">
        <h1 style="margin-top: 0;">3. Technology Stack & Tools Used</h1>
        <p>The project was developed using a modern, scalable technology stack tailored for enterprise application development.</p>
        <table>
            <tr><th>Component</th><th>Technology Used</th></tr>
            <tr><td>Backend Framework</td><td>C#, ASP.NET Core 8.0 MVC</td></tr>
            <tr><td>Database Architecture</td><td>Microsoft SQL Server with Entity Framework Core (Code-First)</td></tr>
            <tr><td>Frontend Design</td><td>HTML5, CSS3, Bootstrap 5, Razor Pages</td></tr>
            <tr><td>Authentication</td><td>Custom Cookie-based Auth with SHA-256 Encryption</td></tr>
            <tr><td>PDF Generation</td><td>jsPDF & AutoTable (Client-side JavaScript)</td></tr>
            <tr><td>Version Control</td><td>Git & GitHub</td></tr>
        </table>

        <h2>4. Server Handling & Architecture</h2>
        <p>The application follows the highly scalable <strong>Model-View-Controller (MVC)</strong> architecture. Server handling is optimized for concurrent requests.</p>
        
        <div class="img-container">
            <img src="data:image/png;base64,$subcatB64">
            <div class="caption">Fig 2: Administrative Panel showing robust Database categorization (Subcategory Master)</div>
        </div>

        <ul>
            <li><strong>Dependency Injection (DI):</strong> Core services such as `IEmailService` and `IWhatsAppService` are injected into the application pipeline, ensuring loose coupling and efficient memory management.</li>
            <li><strong>Database Context:</strong> `ApplicationDbContext` manages the SQL connection pooling. Queries are optimized using LINQ and asynchronous programming (`async/await`) to prevent thread blocking on the Kestrel web server.</li>
            <li><strong>Hosting Environment:</strong> The application is compiled into a standalone Release build and deployed on SmarterASP.net, utilizing IIS reverse proxying to route HTTP requests securely to the .NET runtime.</li>
        </ul>
    </div>

    <!-- APIs and Conclusion -->
    <div class="page">
        <h1 style="margin-top: 0;">5. API Integrations & Core Logic</h1>
        
        <h3>5.1 Twilio WhatsApp API</h3>
        <p>To ensure athletes are instantly notified, the Twilio REST API is integrated. When an admin approves a tournament registration, the `WhatsAppService` formulates a JSON payload containing the user's mobile number and the approval message, transmitting it securely over HTTPS to Twilio's endpoints.</p>

        <div class="img-container">
            <img src="data:image/png;base64,$attendB64">
            <div class="caption">Fig 3: The Daily Attendance Tracking interface used by Gym Staff</div>
        </div>

        <h3>5.2 SMTP Email Service</h3>
        <p>The `System.Net.Mail.SmtpClient` is utilized for standard email dispatches. This is heavily relied upon in the "Forgot Password" flow, where a unique GUID token is generated, stored in the SQL database with an expiry timestamp, and a secure reset link is emailed to the user.</p>

        <h2>6. Conclusion & Future Scope</h2>
        <p>The "Gym Akhada & Tournament Management System" has successfully achieved its primary objective of digitizing gym and tournament operations. By building a custom authentication system, utilizing Entity Framework for complex relational data mapping, and integrating modern communication APIs, the system provides a robust, production-ready solution.</p>
        <p><strong>Future Scope:</strong></p>
        <ul>
            <li>Integration of a payment gateway (e.g., Razorpay/Stripe) to process tournament entry fees online.</li>
            <li>Development of a cross-platform mobile application using .NET MAUI to interface with this backend.</li>
        </ul>
        <br><br>
        <p class="text-center"><strong>--- END OF REPORT ---</strong></p>
    </div>

</body>
</html>
"@

Set-Content -Path "D:\GymAkhada\Reports\Major_Project_Report.html" -Value $html
