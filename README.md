# VoteHub - Online Voting System

ASP.NET Core MVC + Entity Framework Core (Code First) + SQL Server

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full)
- Visual Studio 2022 or VS Code

---

### Step 1: Open Project
```
cd VoteHub
```

### Step 2: Install Packages
```
dotnet restore
```

### Step 3: Update Connection String
`appsettings.json` mein apna SQL Server connection string update karo:
```json
"DefaultConnection": "Server=.;Database=VoteHubDb;Trusted_Connection=True;TrustServerCertificate=True"
```

### Step 4: Create Database (Code First Migration)
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 5: Run Project
```
dotnet run
```

Browser mein open karo: `https://localhost:5001`

---

## Pages / Routes

| Page | URL |
|------|-----|
| Homepage | `/` |
| Voter Register | `/Voter/Register` |
| Voter Login | `/Voter/Login` |
| Cast Vote | `/Voter/Vote` |
| Admin Register | `/Admin/Register` |
| Admin Login | `/Admin/Login` |
| Admin Dashboard | `/Admin/Dashboard` |
| Results | `/Home/Results` |

---

## Flow

1. **Admin** creates account → Login → Add Candidates
2. **Voter** registers → Login → Cast Vote
3. **Admin** declares result → Results page par winner dikhega

---

## Tech Stack
- ASP.NET Core 8 MVC
- Entity Framework Core 8 (Code First)
- SQL Server
- Bootstrap 5
- BCrypt.Net (password hashing)
- Session-based authentication (NO Identity Framework)
