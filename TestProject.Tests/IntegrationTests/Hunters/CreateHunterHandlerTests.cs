using ClassLibrary.HxH_Services.Features.Hunters.Create;
using ClassLibrary.HxH_Services.Features.Hunters.Delete;
using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.Tests.IntegrationTests.Hunters;

public class CreateHunterHandlerTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _dbLogger = NullLogger<OracleDbContext>.Instance;
    private readonly ILogger<CreateHunterHandler> _handlerLogger = NullLogger<CreateHunterHandler>.Instance;
    private readonly ILogger<DeleteHunterByIdHandler> _deleteHandlerLogger = NullLogger<DeleteHunterByIdHandler>.Instance;

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterHaveInvalidAge()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new CreateHunterHandler(dbContext, _handlerLogger);
        var command = new CreateHunterCommand
        {
            Name = "Gon Freecss",
            Age = -1,
            Origin = "Japan",
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Age", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterHaveInvalidName()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new CreateHunterHandler(dbContext, _handlerLogger);
        var command = new CreateHunterCommand
        {
            Name = "",
            Age = 14,
            Origin = "Japan",
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Name", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterHaveInvalidOrigin()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new CreateHunterHandler(dbContext, _handlerLogger);
        var command = new CreateHunterCommand
        {
            Name = "Gon Freecss",
            Age = 14,
            Origin = "",
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Origin", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHunterIsCreated()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new CreateHunterHandler(dbContext, _handlerLogger);
        var command = new CreateHunterCommand
        {
            Name = "Gon Freecss",
            Age = 14,
            Origin = "Japan",
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hunter successfully created", result.Message);
        // Clean up
        var createdHunterId = result.Data;
        var deleteCommand = new DeleteHunterByIdCommand
        {
            Id_Hunter = createdHunterId,
        };
        var deleteHandler = new DeleteHunterByIdHandler(dbContext, _deleteHandlerLogger);
        var deleteResult = await deleteHandler.Handle(deleteCommand, cancellationToken);
        Assert.True(deleteResult.IsSuccess);
    }
}
