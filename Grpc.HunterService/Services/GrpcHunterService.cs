using ClassLibrary.HxH_Services.Features.Hunters.Create;
using ClassLibrary.HxH_Services.Features.Hunters.Delete;
using ClassLibrary.HxH_Services.Features.Hunters.GetAll;
using ClassLibrary.HxH_Services.Features.Hunters.GetById;
using ClassLibrary.HxH_Services.Features.Hunters.Update;
using Grpc.Core;
using Grpc.HunterService.Mappers;
using Grpc.HunterService.Protos;

namespace Grpc.HunterService.Services;

public class GrpcHunterService : HunterProtoService.HunterProtoServiceBase
{
    private readonly GetAllHuntersHandler _getAllHuntersHandler;
    private readonly GetHunterByIdHandler _getHunterByIdHandler;
    private readonly CreateHunterHandler _createHunterHandler;
    private readonly UpdateHunterHandler _updateHunterHandler;
    private readonly DeleteHunterByIdHandler _deleteHunterByIdHandler;
    private readonly ILogger<GrpcHunterService> _logger;

    public GrpcHunterService(
        GetAllHuntersHandler getAllHuntersHandler,
        GetHunterByIdHandler getHunterByIdHandler,
        CreateHunterHandler createHunterHandler,
        UpdateHunterHandler updateHunterHandler,
        DeleteHunterByIdHandler deleteHunterByIdHandler,
        ILogger<GrpcHunterService> logger)
    {
        _getAllHuntersHandler = getAllHuntersHandler;
        _getHunterByIdHandler = getHunterByIdHandler;
        _createHunterHandler = createHunterHandler;
        _updateHunterHandler = updateHunterHandler;
        _deleteHunterByIdHandler = deleteHunterByIdHandler;
        _logger = logger;
    }

    public override Task<HuntersResponse> GetAllHunter(EmptyRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] GetAllHunter called");

        var queryResult = _getAllHuntersHandler.Handle(new GetAllHuntersQuery(), context.CancellationToken);

        var response = new HuntersResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message,
        };

        if (queryResult.Result.Data != null)
        {
            response.Data.AddRange(queryResult.Result.Data.Select(h => GrpcHunterMapper.ToProto(h)));
        }

        return Task.FromResult(response);
    }

    public override Task<HunterResponse> GetByIdHunter(ByIdHunterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] GetGetByIdHunter called");

        var parameter = new GetHunterByIdQuery { Id_Hunter = request.IdHunter };
        var queryResult = _getHunterByIdHandler.Handle(parameter, context.CancellationToken);

        var response = new HunterResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message,
            Data = queryResult.Result.Data != null ? GrpcHunterMapper.ToProto(queryResult.Result.Data) : null
        };

        return Task.FromResult(response);
    }

    public override Task<HunterResponse> CreateHunter(CreateHunterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] CreateHunter called");

        var parameter = new CreateHunterCommand
        {
            Name = request.Name,
            Age = request.Age,
            Origin = request.Origin
        };
        var queryResult = _createHunterHandler.Handle(parameter, context.CancellationToken);

        var response = new HunterResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message,
            Data = queryResult.Result.Data != 0 ? GrpcHunterMapper.ToProto(queryResult.Result.Data, request) : null
        };

        return Task.FromResult(response);
    }

    public override Task<HunterResponse> UpdateHunter(UpdateHunterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] UpdateHunter called");

        var parameter = new UpdateHunterCommand
        {
            Id_Hunter = request.Hunter.IdHunter,
            Name = request.Hunter.Name,
            Age = request.Hunter.Age,
            Origin = request.Hunter.Origin
        };
        var queryResult = _updateHunterHandler.Handle(parameter, context.CancellationToken);

        var response = new HunterResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message,
            Data = queryResult.Result.Data ? GrpcHunterMapper.ToProto(request) : null
        };

        return Task.FromResult(response);
    }

    public override Task<HunterResponse> DeleteHunter(ByIdHunterRequest request, ServerCallContext context)
    {
        _logger.LogInformation("[GrpcHunterService] DeleteHunter called");

        var parameter = new DeleteHunterByIdCommand { Id_Hunter = request.IdHunter };
        var queryResult = _deleteHunterByIdHandler.Handle(parameter, context.CancellationToken);

        var response = new HunterResponse
        {
            IsSuccess = queryResult.Result.IsSuccess,
            Message = queryResult.Result.Message,
            Data = null
        };

        return Task.FromResult(response);
    }
}