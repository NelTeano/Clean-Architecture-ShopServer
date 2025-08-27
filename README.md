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


## AutoMapper Setup

AutoMapper eliminates manual mapping between objects by automatically mapping properties with matching names. This reduces boilerplate code and prevents mapping errors, especially with complex nested objects.

### Why Use AutoMapper?
- **Reduces repetitive mapping code** - No need to manually assign each property
- **Prevents mapping errors** - Automatic property matching reduces human error
- **Handles nested objects** - Automatically maps complex object hierarchies
- **Improves maintainability** - Changes to DTOs/Entities require minimal code updates

### 1. Install AutoMapper Package

Install the AutoMapper NuGet package by Jimmy Bogard. For Clean Architecture, install only in your **Application layer**.

```bash
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### 2. Create Mapping Profiles

Mapping profiles define how objects should be mapped between different types. Create separate profiles for each domain area to keep mappings organized.

```csharp
namespace MyServer.Application.Mappings
{
    public class VariantProfile : Profile
    {
        public VariantProfile()
        {
            // Command DTO -> Entity (for incoming requests)
            CreateMap<CreateVariantDTO, VariantEntity>();
            CreateMap<CreateSubVariantDTO, SubVariantEntity>();
            CreateMap<CreateCategoryDTO, CategoryEntity>();
            CreateMap<CreateProductItemDTO, ProductItemEntity>();
            CreateMap<CreateProductSizeDTO, ProductSizeEntity>();

            // Entity -> Response DTO (for outgoing responses)
            CreateMap<VariantEntity, VariantResponseDto>();
            CreateMap<SubVariantEntity, SubVariantResponseDto>();
            CreateMap<CategoryEntity, CategoryResponseDto>();
            CreateMap<ProductItemEntity, ProductItemResponseDto>();
            CreateMap<ProductSizeEntity, ProductSizeResponseDto>();
        }
    }
}
```

### 3. Register AutoMapper in Dependency Injection

Register AutoMapper and your profiles in your Application layer's dependency injection setup.

```csharp
namespace MyServer.Application
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            // Register MediatR for CQRS pattern
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjector).Assembly);
            });

            // Register AutoMapper with profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<VariantProfile>();
                // Add more profiles as needed
            });

            return services;
        }
    }
}
```

### 4. Use AutoMapper in Your Code

Inject `IMapper` into your handlers, services, or repositories where mapping is needed.

#### With AutoMapper (Clean & Simple)
```csharp
namespace MyServer.Application.Commands.Variant
{
    public record CreateVariantCommand(CreateVariantDTO Dto) : IRequest<VariantResponseDto>;

