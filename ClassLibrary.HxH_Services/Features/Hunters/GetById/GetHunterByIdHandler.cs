using ClassLibrary.HxH_Services.Infrastructure;
using ClassLibrary.HxH_Services.Shared.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace ClassLibrary.HxH_Services.Features.Hunters.GetById;

public class GetHunterByIdHandler
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<GetHunterByIdHandler> _logger;

    public GetHunterByIdHandler(OracleDbContext dbContext, ILogger<GetHunterByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QueryResult<HunterDto>> Handle(GetHunterByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GetHunterByIdHandler] Start handling GetHunterByIdQuery.");    

        var validationIdResult = HunterValidationId.ValidateId(request.Id_Hunter);
        if (validationIdResult != null)
        {
            _logger.LogWarning("[GetHunterByIdHandler] Validation failed: {ValidationResult}", validationIdResult);
            return QueryResult<HunterDto>.Failure(validationIdResult);
        }

        try
        {
            var parameter = new OracleParameter("Id_Hunter", request.Id_Hunter);
            var query = "SELECT Id_Hunter, Name, Age, Origin FROM Hunter WHERE Id_Hunter =: Id_Hunter";
            var dataTable = await _dbContext.ExecuteQueryAsync(cancellationToken, query, parameter);

            if (dataTable.Rows.Count == 0)
            {
                _logger.LogWarning("[GetHunterByIdHandler] No hunter found with Id: {Id_Hunter}", request.Id_Hunter);
                return QueryResult<HunterDto>.Failure("Hunter not found");
            }

            var hunter = HunterMapper.FromDataRow(dataTable.Rows[0]);

            _logger.LogInformation("[GetHunterByIdHandler] Retrieved hunter with Id: {Id_Hunter}", hunter.Id_Hunter);
            return QueryResult<HunterDto>.Success("Ok", hunter);
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[GetHunterByIdHandler] Database error occurred while retrieving the hunter with Id: {Id_Hunter}", request.Id_Hunter);
            return QueryResult<HunterDto>.Failure("Database error occurred while retrieving the hunter.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GetHunterByIdHandler] Error while retrieving hunter with Id: {Id_Hunter}", request.Id_Hunter);
            return QueryResult<HunterDto>.Failure("An error occurred while retrieving the hunter.");
        }
    }
}
