using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using ClassLibrary.HxH_Services.Features.Hunters.Update;

namespace TestProject.Tests.IntegrationTests.Hunters;

public class UpdateHunterHandlerTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _dbLogger = NullLogger<OracleDbContext>.Instance;
    private readonly ILogger<UpdateHunterHandler> _handlerLogger = NullLogger<UpdateHunterHandler>.Instance;

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new UpdateHunterHandler(dbContext, _handlerLogger);
        var command = new UpdateHunterCommand
        {
            Id_Hunter = 1,
            Name = "",
            Age = 30,
            Origin = "Test Origin"
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Name", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenOriginIsEmpty()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new UpdateHunterHandler(dbContext, _handlerLogger);
        var command = new UpdateHunterCommand
        {
            Id_Hunter = 1,
            Name = "Test Hunter",
            Age = 30,
            Origin = ""
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Origin", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAgeIsZeroOrNegative()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new UpdateHunterHandler(dbContext, _handlerLogger);
        var command = new UpdateHunterCommand
        {
            Id_Hunter = 1,
            Name = "Test Hunter",
            Age = 0,
            Origin = "Test Origin"
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Age", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterDoesNotExist()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new UpdateHunterHandler(dbContext, _handlerLogger);
        var command = new UpdateHunterCommand
        {
            Id_Hunter = 99999,
            Name = "Leorio Paladiknight",
            Age = 19,
            Origin = "Ciudad desconocida"
        }; // Assuming this ID does not exist
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Hunter does not exist", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHunterIsUpdated()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new UpdateHunterHandler(dbContext, _handlerLogger);
        var command = new UpdateHunterCommand
        {
            Id_Hunter = 4,
            Name = "Leorio Paladiknight",
            Age = 19,
            Origin = "Ciudad desconocida"
        };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hunter updated successfully", result.Message);
        Console.WriteLine(result.Message);
    }
}
