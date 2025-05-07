using Grpc.HunterService.Protos;
using Grpc.Net.Client;

namespace TestConsoleApp.Services;

public class GrpcClientHunterService
{
    private readonly GrpcChannel _channel;
    private readonly HunterProtoService.HunterProtoServiceClient _client;

    public GrpcClientHunterService(string serviceUrl)
    {
        _channel = GrpcChannel.ForAddress(serviceUrl);
        _client = new HunterProtoService.HunterProtoServiceClient(_channel);
    }

    public async Task GetAll()
    {
        try
        {
            var response = await _client.GetAllHunterAsync(new EmptyRequest());

            if (response.IsSuccess)
            {
                Console.WriteLine("Request GetAll succeeded!");

                foreach (var hunter in response.Data)
                {
                    Console.WriteLine($"Id: {hunter.IdHunter}, Name: {hunter.Name}, Age: {hunter.Age}, Origin: {hunter.Origin}");
                }
            }
            else
            {
                Console.WriteLine("Request failed: " + response.Message);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task GetById(int id)
    {
        try
        {
            var request = new ByIdHunterRequest
            {
                IdHunter = id
            };

            var response = await _client.GetByIdHunterAsync(request);

            if (response.IsSuccess)
            {
                Console.WriteLine("Request GetAll succeeded!");
                Console.WriteLine($"Id: {response.Data.IdHunter}, Name: {response.Data.Name}, Age: {response.Data.Age}, Origin: {response.Data.Origin}");
            }
            else
            {
                Console.WriteLine("Request failed: " + response.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task<int> Create(string Name, int Age, string Origin)
    {
        try
        {
            var request = new CreateHunterRequest
            {
                Name = Name,
                Age = Age,
                Origin = Origin
            };

            var response = await _client.CreateHunterAsync(request);

            if (response.IsSuccess)
            {
                Console.WriteLine("Request Create succeeded!");
                Console.WriteLine($"Id: {response.Data.IdHunter}, Name: {response.Data.Name}, Age: {response.Data.Age}, Origin: {response.Data.Origin}");
                return response.Data.IdHunter;
            }
            else
            {
                Console.WriteLine("Request failed: " + response.Message);
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 0;
        }
    }

    public async Task Update(int id, string Name, int Age, string Origin)
    {
        try
        {
            var request = new UpdateHunterRequest
            {
                Hunter = new Hunter
                {
                    IdHunter = id,
                    Name = Name,
                    Age = Age,
                    Origin = Origin
                }
            };

            var response = await _client.UpdateHunterAsync(request);
            
            if (response.IsSuccess)
            {
                Console.WriteLine("Request Update succeeded!");
                Console.WriteLine($"Id: {response.Data.IdHunter}, Name: {response.Data.Name}, Age: {response.Data.Age}, Origin: {response.Data.Origin}");
            }
            else
            {
                Console.WriteLine("Request failed: " + response.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            var request = new ByIdHunterRequest
            {
                IdHunter = id
            };

            var response = await _client.DeleteHunterAsync(request);
            
            if (response.IsSuccess)
            {
                Console.WriteLine("Request Delete succeeded!");
            }
            else
            {
                Console.WriteLine("Request failed: " + response.Message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
