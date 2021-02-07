using BusLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusLine.Data
{
    public interface IDataService
    {
        public IndexViewModel GetStations();

        public List<int> GetAvailableSeats(int startStations, int endStations);

        public void ReserveSeats(ReserveViewModel model);
    }
}
