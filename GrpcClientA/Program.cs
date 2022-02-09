using Grpc.Net.Client;
using GrpcService1;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcClientA
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClientHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator };
            var httpClient = new HttpClient(httpClientHandler);
            using var channel = GrpcChannel.ForAddress("https://localhost:5001",new GrpcChannelOptions { HttpClient = httpClient});
            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(new HelloRequest { Name = "Grpc" });
            Console.WriteLine("Greeting:" + response.Message);
            var byeresponse = await client.SayGoodByeAsync(new GoodByeRequest { Name = "QYM" });
            Console.WriteLine("Greeting:" + byeresponse.Message);
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
    }
}
