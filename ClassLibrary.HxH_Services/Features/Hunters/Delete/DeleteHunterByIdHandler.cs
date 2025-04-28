using ClassLibrary.HxH_Services.Infrastructure;
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

    public async Task<bool> Handle(DeleteHunterByIdCommand command, CancellationToken cancellationToken)
    {
        var query = "DELETE FROM HUNTERS WHERE ID_HUNTER = :IdHunter";
        var parameters = new OracleParameter[]
        {
            new OracleParameter("IdHunter", command.Id_Hunter)
        };

        var affectedRows = await _dbContext.ExecuteNonQueryAsync(cancellationToken, query, parameters);
        return affectedRows > 0;
    }
}
