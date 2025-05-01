using ClassLibrary.HxH_Services.Infrastructure;
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

    public async Task<HunterDto> Handle(GetHunterByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[GetHunterByIdHandler] Start handling GetHunterByIdQuery.");

        var query = "SELECT Id_Hunter, Name, Age, Origin FROM Hunter WHERE Id_Hunter =: Id_Hunter";
        var parameter = new OracleParameter("Id_Hunter", request.Id_Hunter);
        var dataTable = await _dbContext.ExecuteQueryAsync(cancellationToken, query, parameter);

        if (dataTable.Rows.Count == 0)
        {
            _logger.LogWarning("[GetHunterByIdHandler] No hunter found with Id: {Id_Hunter}", request.Id_Hunter);
            return null;
        }

        var hunter = HunterMapper.FromDataRow(dataTable.Rows[0]);

        _logger.LogInformation("[GetHunterByIdHandler] Retrieved hunter with Id: {Id_Hunter}", hunter.Id_Hunter);
        return hunter;
    }
}
