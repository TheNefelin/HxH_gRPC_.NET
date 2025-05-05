// See https://aka.ms/new-console-template for more information
using Grpc.HunterService.Protos;
using Grpc.Net.Client;

Console.WriteLine("gRPC Test!");

var channel = GrpcChannel.ForAddress("https://localhost:7001");
var client = new HunterProtoService.HunterProtoServiceClient(channel);

try
{
    var response = await client.GetAllHunterAsync(new EmptyRequest());


    if (response.IsSuccess)
    {
        Console.WriteLine("Request succeeded!");

        foreach (var hunter in response.Data)
        {
            Console.WriteLine($"Id: {hunter.IdHunter}, Name: {hunter.Name}, Age: {hunter.Age}, Origin: {hunter.Origin}");
        }
    }
    else
    {
        Console.WriteLine("Request failed.");
    }

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.ReadKey();