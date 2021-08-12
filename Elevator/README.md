# Elevator (0.0.1)

Elevator Control

This API provides simplified endpoints for theoretical elevator behavior. A FIFO Queue is used to simulate an elevator responding to floor 
selections from users. A cached Elevator is used to hold the state of the elevator, i.e. is it moving or stationary, what is the
next stop it needs to fulfill (which depends on if it's moving when this query is sent).

Constraints include: there is only one elevator, and 10 floors.

Using this API is not encouraged, since it's just for this excercise.

## Scenarios
      
* A person requests an elevator be sent to their current floor
* A person requests that they be brought to a floor
* An elevator car requests all floors that it’s current passengers are servicing (e.g. to light up the buttons that show which floors the car is going to)
* An elevator car requests the next floor it needs to service



### A person requests an elevator be sent to their current floor

```c#

private static async Task SendElevatorToFloor(int floor)
{
    var httpClient = new HttpClient();
    var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

    await client.SendElevatorToFloorAsync(floor);
}
```

### A person requests that they be brought to a floor

```c#

private static async Task BringPersonToFloor(int floor)
{
    var httpClient = new HttpClient();
    var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

    await client.BringPersonToFloorAsync(floor);
}
```

### An elevator car requests all floors that it’s current passengers are servicing (e.g. to light up the buttons that show which floors the car is going to)

```c#

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
```

### An elevator car requests the next floor it needs to service

```c#

static async Task RequestNextFloor()
{
    var httpClient = new HttpClient();
    var client = new ElevatorServiceClient("http://localhost:8080", httpClient);

    var nextFloor = await client.RequestNextFloorForServiceAsync();
    Console.WriteLine($"{nextFloor}");
}
```