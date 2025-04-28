using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.Tests.IntegrationTests;

public class OracleDbContextIntegrationTests
{
    private readonly string _realConnectionString = ConnectionString.GetOracleDBConnectionString;
    private readonly ILogger<OracleDbContext> _logger = NullLogger<OracleDbContext>.Instance;

    [Fact]
    public async Task ExecuteNonQueryAsync_CancellationToken_ShouldCancell()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var query = "BEGIN NULL; END;";
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await dbContext.ExecuteNonQueryAsync(cancellationToken, query);
        });
    }

    [Fact]
    public async Task ExecuteNonQueryAsync_ShouldReturn_SuccessResult()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cancellationToken = CancellationToken.None;
        var query = "BEGIN NULL; END;";

        // Act
        var affectedRows = await dbContext.ExecuteNonQueryAsync(cancellationToken, query);

        // Assert
        Assert.True(affectedRows >= -1);
    }

    [Fact]
    public async Task ExecuteQueryAsync_CancellationToken_ShouldCancell()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var query = "SELECT banner FROM v$version WHERE ROWNUM = 1";

        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await dbContext.ExecuteQueryAsync(cancellationToken, query);
        });
    }

    [Fact]
    public async Task ExecuteQueryAsync_ShouldReturn_DataTable()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cancellationToken = CancellationToken.None;
        var query = "SELECT banner FROM v$version WHERE ROWNUM = 1";

        // Act
        var result = await dbContext.ExecuteQueryAsync(cancellationToken, query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Rows.Count > 0);
    }

    [Fact]
    public async Task ExecuteScalarAsync_CancellationToken_ShouldCancell()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var query = "SELECT banner FROM v$version WHERE ROWNUM = 1";
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await dbContext.ExecuteScalarAsync<string>(cancellationToken, query);
        });
    }

    [Fact]
    public async Task ExecuteScalarAsync_ShouldReturn_VersionString()
    {
        // Arrange
        var dbContext = new OracleDbContext(_realConnectionString, _logger);
        var cancellationToken = CancellationToken.None;
        var query = "SELECT banner FROM v$version WHERE ROWNUM = 1";

        // Act
        var result = await dbContext.ExecuteScalarAsync<string>(cancellationToken, query);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(result));
    }
}
