using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using ClassLibrary.HxH_Services.Features.Hunters.GetAll;
using ClassLibrary.HxH_Services.Features.Hunters;

namespace TestProject.Tests.IntegrationTests.Hunters;

public class GetAllHuntersHandlerTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _dbLogger = NullLogger<OracleDbContext>.Instance;
    private readonly ILogger<GetAllHuntersHandler> _handlerLogger = NullLogger<GetAllHuntersHandler>.Instance;

    [Fact]
    public async Task Handle_ShouldReturnListOfHunters()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _dbLogger);
        var handler = new GetAllHuntersHandler(dbContext, _handlerLogger);
        var query = new GetAllHuntersQuery();
        var cancellationToken = CancellationToken.None;

        // Act
        var hunters = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(hunters);
        Assert.IsType<List<HunterDto>>(hunters);
    }
}
