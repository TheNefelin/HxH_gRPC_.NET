# HxH gRPC Microservices Vertical Slice Architecture

### Packages
- HxH.Services
```
Microsoft.Extensions.Logging.Abstractions
Oracle.ManagedDataAccess.Core
```

### Design Patterns
- **CQRS** : Command Query Responsibility Segregation
- **Commands** : Commands are used to change the state of the system.
- **Queries** : Queries are used to retrieve data from the system.
- **Handlers** : Handlers are used to process commands and queries.

### Vertical Slice Architecture
```mermaid
graph TD;
    Infrastructure --> Features
    Features --> Commands
    Commands --> Handlers
    Features --> Queries
    Queries --> Handlers
    Handlers --> Database
    ;
```

### Structure
```
/HxG_gRPC_.NETCore
│
├── ClassLibrary.HxH_Services/
│   ├── Features/
│   │   ├── Hunter_NenType/
│   │   │   └── ...
│   │   ├── Hunters/
│   │   │   ├── Create/
│   │   │   │   ├── CreateHunterCommand.cs
│   │   │   │   ├── CreateHunterHandler.cs
│   │   │   │   └── CreateHunterDto.cs
│   │   │   ├── Delete/
│   │   │   │   ├── DeleteHunterCommand.cs
│   │   │   │   └── DeleteHunterHandler.cs
│   │   │   ├── GetAll/
│   │   │   │   ├── GetAllHuntersHandler.cs
│   │   │   │   └── GetAllHuntersQuery.cs
│   │   │   ├── GetById/
│   │   │   │   ├── GetHunterByIdHandler.cs
│   │   │   │   └── GetHunterByIdQuery.cs
│   │   │   ├── Update/
│   │   │   │   ├── UpdateHunterCommand.cs
│   │   │   │   └── UpdateHunterHandler.cs
│   │   │   ├── HunterDto.cs
│   │   │   └── HunterMapper.cs
│   │   └── NenTypes/
│   │       └── ...
│   ├── Infrastructure
│   │   └── OracleDbContext.cs
│   └── Shared/
│       └── Common/
│           └── QueryResult.cs
│
└── TestProject.Tests/
    ├── IntegrationTests/
    │   ├── Hunters/
    │   │   ├──GetAllHuntersHandlerTests.cs
    │   │   └──GetHunterByIdHandlerTests.cs
    │   └── OracleDbContextIntegrationTests.cs
    └── UnitTests/
        └── notimplement.cs
```

