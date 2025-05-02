using ClassLibrary.HxH_Services.Infrastructure;
using ClassLibrary.HxH_Services.Shared.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClassLibrary.HxH_Services.Features.Hunters.GetAll;

public class GetAllHuntersHandler
{
    private readonly OracleDbContext _dbContext;
    private readonly ILogger<GetAllHuntersHandler> _logger;

    public GetAllHuntersHandler(OracleDbContext dbContext, ILogger<GetAllHuntersHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QueryResult<List<HunterDto>>> Handle(GetAllHuntersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("[GetAllHuntersHandler] Start handling GetAllHuntersQuery.");

            var hunters = new List<HunterDto>();
            var query = "SELECT Id_Hunter, Name, Age, Origin FROM Hunter";
            var dataTable = await _dbContext.ExecuteQueryAsync(cancellationToken, query);

            foreach (DataRow row in dataTable.Rows)
            {
                hunters.Add(HunterMapper.FromDataRow(row));
            }

            _logger.LogInformation("[GetAllHuntersHandler] Retrieved {Count} hunters.", hunters.Count);
            return QueryResult<List<HunterDto>>.Success("Ok", hunters);
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[GetAllHuntersHandler] Oracle error while handling GetAllHuntersQuery.");
            return QueryResult<List<HunterDto>>.Failure("Oracle error while retrieving hunters.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GetAllHuntersHandler] Error while handling GetAllHuntersQuery.");
            return QueryResult<List<HunterDto>>.Failure("Error while retrieving hunters.");
        }
    }
}
