using ClassLibrary.HxH_Services.Features.Hunters.GetAll;
using Grpc.Core;
using Grpc.HunterService.Protos;

namespace Grpc.HunterService.Services;

public class GrpcHunterService : HunterProtoService.HunterProtoServiceBase
{
    private readonly GetAllHuntersHandler _getAllHuntersHandler;
    private readonly ILogger<GrpcHunterService> _logger;

    public GrpcHunterService(GetAllHuntersHandler getAllHuntersHandler, ILogger<GrpcHunterService> logger)
    {
        _getAllHuntersHandler = getAllHuntersHandler;
        _logger = logger;
    }

    public override Task<HuntersResponse> GetAllHunter(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] GetAllHunter called");

        var queryResult = _getAllHuntersHandler.Handle(new GetAllHuntersQuery(), context.CancellationToken);

        var response = new HuntersResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message
        };

        if (queryResult.Result.Data != null)
        {
            response.Data.AddRange(queryResult.Result.Data.Select(h => new Hunter
            {
                IdHunter = h.Id_Hunter,
                Name = h.Name,
                Age = h.Age,
                Origin = h.Origin
            }));
        }

        return Task.FromResult(response);
    }
}
