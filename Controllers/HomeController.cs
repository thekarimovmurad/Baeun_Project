using Baeun_Project.DAL;
using Baeun_Project.Models;
using Baeun_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Baeun_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                Headers = await db.Headers.ToListAsync(),
                Abouts = await db.Abouts.FirstOrDefaultAsync(),
                Projects = await db.Projects.ToListAsync(),
            };
            return View(hvm);
        }
    }
}
