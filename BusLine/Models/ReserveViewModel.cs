using System;

namespace BusLine.Models
{
    public class ReserveViewModel
    {
        public string UserId { get; set; }
        public string startStations { get; set; }

        public string endStations { get; set; }

        public string[] selectedSeats { get; set; }
    }
}
