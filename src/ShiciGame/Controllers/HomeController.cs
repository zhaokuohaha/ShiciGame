using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ShiciGame.Controllers
{
    public class HomeController : Controller
    {
		public HomeController(IServiceProvider services)
		{
			_servers = services;
			dbcon = new ApplicationDbContext(_servers.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
		}
		IServiceProvider _servers;
		ApplicationDbContext dbcon;
        public IActionResult Index()
        {
			var data = dbcon.Shici_Ans.Count();
			ViewData["count"] = data;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