    public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, VariantResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVariantCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<VariantResponseDto> Handle(CreateVariantCommand request, CancellationToken token)
        {
            // Map DTO to Entity with one line
            var entity = _mapper.Map<VariantEntity>(request.Dto);

            await _uow.Variant.Add(entity, token);
            await _uow.CommitAsync(token);

            // Map Entity back to Response DTO
            return _mapper.Map<VariantResponseDto>(entity);
        }
    }
}
```

#### Without AutoMapper (Manual & Error-Prone)
```csharp
public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, VariantResponseDto>
{
    private readonly IUnitOfWork _uow;

    public CreateVariantCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<VariantResponseDto> Handle(CreateVariantCommand request, CancellationToken token)
    {
        // Manual mapping - lots of code, prone to errors
        var entity = new VariantEntity
        {
            VariantName = request.Dto.VariantName,
            SubVariants = request.Dto.SubVariants.Select(sv => new SubVariantEntity
            {
                Name = sv.Name,
                Categories = sv.Categories.Select(c => new CategoryEntity
                {
                    Name = c.Name,
                    Items = c.Items.Select(i => new ProductItemEntity
                    {
                        Name = i.Name,
                        Image = i.Image,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        Sizes = i.Sizes.Select(s => new ProductSizeEntity
                        {
                            Size = s.Size,
                            Price = s.Price
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList()
        };

        await _uow.Variant.Add(entity, token);
        await _uow.CommitAsync(token);

        // Manual mapping for response - even more code!
        return new VariantResponseDto
        {
            Id = entity.Id,
            VariantName = entity.VariantName,
            SubVariants = entity.SubVariants.Select(sv => new SubVariantResponseDto
            {
                Id = sv.Id,
                Name = sv.Name,
                Categories = sv.Categories.Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Items = c.Items.Select(i => new ProductItemResponseDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Image = i.Image,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        Sizes = i.Sizes.Select(s => new ProductSizeResponseDto
                        {
                            Id = s.Id,
                            Size = s.Size,
                            Price = s.Price
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList()
        };
    }
}
```

---

## Serilog Logging Setup

Serilog provides structured logging with rich output formatting and multiple output targets (sinks). It's essential for debugging, monitoring, and troubleshooting applications in production.

### Why Use Serilog?
- **Structured logging** - Log data as structured objects, not just strings
- **Multiple outputs** - Write logs to console, files, databases, external services
- **Rich formatting** - Better readability and searchability
- **Performance** - Efficient logging with minimal overhead

### 1. Install Required Dependencies

```xml
<PackageReference Include="Serilog" Version="4.3.0" />
<PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
<PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="Serilog.Sinks.Seq" Version="9.0.0" />
```

### 2. Configure Logging in appsettings.json

Add Serilog configuration to control logging levels, output destinations, and enrichment.

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day"
        }
      },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341" }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
```

**Configuration Explanation:**
- **MinimumLevel**: Controls which log levels are recorded
- **WriteTo**: Defines where logs are sent (Console, File, Seq)
- **Enrich**: Adds additional context to logs (machine name, thread ID)
- **Seq**: External log server for advanced log analysis (optional)

### 3. Register Serilog in Program.cs

Configure Serilog as the primary logging provider for your application.

```csharp
using Serilog;

// Configure Serilog before building the app
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read from appsettings.json
    .Enrich.FromLogContext()
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

var app = builder.Build();

// Log HTTP requests automatically
app.UseSerilogRequestLogging();
```

### 4. Use Logging in Your Code

Inject `ILogger<T>` into your classes to add logging capabilities.

```csharp
namespace MyServer.Application.Commands.User
{
    public record AddUserCommand(UserEntity User) : IRequest<UserEntity>;

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserEntity>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(IUserRepository userRepository, ILogger<AddUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserEntity> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // Log structured data with {@} for object serialization
            _logger.LogInformation("Handling AddUserCommand for User: {@User}", request.User);

            var result = await _userRepository.AddUser(request.User, cancellationToken);

            // Log scalar values with {property}
            _logger.LogInformation("User successfully added with Id: {UserId}", result.Id);

            return result;
        }
    }
}
```

#### Sample Console Output
```console
2025-08-27 19:02:56.382 +08:00 [INF] Handling AddUserCommand for User: {"Id":"3fa85f64-5717-4562-b3fc-2c963f66afa6","Username":"angelica","Email":"angelica@example.com","FirstName":"angelica","LastName":"teano","PhoneNumber":"09156235126","EmailConfirmed":true,"IsActive":true,"CreatedOn":"2025-08-27T11:02:33.6220000Z","LastLogin":"2025-08-27T11:02:33.6220000Z","Role":"QA","$type":"UserEntity"}
2025-08-27 19:02:59.642 +08:00 [INF] User successfully added with Id: "ba297bc8-ee0c-4b2a-8810-b8cf34578438"
2025-08-27 19:02:59.677 +08:00 [INF] HTTP POST /api/User responded 200 in 4210.5965 ms
```

---

## Unit of Work Pattern

The Unit of Work pattern maintains a list of objects affected by a business transaction and coordinates writing out changes as a single transaction. It ensures data consistency across multiple repository operations.

### Why Use Unit of Work?
- **Transaction Management** - All operations succeed together or fail together
- **Data Consistency** - Prevents partial updates that leave data in inconsistent state
- **Performance** - Reduces database round trips by batching operations
- **Centralized Control** - Single point to manage all repository transactions

### 1. Create Unit of Work Interface

Define the contract for your Unit of Work with all repositories that need transactional support.

```csharp
namespace MyServer.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository Properties - Add all repositories that need transaction support
        IVariantRepository Variant { get; }
        ISubVariantRepository SubVariant { get; }
        ICategoryRepository Category { get; }
        IProductItemRepository ProductItem { get; }
        IProductSizeRepository ProductSize { get; }

        // Transaction Control
        Task<int> CommitAsync(CancellationToken token);
    }
}
```

### 2. Implement Unit of Work Class

The implementation coordinates all repositories and provides transaction control through Entity Framework's DbContext.

```csharp
namespace MyServer.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContextDB _context;

        // Repository instances
        public IVariantRepository Variant { get; }
        public ISubVariantRepository SubVariant { get; }
        public ICategoryRepository Category { get; }
        public IProductItemRepository ProductItem { get; }
        public IProductSizeRepository ProductSize { get; }

        public UnitOfWork(
            ApplicationContextDB context,
            IVariantRepository variantRepository,
            ISubVariantRepository subVariantRepository,
            ICategoryRepository categoryRepository,
            IProductItemRepository productItemRepository,
            IProductSizeRepository productSizeRepository
        )
        {
            _context = context;
            Variant = variantRepository;
            SubVariant = subVariantRepository;
            Category = categoryRepository;
            ProductItem = productItemRepository;
            ProductSize = productSizeRepository;
        }

        /// <summary>
        /// Commits all pending changes to the database as a single transaction
        /// </summary>
        public async Task<int> CommitAsync(CancellationToken token)
        {
            return await _context.SaveChangesAsync(token);
        }

        /// <summary>
        /// Disposes the database context
        /// </summary>
        public void Dispose() => _context.Dispose();
    }
}
```

### 3. Register Unit of Work in Dependency Injection

Register the Unit of Work along with all its dependent repositories. **Important:** All repositories must be registered before the Unit of Work.

```csharp
namespace MyServer.Infrastructure
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Register DbContext
            services.AddDbContext<ApplicationContextDB>(options =>
                options.UseSqlServer(connectionString));

            // Register all repositories FIRST (required by UnitOfWork)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IVariantRepository, VariantRepository>();
            services.AddScoped<ISubVariantRepository, SubVariantRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductItemRepository, ProductItemRepository>();
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();

            // Register Unit of Work (depends on repositories above)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register other services
            services.AddScoped<IPaymentService, StripePaymentService>();

            return services;
        }
    }
}
```

### 4. Use Unit of Work in Your Application

Replace individual repository injections with Unit of Work to ensure transactional consistency.

#### Using with CQRS Pattern + Repository Pattern
```csharp
namespace MyServer.Application.Commands.Variant
{
    public record CreateVariantCommand(CreateVariantDTO Dto) : IRequest<VariantResponseDto>;

    public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, VariantResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateVariantCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<VariantResponseDto> Handle(CreateVariantCommand request, CancellationToken token)
        {
            // All repository operations use the same context
            var entity = _mapper.Map<VariantEntity>(request.Dto);

            // Add to repository (not saved yet)
            await _uow.Variant.Add(entity, token);
            
            // Commit all changes as single transaction
            await _uow.CommitAsync(token);

            return _mapper.Map<VariantResponseDto>(entity);
        }
    }
}
```

#### Using with Repository Pattern Only
```csharp
namespace MyServer.Application.Services
{
    public class VariantService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public VariantService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<VariantResponseDto> CreateVariantAsync(CreateVariantDTO dto, CancellationToken token)
        {
            var entity = _mapper.Map<VariantEntity>(dto);

            // Multiple operations can be performed before committing
            await _uow.Variant.Add(entity, token);
            // await _uow.Category.Add(someCategory, token);
            // await _uow.ProductItem.Add(someItem, token);

            // All operations committed together
            await _uow.CommitAsync(token);

            return _mapper.Map<VariantResponseDto>(entity);
        }
    }
}
```

---

## Debug Tips

### JWT Authentication Issues
- **Verify JWT Headers**: Ensure your Angular client is sending JWT tokens in the Authorization header as `Bearer <token>`
- **Token Validation**: Check if the JWT token is valid and hasn't expired
- **Audience Configuration**: Verify the AUDIENCE URL in your client configuration matches your API settings

### Common Setup Issues
- **Dependency Order**: When registering services, ensure dependencies are registered before classes that depend on them
- **Missing Registrations**: All repositories must be registered in DI before registering Unit of Work
- **Connection Strings**: Verify database connection strings in appsettings.json are correct
- **Package Versions**: Ensure compatible versions of packages (check for major version conflicts)

### Best Practices
- **Test incrementally**: Implement and test each pattern separately before combining them
- **Use structured logging**: Always log important operations and their results
- **Handle exceptions**: Wrap repository operations in try-catch blocks and log exceptions
- **Follow naming conventions**: Use consistent naming across DTOs, Entities, and Response models

---

## Important Notes

- **Follow Clean Architecture**: Keep AutoMapper and business logic in the Application layer
- **Profile Organization**: Create separate mapping profiles for each domain area
- **Error Handling**: Always handle exceptions in command handlers and log them appropriately
- **Testing**: Test mapping configurations and Unit of Work transactions thoroughly
- **Performance**: Consider mapping performance for large object graphs
- **Seq Integration**: Use Seq for advanced log analysis and searching in development/staging environments