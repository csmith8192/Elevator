using System.Collections.Generic;

namespace Elevator
{
    public class Elevator
    {
        public bool InTransit { get; set; }
        public int NextFloor { get; set; }
        public Queue<int> SelectedFloorQueue { get; set; }
    }
}