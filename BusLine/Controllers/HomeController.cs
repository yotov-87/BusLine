using BusLine.Data;
using BusLine.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace BusLine.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ILogger<HomeController> _logger;
        private readonly IDataService dataService;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, IDataService dataService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.dataService = dataService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var model = this.dataService.GetStations();

            return View(model);
        }
        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Check(int startStations, int endStations)
        {
            if(startStations >= endStations)
            {
                return RedirectToAction("Index");
            }
            else
            {
                List<int> freeSeats = this.dataService.GetAvailableSeats(startStations, endStations);

                SeatsViewModel model = new SeatsViewModel(freeSeats, startStations, endStations);

                return View(model);
            }            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Reserve(ReserveViewModel model)
        {
            var user = this.userManager.GetUserAsync(this.User).GetAwaiter().GetResult();
            model.UserId = user.Id;
            this.dataService.ReserveSeats(model);

            return View("ReservationResult");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
