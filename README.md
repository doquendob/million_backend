# Million Real Estate Backend API

A .NET 8 Web API for managing real estate property listings. This backend provides RESTful endpoints for CRUD operations on properties and categories.

## 🚀 Features

- ✅ Full CRUD operations for Properties
- ✅ Category management
- ✅ CORS enabled for frontend integration
- ✅ Swagger/OpenAPI documentation
- ✅ Data validation with DTOs
- ✅ Error handling and logging
- ✅ Pre-seeded sample data

## 🛠️ Technology Stack

- **.NET 8.0** - Latest LTS version
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0** - ORM
- **In-Memory Database** - For development/demo
- **Swagger/Swashbuckle** - API documentation

## 📁 Project Structure

```
million_backend/
├── Controllers/
│   ├── PropertiesController.cs   # Properties CRUD endpoints
│   └── CategoriesController.cs   # Categories endpoints
├── Models/
│   ├── Property.cs               # Property entity
│   └── Category.cs               # Category entity
├── DTOs/
│   ├── PropertyInputDto.cs       # Request DTO with validation
│   └── PropertyResponseDto.cs    # Response DTO
├── Data/
│   └── ApplicationDbContext.cs   # EF Core DbContext with seed data
├── Program.cs                    # Application configuration
├── appsettings.json             # Configuration
├── appsettings.Development.json # Dev configuration
├── MillionBackend.csproj        # Project file
└── README.md                    # This file
```

## 🔧 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor (Visual Studio 2022, VS Code, or Rider recommended)
- Terminal/Command Prompt

### Verify .NET Installation

```bash
dotnet --version
# Should show 8.0.x or higher
```

## 📦 Local Setup Instructions

### Step 1: Navigate to the Backend Directory

```bash
cd /Users/doquendob/Documents/million_backend
```

### Step 2: Restore Dependencies

```bash
dotnet restore
```

This will download all required NuGet packages specified in `MillionBackend.csproj`.

### Step 3: Build the Project

```bash
dotnet build
```

Ensure there are no build errors.

### Step 4: Run the Application

```bash
dotnet run
```

You should see output similar to:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Step 5: Access the API

Once running, you can access:

- **Swagger UI**: http://localhost:5000 (Interactive API documentation)
- **API Base URL**: http://localhost:5000/api
- **HTTPS**: https://localhost:5001 (if enabled)

## 📡 API Endpoints

### Health Check

```
GET /api/health
```

Returns API status and timestamp.

### Properties

| Method | Endpoint               | Description         |
| ------ | ---------------------- | ------------------- |
| GET    | `/api/properties`      | Get all properties  |
| GET    | `/api/properties/{id}` | Get property by ID  |
| POST   | `/api/properties`      | Create new property |
| PUT    | `/api/properties/{id}` | Update property     |
| DELETE | `/api/properties/{id}` | Delete property     |

### Categories

| Method | Endpoint               | Description        |
| ------ | ---------------------- | ------------------ |
| GET    | `/api/categories`      | Get all categories |
| GET    | `/api/categories/{id}` | Get category by ID |

## 🧪 Testing the API

### Using Swagger UI (Recommended)

1. Navigate to http://localhost:5000
2. Expand any endpoint
3. Click "Try it out"
4. Fill in the required fields
5. Click "Execute"

### Using curl

**Get all properties:**

```bash
curl http://localhost:5000/api/properties
```

**Get property by ID:**

```bash
curl http://localhost:5000/api/properties/prop-001
```

**Create a new property:**

```bash
curl -X POST http://localhost:5000/api/properties \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Property",
    "description": "A beautiful test property",
    "addressProperty": "123 Test Street, Test City, TC 12345",
    "type": "House",
    "priceProperty": 500000,
    "imageUrl": "https://example.com/image.jpg",
    "active": true
  }'
```

**Update a property:**

```bash
curl -X PUT http://localhost:5000/api/properties/prop-001 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Property Name",
    "description": "Updated description",
    "addressProperty": "123 Test Street, Test City, TC 12345",
    "type": "House",
    "priceProperty": 550000,
    "active": true
  }'
```

