using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController( ILogger<HomeController> logger
                             , IConfiguration configuration
                             , UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
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
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration);
            List<string> cities = sqlDbRepository.GetCities(out string returnMessage);

            ViewData["Message"] = $"{message}{returnMessage}";

            return View(cities);
        }

        [Authorize]
        public IActionResult BuildingsInCity(string cityId = "")
        {
            if (string.IsNullOrEmpty(cityId))
            {
                return RedirectToAction("InjectionDemo", new { message = "Please select a city." });
            }

            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration);

            string cityName = sqlDbRepository.GetCityName(cityId);
            List<string> cityBuildings = new List<string> { cityName };
            List<string> buildings = sqlDbRepository.GetBuildingsInCity(cityId);

            if (buildings.Count() > 0)
            {
                cityBuildings.AddRange(buildings);
            }
            else
            {
                return RedirectToAction("InjectionDemo",
                                    new { message = $"No buildings in {cityName}." });
            }

            return View(cityBuildings);
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
                SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration);
                List<string> registeredUsers = sqlDbRepository.GetRegisteredUsers();

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
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration);
            List<ProductVM> products = sqlDbRepository.GetProducts();

            return View(products);
        }

        [Authorize]
        public ActionResult DisplayProduct(string prodID)
        {
            SqlDbRepository sqlDbRepository = new SqlDbRepository(_configuration);
            ProductVM product = sqlDbRepository.GetProduct(prodID);

            return View(product);
        }
    }
}