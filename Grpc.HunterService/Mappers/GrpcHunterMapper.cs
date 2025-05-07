using ClassLibrary.HxH_Services.Features.Hunters;
using Grpc.HunterService.Protos;

namespace Grpc.HunterService.Mappers;

public static class GrpcHunterMapper
{
    public static Hunter ToProto(HunterDto hunterDto)
    {
        return new Hunter
        {
            IdHunter = hunterDto.Id_Hunter,
            Name = hunterDto.Name,
            Age = hunterDto.Age,
            Origin = hunterDto.Origin
        };
    }

    public static Hunter ToProto(int id, CreateHunterRequest protoRequest)
    {
        return new Hunter
        {
            IdHunter = id,
            Name = protoRequest.Name,
            Age = protoRequest.Age,
            Origin = protoRequest.Origin
        };
    }

    public static Hunter ToProto(UpdateHunterRequest protoRequest)
    {
        return new Hunter
        {
            IdHunter = protoRequest.Hunter.IdHunter,
            Name = protoRequest.Hunter.Name,
            Age = protoRequest.Hunter.Age,
            Origin = protoRequest.Hunter.Origin
        };
    }
}