**Delete a property:**

```bash
curl -X DELETE http://localhost:5000/api/properties/prop-001
```

**Get all categories:**

```bash
curl http://localhost:5000/api/categories
```

## 🔌 Frontend Integration

### Update Frontend Environment Variables

In your frontend project (`million_frontend`):

1. Create or update `.env.local`:

```bash
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```

2. Start the frontend:

```bash
cd /Users/doquendob/Documents/million_frontend
npm run dev
```

3. Access the frontend at http://localhost:3000

The frontend will now communicate with the backend API!

## 📊 Sample Data

The API comes pre-seeded with:

- **5 Categories**: House, Apartment, Villa, Townhouse, Estate
- **5 Sample Properties**: Various property types with different prices

The data is automatically loaded when the application starts (In-Memory database).

## 🔒 CORS Configuration

CORS is configured to allow requests from:

- `http://localhost:3000` (Next.js default)
- `http://localhost:3001`
- `https://localhost:3000`

To add more origins, edit `Program.cs`:

```csharp
policy.WithOrigins(
    "http://localhost:3000",
    "http://localhost:3001",
    "https://your-domain.com"
)
```

## 🐛 Troubleshooting

### Port Already in Use

If port 5000 is already in use, you can change it in `Properties/launchSettings.json` or run:

```bash
dotnet run --urls "http://localhost:5500"
```

### CORS Errors

If you get CORS errors:

1. Ensure the backend is running before the frontend
2. Check that frontend URL is in the CORS policy in `Program.cs`
3. Restart both frontend and backend

### Build Errors

If you encounter build errors:

```bash
# Clean the project
dotnet clean

# Restore packages
dotnet restore

# Build again
dotnet build
```

## 🚀 Running in Production

For production deployment:

1. **Use a real database** (SQL Server, PostgreSQL, etc.)

   - Replace `UseInMemoryDatabase` with your database provider in `Program.cs`
   - Add connection string to `appsettings.json`

2. **Configure production CORS**

   - Update CORS policy with your production frontend URL

3. **Enable HTTPS**

   - Configure SSL certificates
   - Update frontend to use HTTPS endpoint

4. **Publish the application**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

## 📚 Additional Commands

```bash
# Watch mode (auto-restart on changes)
dotnet watch run

# Run specific configuration
dotnet run --configuration Release

# List available endpoints
dotnet run --urls "http://localhost:5000" --list-endpoints

# Check for outdated packages
dotnet list package --outdated
```

## 🔄 Database Migration (Future)

When switching to a real database:

```bash
# Add migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## 📝 API Response Examples

### Success Response (Get Properties)

```json
[
  {
    "id": "prop-001",
    "name": "Modern Downtown Loft",
    "description": "Stunning contemporary loft...",
    "addressProperty": "245 Market Street, San Francisco, CA 94102",
    "type": "Apartment",
    "priceProperty": 1250000,
    "imageUrl": "https://images.unsplash.com/photo-...",
    "active": true,
    "createdAt": "2025-11-15T10:30:00Z",
    "idOwner": "owner-123"
  }
]
```

### Error Response

```json
{
  "message": "Property with ID 'invalid-id' not found"
}
```

### Validation Error Response

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["Name is required"],
    "PriceProperty": ["Price must be a positive value"]
  }
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is for educational purposes.

## 🆘 Support

If you encounter any issues:

1. Check the console output for detailed error messages
2. Verify all prerequisites are installed
3. Review the Troubleshooting section
4. Check Swagger UI at http://localhost:5000 for API documentation

## 🎯 Next Steps

- [ ] Add authentication/authorization
- [ ] Implement filtering and pagination
- [ ] Add image upload functionality
- [ ] Switch to persistent database (SQL Server/PostgreSQL)
- [ ] Add unit and integration tests
- [ ] Implement caching
- [ ] Add rate limiting
- [ ] Deploy to Azure/AWS

---

**Happy Coding! 🚀**
