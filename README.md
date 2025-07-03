# Clean Architecture Setup Guide

## Project Structure

```
MySolution/
├── MyServer.API/            ← Startup project
├── MyServer.Application/    ← Business rules, interfaces
├── MyServer.Domain/         ← Entities (pure models) // it set to identified either Core or Domain
└── MyServer.Infrastructure/ ← EF Core, Repositories, Migrations
```

## Setting Up Clean Architecture

### 1. Create .NET Web API Project

Start by creating a new .NET Web API project as your main startup project.

### 2. Add Class Library Projects

**In the ROOT folder of the project**, add new project type **CLASS LIBRARIES** for:
- Infrastructure
- Application
- Core

**Note:** The name should follow the pattern `<ProjectName>.<Layer>` (e.g., `MyServer.Infrastructure`). **Also remove the class file that auto-generated after adding the project.**

### 3. Create DependencyInjector Classes

Create a class file named `DependencyInjector` in each layer (Infrastructure, Application, Core, and API).

#### Example Code for Layers:

```csharp
namespace MyServer.Core
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services)
        {
            // Register application services here
            // Example: services.AddTransient<IMyService, MyService>();

            return services;
        }
    }
}
```

#### Example for API (root app project):

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI(configuration);
        return services;
    }
}
```

### 4. Install Required NuGet Packages

Install the following packages using NuGet:
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.DependencyInjection.Abstraction`

### 5. Register Services in Program.cs

Add the following line to your `Program.cs`:

```csharp
builder.Services.AddAppDI(builder.Configuration); // Register application services
```

### 6. Add Project References

**Add the following project references:**
- **In API LAYER:** Add Infrastructure and Application
- **In APPLICATION LAYER:** Add Core
- **In INFRASTRUCTURE LAYER:** Add Application
- **In CORE LAYER:** No references added

## Database Setup with Microsoft SQL Server

### 1. Install Required Software

Install the following software:
- **Microsoft SQL Server** (from Microsoft Website)
- **SSMS** (SQL Server Management Studio - available through Visual Studio Installer from Microsoft Website)

Set them up properly and get the connection string.

**Note:** **Make sure you installed them properly to prevent connection problems.**

### 2. Install Required NuGet Packages

Install the following dependencies using NuGet:
- EF Core Design
- EF Core SQL Server
- EF Core Tools

### 3. Create Data Models

Create data models and store them in an `Entities` folder that you will create in the **CORE/DOMAIN layer**.

### 4. Setup and Create DB Context

Create a DB Context and register the data models you created at the same time.

#### Example Code:

```csharp
namespace MyServer.Infrastructure.Data
{
    public class ApplicationContextDB(DbContextOptions<ApplicationContextDB> options) : DbContext(options)
    {
        public DbSet<UserEntity> User { get; set; }
    }
}
```

### 5. Register DB Context in Infrastructure Layer

Register the DB Context as a service in the `DependencyInjector.cs` of the Infrastructure Layer.

#### Example Code:

```csharp
public static class DependencyInjector
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationContextDB>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
```

### 6. Add Connection String to appsettings.json

**In the API LAYER**, add your connection string to `appsettings.json`:

#### Example Code:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyServerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 7. Run Migrations

**Go to Tools and run using Package Manager Console:**

```
Add-Migration dbinit
```
*The `dbinit` could be any name - it's just a description for your migration*

```
Update-Database
```
*This updates the database. After this command, you should see a DB generated in SSMS that your application initialized*

**Note:** **If the migration succeeds without errors, you can proceed to Update. If not, run `Remove-Migration` to reconfigure the problem.**

## Auth0 Setup with .NET JWT and Angular

### 1. Create API in Auth0 Dashboard

Create an API in your Auth0 Dashboard (you'll connect it to .NET later).
- **Set Audience URL matching to your frontend** (will be used soon in frontend for credentials to recognize the API service)

### 2. Setup .NET for Authentication

#### Install Required Package:
- `Microsoft.AspNetCore.Authentication.JwtBearer`

#### Add to appsettings.json:

```json
"Auth0": {
  "Domain": "dev-m051ksckbf8nu4k3.us.auth0.com",
  "Audience": "https://localhost:7251/api"
}
```

#### Setup JWT Code:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = domain,
        ValidAudience = builder.Configuration["Auth0:Audience"],
        NameClaimType = ClaimTypes.NameIdentifier,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
```

### 3. Test Authorization

- Test the Authorization Request using Swagger
- **Make sure to set AUTHORIZATION requirement in your CONTROLLER**
- **Insert valid JWT Token from your Auth0 dashboard application Test Tab to be authorized**
- **Note:** Use `Bearer <token>` format - you should be able to use the authorized endpoints if you're using a valid token

### 4. Setup Angular Frontend

#### Create SPA in Auth0 Dashboard:
- Create SPA in the application tab of your Auth0 Dashboard
- **Make sure to setup the Allowed CALLBACK, LOGOUT, ORIGINS with your valid URLs**

#### Setup Angular Configuration:
Read and setup the Quickstart. **Use your domain and clientId from your SPA and the audience is the one your API is using that you set earlier.**

**Make sure to setup properly - especially the App and AppConfig should be in it at the same time:**

```typescript
bootstrapApplication(App, {
  ...appConfig,
  providers: [
    ...(appConfig.providers || []),
    provideAuth0({
      domain: 'dev-m051ksckbf8nu4k3.us.auth0.com',
      clientId: 'BAoDhesJuj1986ZoA2IAq5UIGE3UisPh',
      authorizationParams: {
        audience: 'https://localhost:7251/api', 
        redirect_uri: window.location.origin
      }
    })
  ]
});
```

### 5. Create Auth Components

- Create an Auth Button Component - you should be able to redirect and make a login
- Setup other functionalities like LOGOUT and AUTH GUARD
- Try fetching from your .NET Web API - you should be able to fetch if you did everything properly

#### Debug Tips:
- **Check if your Angular is using JWT Bearer Headers when requesting**
- **Check if it's using the valid JWT that your API created**
- **Check AUDIENCE URL in Main.ts if it's not working properly**

---

## Important Notes

- Follow the exact project structure and naming conventions
- Ensure all dependencies are properly installed
- Test each step before proceeding to the next
- Pay special attention to the highlighted sections for critical configuration points