using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistracijaAPI.Models;
using System;
using System.Data.SQLite;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace RegularServiceAPI.Controllers {
    public class CustomerController : Controller {
        private const string dbPath = "data source=.\\DataBase\\RegularService";

        [HttpPost]
        public void NewCustomer([FromBody] CarOwnerData customer) {

            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    try {
                        cmd.CommandText = "INSERT INTO customers(firstname, lastname, email, username, password, cardNumber, cardExpDate, cvv)" +
                                     "values(:firstname, :lastname, :email, :username, :password, :cardNumber, :cardExpDate, :cvv)";
                        cmd.Parameters.Add(new SQLiteParameter("firstname", customer.FirstName));
                        cmd.Parameters.Add(new SQLiteParameter("lastname", customer.LastName));
                        cmd.Parameters.Add(new SQLiteParameter("email", customer.Email));
                        cmd.Parameters.Add(new SQLiteParameter("username", customer.Username));
                        cmd.Parameters.Add(new SQLiteParameter("password", customer.Password));
                        cmd.Parameters.Add(new SQLiteParameter("cardNumber", customer.CardNumber));
                        cmd.Parameters.Add(new SQLiteParameter("cardExpDate", customer.CardExpDate));
                        cmd.Parameters.Add(new SQLiteParameter("cvv", customer.Cvv));

                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e) {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        [HttpGet]
        public CarOwnerData GetCarOwnerData(string Username, string Password) {
            using (SQLiteConnection conn = new SQLiteConnection(dbPath)) {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    try {
                        cmd.CommandText = @"SELECT firstName, lastName, cardNumber, cardExpDate, cvv FROM customers WHERE username = :username AND password = :password";
                        cmd.Parameters.Add(new SQLiteParameter("username", Username));
                        cmd.Parameters.Add(new SQLiteParameter("password", Password));

                        using (SQLiteDataReader reader = cmd.ExecuteReader()) {
                            CarOwnerData registratedUser = new CarOwnerData();
                            while (reader.Read()) {
                                registratedUser.FirstName = reader.GetString(reader.GetOrdinal("firstName"));
                                registratedUser.LastName = reader.GetString(reader.GetOrdinal("lastName"));
                                registratedUser.CardNumber = reader.GetInt32(reader.GetOrdinal("cardNumber"));
                                registratedUser.CardExpDate = reader.GetString(reader.GetOrdinal("cardExpDate"));
                                registratedUser.Cvv = reader.GetInt16(reader.GetOrdinal("cvv"));
                            }
                            return registratedUser;
                        }
                    }
                    catch (Exception e) {
                        throw new Exception(e.Message);
                    }
                }
            }
        }
    }
}