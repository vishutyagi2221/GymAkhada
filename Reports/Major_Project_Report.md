<style>
body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; line-height: 1.6; padding: 20px; }
h1, h2, h3 { color: #333; }
.cover-page { text-align: center; margin-top: 100px; margin-bottom: 200px; page-break-after: always; }
.cover-page h1 { font-size: 3em; margin-bottom: 10px; }
.cover-page h2 { font-size: 1.5em; font-weight: normal; margin-bottom: 50px; }
.cover-details { margin-top: 50px; font-size: 1.2em; }
.page-break { page-break-before: always; }
table { width: 100%; border-collapse: collapse; margin-top: 10px; margin-bottom: 20px; }
th, td { border: 1px solid #ccc; padding: 10px; text-align: left; }
th { background-color: #f4f4f4; }
.image-placeholder { background-color: #eaeaea; border: 2px dashed #999; padding: 40px; text-align: center; margin: 20px 0; color: #555; }
</style>

<div class="cover-page">
    <h1>MAJOR PROJECT REPORT</h1>
    <h2>Gym Akhada & Tournament Management System</h2>
    <div class="cover-details">
        <p><strong>Submitted by:</strong></p>
        <p><strong>Name:</strong> ANSH</p>
        <p><strong>Roll No:</strong> 2823361</p>
        <br>
        <p><em>In partial fulfillment of the requirements for the Degree</em></p>
    </div>
</div>

## Acknowledgement
I would like to express my profound gratitude to all those who have been instrumental in the successful completion of this project. I am deeply thankful to my mentors and faculty for their continuous guidance, support, and invaluable feedback throughout the development of the "Gym Akhada & Tournament Management System".

This journey has been a tremendous learning experience, and I am grateful for the opportunity to apply theoretical concepts to a practical, real-world application.

-- **ANSH (Roll No: 2823361)**

<div class="page-break"></div>

## 1. Introduction
The "Gym Akhada & Tournament Management System" is a comprehensive web-based application designed to streamline the operations of a modern gym and facilitate the complex process of organizing athletic tournaments. With the growing number of participants in local and regional fitness competitions, manual management using spreadsheets or paper records is highly inefficient. 

This project aims to digitize and automate:
- Gym member registrations and profiles.
- Creation of detailed tournament structures (Age, Weight, and Open categories).
- User enrollment workflows.
- Administrative approval pipelines with automated Email/WhatsApp notifications.

## 2. Technologies Used
*   **Backend Framework**: ASP.NET Core 8.0 MVC (C#)
*   **Database**: Microsoft SQL Server accessed via Entity Framework Core (Code-First Approach)
*   **Frontend**: Razor Pages, HTML5, Vanilla CSS, Bootstrap 5, JavaScript
*   **External Integrations**:
    *   **SMTP Service**: For automated Email notifications (Approval/Denial, Password Reset).
    *   **Twilio API**: For automated WhatsApp notifications.
    *   **jsPDF & AutoTable**: For client-side, text-extractable PDF generation with watermarks.

<div class="page-break"></div>

## 3. System Architecture & Workings
The system strictly follows the Model-View-Controller (MVC) architectural pattern.

### 3.1 Role-Based Access Control
The application handles two distinct roles:
*   **Admin**: Has full access to manage members, categories, subcategories, tournaments, and participant approvals.
*   **User (Member)**: Has a personalized portal to view active tournaments, examine prize distributions, and submit participation requests.

### 3.2 Tournament Registration Flow
1. Admin creates a Tournament and specifies Gold, Silver, and Bronze prizes.
2. User logs into the Member Portal and views the Tournament.
3. User clicks "Register". The system records their request as "Pending".
4. Admin reviews the request in the "Registrations" panel, seeing the user's detailed profile (Age, Weight, Aadhar).
5. Admin clicks "Approve" or "Deny".
6. System automatically dispatches an Email/WhatsApp to the user with the decision.

### 3.3 Security Features
*   **Password Hashing**: SHA-256 is used to securely hash and store passwords.
*   **Cookie Authentication**: Secure, persistent cookie-based login state.
*   **Password Reset**: Integrated secure token generation with expiry validation via Email/WhatsApp links.

<div class="page-break"></div>

## 4. User Interface & Screen Workings (Placeholders)

*(Note: During final printing, replace these placeholders with actual screenshots of the running application)*

<div class="image-placeholder">
    <h3>Screenshot: Admin Dashboard</h3>
    <p>Displays total members, active tournaments, and pending approvals.</p>
</div>

<div class="image-placeholder">
    <h3>Screenshot: Member Portal</h3>
    <p>Shows the stunning dark-themed UI where users can view "Prize Distributions" and join tournaments.</p>
</div>

<div class="image-placeholder">
    <h3>Screenshot: PDF Generation (Admin)</h3>
    <p>Demonstrates the dynamically generated PDF list of participants with the 'AKHADA' watermark.</p>
</div>

## 5. Conclusion
The Gym Akhada & Tournament Management System successfully achieves its goal of bridging the gap between gym administration and tournament execution. The integration of modern notifications (WhatsApp/Email) and secure dynamic PDF generation significantly reduces administrative overhead. This project served as an excellent practical implementation of advanced ASP.NET Core concepts, database design, and UI/UX principles.
