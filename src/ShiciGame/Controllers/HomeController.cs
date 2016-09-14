/*
 * 主页
 */
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ShiciGame.Entities;
using Newtonsoft.Json;

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
		
		/// <summary>
		/// 随机获取一定数量的诗句
		/// </summary>
		/// <param name="count">数量</param>
		/// <returns></returns>
		public JsonResult GetRandomVerse(int count)
		{
			var data = dbcon.Shici_Ans.Skip(new Random().Next(dbcon.Shici_Ans.Count() - count)).Take(count).ToList();
			return Json(data);
		}
    }
}
