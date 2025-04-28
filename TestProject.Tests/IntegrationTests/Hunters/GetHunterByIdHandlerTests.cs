using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using ClassLibrary.HxH_Services.Features.Hunters.GetById;
using ClassLibrary.HxH_Services.Features.Hunters;

namespace TestProject.Tests.IntegrationTests.Hunters;

public class GetHunterByIdHandlerTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _dbLogger = NullLogger<OracleDbContext>.Instance;
    private readonly ILogger<GetHunterByIdHandler> _handlerLogger = NullLogger<GetHunterByIdHandler>.Instance;

    [Fact]
    public async Task Handle_ShouldReturn_HunterDto_WhenHunterExists()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new GetHunterByIdHandler(dbContext, _handlerLogger);
        var query = new GetHunterByIdQuery { Id_Hunter = 1 };
        var cancellationToken = CancellationToken.None;

        // Act
        var hunter = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(hunter);
        Assert.IsType<HunterDto>(hunter);
    }

    [Fact]
    public async Task Handle_ShouldReturn_Null_WhenHunterDoesNotExist()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new GetHunterByIdHandler(dbContext, _handlerLogger);
        var query = new GetHunterByIdQuery { Id_Hunter = 9999 };
        var cancellationToken = CancellationToken.None;

        // Act
        var hunter = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.Null(hunter);
    }
}
