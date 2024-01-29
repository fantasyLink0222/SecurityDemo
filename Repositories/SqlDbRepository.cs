using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using SecurityDemo.Data;
using SecurityDemo.Models;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SecurityDemo.Repositories
{
    public class SqlDbRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public SqlDbRepository(IConfiguration configuration,ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }



   
        public List<City> GetCities(out string message)
        {
            message = string.Empty;
            List<City> cities = new List<City>();
            

            try
            {
                cities = _context.Cities.ToList();
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

  
        public string GetCityName(int cityId)
        {
            string cityName = string.Empty;

            try
            {
               
                    cityName = _context.Cities
                                      .Where(c => c.cityId == cityId)
                                      .Select(c => c.cityName)
                                      .FirstOrDefault();
               
            }
            catch (Exception e)
            {
                string message = $"Error executing QL statement: {e.Message}";
            }
            return cityName;
        }

      
        public List<BuidlingRoomCityVM> GetBuildingsInCity(int cityId)
        {
            List<BuidlingRoomCityVM> list = new List<BuidlingRoomCityVM>();
            

            try
            {
               
                   list = _context.Buildings
                                    .Where(b => b.cityId == cityId)      
                                       .SelectMany(b => b.rooms, (b, r) => new BuidlingRoomCityVM
                                       {
                                           buildingId = b.buildingId,
                                           buildingName = b.name,
                                           roomName = r.name,
                                           capacity = r.capacity,
                                           cityName = b.city.cityName
                                       }).ToList();  
                
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }
            return list;
        }
       
        public async Task<List<string>> GetRegisteredUsers(UserManager<IdentityUser> userManager)
        {
            var list = new List<string>();
            var users = userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var rolesString = string.Join(",", roles);
                list.Add($"{user.Id},{user.UserName},{rolesString}");
            }

            return list;
        }




        //Change the GetProducts and GetProduct methods to use Entity Framework Core
        public List<ProductVM> GetProducts()
        {
            List<ProductVM> productsVM = new List<ProductVM>();

            try
            {
                productsVM = _context.Products
             .Select(product => new ProductVM
             {
                 ProdName = product.prodName,
                 ProdID = product.prodID,
                 Price = product.price
             })
             .ToList();
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }
            return productsVM;
        }
        public ProductVM GetProduct(string productID)
        {
            ProductVM productVM = new ProductVM();

            try
            {
                var product = _context.Products.Where(p => p.prodID == productID).FirstOrDefault();

                if (product != null) {
                    productVM = new ProductVM
                    {
                        ProdName = product.prodName,
                        ProdID = product.prodID,
                        Price = product.price
                    };
                }
                else
                {
                    productVM = null;
                }
            }
            catch (Exception e)
            {
                string message = $"Error retrieving city name: {e.Message}";
            }

            return productVM;
        }


        /*public List<string> GetRegisteredUsers()
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
       }*/


        /*public List<Building> GetBuildingsInCity2(int cityId)
        {
            string message = string.Empty;
            List<Building> list = new List<Building>();
           
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
         }*/

    }
}
