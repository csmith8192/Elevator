using ClientApi.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElevatorConsoleClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await SendElevatorToFloor(4);

            await BringPersonToFloor(6);

            await RequestNextFloor();

            await SendElevatorToFloor(3);

            await BringPersonToFloor(2);

            await RequestAllFloors();
        }

        private static async Task SendElevatorToFloor(int floor)
        {
            var httpClient = new HttpClient();
            var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

            await client.SendElevatorToFloorAsync(floor);
        }

        private static async Task BringPersonToFloor(int floor)
        {
            var httpClient = new HttpClient();
            var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

            await client.BringPersonToFloorAsync(floor);
        }

        private static async Task RequestNextFloor()
        {
            var httpClient = new HttpClient();
            var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

            var nextFloor = await client.RequestNextFloorForServiceAsync();
            Console.WriteLine($"{nextFloor}");
        }

        private static async Task RequestAllFloors()
        {
            var httpClient = new HttpClient();
            var client = new ElevatorServiceClient("http://localhost:8080", httpClient);
            var floors = await client.RequestAllFloorsBeingServicedAsync();

            foreach (var floor in floors)
            {
                Console.WriteLine($"{floor}");
            }
        }
    }
}