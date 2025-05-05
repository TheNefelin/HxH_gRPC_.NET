//using Grpc.Net.Client;

//namespace TestProject.Tests.IntegrationTests.Hunters.Grpc;

//public class GrpcHunterServiceTests
//{
//    private readonly GrpcChannel _channel;
//    private readonly HunterProtoService.HunterProtoServiceClient _client;

//    public GrpcHunterServiceTests()
//    {
//        _channel = GrpcChannel.ForAddress("https://localhost:7001");
//        _client = new HunterProtoService.HunterProtoServiceClient(channel);
//    }

//    [Fact]
//    public async Task GetAllHunter_ReturnsSuccessAndHunters()
//    {
//        var request = new EmptyRequest();
//        var response = await _client.GetAllHunterAsync(new EmptyRequest());

//        Assert.True(response.IsSuccess);
//        Assert.NotNull(response.Data);
//        Assert.NotEmpty(response.Data);
//    }
//}
