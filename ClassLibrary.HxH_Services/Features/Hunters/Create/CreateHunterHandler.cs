using ClassLibrary.HxH_Services.Infrastructure;
using ClassLibrary.HxH_Services.Shared.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace ClassLibrary.HxH_Services.Features.Hunters.Create;

public class CreateHunterHandler
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<CreateHunterHandler> _logger;

    public CreateHunterHandler(OracleDbContext dbContext, ILogger<CreateHunterHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QueryResult<int>> Handle(CreateHunterCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[CreateHunterHandler] Start handling CreateHunterCommand.");

        var validatetionResult = HunterValidationRules.Validate(command.Name, command.Age, command.Origin);
        if (validatetionResult != null)
        {
            _logger.LogWarning("[CreateHunterHandler] Validation failed: {ValidationResult}", validatetionResult);
            return QueryResult<int>.Failure(validatetionResult);
        }

        try
        {
            var idHunterParam = new OracleParameter("Id_Hunter", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new OracleParameter[]
            {
                new OracleParameter("Name", command.Name),
                new OracleParameter("Age", command.Age),
                new OracleParameter("Origin", command.Origin),
                idHunterParam
            };

            var insertQuery = "INSERT INTO Hunter (Name, Age, Origin) VALUES (:Name, :Age, :Origin) RETURNING Id_Hunter INTO :Id_Hunter";

            await _dbContext.ExecuteNonQueryAsync(cancellationToken, insertQuery, parameters);

            var newIdHunter = Convert.ToInt32(((OracleDecimal)idHunterParam.Value).ToInt32());

            if (newIdHunter == -1)
            {
                _logger.LogError("[CreateHunterHandler] Failed to retrieve the new Hunter Id.");
                return QueryResult<int>.Failure("Failed to retrieve the new Hunter Id.");
            }

            _logger.LogInformation("[CreateHunterHandler] Created Hunter with Id: {Id_Hunter}.", newIdHunter);
            return QueryResult<int>.Success("Hunter successfully created", newIdHunter);
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[CreateHunterHandler] Database error occurred while creating the hunter.");
            return QueryResult<int>.Failure("Database error occurred while creating the hunter");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CreateHunterHandler] Unexpected error occurred while creating the hunter.");
            return QueryResult<int>.Failure("An unexpected error occurred while creating the hunter");
        }
    }
}