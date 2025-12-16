# Manual Run Guide - Auto Ballooning IES Application

## Prerequisites

### Required Software
1. **.NET 8.0 SDK** - Download from https://dotnet.microsoft.com/download/dotnet/8.0
2. **Node.js** (v16 or higher) - Download from https://nodejs.org/
3. **SQL Server** (Express or higher) - For database
4. **Visual Studio 2022** or **Visual Studio Code** (Optional but recommended)

### Database Setup
1. Create a SQL Server database named `DIM_RECORDING_USR` (or update connection string)
2. Run the database scripts from the `database/` folder
3. Ensure SQL Server is running and accessible

## Step-by-Step Instructions

### Step 1: Configure Database Connection

1. Open `appsettings.json` in the root of `BubbleDrawingAutomationWeb` folder
2. Update the `MPMConnStr` connection string:
   ```json
   "MPMConnStr": "Server=YOUR_SERVER_NAME;Database=DIM_RECORDING_USR;Trusted_Connection=True;TrustServerCertificate=Yes;"
   ```
   Or with SQL Authentication:
   ```json
   "MPMConnStr": "Server=YOUR_SERVER_NAME;Database=DIM_RECORDING_USR;User ID=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=Yes;"
   ```

### Step 2: Install Backend Dependencies

1. Open PowerShell or Command Prompt
2. Navigate to the project root:
   ```bash
   cd IES_server_package\BubbleDrawingAutomationWeb
   ```
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

### Step 3: Install Frontend Dependencies

1. Navigate to the ClientApp folder:
   ```bash
   cd ClientApp
   ```
2. Install npm packages:
   ```bash
   npm install
   ```
   **Note:** This may take several minutes on first run

### Step 4: Configure Frontend Environment (Optional)

If you need to configure frontend environment variables, create a `.env` file in `ClientApp` folder:
```
REACT_APP_SERVER=http://localhost:44436
REACT_APP_API_URL=http://localhost:44436/api
```

### Step 5: Build the Application

#### Option A: Build from Command Line

1. **Build Backend:**
   ```bash
   cd IES_server_package\BubbleDrawingAutomationWeb
   dotnet build
   ```

2. **Build Frontend:**
   ```bash
   cd ClientApp
   npm run build
   ```

#### Option B: Build from Visual Studio

1. Open `BubbleDrawingAutomationWeb.sln` in Visual Studio
2. Right-click solution → **Restore NuGet Packages**
3. Build → **Build Solution** (Ctrl+Shift+B)

### Step 6: Run the Application

#### Option A: Run from Command Line (Recommended for Development)

1. **Run Backend:**
   ```bash
   cd IES_server_package\BubbleDrawingAutomationWeb
   dotnet run
   ```
   The backend will start on `http://localhost:44436` (or port specified in launchSettings.json)

2. **Run Frontend (in a separate terminal):**
   ```bash
   cd IES_server_package\BubbleDrawingAutomationWeb\ClientApp
   npm start
   ```
   The frontend will start on `http://localhost:3000` (or next available port)

#### Option B: Run from Visual Studio

1. Set `BubbleDrawingAutomationWeb` as the startup project
2. Press **F5** or click **Run**
3. Visual Studio will automatically:
   - Start the backend API
   - Launch the frontend development server
   - Open the browser

### Step 7: Access the Application

1. Open your browser
2. Navigate to: `http://localhost:3000` (or the port shown in terminal)
3. You should see the login page

### Step 8: First-Time Setup

1. **Login:** Use your configured user credentials
2. **Configure Database Connections:**
   - Go to Admin/Configuration
   - Set up MPM, QDMS, and CWI database connections
   - Configure drawing folder paths

## Troubleshooting

### Common Issues

1. **Port Already in Use:**
   - Change port in `launchSettings.json` or `Properties/launchSettings.json`
   - Or kill the process using the port:
     ```bash
     netstat -ano | findstr :44436
     taskkill /PID <PID> /F
     ```

2. **Database Connection Failed:**
   - Verify SQL Server is running
   - Check connection string in `appsettings.json`
   - Ensure database exists
   - Check Windows Authentication or SQL Authentication credentials

3. **npm install fails:**
   - Clear npm cache: `npm cache clean --force`
   - Delete `node_modules` folder and `package-lock.json`
   - Run `npm install` again

4. **Frontend not loading:**
   - Check if backend API is running
   - Verify proxy settings in `package.json` match backend port
   - Check browser console for errors

5. **OCR/Tesseract errors:**
   - Ensure `tessdata` folder exists in project root
   - Verify Tesseract language files are present

### Development vs Production

- **Development:** Uses `appsettings.Development.json` and runs on localhost
- **Production:** Uses `appsettings.json` and requires proper CORS configuration

## Project Structure

```
BubbleDrawingAutomationWeb/
├── Controllers/          # API Controllers
├── BusinessLogic/        # Business logic layer
├── DataAccess/          # Data access layer
├── Models/              # Entity Framework models
├── ClientApp/           # React frontend
│   ├── src/
│   │   ├── components/  # React components
│   │   ├── Store/       # State management
│   │   └── App.js       # Main app component
│   └── package.json     # Frontend dependencies
├── appsettings.json     # Production configuration
└── Program.cs           # Application entry point
```

## Additional Notes

- The application uses **session-based authentication**
- Drawing images should be placed in `ClientApp/src/drawing/` for development
- Error logs are written to `ErrorLog/` folder
- The application supports multi-page drawings (TIFF, PDF, PNG)

## Support

For issues or questions, check:
- Database connection strings
- File paths for drawings
- User permissions in database
- CORS configuration for production deployment











