using System;
using System.Collections.Generic;

namespace RegularServiceAPI.Models {
    public class Service {

        public string CustomerName { get; set; }
        public string Car { get; set; }
        public int YearOfMake { get; set; }
        public string EngineType { get; set; }
        public DateTime TimeOfService { get; set; }

        public Service() { }

    }
}
