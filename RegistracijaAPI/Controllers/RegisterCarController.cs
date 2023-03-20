using Microsoft.AspNetCore.Mvc;
using RegistracijaAPI.Models;
using RegularServiceAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RegistracijaAPI.Controllers {
    public class RegisterCarController : Controller {
        private const string dbPath = "data source=.\\DataBase\\RegularService";
        public IActionResult Index() {
            return View();
        }

        public List<CarData> GetCarManufacturers() {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    cmd.CommandText = @"SELECT id, manufacturer FROM carManufacturers order by manufacturer ASC";

                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        List<CarData> carManufacturers = new List<CarData>();
                        while (reader.Read()) {
                            CarData cd = new CarData();
                            cd.Id = reader.GetInt16(reader.GetOrdinal("id"));
                            cd.Manufacturer = reader.GetString(reader.GetOrdinal("manufacturer"));
                            carManufacturers.Add(cd);
                        }
                        return carManufacturers;
                    }
                }
            }
        }

        public List<CarData> GetCarModels(string Manufacturer) {

            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    cmd.CommandText = @"SELECT mod.model as model, mod.id
                                        FROM carModels as mod
                                        LEFT JOIN carManufacturers as manu ON mod.manufacturerId = manu.id
                                        WHERE manu.id = :Manufacturer ORDER BY model ASC";
                    cmd.Parameters.Add(new SQLiteParameter("Manufacturer", Convert.ToInt32(Manufacturer)));
                    List<CarData> carModels = new List<CarData>();
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            CarData cd = new CarData();
                            cd.Id = reader.GetInt16(reader.GetOrdinal("id"));
                            cd.Model = reader.GetString(reader.GetOrdinal("model"));
                            carModels.Add(cd);
                        }
                        return carModels;
                    }
                }
            }
        }

        public void AddCarService([FromBody] CarService carService) {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    cmd.CommandText = @"INSERT INTO service (customerId, carManufacturerId, carModelId, carYearOfMake, engineType, dateOfService)
                                                      values ((SELECT id FROM Customers WHERE cardNumber = :customerId), :carManufacturerId, :carModelId, :carYearOfMake, :engineType, :dateOfService)";


                    cmd.Parameters.Add(new SQLiteParameter("customerId", carService.CardNumber));
                    cmd.Parameters.Add(new SQLiteParameter("carManufacturerId", Convert.ToInt16(carService.CarData.Manufacturer)));
                    cmd.Parameters.Add(new SQLiteParameter("carModelId", Convert.ToInt16(carService.CarData.Model)));
                    cmd.Parameters.Add(new SQLiteParameter("carYearOfMake", carService.CarYearOfMake));
                    cmd.Parameters.Add(new SQLiteParameter("engineType", carService.EngineType));
                    cmd.Parameters.Add(new SQLiteParameter("dateOfService", carService.DateOfService));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Service> GetServices() {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    cmd.CommandText = @"SELECT cust.firstName || ' ' || cust.lastName as CustomerName, carMan.manufacturer || ' ' || carMod.Model as Car, carYearOfMake as YearOfMake, EngineType, dateOfService as TimeOfService
                                        FROM service 
                                        LEFT JOIN customers as cust on service.customerId = cust.id
                                        LEFT JOIN carManufacturers as carMan on service.carManufacturerId = carMan.id
                                        LEFT JOIN carModels as carMod on service.carModelId = carMod.id ORDER BY TimeOfService ASC";
                    List<Service> ListOfServices = new List<Service>();
                    using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            Service service = new Service();
                            service.CustomerName = reader.GetString(reader.GetOrdinal("CustomerName"));
                            service.Car = reader.GetString(reader.GetOrdinal("Car"));
                            service.YearOfMake = reader.GetInt32(reader.GetOrdinal("YearOfMake"));
                            service.EngineType = reader.GetString(reader.GetOrdinal("engineType"));
                            service.TimeOfService = reader.GetDateTime(reader.GetOrdinal("TimeOfService"));
                            ListOfServices.Add(service);
                        }
                        return ListOfServices;
                    }
                }
            }
        }


    }
}