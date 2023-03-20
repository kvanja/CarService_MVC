using RegistracijaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegularServiceAPI.Models {
    public class CarService {
        public CarService() { }
        public int CarYearOfMake { get; set; }
        public string EngineType { get; set; }
        public long CardNumber { get; set; }
        public DateTime DateOfService { get; set; }
        public CarData CarData { get; set; }
    }
}
