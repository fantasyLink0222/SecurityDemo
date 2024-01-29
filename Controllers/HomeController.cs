using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.IdentityModel.Tokens;
using SecurityDemo.Data;
using SecurityDemo.Models;
using SecurityDemo.Repositories;
using System.Diagnostics;

namespace SecurityDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController( ILogger<HomeController> logger
                             , IConfiguration configuration
                             , UserManager<IdentityUser> userManager
                             , ApplicationDbContext context  )
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            string password = "P@ssw0rd!";
            var hasher = new PasswordHasher<IdentityUser>();
            string hashedPassword = hasher.HashPassword(null, password);

            // Now reverse it.
            var result = hasher.VerifyHashedPassword(null, hashedPassword, password);
            return View();
        }

        [Authorize]
        public IActionResult InjectionDemo(string message = "")
        {
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration,_context);
            List<City> cities = sqlDbRepository.GetCities(out string returnMessage);

            ViewData["Message"] = $"{message}{returnMessage}";

            return View(cities);
        }

        [Authorize]
        public IActionResult BuildingsInCity(int cityId )
        {
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration, _context);

           
            var city = _context.Cities.Find(cityId);
            if (city == null)
            {
                return RedirectToAction("InjectionDemo", new { message = "Please select a city." });
            }



         
            List<BuidlingRoomCityVM> buildings = sqlDbRepository.GetBuildingsInCity(cityId);

            if (buildings.Count() > 0)
            {
                return View(buildings);
            }
            else
            {
                return RedirectToAction("InjectionDemo",
                                    new { message = $"No buildings in this city." });
            }

           
        }

        public async Task<IActionResult> AdminArea()
        {
            var roles = new List<string>();
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    roles = (List<string>)await _userManager.GetRolesAsync(user);
                }
            }

            if (roles.Contains("Admin"))
            {
              
                SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration, _context);
                List<string> registeredUsers = await sqlDbRepository.GetRegisteredUsers(_userManager);

                return View(registeredUsers);
            }
            return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public ActionResult Products()
        {
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration, _context);
            List<ProductVM> products = sqlDbRepository.GetProducts();

            return View(products);
        }

        [Authorize]
        public ActionResult DisplayProduct(string prodID)

        {
            string sanitizedProdID = prodID.Replace("'", "''");
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration, _context);
            ProductVM product = sqlDbRepository.GetProduct(sanitizedProdID);

            return View(product);
        }
    }
}