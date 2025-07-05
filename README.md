# API_Faculty_Project

An ASP.NET Core Web API for managing faculty-related data, including **Departments**, **Instructors**, **Accounts**, and **Courses**. This API is built using a clean architecture approach and is suitable for integration with web
or mobile applications in an educational environment.

---

## 🚀 Features

- ASP.NET Core Web API (latest LTS version)
- Entity Framework Core for data access
- RESTful endpoints for CRUD operations
- Structured layers: Controllers, Services, Repositories
- Authentication & Authorization (JWT)
- Dependency Injection and Model Validation
- Swagger/OpenAPI documentation support

---

## 📁 Project Structure
API_Faculty_Project/
│
├── Controllers/ # API Controllers (Department, Instructor, Course,Account)
├── Models/ # Entity Models
├── DTOs/ # Data Transfer Objects
├── Data/ # EF Core DB Context & Migrations
├── Services/ # Business logic services
├── API_Faculty_Project.csproj
├── Program.cs # App bootstrap and middleware config
├── appsettings.json # Configuration file
└── README.md # Project documentation


---

## 🛠️ Getting Started

### Prerequisites

- [.NET 6.0 or later](https://dotnet.microsoft.com/)
- SQL Server (LocalDB or full instance)
- Visual Studio 2022+ or Visual Studio Code

---

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/your-username/API_Faculty_Project.git
cd API_Faculty_Project


🔌 API Endpoints
📚 Departments
Method	Endpoint	Description
GET	/api/departments	Get all departments
GET	/api/departments/{id}	Get department by ID
POST	/api/departments	Create new department
PUT	/api/departments/{id}	Update department
DELETE	/api/departments/{id}	Delete department

👨‍🏫 Instructors
Method	Endpoint	Description
GET	/api/instructors	Get all instructors
GET	/api/instructors/{id}	Get instructor by ID
POST	/api/instructors	Create new instructor
PUT	/api/instructors/{id}	Update instructor
DELETE	/api/instructors/{id}	Delete instructor

📘 Courses
Method	Endpoint	Description
GET	/api/resources	Get all course resources
GET	/api/resources/{id}	Get resource by ID
POST	/api/resources	Create new resource
PUT	/api/resources/{id}	Update resource
DELETE	/api/resources/{id}	Delete resource

🔐 Authentication & Authorization (JWT)
This API uses JWT (JSON Web Tokens) for secure authentication and role-based authorization.

🔑 Login to Get Token
Authenticate by sending a POST request to:
POST /api/auth/login

Request Body Example:
{
  "username": "admin",
  "password": "admin123"
}

Successful Response:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp..."
}

Use this token in the Authorization header for all protected requests:
Authorization: Bearer <your-token>
🔒 Protected Endpoints
Use [Authorize] to secure endpoints. Roles can be specified via [Authorize(Roles = "Admin")].

Endpoint	Access Role
GET /api/Course	Any Authenticated User
POST /api/Course	Any Authenticated User
PUT /api/Course/id	Any Authenticated User
DELETE /api/Course/id	Any Authenticated User

🛡️ Token Validation
Tokens are validated using the following settings from appsettings.json:
"JwtSettings": {
  "Key": "YourSecretKeyHere",
  "Issuer": "API_Faculty_Project",
  "Audience": "FacultyUsers",
  "DurationInMinutes": 60
}
📘 Swagger UI Authorization
You can authorize your requests directly in Swagger UI:
Click the "Authorize" button.
Enter your token:
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...
Swagger will attach the token to future requests.

📖 API Documentation
Once the app is running, access Swagger UI:

https://localhost:{port}/swagger
Swagger provides interactive documentation and testing for all endpoints.

🧪 Technologies Used
ASP.NET Core Web API
Entity Framework Core
SQL Server
JWT (JSON Web Tokens) for secure authentication and role-based authorization
Swagger / Swashbuckle

📝 License
This project is licensed under the MIT License.

🙌 Contributing
Contributions, issues, and feature requests are welcome!
Feel free to check the issues page.
Let me know if you’d like this exported as a file, or if you want me to add instructions for JWT authentication, Docker, or CI/CD setup.
