using System.Collections.Generic;

namespace RegistracijaAPI.Models {
    public class CarOwnerData {

        public CarOwnerData() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CardExpDate { get; set; }
        public long CardNumber { get; set; }
        public int Cvv { get; set; }
        public List<CarData> CarsList = new List<CarData>();
    }
}
