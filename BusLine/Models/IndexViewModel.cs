using System;
using System.Collections.Generic;

namespace BusLine.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Stations = new Dictionary<int, string>();
        }

        public Dictionary<int,string> Stations { get; set; }
    }
}
