using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using ClassLibrary.HxH_Services.Features.Hunters.Delete;

namespace TestProject.Tests.IntegrationTests.Hunters;

public class DeleteHunterByIdHandlerTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _dbLogger = NullLogger<OracleDbContext>.Instance;
    private readonly ILogger<DeleteHunterByIdHandler> _handlerLogger = NullLogger<DeleteHunterByIdHandler>.Instance;

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterDoesNotExist()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new DeleteHunterByIdHandler(dbContext, _handlerLogger);
        var command = new DeleteHunterByIdCommand { Id_Hunter = 99999 }; // Assuming this ID does not exist
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Hunter does not exist", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHunterHasDependencies()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new DeleteHunterByIdHandler(dbContext, _handlerLogger);
        var command = new DeleteHunterByIdCommand { Id_Hunter = 2 }; // Assuming this ID has dependencies
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("This hunter has dependencies in Hunter_Nen", result.Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenIdIsInvalid()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new DeleteHunterByIdHandler(dbContext, _handlerLogger);
        var command = new DeleteHunterByIdCommand { Id_Hunter = -1 };
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(command, cancellationToken);
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid Id_Hunter", result.Message);
    }

    //[Fact]
    //public async Task Handle_ShouldReturnSuccess_WhenHunterExistsAndNoDependencies()
    //{
    //    // Arrange
    //    var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
    //    var handler = new DeleteHunterByIdHandler(dbContext, _handlerLogger);
    //    var command = new DeleteHunterByIdCommand { Id_Hunter = 22 }; // Assuming 1 is a valid Id_Hunter
    //    var cancellationToken = CancellationToken.None;
    //    // Act
    //    var result = await handler.Handle(command, cancellationToken);
    //    // Assert
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal("Hunter deleted successfully", result.Message);
    //}
}
