using ClassLibrary.HxH_Services.Infrastructure;
using Microsoft.Extensions.Logging;
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

    public async Task<List<HunterDto>> Handle(GetAllHuntersQuery request, CancellationToken cancellationToken)
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
        return hunters;
    }
}
