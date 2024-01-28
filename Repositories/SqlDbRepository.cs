using Microsoft.Data.Sqlite;
using SecurityDemo.Models;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SecurityDemo.Repositories
{
    public class SqlDbRepository
    {
        private readonly IConfiguration _configuration;

        public SqlDbRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> GetCities(out string message)
        {
            message = string.Empty;
            List<string> cities = new List<string>();
            string sql = "SELECT * FROM Cities;";

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";
                SqliteConnection conn = new SqliteConnection(connectionString);
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                SqliteDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    cities.Add($"{sdr.GetInt32(0)},{sdr.GetString(1)}");
                }
            }
            catch (Exception e)
            {
                message = $"Error retrieving cities: {e.Message}";
            }
            if (cities.Count() == 0)
            {
                message = $"No cities";
            }
            return cities;
        }
        public string GetCityName(string cityId)
        {
            string cityName = string.Empty;
            string sql = $"SELECT CityName FROM Cities WHERE cityId = {cityId};";

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";
                SqliteConnection conn = new SqliteConnection(connectionString);
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                SqliteDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    cityName = $"{sdr.GetString(0)}";
                }
            }
            catch (Exception e)
            {
                string message = $"Error executing QL statement: {e.Message}";
            }
            return cityName;
        }

        public List<string> GetBuildingsInCity(string cityId)
        {
            List<string> list = new List<string>();

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = $"SELECT Buildings.buildingId, Buildings.name, Rooms.name, " +
                                 $"Rooms.capacity FROM Buildings INNER JOIN Rooms ON " +
                                 $"Buildings.buildingId = Rooms.buildingId WHERE " +
                                 $"Buildings.cityId = '{cityId}';";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add($"{reader.GetInt32(0)},{reader.GetString(1)}," +
                                         $"{reader.GetString(2)},{reader.GetInt32(3)}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }
            return list;
        }
        public List<string> GetRegisteredUsers()
        {
            List<string> list = new List<string>();
            string cityName = string.Empty;

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = $"SELECT AspNetUsers.Id, AspNetUsers.UserName, " +
                                    $"AspNetUserRoles.RoleId FROM AspNetUsers INNER " +
                                    $"JOIN AspNetUserRoles ON AspNetUsers.Id = " +
                                    $"AspNetUserRoles.UserId INNER JOIN AspNetRoles " +
                                    $"ON AspNetUserRoles.RoleId = AspNetRoles.Id;";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add($"{reader.GetString(0)},{reader.GetString(1)}," +
                                            $"{reader.GetString(2)}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }
            return list;
        }
        public List<string> GetBuildingsInCity2(string cityId)
        {
            string message = string.Empty;
            List<string> list = new List<string>();
            string sql = $"SELECT Buildings.buildingId, Buildings.name, Rooms.name, " +
                         $"Rooms.capacity FROM Buildings INNER JOIN Rooms ON " +
                         $"Buildings.buildingId = Rooms.buildingId WHERE " +
                         $"Buildings.cityId = '1';";

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";
                SqliteConnection conn = new SqliteConnection(connectionString);
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                SqliteDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    list.Add($"{sdr.GetInt32(0)},{sdr.GetString(1)}," +
                             $"{sdr.GetString(2)},{sdr.GetInt32(3)}");
                }
            }
            catch (Exception e)
            {
                message = $"Error retrieving cities: {e.Message}";
            }
            if (list.Count() == 0)
            {
                message = $"No cities";
            }
            return list;
        }

        public List<ProductVM> GetProducts()
        {
            List<ProductVM> products = new List<ProductVM>();

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = $"SELECT * FROM Products";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new ProductVM
                                {
                                    ProdName = (string)reader["prodName"],
                                    ProdID = (string)reader["prodID"],
                                    Price = (decimal)reader["price"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }
            return products;
        }

        public ProductVM GetProduct(string productID)
        {
            ProductVM productVM = new ProductVM();

            try
            {
                string connectionString = "Data Source=.\\wwwroot\\sql.db";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string sql = $"SELECT * FROM products WHERE prodID= {productID}";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                productVM = new ProductVM
                                {
                                    ProdName = (string)reader["prodName"],
                                    ProdID = (string)reader["prodID"],
                                    Price = (decimal)reader["price"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }

            return productVM;
        }
    }
}
