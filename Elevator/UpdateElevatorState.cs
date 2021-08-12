using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elevator
{
    public class UpdateElevatorState : IHostedService, IDisposable
    {
        private Timer _timer;
        private int _number;
        private readonly ILogger<UpdateElevatorState> _logger;
        private readonly IMemoryCache _cache;

        public UpdateElevatorState(ILogger<UpdateElevatorState> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(o =>
            {
                Interlocked.Increment(ref _number);

                GetNextStop(false);

                _logger.LogInformation($"Reached requested floor; remove from queue {_number}");
            },
            null,
            TimeSpan.FromSeconds(20),
            TimeSpan.FromSeconds(30));

            //give people time to enter and exit the elevator
            Thread.Sleep(20);

            GetNextStop(true);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private int GetNextStop(bool InTransit)
        {
            var _theElevator = _cache.Get<Elevator>("myElevator");

            if (_theElevator == null) _cache.Set<Elevator>("myElevator", new Elevator() { InTransit = false, NextFloor = 1, SelectedFloorQueue = new Queue<int>() });

            _theElevator.InTransit = InTransit;

            //if the queue is empty, go back to the first floor
            if (_theElevator.SelectedFloorQueue.Count == 0)
            {
                _theElevator.SelectedFloorQueue.Enqueue(1);
                _theElevator.NextFloor = 1;
                _cache.Set<Elevator>("myElevator", _theElevator);
                return 0;
            }

            if (InTransit)
            {
                //if we're between stops and moving, the next stop is the next floor in queue that hasn't been processed yet.
                _theElevator.NextFloor = _theElevator.SelectedFloorQueue.Peek();
            }
            else
            {
                //if we're stopped on a floor, the next stop is the floor after the current one in queue
                _theElevator.SelectedFloorQueue.Dequeue();
                _theElevator.NextFloor = _theElevator.SelectedFloorQueue.FirstOrDefault();
            }

            _theElevator.NextFloor = _theElevator.NextFloor == 0 ? 1 : _theElevator.NextFloor;

            _cache.Set<Elevator>("myElevator", _theElevator);

            return 0;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}