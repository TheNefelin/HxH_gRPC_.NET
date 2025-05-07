using TestConsoleApp.Services;

Console.WriteLine("gRPC Test!");

var serviceUrl = "https://localhost:7001";
var grpcHunter = new GrpcClientHunterService(serviceUrl);

Console.WriteLine("-----------------------------------");
await grpcHunter.GetAll();
Console.WriteLine("-----------------------------------");
await grpcHunter.GetById(1);
Console.WriteLine("-----------------------------------");
var id = await grpcHunter.Create("Test1", 100, "Test1");
Console.WriteLine("-----------------------------------");
await grpcHunter.Update(id, "Test2", 200, "Test2");
Console.WriteLine("-----------------------------------");
await grpcHunter.Delete(id);
Console.WriteLine("-----------------------------------");

Console.ReadKey();