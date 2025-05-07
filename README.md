# KauRestaurant

KauRestaurant is a comprehensive restaurant management system built with ASP.NET Core Razor Pages. It provides a platform for restaurants to manage their menus, meals, and customer interactions.

---

## 🍽️ Features

- **Menu Management**  
  Create and manage weekly menus for your restaurant.

- **Meal Catalog**  
  Maintain a detailed catalog of meals with nutritional information.

- **Review System**  
  Allow customers to leave reviews and ratings for meals.

- **User Management**  
  Different user roles (Administrators and Customers) with specific permissions.

- **Feedback System**  
  Collect and manage customer feedback.

- **Admin Dashboard**  
  Comprehensive dashboard for restaurant administrators.

- **Responsive Design**  
  Mobile-friendly interface for all users.

---

## 🚀 Technologies Used

- **Framework:** ASP.NET Core 8.0 with Razor Pages  
- **Database:** Entity Framework Core with SQL Server  
- **Authentication:** ASP.NET Core Identity  
- **Frontend:** Bootstrap 5.3, HTML5, CSS3, JavaScript  
- **Real-time Communication:** SignalR  
- **Other Tools:** QRCoder for QR code generation  

---

## 📋 Prerequisites

- .NET 8.0 SDK or later  
- SQL Server (or SQL Server Express)  
- Visual Studio 2022 or another compatible IDE  

---

## ⚙️ Installation

1. **Clone the repository**  
   git clone https://github.com/yourusername/KauRestaurant.git

2. **Navigate to the project directory**  
   cd KauRestaurant

3. **Restore NuGet packages**  
   dotnet restore

4. **Configure database connection**  
   Open `appsettings.json` and update the `ConnectionStrings:DefaultConnection` to point to your SQL Server instance.

5. **Apply database migrations**  
   dotnet ef database update

6. **Run the application**  
   dotnet run

---

## 🔧 Configuration

Edit the `appsettings.json` to customize:

- **Database Connection:** Your SQL Server connection string.  
- **Identity Settings:** User authentication and security options.  
- **Restaurant Information:** Details displayed throughout the application (name, address, contact).

---

## 🧰 Project Structure

    ├── Areas/
    │   └── Identity/        # Identity-related pages and models
    ├── Controllers/         # Admin and User controllers
    ├── Data/                # DbContext and data seeders
    ├── Models/              # Application data models
    ├── Pages/               # Razor Pages (views + page models)
    ├── wwwroot/             # Static files (CSS, JS, images)
    └── appsettings.json     # Configuration file

---

## 👥 User Roles

- **Administrators (A1, A2, A3):**  
  Manage menus, meals, and user accounts.

- **Customers:**  
  Browse menus, view meal details, and leave reviews.

---

## 👏 Acknowledgements

- **[Bootstrap](https://getbootstrap.com/)** — Responsive UI framework  
- **[ASP.NET Core](https://docs.microsoft.com/aspnet/core)** — Web framework  
- **[Entity Framework Core](https://docs.microsoft.com/ef/core)** — ORM for .NET  
