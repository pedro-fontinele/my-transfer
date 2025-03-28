# My Transfer 📂🔐

## 📌 Description

**My Transfer** is a secure file transfer system developed with cutting-edge technologies to enable efficient and reliable file management via SFTP. Designed to simplify file transfers, the application provides a robust solution for organizations and developers seeking seamless file exchange capabilities.

---

## 🚀 Technologies

![.NET](https://img.shields.io/badge/.NET-7-purple)
![Angular](https://img.shields.io/badge/Frontend-Angular-red)
![SFTP](https://img.shields.io/badge/Protocol-SFTP-green)
![SQLite](https://img.shields.io/badge/Database-SQLite-blue)
![PO UI](https://img.shields.io/badge/UI%20Library-PO%20UI-lightblue)

### Tech Stack
- **Backend**: .NET 7, Entity Framework (EF)
- **Frontend**: Angular, PO UI
- **Transfer Protocol**: SFTP (SSH File Transfer Protocol)
- **Database**: SQLite

---

## 🛠️ System Requirements

### 🔹 Prerequisites

Ensure the following are installed:

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Node.js and Angular CLI](https://nodejs.org/)
- SQLite (included with Entity Framework)

### 🔹 Repository Setup

```bash
# Clone the repository
git clone https://github.com/pedro-fontinele/my-transfer.git
cd my-transfer
```

### 🔹 Backend Configuration (.NET 7)

1. Navigate to backend directory:
```bash
cd backend
dotnet restore
```

2. Configure SQLite connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MyTransfer.db"
  },
  "SftpSettings": {
    "Host": "your-sftp-server.com",
    "Port": 22,
    "Username": "username",
    "Password": "password"
  }
}
```

3. Apply database migrations:
```bash
dotnet ef database update
```

4. Launch the API:
```bash
dotnet run
# API runs at http://localhost:5000
```

### 🔹 Frontend Setup (Angular + PO UI)

1. Navigate to frontend directory:
```bash
cd frontend
npm install
```

2. Start the application:
```bash
ng serve
# Interface accessible at http://localhost:4200
```

## 📂 Key Features

- 📤 **Secure File Upload** via SFTP
- 📥 **Seamless File Download**
- 📄 **Comprehensive File Listing**
- 🔐 **Robust Authentication**
- 📊 **Transfer Statistics Dashboard**

## 👥 Contribution Guidelines

Want to contribute? Great! Follow these steps:

1. **Fork** the repository
2. Create a feature branch:
```bash
git checkout -b feature/amazing-improvement
```
3. Commit changes:
```bash
git commit -m 'Add groundbreaking feature'
```
4. Push to your branch:
```bash
git push origin feature/amazing-improvement
```
5. Open a **Pull Request**

## 📝 Licensing

This project is licensed under the **MIT License**. 
See the [LICENSE](LICENSE) file for comprehensive details.

## 💡 Support & Contact

Encountering issues or have suggestions?
- Open an issue in the repository
- Email: [your-contact@email.com]
- GitHub: [@pedro-fontinele](https://github.com/pedro-fontinele)

---

**My Transfer** - Simplifying Secure File Transfers! 🚀🔒
