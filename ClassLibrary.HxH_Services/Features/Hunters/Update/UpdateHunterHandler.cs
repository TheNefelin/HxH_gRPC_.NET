using ClassLibrary.HxH_Services.Infrastructure;
using ClassLibrary.HxH_Services.Shared.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Reflection.Metadata;

namespace ClassLibrary.HxH_Services.Features.Hunters.Update;

public class UpdateHunterHandler
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<UpdateHunterHandler> _logger;

    public UpdateHunterHandler(OracleDbContext dbContext, ILogger<UpdateHunterHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QueryResult<bool>> Handle(UpdateHunterCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[UpdateHunterHandler] Start handling UpdateHunterCommand.");

        var validateIdResult = HunterValidationId.ValidateId(command.Id_Hunter);
        if (validateIdResult != null)
        {
            _logger.LogWarning("[UpdateHunterHandler] Validation failed: {ValidationResult}", validateIdResult);
            return QueryResult<bool>.Failure(validateIdResult);
        }

        var validatetionResult = HunterValidationRules.Validate(command.Name, command.Age, command.Origin);
        if (validatetionResult != null)
        {
            _logger.LogWarning("[CreateHunterHandler] Validation failed: {ValidationResult}", validatetionResult);
            return QueryResult<bool>.Failure(validatetionResult);
        }

        try
        {
            var checkParameters = new OracleParameter("Id_Hunter", command.Id_Hunter);

            var checkExistsQuery = "SELECT COUNT(1) FROM Hunter WHERE Id_Hunter = :Id_Hunter";
            var rowCount = await _dbContext.ExecuteScalarAsync<int>(cancellationToken, checkExistsQuery, checkParameters);

            if (rowCount == 0)
            {
                _logger.LogWarning("[DeleteHunterByIdHandler] Hunter does not exist with Id: {Id_Hunter}", command.Id_Hunter);
                return QueryResult<bool>.Failure("Hunter does not exist");
            }

            var updateParameters = new OracleParameter[]
            {
                new OracleParameter("Name", command.Name),
                new OracleParameter("Age", command.Age),
                new OracleParameter("Origin", command.Origin),
                new OracleParameter("Id_Hunter", command.Id_Hunter),
            };  

            var updateQuery = "UPDATE Hunter SET Name = :Name, Age = :Age, Origin = :Origin WHERE Id_Hunter = :Id_Hunter";
            var affectedRows = await _dbContext.ExecuteNonQueryAsync(cancellationToken, updateQuery, updateParameters);

            _logger.LogInformation("[UpdateHunterHandler] Updated {AffectedRows} rows.", affectedRows);
            return QueryResult<bool>.Success("Hunter updated successfully", affectedRows > 0);
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[UpdateHunterHandler] Oracle error while updating hunter.");
            return QueryResult<bool>.Failure("Oracle error while updating hunter");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[UpdateHunterHandler] Error while updating hunter.");
            return QueryResult<bool>.Failure("Error while updating hunter");
        }
    }
}