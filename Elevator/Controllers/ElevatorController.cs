using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Elevator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElevatorController : ControllerBase
    {
        private readonly ILogger<ElevatorController> _logger;
        private readonly IMemoryCache _cache;

        public ElevatorController(ILogger<ElevatorController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
        }

        [Route("SendElevatorToFloor")]
        [HttpPost]
        public IActionResult SendElevatorToFloor(int floor)
        {
            if (floor < 1 || floor > 10) return new JsonResult("Floor must be greater than zero and less than 11.");

            var _theElevator = _cache.Get<Elevator>("myElevator");
            _theElevator.SelectedFloorQueue.Enqueue(floor);
            _theElevator.InTransit = true;
            _theElevator.NextFloor = floor;
            _cache.Set<Elevator>("myElevator", _theElevator);

            return Ok();
        }

        [Route("BringPersonToFloor")]
        [HttpPost]
        public IActionResult BringPersonToFloor(int floor)
        {
            if (floor < 1 || floor > 10) return new JsonResult("Floor must be greater than zero and less than 11.");

            var _theElevator = _cache.Get<Elevator>("myElevator");
            _theElevator.SelectedFloorQueue.Enqueue(floor);
            _theElevator.InTransit = true;
            _theElevator.NextFloor = floor;

            _cache.Set<Elevator>("myElevator", _theElevator);

            return Ok();
        }

        [Route("RequestAllFloorsBeingServiced")]
        [HttpGet]
        public IList<int> RequestAllFloorsBeingServiced()
        {
            var _theElevator = _cache.Get<Elevator>("myElevator");

            if (_theElevator.SelectedFloorQueue.Count > 0)
                return _theElevator.SelectedFloorQueue.ToList();
            else
            {
                var _newFloor = new List<int>();
                _newFloor.Add(1);

                return _newFloor;
            }
        }

        [Route("RequestNextFloorForService")]
        [HttpGet]
        public int RequestNextFloorForService()
        {
            var _theElevator = _cache.Get<Elevator>("myElevator");

            if (_theElevator.SelectedFloorQueue.Count > 0)
                return _theElevator.NextFloor;
            else
                return 1;
        }
    }
}