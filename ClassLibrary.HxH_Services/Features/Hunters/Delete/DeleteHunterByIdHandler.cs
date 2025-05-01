using ClassLibrary.HxH_Services.Infrastructure;
using ClassLibrary.HxH_Services.Shared.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace ClassLibrary.HxH_Services.Features.Hunters.Delete;

public class DeleteHunterByIdHandler
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<DeleteHunterByIdHandler> _logger;

    public DeleteHunterByIdHandler(OracleDbContext dbContext, ILogger<DeleteHunterByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QueryResult<int>> Handle(DeleteHunterByIdCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[DeleteHunterByIdHandler] Start handling DeleteHunterByIdCommand.");

        if (command.Id_Hunter <= 0)
        {
            _logger.LogWarning("[DeleteHunterByIdHandler] Invalid Id_Hunter: {Id_Hunter}", command.Id_Hunter);
            return QueryResult<int>.Failure("Invalid Id_Hunter");
        }

        try
        {
            var parameters = new OracleParameter[]
            {
                new OracleParameter("Id_Hunter", command.Id_Hunter),
            };

            var checkExistsQuery = "SELECT COUNT(1) FROM Hunter WHERE Id_Hunter =: Id_Hunter";
            var rowCount = await _dbContext.ExecuteScalarAsync<int>(cancellationToken, checkExistsQuery, parameters);

            if (rowCount == 0)
            {
                _logger.LogWarning("[DeleteHunterByIdHandler] Hunter does not exist with Id: {Id_Hunter}", command.Id_Hunter);
                return QueryResult<int>.Failure("Hunter does not exist");
            }

            var checkDependecyQuery = "SELECT COUNT(1) FROM hunter_Nen WHERE Id_Hunter = :Id_Hunter";
            var dependencyCount = await _dbContext.ExecuteScalarAsync<int>(cancellationToken, checkDependecyQuery, parameters);

            if (dependencyCount > 0)
            {
                _logger.LogWarning("[DeleteHunterByIdHandler] Hunter cannot be deleted due to existing dependencies with Id: {Id_Hunter}", command.Id_Hunter);
                return QueryResult<int>.Failure("This hunter has dependencies in Hunter_Nen");
            }

            var deleteQuery = "DELETE FROM Hunter WHERE Id_Hunter = :Id_Hunter";
            var affectedRows = await _dbContext.ExecuteNonQueryAsync(cancellationToken, deleteQuery, parameters);

            _logger.LogInformation("[DeleteHunterByIdHandler] Deleted {AffectedRows} rows.", affectedRows);
            return QueryResult<int>.Success("Hunter deleted successfully", affectedRows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[DeleteHunterByIdHandler] Unexpected error while deleting hunter with Id: {Id_Hunter}", command.Id_Hunter);
            return QueryResult<int>.Failure("An unexpected error occurred while deleting the hunter.");
        }
    }
}
