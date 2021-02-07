using System;
using System.Collections.Generic;

namespace BusLine.Models
{
    public class SeatsViewModel
    {
        public SeatsViewModel(List<int> freeSeats, int startStation, int endStation)
        {
            this.FreeSeats = freeSeats;
            this.StartStationValue = startStation;
            this.EndStationValue = endStation;
        }

        public List<int> FreeSeats { get; set; }

        public int StartStationValue { get; set; }

        public int EndStationValue { get; set; }
    }
}
