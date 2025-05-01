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

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            _logger.LogWarning("[CreateHunterHandler] Invalid Name: {Name}", command.Name);
            return QueryResult<int>.Failure("Invalid Name");
        }

        if (string.IsNullOrWhiteSpace(command.Origin))
        {
            _logger.LogWarning("[CreateHunterHandler] Invalid Origin: {Origin}", command.Origin);
            return QueryResult<int>.Failure("Invalid Origin");
        }

        if (command.Age <= 0)
        {
            _logger.LogWarning("[CreateHunterHandler] Invalid Age: {Age}", command.Age);
            return QueryResult<int>.Failure("Invalid Age");
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